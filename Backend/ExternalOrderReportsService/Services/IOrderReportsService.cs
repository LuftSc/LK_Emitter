using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Contracts;

namespace ExternalOrderReportsService.Services
{
    public interface IOrderReportsService
    {
        Task<Result<DocumentInfo>> LoadReportOrderFromStorage
            (Guid reportOrderId);
        Task<Result<Guid>> RequestListOfShareholdersForMeetingReport
            (DateTime r, ListOfShareholdersRequest request);
        Task<Result<Guid>> RequestReeRepReport(
            DateTime requestDate,
            ReeRepNotSignRequest request);
        Task<Result<Guid>> RequestDividendListReport
            (DateTime requestDate,
            ReportAboutDividendListNotSignRequest request);
    }
}