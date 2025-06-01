using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.DTO
{
    public record ActionsReportDTO(
        string SenderId,
        Guid Id,
        string Title,
        double Size,
        DateTime DateOfGeneration
        )
    {
    }
}
