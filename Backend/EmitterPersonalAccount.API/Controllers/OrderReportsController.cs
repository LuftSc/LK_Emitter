//using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.Application.Features.OrderReports;
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

        public OrderReportsController(IMediator mediator)
        {
            this.mediator = mediator;
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
