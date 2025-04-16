namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit
{
    public record OrderReportPaginationList(
        int TotalSize,
        List<OrderReportInfo> OrderReports
        )
    {
    }
}
