using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.Cqs
{
    public abstract class CommandHandler<TCommand> : HandleBase<TCommand, Core.Domain.SharedKernal.Result.Result>, 
        ICommandHandler<TCommand>
            where TCommand : Command
    {
        protected Core.Domain.SharedKernal.Result.Result Success() => Core.Domain.SharedKernal.Result.Result.Success();

        //protected Result.Result<TResult> Success(TResult result) => Result.Result<TResult>.Success(result);
        protected Core.Domain.SharedKernal.Result.Result Error(IError error) => Core.Domain.SharedKernal.Result.Result.Error(error);
    }
}
