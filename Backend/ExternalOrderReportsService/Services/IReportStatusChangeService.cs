using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace ExternalOrderReportsService.Services
{
    public interface IReportStatusChangeService
    {
        Task<Result> SetFailedStatus(string userId, OrderReport report, MethodResultSending method);
        Task<Result> SetProcessingStatus(string userId, OrderReport report, MethodResultSending method);
        Task<Result> SetSuccessfullStatus(string userId, OrderReport report, Guid externalReportId, MethodResultSending method);
    }
}