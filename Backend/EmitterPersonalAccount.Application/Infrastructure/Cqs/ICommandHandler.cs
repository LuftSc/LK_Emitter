using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.Cqs
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<Core.Domain.SharedKernal.Result.Result> Handle(TCommand command, CancellationToken cancellationToken);
    }
}
