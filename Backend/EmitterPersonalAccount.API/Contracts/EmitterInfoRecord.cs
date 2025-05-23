using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;

namespace EmitterPersonalAccount.API.Contracts
{
    public record EmitterInfoRecord(
        string FullName,
        string ShortName,
        string Inn,
        string Jurisdiction,
        OGRNRecord OGRN,
        RegistrationRecord registration
        )
    {
    }

    public record OGRNRecord(
        string Number,
        DateOnly DateOfAssignment, 
        string Issuer
        )
    {
    }

    public record RegistrationRecord(
        string Number, 
        DateOnly RegistrationDate, 
        string Issuer
        )
    {

    }
}
