using EmitterPersonalAccount.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents
{
    public record DocumentDTO(
        Guid Id,
        Role SenderRole,
        string Title,
        string FileExtnsion,
        DateTime UploadDate,
        double Size
        )
    {
    }
}
