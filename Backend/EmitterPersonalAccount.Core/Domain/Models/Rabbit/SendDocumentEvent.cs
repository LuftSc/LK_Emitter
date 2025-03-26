using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit
{
    public class SendDocumentEvent
    {
        public required List<DocumentInfo> Documents { get; set; } = [];
        public required Guid SenderId { get; set; }
        public required Guid EmitterId { get; set; }
        public required bool WithDigitalSignature { get; set; } = false;
    }
}
