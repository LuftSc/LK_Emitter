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
            if (request == null || request.Files.Count == 0) 
                return BadRequest("List files null or empty!");

            var result = await mediator.Send(request);

            if (!result.IsSuccessfull) return BadRequest(result.GetErrors());

            return Ok();
        }

        [HttpGet("get-documents-info/{userId:guid}")]
        public async Task<ActionResult<List<DocumentInfoResponse>>> 
            GetDocumentsInfo(Guid userId)
        {// Достаёт информацию по документам из БД
            if (userId == Guid.Empty)
                return BadRequest("User Id can not be empty or null!");

            var getDocsInfoQuery = new GetDocumentsInfoQuery() { UserId = userId };

            var result = await mediator.Send(getDocsInfoQuery);

            if (!result.IsSuccessfull)
                return BadRequest(result.GetErrors());

            return Ok(result.Value);    
        }

        [HttpGet("download/{documentId:guid}")]
        public async Task<ActionResult> Download(Guid documentId)
        {
            if (documentId == Guid.Empty)
                return BadRequest("Document id can not be empty!");

            var downloadDocumentQuery = 
                new DownloadDocumnetQuery() { DocumentId = documentId };

            var result = await mediator.Send(downloadDocumentQuery);

            if (!result.IsSuccessfull)
                return BadRequest(result.GetErrors());

            return File(result.Value.Content, 
                result.Value.ContentType, 
                result.Value.FileName);
        }
        /* public async Task<ActionResult> VerifyDocumentSignature()
         {// Принимает документ и считает его хэш-сумму
             // а потом сравнивает с той, которая есть в свойствах
             return await Task.FromResult(Ok());
         }

         public async Task<ActionResult> DeleteDocument()
         {// Удаляет документ
             return await Task.FromResult(Ok());
         }*/

        [HttpDelete("delete-document/{documentId:guid}")]
        public async Task<ActionResult> DeleteDocument(Guid documentId)
        {// Удаляет документ
            if (documentId == Guid.Empty)
                return BadRequest("Document id can not be empty!");

            var deleteDocumentCommand = new DeleteDocumentCommand() 
                { DocumentId = documentId };

            var result = await mediator.Send(deleteDocumentCommand);

            if (!result.IsSuccessfull)
                return BadRequest(result?.GetErrors());

            return Ok();
        }
    }
}
