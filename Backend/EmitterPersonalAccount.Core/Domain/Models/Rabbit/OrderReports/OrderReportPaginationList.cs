namespace EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports
{
    public record OrderReportPaginationList(
        int TotalSize,
        List<OrderReportDTO> OrderReports
        )
    {
    }
}
