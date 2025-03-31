//using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.API.Contracts;
using EmitterPersonalAccount.Application.Features.OrderReports;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using ExternalOrderReportsService.Contracts;
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
        private readonly IOrderReportsRepository orderReportsRepository;

        public OrderReportsController(IMediator mediator, 
            IOrderReportsRepository orderReportsRepository)
        {
            this.mediator = mediator;
            this.orderReportsRepository = orderReportsRepository;
        }

        [Authorize]
        [HttpGet("get-report-orders/{emitterId:guid}")]
        public async Task<ActionResult<List<OrderReportDTO>>> GetReportOrders(Guid emitterId)
        {
            var result = await orderReportsRepository.GetAllByEmitterId(emitterId);

            if (!result.IsSuccessfull) 
                return BadRequest(result.GetErrors());

            var response = result.Value.Select(o =>
            new OrderReportDTO(o.Id, o.FileName, o.Status.ToString(), o.RequestDate, o.ExternalStorageId))
                .ToList();

            return Ok(response);  
        }

        [Authorize]
        [HttpPost("list-of-shareholders")]
        public async Task<ActionResult> RequestListOfShareholdersReport(
            [FromBody] RequestListOfShareholdersCommand request)
        {
            if (request == null) return BadRequest("Request body can not be null!");

            var userId = HttpContext.User.FindFirst(CustomClaims.UserId).Value;

            request.UserId = userId;

            var deliveryResult = await mediator.Send(request);

            if (!deliveryResult.IsSuccessfull) 
                return BadRequest(deliveryResult.GetErrors());

            return Ok();
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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
