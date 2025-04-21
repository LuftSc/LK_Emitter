using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO.ListOSA;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Contracts;

namespace ExternalOrderReportsService.Services
{
    public interface IOrderReportsService
    {
        Task<Result<DocumentInfo>> DownloadReport(Guid reportOrderId);
        Task<Result> RequestReport(GenerateListOSARequest requestData, DateTime sendingDate, string userId);
        Task<Result> RequestReport(ReeRepNotSignRequest requestData, DateTime sendingDate, string userId);
        Task<Result> RequestReport(ReportAboutDividendListNotSignRequest requestData, DateTime sendingDate, string userId);
    }
}