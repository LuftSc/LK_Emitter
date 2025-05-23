using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit
{
    public class SendDocumentEvent
    {
        public required List<DocumentInfo> Documents { get; set; } = [];
        public required Guid SenderId { get; set; }
        public required Role Role { get; set; }
        public required int IssuerId { get; set; }
        public required bool WithDigitalSignature { get; set; } = false;
    }
}
