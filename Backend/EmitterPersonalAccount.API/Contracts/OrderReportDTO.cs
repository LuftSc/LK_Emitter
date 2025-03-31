namespace EmitterPersonalAccount.API.Contracts
{
    public record OrderReportDTO(
        Guid Id,
        string FileName,
        string Status,
        DateTime RequestTime,
        Guid IdForDownload
        )
    {
    }
}
