namespace EmitterPersonalAccount.API.Contracts
{
    public record OrderReportPaginationList(
        int TotalSize,
        List<OrderReportDTO> OrderReports
        )
    {
    }
}
