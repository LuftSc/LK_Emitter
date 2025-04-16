using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ResultHubService.Controllers
{
    [ApiController]
    [Route("results")]
    public class ResultsHubController : ControllerBase
    {
        [HttpPost("connect")]
        public IActionResult HandleConnect()
        {
            // Извлекаем JWT из заголовка Authorization
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Декодируем токен и получаем userId
            var userId = GetUserIdFromJwt(jwtToken);
            return Ok();
        }

        [HttpPost("disconnect")]
        public IActionResult HandleDisconnect()
        {
            // Извлекаем JWT из заголовка Authorization
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Декодируем токен и получаем userId
            var userId = GetUserIdFromJwt(jwtToken);
            return Ok();
        }
        private string GetUserIdFromJwt(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            return token.Claims.First(c => c.Type == "userId").Value;
        }
    }
}
