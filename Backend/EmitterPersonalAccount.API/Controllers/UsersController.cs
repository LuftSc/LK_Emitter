using AuthService.Application.Features.Users;
using AuthService.Controllers;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmitterPersonalAccount.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly HttpClient httpClient;

        public UsersController(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            //this.httpClientFactory = httpClientFactory;
        }

        //[Authorize]
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
            //var client = httpClientFactory.CreateClient();
            var url = "https://localhost:7034/Users/Login";

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode) return BadRequest();

            // Логирование всех клэймов
            foreach (var claim in HttpContext.User.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            return Ok();
        }

        [HttpPost("login-user-without-2fa")]
        public async Task<ActionResult<string>> LoginWithout2FA([FromBody] LoginUserQuery request)
        {
            var url = "https://localhost:7034/Users/LoginWithout2FA";

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode) return BadRequest();

            var token = await response.Content.ReadAsStringAsync();

            return Ok(JsonConvert.SerializeObject(token));
        }

        [HttpPost("register-new-user")]
        public async Task<ActionResult> Register([FromBody] RegisterUserCommand request)
        {
            var url = "https://localhost:7034/Users/Register";

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode) return BadRequest();

            return Ok();
        }

        [HttpPost("verify-code")]
        public async Task<ActionResult> VerifyCode([FromBody] VerifyCodeDTO request)
        {
            var url = "https://localhost:7034/Users/VerifyCode";

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode) return BadRequest();

            return Ok();
        }

        [HttpPost("restore-password/{userId:guid}")]
        public async Task<ActionResult> RestorePassword(Guid userId, [FromBody] string newPassword)
        {
            var url = "https://localhost:7034/Users/RestorePassword";

            var request = new RestorePasswordCommand() 
                { UserId = userId, NewPassword = newPassword };

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode) return BadRequest();

            return Ok();
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
