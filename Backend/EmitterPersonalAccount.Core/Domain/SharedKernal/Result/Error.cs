using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.Result
{
    public abstract class Error : IError
    { // Будем использовать для ошибок этот класс
        public abstract string Type { get; }

        public Dictionary<string, object> Data { get; } = [];
    }
}
