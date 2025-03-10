using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.Cqs
{
    // IRequest<Result.Result<TResult>> - для связи с MediatR
    public abstract class Query<TResult> : IQuery<Result<TResult>>
        , IRequest<Result<TResult>>
    {
    }
}
