using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.Result
{
    // Результат для возвращения каких-то данных
    public class Result<TValue> : Result, IResult<TValue>
    {
        public TValue Value { get; }

        // Чтобы никто другой не мог создавать результат как объект,
        // Делаем конструкторы приватными + создаём 2 хэлпера для 
        // создания успешного результата и результата с ошибкой
        private Result(TValue value)
        {
            Value = value;
        }
        private Result(IError error)
        {
            AddError(error);
        }

        public static Result<TValue> Success(TValue value)
        {
            return new Result<TValue>(value);
        }

        public static Result<TValue> Error(IError error)
        {
            return new Result<TValue>(error);
        }
    }
}
