using AuthService.Application.Features.Email;
using AuthService.Application.Features.Users;
using AuthService.Authentification;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Cryptography;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ISender sender;
        private readonly IDistributedCache cache;
        private readonly IJwtProvider jwtProvider;

        public UsersController(IMediator mediator, ISender sender, 
            IDistributedCache cache, IJwtProvider jwtProvider)
        {
            this.mediator = mediator;
            this.sender = sender;
            this.cache = cache;
            this.jwtProvider = jwtProvider;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Login([FromBody] LoginUserQuery request)
        {
            var loginResult = await mediator.Send(request);

            if (!loginResult.IsSuccessfull)
            {
                return BadRequest(loginResult.GetErrors());
            }

            await cache.SetStringAsync($"id-{request.Email}", loginResult.Value);

            var sendEmailCommand = new SendConfirmationEmailCommand() { Recipient = request.Email };

            var emailSendResult = await mediator.Send(sendEmailCommand);

            if (!emailSendResult.IsSuccessfull) 
            {
                return BadRequest(emailSendResult.GetErrors());
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> VerifyCode([FromBody] VerifyCodeDTO request)
        {// Ещё можно как-то запускать таймер(например, 5 минут, и через это время удалять код)
            var savedCode = await cache.GetStringAsync(request.Email);
            if (savedCode != request.Code)
            {
                return BadRequest("Code is wrong!");
            }

            var userId = await cache.GetStringAsync($"id-{request.Email}");
            if (userId is null)
            {
                return BadRequest("The auth confirmation code has expired!");
            }

            var token = jwtProvider.GenerateToken(userId);
            HttpContext.Response.Cookies.Append("tasty-cookies", token);

            await cache.RemoveAsync(request.Email);
            await cache.RemoveAsync($"id-{request.Email}");

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register([FromBody] RegisterUserCommand request)
        {
            var result = await mediator.Send(request);

            if (!result.IsSuccessfull)
            {
                return BadRequest(result.GetErrors());
            }

            return Ok();
        }
    }

    public record VerifyCodeDTO(string Email, string Code) { }
}
