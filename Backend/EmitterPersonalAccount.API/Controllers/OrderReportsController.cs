//using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.API.Contracts;
using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Application.Features.OrderReports;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult> GetReportOrdersByPage
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
        public async Task<ActionResult> DownloadReportOrder(Guid reportOrderId)
        {
            if (reportOrderId == Guid.Empty)
                return BadRequest("Document id can not be empty!");

            var downloadDocumentQuery = new DownloadReportOrderQuery() 
                {  ReportOrderId = reportOrderId, };

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
