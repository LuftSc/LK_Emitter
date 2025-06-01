using DocumentFormat.OpenXml.Bibliography;
using EmitterPersonalAccount.API.Contracts;
using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Application.Infrastructure.Rpc;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Logs;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace EmitterPersonalAccount.API.Controllers
{
    [ApiController]
    [Route("usersActions")]
    public class UsersActionsController : ControllerBase
    {
        private readonly IRabbitMqPublisher publisher;
        private readonly IUserExitService userExitService;
        private readonly IRpcClient rpcClient;

        public UsersActionsController(
            IRabbitMqPublisher publisher, 
            IUserExitService userExitService,
            IRpcClient rpcClient)
        {
            this.publisher = publisher;
            this.userExitService = userExitService;
            this.rpcClient = rpcClient;
        }
        [Permission(Permission.ProfileActions)]
        [HttpPost("logged")]
        [SwaggerOperation(Summary = "Сообщить системе о входе пользователя",
           Description = "Записывает в логи событие [вход пользователя]")]
        public async Task<ActionResult> Logged(CancellationToken cancellation)
        {
            var userId = ClaimService.Get(HttpContext, CustomClaims.UserId);

            if (!userId.IsSuccessfull)
                return BadRequest(userId.GetErrors());

            var ev = new UserActionLogEvent(
                Guid.Parse(userId.Value),
                ActionLogType.LoginToSystem.Type,
                DateTime.Now.ToUniversalTime().AddHours(5));

            var deliveryResult = await publisher
                .SendMessageAsync(
                    JsonSerializer.Serialize(ev),
                    RabbitMqAction.WriteUsersLogs,
                    cancellation);

            if (!deliveryResult) return BadRequest();

            return Ok();
        }

        [Permission(Permission.ProfileActions)]
        [HttpPost("logout")]
        [SwaggerOperation(Summary = "Сообщить системе о выходе пользователя",
            Description = "Записывает в логи событие [выход пользователя]")]
        public ActionResult Logout(
            CancellationToken cancellation)
        {
            var userId = ClaimService.Get(HttpContext, CustomClaims.UserId);

            if (!userId.IsSuccessfull)
                return BadRequest(userId.GetErrors());

            var logoutResult = userExitService
                .OnLogout(Guid.Parse(userId.Value), cancellation);

            if (!logoutResult.IsSuccessfull)
                return BadRequest(logoutResult.GetErrors());

            return Ok();
        }

        [HttpPost("report")]
        [SwaggerOperation(Summary = "Отправить запрос за сбор действий пользователя",
            Description = "Начинает собирать действия пользователей")]
        public async Task<ActionResult> GenerateActionsReport(
            [FromBody] GenerateActionsReportFilters filters,
            CancellationToken cancellationToken)
        {
            var userIdGettingResult = ClaimService.Get(HttpContext, CustomClaims.UserId);

            if (!userIdGettingResult.IsSuccessfull)
                return BadRequest(userIdGettingResult.GetErrors());

            var getLogsEvent = new GetUsersLogsEvent(
                userIdGettingResult.Value,
                filters.UserId,
                filters.StartDate,
                filters.EndDate
            );

            var message = JsonSerializer.Serialize(getLogsEvent);

            var deliveryResult = await publisher
                .SendMessageAsync(message, RabbitMqAction.CollectUserLogs, cancellationToken);
            Console.WriteLine("Отправили запрос в рэббит на генерацию отчёта");

            if (!deliveryResult)
                return BadRequest("Ошибка при доставке сообщения в RabbitMQ");

            return Ok();
        }

        [HttpGet("report")]
        [SwaggerOperation(Summary = "Получить список сделанных ранее выгрузок",
            Description = "Возвращает список Excel-файлов-отчётов, собранных ранее")]
        public async Task<ActionResult<List<ActionsReportDTO>>> GetListActionsReports(CancellationToken cancellationToken)
        {
            var ev = JsonSerializer.Serialize(DateTime.Now);

            var actionsReportsGettingResult = await rpcClient
                .CallAsync<List<ActionsReportDTO>>(
                    ev,
                    RabbitMqAction.GetActionsReports,
                    cancellationToken);

            Console.WriteLine("Отправили запрос в рэббит на получение списка отчётов");

            if (!actionsReportsGettingResult.IsSuccessfull)
                return BadRequest(actionsReportsGettingResult.GetErrors());

            return Ok(actionsReportsGettingResult.Value);
        }

        [HttpGet("report/{reportId:guid}")]
        [SwaggerOperation(Summary = "Скачать выбранный отчёт по действиям пользователей",
            Description = "Загружает отчёт в виде Excel-файла")]
        public async Task<ActionResult> DownloadActionReport(Guid reportId, CancellationToken cancellation)
        {
            var message = JsonSerializer.Serialize(reportId);

            var actionsReportDownloadResult = await rpcClient
                .CallAsync<DocumentInfo>(
                    message,
                    RabbitMqAction.DownloadActionsReport,
                    cancellation);

            if (!actionsReportDownloadResult.IsSuccessfull)
                return BadRequest(actionsReportDownloadResult.GetErrors());

            var file = actionsReportDownloadResult.Value;

            return File(file.Content, file.ContentType, file.FileName);
        }
    }
}
