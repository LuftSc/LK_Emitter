using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsService.DataAccess.Repositories
{
    public sealed class DocumentsRepository 
        : EFRepository<Document, DocumentsDbContext>, 
        IDocumentRepository
    {
        private readonly DocumentsDbContext context;
        private readonly IHashService hashService;

        public DocumentsRepository(DocumentsDbContext context, 
            IHashService hashService) : base(context) 
        {
            this.context = context;
            this.hashService = hashService;
        }
        public async Task<Result<List<DocumentDTO>>> CreateDocumentsAsync(Guid senderId, int issuerId, 
            List<DocumentInfo> documentsInfo, CancellationToken cancellationToken,
            bool withDigitalSignature = false)
        {
            var sender = await context.Users
                .Include(u => u.Registrator)
                .FirstOrDefaultAsync(u => u.Id == senderId);

            if (sender is null)
                return Result<List<DocumentDTO>>.Error(new DocumentSenderNotFoundError());

            var documents = new List<Document>(documentsInfo.Count);

            var documentsResults = documentsInfo
                .Select(d => Document.Create(sender, d.FileName,
                    DateTime.Now.ToUniversalTime().AddHours(5), d.Content,
                    withDigitalSignature
                        ? hashService.ComputeHash(d.Content)
                        : string.Empty, 
                    issuerId
                ));

            foreach (var documentResult in documentsResults)
            {
                if (documentResult.IsSuccessfull)
                    documents.Add(documentResult.Value);
                else return Result<List<DocumentDTO>>.Error(new DocumentCreatingError());
            }
            await AddRangeAsync(documents, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            var docsDTO = documents.Select(d => new DocumentDTO
                (d.Id, d.Title, d.Type, d.UploadDate, d.GetSize(), d.IsEmitterSended))
                .ToList();

            return Result<List<DocumentDTO>>.Success(docsDTO);    
        }
        public async Task<Result<Tuple<int, List<Document>>>> GetByPage
            (int issuerId, int page, int pageSize)
        {
            var query = context.Documents
                .AsNoTracking()
                .AsQueryable()
                .Where(o => o.IssuerId == issuerId)
                .OrderByDescending(o => o.UploadDate);

            var totalSize = await query.CountAsync();

            var documents = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Result<Tuple<int, List<Document>>>
                .Success(Tuple.Create(totalSize, documents));
        }

        public async Task<Result<List<Document>>> GetByUserId(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);

            if (user is null)
                return Result<List<Document>>
                    .Error(new UserNotFoundError());

            var documents = await context.Documents
                .Where(d => d.User == user)
                .ToListAsync();

            if (documents == null)
                return Result<List<Document>>
                    .Error(new EmitterNotFoundError());

            return Result<List<Document>>.Success(documents);
        }
        public async Task<Result<List<Document>>> GetByEmitterId(int issuerId)
        {
            /*var emitter = await context.Emitters
                .Include(e => e.Documents)
                .FirstOrDefaultAsync(e => e.Id == emitterId);*/

            /* if (emitter is null)
                 return Result<List<Document>>
                     .Error(new EmitterNotFoundError());*/
            var documents = await context.Documents
                .AsNoTracking()
                .Where(d => d.IssuerId == issuerId)
                .ToListAsync();

            return Result<List<Document>>.Success(documents);
        }

        public async Task<Result> DeleteByIdAsync(Guid documentId, CancellationToken cancellationToken)
        {
            await context.Documents
                .Where(d => d.Id == documentId)
                .ExecuteDeleteAsync(cancellationToken);

            return Result.Success();
        }
    }
    public class RecipientNotFoundError : Error
    {
        public override string Type => nameof(RecipientNotFoundError);
    }
    public class DocumentCreatingError : Error
    {
        public override string Type => nameof(DocumentCreatingError);
    }
    public class EmitterNotFoundError : Error
    {
        public override string Type => nameof(EmitterNotFoundError);
    }
    public class DocumentSenderNotFoundError : Error
    {
        public override string Type => nameof(DocumentSenderNotFoundError);
    }
    public class UserNotFoundError : Error
    {
        public override string Type => nameof(UserNotFoundError);
    }
}
