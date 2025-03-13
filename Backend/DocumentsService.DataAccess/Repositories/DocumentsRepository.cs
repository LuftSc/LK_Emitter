using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
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

        public async Task<Result> AddRangeToUserByEmail(string email, 
            List<DocumentInfo> documentsInfo, CancellationToken cancellationToken,
            bool withDigitalSignature = false)
        {
            // Можно заменить на FindAsync, но нужно откуда-то взять Id-шник
            var recipient = await context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (recipient == null) 
                return Result.Error(new RecipientNotFoundError());

            var documents = new List<Document>(documentsInfo.Count);

            var documentsResults = documentsInfo
                .Select(d => Document.Create(recipient, d.FileName, 
                    DateTime.Now.ToUniversalTime(), d.Content,
                    withDigitalSignature
                        ? hashService.ComputeHash(d.Content)
                        : string.Empty
                ));

            foreach (var documentResult in documentsResults)
            {
                if (documentResult.IsSuccessfull)
                    documents.Add(documentResult.Value);
                else return Result.Error(new DocumentCreatingError());
            }

            await context.Documents.AddRangeAsync(documents);

            recipient.Documents.AddRange(documents);

            await context.SaveChangesAsync();

            return Result.Success();
        }
        
        public async Task<Result<List<Document>>> GetByUserId(Guid userId)
        {
            Console.WriteLine("Попали в Docs Repository документов");
            var userWithDocuments = await context.Users
                .AsNoTracking()
                .Include(u => u.Documents)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (userWithDocuments == null)
                return Result<List<Document>>.Error(new UserNotFoundError());

            return Result<List<Document>>.Success(userWithDocuments.Documents);
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
    public class UserNotFoundError : Error
    {
        public override string Type => nameof(UserNotFoundError);
    }
}
