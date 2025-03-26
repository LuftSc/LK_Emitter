using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
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
        Task<Result> AddRangeByEmitterId(Guid senderId, Guid emitterId,
            List<DocumentInfo> documentsInfo, CancellationToken cancellationToken,
            bool withDigitalSignature = false);
        Task<Result<List<Document>>> GetByUserId(Guid userId);
        Task<Result<List<Document>>> GetByEmitterId(Guid emitterId);
        Task<Result> DeleteByIdAsync(Guid documentId, CancellationToken cancellationToken);
    }
}
