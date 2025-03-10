using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.Cqs
{
    public interface IQueryHandler<in TQuery, TResult>
        // TQuery - некий запрос, TResult - результат выполнения TQuery
        where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default);
    }
}
