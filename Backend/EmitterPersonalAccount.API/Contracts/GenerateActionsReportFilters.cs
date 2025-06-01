namespace EmitterPersonalAccount.API.Contracts
{
    public record GenerateActionsReportFilters(
        Guid? UserId,
        DateTime? StartDate,
        DateTime? EndDate
        )
    {
    }
}
