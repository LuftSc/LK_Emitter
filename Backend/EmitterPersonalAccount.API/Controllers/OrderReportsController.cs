using EmitterPersonalAccount.Application.Features.OrderReports;
using MediatR;
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
        [HttpPost("list-of-shareholders")]
        public async Task<ActionResult> RequestListOfShareholdersReport(
            [FromBody] RequestListOfShareholdersCommand request)
        {
            if (request == null) return BadRequest("Request body can not be null!");

            var deliveryResult = await mediator.Send(request);

            if (!deliveryResult.IsSuccessfull) 
                return BadRequest(deliveryResult.GetErrors());

            return Ok();
        }
    }
   /* [ApiController]
    [Route("[controller]")]
    public class DirectivesController : ControllerBase
    {*//*
        public async Task<ActionResult> CreateShareholdersMettingDirective()
        {// Создаёт запрос распоряжения на предоставление информации
         // Эмитенту для общего собрания акционеров
         // асинхронно, через очередь
            return await Task.FromResult(Ok());
        }
        public async Task<ActionResult> CreateRegistryInfoDirective()
        {// Создаёт запрос распоряжения на предоставление информации
         // из реестра
            return await Task.FromResult(Ok());
        }
        public async Task<ActionResult> CreateInvestorListDirective()
        {// Создаёт запрос распоряжения на предоставление Списка лиц
         // имеющих право на получение доходов по ценным бумагам
            return await Task.FromResult(Ok());
        }*//*
    }*/
}
