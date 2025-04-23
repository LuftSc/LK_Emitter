//using AuthService.Application.Features.Users;
//using AuthService.Controllers;
using EmitterPersonalAccount.API.Contracts;
using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
//using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public UsersController(IMediator mediator, 
            IDistributedCache distributedCache, 
            IJwtProvider jwtProvider,
            IEmittersRepository emittersRepository, 
            IRegistratorRepository registratorRepository, 
            IUserRepository userRepository, 
            IPasswordHasher passwordHasher, 
            IRabbitMqPublisher publisher)
        {
            this.mediator = mediator;
            this.distributedCache = distributedCache;
            this.jwtProvider = jwtProvider;
            this.emittersRepository = emittersRepository;
            this.registratorRepository = registratorRepository;
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.publisher = publisher;
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

            var token = jwtProvider.GenerateToken(loginResult.Value);

            HttpContext.Response.Cookies.Append("tasty-cookies", token);

            return Ok();
        }

        [HttpPost("register-new-user")]
        public async Task<ActionResult> Register([FromBody] RegisterUserCommand request)
        {
            if (string.IsNullOrEmpty(request.Email)) 
                return BadRequest("Email can not be empty or null!");
            if (string.IsNullOrEmpty(request.Password))
                return BadRequest("Password can not be empty or null!");

            var result = await mediator.Send(request);

            if (!result.IsSuccessfull) return BadRequest(result.GetErrors());

            return Ok();
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

        [HttpPost("restore-password")]
        public async Task<ActionResult> RestorePassword([FromBody] RestorePasswordCommand request)
        {
            var restoreResult = await mediator.Send(request);

            if (!restoreResult.IsSuccessfull)
                return BadRequest(restoreResult.GetErrors());

            return Ok();
        }

        [HttpPost("bind-to-emitter/{emitterId:guid}")]
        public async Task<ActionResult> BindToEmitterById(Guid emitterId, Guid userId)
        {
            var result = await emittersRepository.BindUser(emitterId, userId);

            if (!result.IsSuccessfull) 
                return BadRequest(result.GetErrors());

            return Ok();
        }
        [HttpPost("register-and-bind-to-emitter/{emitterId:guid}")]
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
        }

        [HttpPost("bind-to-registrator/{registratorId:guid}")]
        public async Task<ActionResult> BindToRegistratorById(Guid registratorId, Guid userId)
        {
            var result = await registratorRepository.BindUser(registratorId, userId);

            if (!result.IsSuccessfull)
                return BadRequest(result.GetErrors());

            return Ok();
        }

        [Authorize]
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
