using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace ExternalOrderReportsService.Services
{
    public interface IReportStatusChangeService
    {
        Task<Result> SetFailedStatus(string userId, OrderReport report);
        Task<Result> SetProcessingStatus(string userId, OrderReport report);
        Task<Result> SetSuccessfullStatus(string userId, OrderReport report, Guid externalReportId);
    }
}