//using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.API.Contracts;
using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Application.Features.OrderReports;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace EmitterPersonalAccount.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderReportsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IRabbitMqPublisher publisher;
        private readonly IRpcClient rpcClient;

        public OrderReportsController(IMediator mediator, 
            IRabbitMqPublisher publisher, IRpcClient rpcClient)
        {
            this.mediator = mediator;
            this.publisher = publisher;
            this.rpcClient = rpcClient;
        }

       /* [Authorize]
        [HttpGet("get-all-report-orders/{emitterId:guid}")]
        public async Task<ActionResult<List<OrderReportDTO>>> GetReportOrders(Guid emitterId)
        {
            var result = await orderReportsRepository.GetAllByEmitterId(emitterId);

            if (!result.IsSuccessfull) 
                return BadRequest(result.GetErrors());

            var response = result.Value.Select(o =>
            new OrderReportDTO(o.Id, o.FileName, o.Status.ToString(), o.RequestDate, o.ExternalStorageId))
                .ToList();

            return Ok(response);  
        }*/

        [Permission(Permission.OrderReportsActions)]
        [HttpGet("get-report-orders/{issuerId:int}/")]
        [SwaggerOperation(Summary = "Получить список запрошенных отчётов",
            Description = "Возвращает список отчётов выбранного эмитента")]
        public async Task<ActionResult<OrderReportPaginationList>> GetReportOrdersByPage
            (int issuerId, [FromQuery] PaginationInfo pagination)
        {
            var userId = HttpContext.User.FindFirst(CustomClaims.UserId).Value;
            
            var getReportsEvent = new GetOrderReportsEvent()
            {
                IssuerId = issuerId,
                UserId = userId,
                Page = pagination.Page,
                PageSize = pagination.PageSize
            };

            var result = await rpcClient
                .CallAsync<OrderReportPaginationList>
                    (JsonSerializer.Serialize(getReportsEvent),
                    RabbitMqAction.GetOrderReports,
                    default);

            if (!result.IsSuccessfull)
                return BadRequest(result.GetErrors());

            return Ok(result.Value);
        }

        [Permission(Permission.OrderReportsActions)]
        [HttpPost("list-of-shareholders")]
        [SwaggerOperation(Summary = "Запросить список участников собрания акционером",
            Description = "Отправляет на генерацию отчёт типа [список участников собрания акционеров]")]
        public async Task<ActionResult> RequestListOfShareholdersReport(
            [FromBody] RequestListOfShareholdersCommand request)
        {
            Console.WriteLine("Запрос пошёл");
            if (request == null) return BadRequest("Request body can not be null!");

            var userId = HttpContext.User.FindFirst(CustomClaims.UserId).Value;

            request.UserId = userId;

            var deliveryResult = await mediator.Send(request);

            if (!deliveryResult.IsSuccessfull) 
                return BadRequest(deliveryResult.GetErrors());

            return Ok();
        }

        [Permission(Permission.OrderReportsActions)]
        [HttpPost("ree-rep")]
        [SwaggerOperation(Summary = "Запросить информацию из реестра",
            Description = "Отправляет на генерацию отчёт типа [запрос информации из реестра]")]
        public async Task<ActionResult> RequestReeRepReport(
            [FromBody] RequestReeRepCommand request)
        {
            if (request == null) return BadRequest("Request body can not be null!");

            var userId = HttpContext.User.FindFirst(CustomClaims.UserId).Value;

            request.UserId = userId;

            var deliveryResult = await mediator.Send(request);

            if (!deliveryResult.IsSuccessfull)
                return BadRequest(deliveryResult.GetErrors());

            return Ok();
        }

        [Permission(Permission.OrderReportsActions)]
        [HttpPost("dividend-list")]
        [SwaggerOperation(Summary = "Запросить дивидендный список",
            Description = "Отправляет на генерацию отчёт типа [дивидендный список]")]
        public async Task<ActionResult> RequestDividendListReport(
            [FromBody] RequestDividendListCommand request)
        {
            if (request == null) return BadRequest("Request body can not be null!");

            var userId = HttpContext.User.FindFirst(CustomClaims.UserId).Value;

            request.UserId = userId;

            var deliveryResult = await mediator.Send(request);

            if (!deliveryResult.IsSuccessfull)
                return BadRequest(deliveryResult.GetErrors());

            return Ok();
        }

        [Permission(Permission.OrderReportsActions)]
        [HttpGet("download-report-order/{reportOrderId:guid}")]
        [SwaggerOperation(Summary = "Скачать отчёт",
            Description = "Загружает выбранный отчёт")]
        public async Task<ActionResult> DownloadReportOrder(Guid reportOrderId, ReportType type)
        {
            if (reportOrderId == Guid.Empty)
                return BadRequest("Document id can not be empty!");

            var userIdGettingResult = ClaimService.Get(HttpContext, CustomClaims.UserId);

            if (!userIdGettingResult.IsSuccessfull)
                return BadRequest(userIdGettingResult.GetErrors());

            var downloadDocumentQuery = new DownloadReportOrderQuery()
            {
                UserId = Guid.Parse(userIdGettingResult.Value),
                ReportOrderId = reportOrderId,
                ReportType = type
            };

            var result = await mediator.Send(downloadDocumentQuery);

            if (!result.IsSuccessfull)
                return BadRequest(result.GetErrors());

            return File(
                result.Value.Content, 
                result.Value.ContentType, 
                result.Value.FileName);
        }
    }
}
