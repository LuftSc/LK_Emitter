using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO
{
    public class RegistrationInfo : ValueObject
    {
        private RegistrationInfo()
        {
            
        }
        private RegistrationInfo(string number, DateOnly registrationDate, string issuer)
        {
            Number = number;
            RegistrationDate = registrationDate;
            Issuer = issuer;
        }
        public string Number { get; private set; }
        public DateOnly RegistrationDate { get; private set; } = new ();
        public string Issuer { get; private set; }
        public static Result<RegistrationInfo> Create(string number,
            DateOnly registrationDate, string issuer)
        {
            return Result.Success(new RegistrationInfo(number, registrationDate, issuer));
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
            yield return RegistrationDate;
            yield return Issuer;
        }
    }
}
