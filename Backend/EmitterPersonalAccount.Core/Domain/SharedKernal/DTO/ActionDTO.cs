namespace EmitterPersonalAccount.Core.Domain.SharedKernal.DTO
{
    public record ActionDTO(
        string Name,
        string Surname,
        string Patronymic,
        string ActionType,
        DateTime Timestamp,
        string IpAddress,
        string AdditionalInformation
        )
    {
    }
}
