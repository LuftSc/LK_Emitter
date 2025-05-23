using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO
{
    public class EmitterInfo : ValueObject
    {
        private EmitterInfo()
        {
            
        }

        [JsonConstructor]
        private EmitterInfo(
            string fullName,
            string shortName,
            string inn,
            string jurisdiction,
            OGRNInfo OGRN,
            RegistrationInfo registration)
        {
            FullName = fullName;
            ShortName = shortName;
            INN = inn;
            Jurisdiction = jurisdiction;
            this.OGRN = OGRN;
            Registration = registration;
        }
        public string FullName { get; private set; }
        public string ShortName { get; private set; }
        public string INN { get; private set; }
        public string Jurisdiction { get; private set; }
        public OGRNInfo OGRN { get; private set; } = OGRNInfo.Empty;
        public RegistrationInfo Registration { get; private set; } = RegistrationInfo.Empty;
        public static Result<EmitterInfo> Create(
            string fullName,
            string shortName,
            string inn,
            string jurisdiction,
            OGRNInfo OGRN,
            RegistrationInfo registration)
        {
            return Result.Success(new EmitterInfo(
                fullName,
                shortName,
                inn,
                jurisdiction,
                OGRN,
                registration));
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FullName;
            yield return ShortName;
            yield return INN;
            yield return Jurisdiction;
            yield return OGRN;
            yield return Registration;
        }
    }
}
