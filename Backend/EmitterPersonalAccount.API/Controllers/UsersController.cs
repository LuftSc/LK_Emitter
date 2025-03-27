//using AuthService.Application.Features.Users;
//using AuthService.Controllers;
using EmitterPersonalAccount.API.Contracts;
using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public UsersController(IMediator mediator, 
            IDistributedCache distributedCache, 
            IJwtProvider jwtProvider,
            IEmittersRepository emittersRepository)
        {
            this.mediator = mediator;
            this.distributedCache = distributedCache;
            this.jwtProvider = jwtProvider;
            this.emittersRepository = emittersRepository;
        }

        [Authorize]
        [HttpGet("get-current-user")]
        public async Task<ActionResult<Guid>> GetCurrentUserId()
        {
            var userId = HttpContext.User.FindFirst(CustomClaims.UserId).Value;

            if (userId == null) return BadRequest("user id can not be null");
            
            Guid.TryParse(userId, out Guid userGuid);

            return Ok(JsonConvert.SerializeObject(userGuid));
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
                .Select(e => new EmitterInfoDTO(e.Item1, e.Item2))
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
}
