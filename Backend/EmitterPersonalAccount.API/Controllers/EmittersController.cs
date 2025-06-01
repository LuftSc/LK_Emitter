using EmitterPersonalAccount.API.Contracts;
using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace EmitterPersonalAccount.API.Controllers
{
    [ApiController]
    [Route("emitters")]
    public class EmittersController : ControllerBase
    {
        private readonly IEmittersRepository emittersRepository;
        private readonly IOutboxService outboxService;
        private readonly IUserRepository userRepository;

        public EmittersController(IEmittersRepository emittersRepository, 
            IOutboxService outboxService, IUserRepository userRepository)
        {
            this.emittersRepository = emittersRepository;
            this.outboxService = outboxService;
            this.userRepository = userRepository;
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Зарегистрировать нового эмитента",
            Description = "Регистрирует нового эмитента в системе")]
        public async Task<ActionResult> RegisterEmitter(
            [FromBody] ShortlyEmitterDTO request, 
            CancellationToken cancellation)
        {
            var ogrnVO = OGRNInfo.Create(
                request.EmitterInfo.OGRN.Number,
                request.EmitterInfo.OGRN.DateOfAssignment,
                request.EmitterInfo.OGRN.Issuer);

            if (ogrnVO.IsFailure) return BadRequest(ogrnVO.Error);

            var registrationVO = RegistrationInfo.Create(
                request.EmitterInfo.registration.Number,
                request.EmitterInfo.registration.RegistrationDate,
                request.EmitterInfo.registration.Issuer);

            if (registrationVO.IsFailure) return BadRequest(registrationVO.Error);

            var emitterInfoVO = EmitterInfo.Create(
                request.EmitterInfo.FullName,
                request.EmitterInfo.ShortName,
                request.EmitterInfo.Inn,
                request.EmitterInfo.Jurisdiction,
                ogrnVO.Value,
                registrationVO.Value
            );

            if (emitterInfoVO.IsFailure) return BadRequest(emitterInfoVO.Error);

            var creatingResult = Emitter
                .Create(emitterInfoVO.Value, request.IssuerId);

            if (!creatingResult.IsSuccessfull)
                return BadRequest(creatingResult.GetErrors());

            var addingResult = await emittersRepository
                .AddAsync(creatingResult.Value, cancellation);

            if (addingResult is null) return BadRequest("Failed on adding Emitter Entity to context");

            var outboxSavingResult = await outboxService.CreateAndSaveOutboxMessage(
                OutboxMessageType.AddEmitter,
                JsonSerializer.Serialize(
                    Tuple.Create(
                        creatingResult.Value.Id,
                        creatingResult.Value.EmitterInfo,
                        creatingResult.Value.IssuerId
                    )
                ),
                cancellation);

            if (!outboxSavingResult.IsSuccessfull)
                return BadRequest(outboxSavingResult.GetErrors());

            return Ok();
        }

        [HttpPost("bind-user-to-emitters")]
        [SwaggerOperation(Summary = "Прикрепить к эмитенту",
            Description = "Добавляет пользователя в список уполномоченных представителей эмитента")]
        public async Task<ActionResult> BindToEmitters(
            [FromBody] BindToEmittersDTO request,
            CancellationToken cancellation
            )
        {
            var result = await userRepository
                .BindToEmitters(request.UserId, request.EmittersIdList, cancellation);

            if (!result.IsSuccessfull)
                return BadRequest(result.GetErrors());

            return Ok();
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Найти эмитента по названию",
            Description = "Возвращает список эмитентов, удовлетворяющих частичному названию")]
        public async Task<ActionResult<List<EmitterInfoDTO>>> SearchEmitters(string searchTerm, int page = 1, int pageSize = 20)
        {
            var result = await emittersRepository.SearchEmitter(searchTerm, page, pageSize);

            if (result is null) return BadRequest();

            var response = result
                .Select(t => new EmitterInfoDTO(t.Item1, t.Item2, t.Item3))
                .ToList();

            return Ok(response);
        }

        [Permission(Permission.ChoiceOfEmitters)]
        [HttpGet("get-binding-to-user-emitters")]
        [SwaggerOperation(Summary = "Получить список прикреплённых эмитентов",
            Description = "Возвращает список эмитентов, уполномоченным представителем которых является текущий пользователь")]
        public async Task<ActionResult<List<EmitterInfoDTO>>> GetBindingToUserEmitters
            (
            int page = 1,
            int pageSize = 10,
            CancellationToken cancellation = default)
        {
            var userId = HttpContext.User.FindFirst(CustomClaims.UserId).Value;

            if (userId == null) return BadRequest("user id can not be null");

            Guid.TryParse(userId, out Guid userGuid);

            var emitters = await userRepository
                .GetEmittersCurrentUser(userGuid, page, pageSize, cancellation);

            if (!emitters.IsSuccessfull)
                return BadRequest(emitters.GetErrors());

            var result = emitters.Value
                .Select(e => new EmitterInfoDTO(e.Id, e.EmitterInfo, e.IssuerId));

            return Ok(result);

        }
    }
}
