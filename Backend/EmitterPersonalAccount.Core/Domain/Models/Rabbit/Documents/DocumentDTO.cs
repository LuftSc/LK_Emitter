using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents
{
    public record DocumentDTO(
        Guid Id,
        string Title,
        string FileExtnsion,
        DateTime UploadDate,
        double Size,
        bool IsEmitterSended
        )
    {
    }
}
