using EmitterPersonalAccount.API.Contracts;
using EmitterPersonalAccount.API.Swagger;
using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        [Permission(Permission.DocumentsActions)]
        [FileUploadOperation.FileContentType]
        [HttpPost("send-documents")]
        [SwaggerOperation(Summary = "Отправить документы",
            Description = "Отправляет документы в хранилище выбранного эмитента")]
        public async Task<ActionResult> SendDocument([FromForm] SendDocumentsCommand request)
        {// Сохраняет документ в БД и отправляет его получателю
            // ФЛАГ: С подписью\ без подписи
            if (request == null || request.Files.Count == 0) 
                return BadRequest("List files null or empty!");
            
            var userIdGettingResult = ClaimService.Get(HttpContext, CustomClaims.UserId);
            var roleGettingResult = ClaimService.Get(HttpContext, CustomClaims.Role);

            if (!userIdGettingResult.IsSuccessfull 
                || !roleGettingResult.IsSuccessfull)
                return BadRequest
                    (Result.Error(new InvdlidJWTTokenError()));

            var userId = Guid.Parse(userIdGettingResult.Value);
            var role = (Role)Enum.Parse(typeof(Role), roleGettingResult.Value);

            request.SenderId = userId;
            request.Role = role;

            var result = await mediator.Send(request);

            if (!result.IsSuccessfull) return BadRequest(result.GetErrors());

            return Ok();
        }

        [Permission(Permission.DocumentsActions)]
        [HttpGet("get-documents-info/{issuerId:int}")]
        [SwaggerOperation(Summary = "Получить список документов по эмитенту",
            Description = "Возвращает список документов в рамках выбранного эмитента")]
        public async Task<ActionResult<DocumentPaginationList>> GetDocumentsByPage
            (int issuerId, [FromQuery] PaginationInfo pagination)
        {
            var getDocsInfoQuery = new GetDocumentsInfoQuery() 
            { IssuerId = issuerId, Page = pagination.Page, PageSize = pagination.PageSize };

            var result = await mediator.Send(getDocsInfoQuery);

            if (!result.IsSuccessfull)
                return BadRequest(result.GetErrors());

            return Ok(result.Value);
        }

        [Permission(Permission.DocumentsActions)]
        [HttpGet("download/{documentId:guid}")]
        [SwaggerOperation(Summary = "Скачать документ",
            Description = "Загружает выбранный документ")]
        public async Task<ActionResult> GetDownloadLink(Guid documentId)
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

        [Permission(Permission.DocumentsActions)]
        [HttpDelete("delete-document/{documentId:guid}")]
        [SwaggerOperation(Summary = "Удалить документ",
            Description = "Удаляет выбранный документ")]
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

    public class InvdlidJWTTokenError : Error
    {
        public override string Type => nameof(InvdlidJWTTokenError);
    }
}
