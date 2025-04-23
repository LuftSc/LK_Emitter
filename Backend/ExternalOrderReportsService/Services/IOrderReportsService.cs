using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Contracts;

namespace ExternalOrderReportsService.Services
{
    public interface IOrderReportsService
    {
        Task<Result<DocumentInfo>> DownloadReport(Guid reportOrderId);
        Task<Result> RequestReport(GenerateListOSARequest requestData, DateTime sendingDate, string userId);
        Task<Result> RequestReport(GenerateReeRepRequest requestData, DateTime sendingDate, string userId);
        Task<Result> RequestReport(GenerateDividendListRequest requestData, DateTime sendingDate, string userId);
    }
}