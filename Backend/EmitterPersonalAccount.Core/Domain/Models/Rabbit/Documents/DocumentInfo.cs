using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents
{
    public class DocumentInfo
    {
        public required string FileName { get; set; }
        public required string ContentType { get; set; }
        public required byte[] Content { get; set; }
    }
}
