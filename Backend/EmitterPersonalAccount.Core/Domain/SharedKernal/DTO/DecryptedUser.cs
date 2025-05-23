using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.DTO
{
    public record DecryptedUser(
        Guid Id,
        string FullName,
        string Email,
        string Phone,
        DateOnly BirthDate,
        DecryptedPassport Passport,
        Role Role,
        List<Emitter>? Emitters
        )
    {
    }

    public record DecryptedPassport(
        string Series,
        string Number,
        DateOnly? DateOfIssuer,
        string Issuer,
        string UnitCode
        )
    {

    }
}
