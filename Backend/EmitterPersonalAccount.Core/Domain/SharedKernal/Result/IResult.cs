using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.Result
{
    public interface IResult
    {
        bool IsSuccessfull { get; }

        IReadOnlyList<IError> GetErrors();
    }
}
