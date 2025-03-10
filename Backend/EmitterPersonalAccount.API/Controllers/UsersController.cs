using AuthService.Application.Features.Users;
using AuthService.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

            return Ok();
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
        /*public async Task<ActionResult> RestorePassword()
        {// Вводят логин + новый пароль =>
         // Вместо старого пароля у пользователя записываем новый
            return await Task.FromResult(Ok());
        }
        public async Task<ActionResult> GetUserInfo()
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
