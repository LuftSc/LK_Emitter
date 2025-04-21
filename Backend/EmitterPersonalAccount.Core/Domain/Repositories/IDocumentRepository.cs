using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Repositories
{
    public interface IDocumentRepository : IRepository<Document>
    {
        Task<Result<List<DocumentDTO>>> CreateDocumentsAsync(Guid senderId, int issuerId,
            List<DocumentInfo> documentsInfo, CancellationToken cancellationToken,
            bool withDigitalSignature = false);
        Task<Result<List<Document>>> GetByUserId(Guid userId);
        Task<Result<List<Document>>> GetByEmitterId(int issuerId);
        Task<Result<Tuple<int, List<Document>>>> GetByPage
            (int issuerId, int page, int pageSize);
        Task<Result> DeleteByIdAsync(Guid documentId, CancellationToken cancellationToken);
    }
}
