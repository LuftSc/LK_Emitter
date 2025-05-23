namespace EmitterPersonalAccount.API.Contracts
{
    public record PassportDTO (
        string Series, 
        string Number, 
        DateOnly? DateOfIssuer,
        string Issuer, 
        string UnitCode
    )
    {
        
    }
}
