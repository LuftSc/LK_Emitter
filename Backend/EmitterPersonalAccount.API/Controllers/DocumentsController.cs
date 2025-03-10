using EmitterPersonalAccount.API.Swagger;
using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmitterPersonalAccount.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public DocumentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [FileUploadOperation.FileContentType]
        [HttpPost("send-documents")]
        public async Task<ActionResult> SendDocument(SendDocumentsCommand request)
        {// Сохраняет документ в БД и отправляет его получателю
            // ФЛАГ: С подписью\ без подписи
            if (request.Files.Count == 0) return BadRequest("List files is empty!");

            var result = await mediator.Send(request);

            if (!result.IsSuccessfull) return BadRequest(result.GetErrors());

            return Ok();
        }
       /* public async Task<ActionResult> VerifyDocumentSignature()
        {// Принимает документ и считает его хэш-сумму
            // а потом сравнивает с той, которая есть в свойствах
            return await Task.FromResult(Ok());
        }
        public async Task<ActionResult> GetDocuments()
        {// Достаёт документы из БД и возвращает их на фронтенд
            return await Task.FromResult(Ok());
        }
        public async Task<ActionResult> DeleteDocument()
        {// Удаляет документ
            return await Task.FromResult(Ok());
        }*/
    }
}
