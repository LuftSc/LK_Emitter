using EmitterPersonalAccount.Core.Domain.SharedKernal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit
{
    public class SendListOSAResultContent
    {
        public required Guid ExternalDocumentId {  get; set; }
        public required DateTime RequestDate { get; set; }
        public required string UserId { get; set; }
        public required Guid DocumentId { get; set; }
        public required CompletionStatus Status { get; set; }
    }
}
