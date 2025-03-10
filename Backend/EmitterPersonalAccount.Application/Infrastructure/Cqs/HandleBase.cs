using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.Cqs
{
    // IRequestHandler связывает обычные command и query с самим handler'ом
    // который будет выполняться
    public abstract class HandleBase<TRequest, TResult> : IRequestHandler<TRequest, TResult>
        // Получаем какой-то запрос и возвращаем какой-то результат
        where TRequest : IRequest<TResult> // query или command
        where TResult : class, IResult
    {
        // Чтобы иметь возможность менять ключевые вещи с точки зрения обработки
        // Нужно явно имплементировать интерфейс
        async Task<TResult> IRequestHandler<TRequest, TResult>.Handle(TRequest request, CancellationToken cancellationToken)
        {
            var result = await CoreHandle(request, cancellationToken);

            return (TResult)result;
        }

        // В данном виртуальном методе релизуем некоторую обязательную логику,
        // Которую можно будет дополнить через метод Handle()
        protected virtual async Task<IResult> CoreHandle(TRequest request, CancellationToken cancellationToken)
        {
            var canHandle = await CanHandle(request, cancellationToken);
            if (!canHandle.IsSuccessfull)
            {
                return canHandle;
            }

            return await Handle(request, cancellationToken);
        }

        // Тут в классе наследнике будет конкретная реализация
        public abstract Task<TResult> Handle(TRequest request, CancellationToken cancellationToken);

        // Т.к. внутри медиаторского RequestHandler'а нет логики о том, 
        // можно\нельзя выполнять этот метод, сделаем сами
        protected virtual Task<Core.Domain.SharedKernal.Result.Result> CanHandle(TRequest request, CancellationToken cancellationToken) 
        { 
            return Task.FromResult(Core.Domain.SharedKernal.Result.Result.Success());
        }
    }
}
