using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO
{
    public class OGRNInfo : ValueObject
    {
        private OGRNInfo()
        {
            
        }
        private OGRNInfo(string number,
            DateOnly dateOfAssignment, string issuer)
        {
            Number = number;
            DateOfAssignment = dateOfAssignment;
            Issuer = issuer;
        }
        public string Number { get; private set; }
        public DateOnly DateOfAssignment { get; private set; }
        public string Issuer { get; private set; }

        public static Result<OGRNInfo> Create(string number,
            DateOnly dateOfAssignment, string issuer)
        {
            return Result.Success(new OGRNInfo(number, dateOfAssignment, issuer));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
            yield return DateOfAssignment;
            yield return Issuer;
        }
    }
}
