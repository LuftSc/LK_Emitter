using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Contracts;

namespace ExternalOrderReportsService.Services
{
    public interface IRequestSender
    {
        Task<Result<Guid>> SendDividendListReportRequest(DateTime requestDate, GenerateDividendListRequest request);
        Task<Result<DocumentInfo>> SendDownloadReportRequest(Guid reportOrderId);
        Task<Result<Guid>> SendListOfShareholdersReportRequest(
            DateTime requestDate,
            GenerateListOSARequest request);
        Task<Result<Guid>> SendReeRepReportRequest(DateTime requestDate, GenerateReeRepRequest request);
    }
}