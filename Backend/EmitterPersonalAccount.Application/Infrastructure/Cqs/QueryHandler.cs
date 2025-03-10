using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.Cqs
{
    public abstract class QueryHandler<TQuery, TResult> 
        : HandleBase<TQuery, Result<TResult>>,
          IQueryHandler<TQuery, Result<TResult>>
            where TQuery : Query<TResult>
    {
        protected Core.Domain.SharedKernal.Result.Result Success() => Core.Domain.SharedKernal.Result.Result.Success();
        // Успешно, но без результата

        protected Result<TResult> Success(TResult result) => Result<TResult>.Success(result);
        // Успешно + возврат каких-то данных

        protected Result<TResult> Error(IError error) => Result<TResult>.Error(error);
        // С ошибкой
    }
}
