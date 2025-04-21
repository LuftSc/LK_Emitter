using System.Text.Json;
using System.Text;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Contracts;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;

namespace ExternalOrderReportsService.Services
{
    public class RequestSender : IRequestSender
    {
        private readonly HttpClient httpClient;

        public RequestSender(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<Result<DocumentInfo>> SendDownloadReportRequest
            (Guid reportOrderId)
        {
            var url = $"https://localhost:7024/api/LoadPdfFromStorage?code_doc={reportOrderId}";

            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return Result<DocumentInfo>
                    .Error(new BadResponseFromRegistratorAPIError());

            var content = await response.Content.ReadAsByteArrayAsync();
            var mimeType = response.Content.Headers.ContentType?.MediaType;
            var contentDisposition = response.Content.Headers.ContentDisposition;
            var fileName = contentDisposition?.FileNameStar ?? contentDisposition?.FileName;

            var documentInfo = new DocumentInfo()
            {
                Content = content,
                ContentType = mimeType,
                FileName = fileName
            };

            return Result<DocumentInfo>.Success(documentInfo);
        }
        public async Task<Result<Guid>> SendListOfShareholdersReportRequest(
            DateTime requestDate,
            ListOfShareholdersRequest request)
        {
            var dateString = requestDate.ToString("yyyy-MM-dd HH:mm:ss");
            // Заменить на URL вашего API: 
            // http://host:port/api/ListOfShareholdersForMeetingNotSign?r=2023-07-11 09:15:15
            var url = $"https://localhost:7024/api/ListOfShareholdersForMeetingNotSign?r={dateString}";

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode) return Result<Guid>
                    .Error(new GetListOfShareholdersRequestError());

            var content = await response.Content.ReadAsStringAsync();

            var orderId = JsonSerializer.Deserialize<Guid>(content);

            return Result<Guid>.Success(orderId);
        }
        public async Task<Result<Guid>> SendReeRepReportRequest(
            DateTime requestDate,
            ReeRepNotSignRequest request)
        {
            var dateString = requestDate.ToString("yyyy-MM-dd HH:mm:ss");
            // Заменить на URL вашего API: 
            // http://host:port/api/ReeRepNotSign?r=2023-07-11 09:15:15
            var url = $"https://localhost:7024/api/ReeRepNotSign?r={dateString}";

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode) return Result<Guid>
                    .Error(new GetReeRepRequestError());

            var content = await response.Content.ReadAsStringAsync();

            var orderId = JsonSerializer.Deserialize<Guid>(content);

            return Result<Guid>.Success(orderId);
        }
        public async Task<Result<Guid>> SendDividendListReportRequest
            (DateTime requestDate,
            ReportAboutDividendListNotSignRequest request)
        {
            var dateString = requestDate.ToString("yyyy-MM-dd HH:mm:ss");
            // Заменить на URL вашего API: 
            // http://host:port/api/ReportAboutDividendListNotSignController?r=2023-07-11 09:15:15
            var url = $"https://localhost:7024/api/ReportAboutDividendListNotSignController?r={dateString}";

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode) return Result<Guid>
                    .Error(new GetDividendListRequestError());

            var content = await response.Content.ReadAsStringAsync();

            var orderId = JsonSerializer.Deserialize<Guid>(content);

            return Result<Guid>.Success(orderId);
        }
    }
    public class GetDividendListRequestError : Error
    {
        public override string Type => nameof(GetDividendListRequestError);
    }
    public class GetListOfShareholdersRequestError : Error
    {
        public override string Type => nameof(GetListOfShareholdersRequestError);
    }
    public class GetReeRepRequestError : Error
    {
        public override string Type => nameof(GetReeRepRequestError);
    }
    public class BadResponseFromRegistratorAPIError : Error
    {
        public override string Type => nameof(BadResponseFromRegistratorAPIError);
    }
    public class DownloadingReportOrderFromStorageError : Error
    {
        public override string Type => nameof(DownloadingReportOrderFromStorageError);
    }
}
