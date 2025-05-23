using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents
{
    public record GetDocumentsEvent(
        int IssuerId,
        int Page,
        int PageSize
        )
    {
    }
}
