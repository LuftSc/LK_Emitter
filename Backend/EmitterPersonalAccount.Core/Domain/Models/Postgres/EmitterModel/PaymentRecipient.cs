using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel
{
    public class PaymentRecipient : ValueObject
    {
        private PaymentRecipient()
        {
            
        }
        private PaymentRecipient(string name, string inn, string assignment)
        {
            Name = name;
            INN = inn;
            Assignment = assignment;
        }
        public string Name { get; private set; }
        public string INN { get; private set; }
        public string Assignment { get; private set; }
        public static Result<PaymentRecipient> Create(string name, 
            string inn, string assignment)
        {
            return Result.Success(new PaymentRecipient(name, inn, assignment));
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return INN;
            yield return Assignment;
        }
    }
}
