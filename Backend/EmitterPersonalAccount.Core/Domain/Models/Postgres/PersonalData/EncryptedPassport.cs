using CSharpFunctionalExtensions;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.PersonalData
{
    public class EncryptedPassport : ValueObject
    {
        private EncryptedPassport()
        {
            
        }

        private EncryptedPassport(
            string series,
            string number,
            string dateOfIssue,
            string issuer,
            string unitCode
            )
        {
            Series = series;
            Number = number;
            DateOfIssue = dateOfIssue;
            Issuer = issuer;
            UnitCode = unitCode;
        }
        public static readonly EncryptedPassport Empty = new();
        public string Series { get; private set; } = string.Empty;
        public string Number { get; private set; } = string.Empty;
        public string DateOfIssue { get; private set; } = string.Empty;
        public string Issuer { get; private set; } = string.Empty;
        public string UnitCode { get; private set; } = string.Empty;

        public static SharedKernal.Result.Result<EncryptedPassport> Create(
            string series,
            string number,
            string dateOfIssue,
            string issuer,
            string unitCode
            )
        {
            return SharedKernal.Result.Result<EncryptedPassport>
                .Success(new EncryptedPassport(series, number, dateOfIssue, issuer, unitCode));    
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Series;
            yield return Number;
            yield return DateOfIssue;
            yield return Issuer;
            yield return UnitCode;
        }
    }
}
