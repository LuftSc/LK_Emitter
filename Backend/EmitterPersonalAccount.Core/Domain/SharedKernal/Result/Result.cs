using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.Result
{
    public class Result : IResult
    {// Реализация результата БЕЗ данных

        // Выполнение считаем успешным, если нет ошибок
        public bool IsSuccessfull => _errors.Count < 1;

        private readonly List<IError> _errors = [];

        protected void AddError(IError error)
        {
            ArgumentNullException.ThrowIfNull(error);

            _errors.Add(error);
        }
        public IReadOnlyList<IError> GetErrors() => _errors;

        // Чтобы уменьшить нагрузку при создании новых пустых объектов,
        // Создадим один статичный пустой объект и будем возвращать его
        private static readonly Result _success = new();
        public static Result Success() => _success;

        // Вместе с результатом возвращаем ошибку
        public static Result Error(IError error)
        {
            var result = new Result();
            result.AddError(error);
            return result;
        }
    }
}
