using EmitterPersonalAccount.API.Contracts;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmitterPersonalAccount.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmittersController : ControllerBase
    {
        private readonly IEmittersRepository emittersRepository;
        private readonly IOutboxService outboxService;

        public EmittersController(IEmittersRepository emittersRepository, 
            IOutboxService outboxService)
        {
            this.emittersRepository = emittersRepository;
            this.outboxService = outboxService;
        }

        [HttpPost("register")]
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
    }
}
