using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.Cqs
{
    // IRequest - из библиотеки MediatR
    public abstract class Command : IRequest<Core.Domain.SharedKernal.Result.Result>, ICommand
    {

    }
}
