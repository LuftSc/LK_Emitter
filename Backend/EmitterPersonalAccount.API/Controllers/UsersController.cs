//using AuthService.Application.Features.Users;
//using AuthService.Controllers;
using EmitterPersonalAccount.API.Contracts;
using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.DataAccess.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

//using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using System.Text.Json;
using System.Threading;
using static System.Net.WebRequestMethods;


namespace EmitterPersonalAccount.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IDistributedCache distributedCache;
        private readonly IJwtProvider jwtProvider;
        private readonly IEmittersRepository emittersRepository;
        private readonly IRegistratorRepository registratorRepository;
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly IRabbitMqPublisher publisher;
        private readonly IOutboxService outboxService;
        private readonly IUsersService usersService;

        public UsersController(IMediator mediator, 
            IDistributedCache distributedCache, 
            IJwtProvider jwtProvider,
            IEmittersRepository emittersRepository, 
            IRegistratorRepository registratorRepository, 
            IUserRepository userRepository, 
            IPasswordHasher passwordHasher, 
            IRabbitMqPublisher publisher, 
            IOutboxService outboxService, 
            IUsersService usersService)
        {
            this.mediator = mediator;
            this.distributedCache = distributedCache;
            this.jwtProvider = jwtProvider;
            this.emittersRepository = emittersRepository;
            this.registratorRepository = registratorRepository;
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.publisher = publisher;
            this.outboxService = outboxService;
            this.usersService = usersService;
        }

        [HttpGet("get-current-user")]
        public async Task<ActionResult<Guid>> GetCurrentUserId()
        {
            var userIdResult = ClaimService.Get(HttpContext, CustomClaims.UserId);

            if (!userIdResult.IsSuccessfull)
                return BadRequest(userIdResult.GetErrors());
            
            Guid.TryParse(userIdResult.Value, out Guid userGuid);

            return Ok(JsonSerializer.Serialize(userGuid));
        }

        [Permission(Permission.ProfileActions)]
        [HttpGet("get-personal-data-current")]
        public async Task<ActionResult<UserDataDTO>> GetPersonalDataCurrentUser
            (CancellationToken cancellation)
        {
            var userIdResult = ClaimService.Get(HttpContext, CustomClaims.UserId);

            if (!userIdResult.IsSuccessfull)
                return BadRequest(userIdResult.GetErrors());

            Guid.TryParse(userIdResult.Value, out Guid userGuid);

            var decryptedUserDataGettingResult = await usersService
                .GetUserPersonalData(userGuid, cancellation);

            if (!decryptedUserDataGettingResult.IsSuccessfull)
                return BadRequest(decryptedUserDataGettingResult.GetErrors());

            var userData = decryptedUserDataGettingResult.Value;

            var splittedFullName = new string[] { string.Empty, string.Empty, string.Empty };

            if (userData.FullName.Length > 0)
            {
                var words = userData.FullName.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    splittedFullName[i] = words[i];
                }
            }

            return new UserDataDTO(
                userData.Id,
                splittedFullName[1],
                splittedFullName[0],
                splittedFullName[2],
                userData.Email,
                userData.Phone,
                userData.BirthDate,
                userData.Passport,
                userData.Role
            );
        }

        [HttpPost("login-user")]
        public async Task<ActionResult> Login([FromBody] LoginUserQuery request)
        {
            var loginResult = await mediator.Send(request);

            if (!loginResult.IsSuccessfull)
                return BadRequest(loginResult.GetErrors());

            await distributedCache
                .SetStringAsync($"id-{request.Email}", loginResult.Value);

            var sendEmailCommand = new SendConfirmationCodeCommand() 
            { 
                RecipientEmail = request.Email
            };

            var emailSendResult = await mediator.Send(sendEmailCommand);

            if (!emailSendResult.IsSuccessfull)
            {
                return BadRequest(emailSendResult.GetErrors());
            }

            return Ok();
        }

        [HttpPost("login-user-without-2fa")]
        public async Task<ActionResult> LoginWithout2FA([FromBody] LoginUserQuery request)
        {
            var loginResult = await mediator.Send(request);

            if (!loginResult.IsSuccessfull)
                return BadRequest(loginResult.GetErrors());

            var claimsData = JsonSerializer
                .Deserialize<UserClaimsData>(loginResult.Value);

            var token = jwtProvider.GenerateToken(
                claimsData.UserId.ToString(), 
                claimsData.Role);

            HttpContext.Response.Cookies.Append("tasty-cookies", token);

            return Ok();
        }

       /* [HttpPost("register-new-user")]
        public async Task<ActionResult> Register([FromBody] RegisterUserCommand request)
        {
            if (string.IsNullOrEmpty(request.Email)) 
                return BadRequest("Email can not be empty or null!");
            if (string.IsNullOrEmpty(request.Password))
                return BadRequest("Password can not be empty or null!");

            var result = await mediator.Send(request);

            if (!result.IsSuccessfull) return BadRequest(result.GetErrors());

            return Ok();
        }*/

        [HttpPost("register")]
        public async Task<ActionResult> RegisterNewUser(
            [FromBody] RegisterUserRequest request, 
            CancellationToken cancellation)
        {
            var userSavingResult = await usersService.EncryptAndSaveToDb(
                request.Email,
                request.Password,
                request.EmittersGuids,
                request.BirthDate,
                request.Passport?.DateOfIssuer,
                request.Passport?.Series,
                request.Passport?.Number,
                request.Passport?.Issuer,
                request.Passport?.UnitCode,
                request.Role,
                request.FullName,
                request.Phone
            );

            if (!userSavingResult.IsSuccessfull)
                return BadRequest(userSavingResult.GetErrors());

            return Ok();
        }

        [HttpGet("search-users")]
        public async Task<ActionResult<List<UserWithEmittersDTO>>> SearchUsersByName(string searchTerm, int page = 1, int pageSize = 20)
        {
            var gettingResult = await usersService
                .SearchUsersByFullName(searchTerm, page, pageSize);

            if (!gettingResult.IsSuccessfull) 
                return BadRequest(gettingResult.GetErrors());

            var result = gettingResult.Value
                .Select(user => new UserWithEmittersDTO(
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.Phone,
                    user.BirthDate,
                    user.Passport,
                    user.Role,
                    user.Emitters
                        .Select(emitter => new EmitterInfoDTO(
                            emitter.Id,
                            emitter.EmitterInfo,
                            emitter.IssuerId))
                        .ToList()))
                .ToList();

            return Ok(result);
        }

        [HttpPost("add-new-role/{userId:guid}")]
        public async Task<ActionResult> AddNewRole(
            Guid userId, 
            Role role, 
            List<Guid>? emittersId, 
            CancellationToken cancellation)
        {
            var result = await userRepository
                .AddRoleToUser(userId, role, emittersId, cancellation);

            if (!result.IsSuccessfull) return BadRequest(result.GetErrors());

            return Ok();
        }

        [HttpDelete("unbind-from-emitter")]
        public async Task<ActionResult> UnbindFromEmitter(
            Guid userId, 
            Guid emitterId, 
            CancellationToken cancellation)
        {
            var result = await userRepository
                .UnbindFromEmitter(userId, emitterId, cancellation);

            if (!result.IsSuccessfull)
                return BadRequest(result.GetErrors());

            return Ok();
        }

        [HttpPost("bind-to-emitters")]
        public async Task<ActionResult> BindToEmitters(
            [FromBody] BindToEmittersDTO request,
            CancellationToken cancellation
            )
        {
            var result = await userRepository
                .BindToEmitters(request.UserId, request.EmittersIdList, cancellation);

            if (!result.IsSuccessfull)
                return BadRequest(result.GetErrors());

           /* var outboxSavingResult = await outboxService.CreateAndSaveOutboxMessage(
                OutboxMessageType.AddUserEmitterBinding,
                JsonSerializer.Serialize(Tuple.Create(request.EmittersIdList, request.UserId)),
                cancellation
                );

            if (!outboxSavingResult.IsSuccessfull)
                return BadRequest(outboxSavingResult.GetErrors());*/

            return Ok();
        }

        /* [HttpPost("bind-to-emitter/{emitterId:guid}")]
         public async Task<ActionResult> BindToEmitterById
             (Guid emitterId, Guid userId, CancellationToken cancellation)
         {
             var result = await emittersRepository.BindUser(emitterId, userId);

             if (!result.IsSuccessfull)
                 return BadRequest(result.GetErrors());

             var outboxSavingResult = await outboxService.CreateAndSaveOutboxMessage(
                 OutboxMessageType.AddUserEmitterBinding,
                 JsonSerializer.Serialize(Tuple.Create(emitterId, userId)),
                 cancellation
                 );

             if (!outboxSavingResult.IsSuccessfull) 
                 return BadRequest(outboxSavingResult.GetErrors());

             return Ok();
         }
 */
        /*[HttpPost("register-new-emitter")]
        public async Task<ActionResult> RegisterEmitter([FromBody] EmitterInfo emitterInfo, 
            CancellationToken cancellation)
        {
            var emitter = Emitter.Create(emitterInfo);

            if (!emitter.IsSuccessfull) return BadRequest();

            await emittersRepository.AddAsync(emitter.Value, cancellation);
            await emittersRepository.UnitOfWork.SaveChangesAsync(cancellation);

            return Ok();
        }*/
        [Permission(Permission.ProfileActions)]
        [HttpPut("update")]
        public async Task<ActionResult> Update(
            [FromBody] UserDataDTO request)
        {
            var result = await usersService.UpdateUser(
                request.Id,
                request.BirthDate,
                request.Passport.DateOfIssuer,
                $"{request.Surname} {request.Name} {request.Patronymic}",
                request.Email,
                request.Phone,
                request.Passport.Series,
                request.Passport.Number,
                request.Passport.Issuer,
                request.Passport.UnitCode
            );

            if (!result.IsSuccessfull) 
                return BadRequest(result.GetErrors());

            return Ok();
        }

        [HttpGet("get-projections")]
        public async Task<ActionResult> GetEmittersProjections()
        {
            var projections = await emittersRepository.GetProjections();

            if (projections.IsSuccessfull)
            {
                return Ok(projections);
            }

            return BadRequest(projections.GetErrors());
        }

        [HttpPost("verify-code")]
        public async Task<ActionResult> VerifyCode([FromBody] VerifyConfirmationCodeQuery request)
        {
            request.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            // Ещё можно как-то запускать таймер(например, 5 минут, и через это время удалять код)
            var verificationResult = await mediator.Send(request);

            if (!verificationResult.IsSuccessfull)
                return BadRequest(verificationResult.GetErrors());

            var token = verificationResult.Value;

            HttpContext.Response.Cookies.Append("tasty-cookies", token);

            return Ok();
        }

        [Permission(Permission.ProfileActions)]
        [HttpPost("restore-password")]
        public async Task<ActionResult> RestorePassword([FromBody] RestorePasswordCommand request)
        {
            var restoreResult = await mediator.Send(request);

            if (!restoreResult.IsSuccessfull)
                return BadRequest(restoreResult.GetErrors());

            return Ok();
        }

       /* [HttpPost("bind-to-emitter/{emitterId:guid}")]
        public async Task<ActionResult> BindToEmitterById(Guid emitterId, Guid userId)
        {
            var result = await emittersRepository.BindUser(emitterId, userId);

            if (!result.IsSuccessfull) 
                return BadRequest(result.GetErrors());

            return Ok();
        }*/
        /*[HttpPost("register-and-bind-to-emitter/{emitterId:guid}")]
        public async Task<ActionResult> RegisterAndBindToEmitter(Guid emitterId, 
            [FromBody] RegisterUserCommand request)
        {
            var passwordHash = passwordHasher.Generate(request.Password);

            var userCreateResult = Core.Domain.Models.Postgres.User
                .Create(request.Email, passwordHash);

            if (!userCreateResult.IsSuccessfull)
                return BadRequest(userCreateResult.GetErrors());

            await userRepository.AddAsync(userCreateResult.Value, default);
            await userRepository.UnitOfWork.SaveChangesAsync(default);

            var result = await emittersRepository.BindUser(emitterId, userCreateResult.Value.Id);

            if (!result.IsSuccessfull)
                return BadRequest(result.GetErrors());

            return Ok();
        }*/

       /* [HttpPost("bind-to-registrator/{registratorId:guid}")]
        public async Task<ActionResult> BindToRegistratorById(Guid registratorId, Guid userId)
        {
            var result = await registratorRepository.BindUser(registratorId, userId);

            if (!result.IsSuccessfull)
                return BadRequest(result.GetErrors());

            return Ok();
        }*/

        [HttpGet("search-emitters")]
        public async Task<ActionResult<List<EmitterInfoDTO>>> SearchEmitters(string searchTerm, int page = 1, int pageSize = 20)
        {
            var result = await emittersRepository.SearchEmitter(searchTerm, page, pageSize);

            if (result is null) return BadRequest();

            var response = result
                .Select(t => new EmitterInfoDTO(t.Item1, t.Item2, t.Item3))
                .ToList();

            return Ok(response);
        }

        //[Authorize]
        /*[Permission(Permission.ChoiceOfEmitters)]
        [HttpGet("get-emitters")]
        public async Task<ActionResult<List<EmitterInfoDTO>>> GetAllUserEmitters()
        {
            var userId = HttpContext.User.FindFirst(CustomClaims.UserId).Value;

            if (userId == null) return BadRequest("user id can not be null");

            Guid.TryParse(userId, out Guid userGuid);

            var result = await emittersRepository.GetAllByUserId(userGuid);

            if (!result.IsSuccessfull)
                return BadRequest(result.GetErrors());

            var response = result.Value
                .Select(e => new EmitterInfoDTO(e.Item1, e.Item2, e.Item3))
                .ToList();

            return Ok(response);
        }*/

        [Permission(Permission.ChoiceOfEmitters)]
        [HttpGet("get-binding-emitters")]
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
        /*public async Task<ActionResult> GetUserInfo()
        {// Получается вся информация о пользователе
            // ФИО, др, паспортные данные
            return await Task.FromResult(Ok());
        }

        public async Task<ActionResult> UpdateUserInfo()
        {// Обновляет данные о пользователе в нашей БД
         // и в БД регистратора
            return await Task.FromResult(Ok());
        }*/

    }

    public class UserNonAuthentificatedError : Error
    {
        public override string Type => nameof(UserNonAuthentificatedError);
    }
}
