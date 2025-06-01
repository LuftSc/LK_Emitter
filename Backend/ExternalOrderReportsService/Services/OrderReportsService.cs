using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Logs;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Contracts;

//using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace ExternalOrderReportsService.Services
{
    public class OrderReportsService : IOrderReportsService
    {
        private readonly IReportStatusChangeService statusChangeService;
        private readonly IRequestSender requestSender;
        private readonly IOrderReportsRepository orderReportsRepository;
        private readonly IRabbitMqPublisher publisher;

        public OrderReportsService(IReportStatusChangeService changeStatusService,
            IRequestSender requestSender, 
            IOrderReportsRepository orderReportsRepository,
            IRabbitMqPublisher publisher)
        {
            this.statusChangeService = changeStatusService;
            this.requestSender = requestSender;
            this.orderReportsRepository = orderReportsRepository;
            this.publisher = publisher;
        }
        public async Task<Result<DocumentInfo>> DownloadReport
            (Guid userId, Guid reportOrderId, ReportType reportType)
        {
            var downloadResult = await requestSender
                .SendDownloadReportRequest(reportOrderId);

            if (!downloadResult.IsSuccessfull)
                return downloadResult;

            var reportLogType = reportType switch
            {
                ReportType.ListOfShareholders => ActionLogType.DownloadListOSA.Type,
                ReportType.ReeRepNotSign => ActionLogType.DownloadReeRep.Type,
                ReportType.DividendList => ActionLogType.DownloadDividendList.Type,
                _ => "Загрузка отчёта: тип не определён",
            };

            var logEvent = new UserActionLogEvent
            (
                userId,
                reportLogType,
                DateTime.Now.ToUniversalTime().AddHours(5)
            );

            await publisher
                .SendMessageAsync(
                    JsonSerializer.Serialize(logEvent), 
                    RabbitMqAction.WriteUsersLogs, 
                    default);

            return Result<DocumentInfo>.Success(downloadResult.Value);
        }
        public async Task<Result> RequestReport(GenerateListOSARequest requestData, DateTime sendingDate, string userId)
        {
            var fileName = "Лист участников собрания акционеров";
            var issuerId = requestData.IssuerId;
            var internalId = Guid.NewGuid();

            var requestDataWithInternalId = requestData with
            {
                InternalDocumentId = internalId.ToString()
            };

            await orderReportsRepository.SaveAsync(requestDataWithInternalId, default);

            var result = await RequestReportBase(
                requestDataWithInternalId,
                fileName,
                sendingDate,
                issuerId,
                userId,
                internalId,
                requestSender.SendListOfShareholdersReportRequest,
                ActionLogType.RequestListOSA,
                ReportType.ListOfShareholders);

            return result;
        }
        public async Task<Result> RequestReport(GenerateReeRepRequest requestData, DateTime sendingDate, string userId)
        {
            var fileName = "Список ЗЛ";
            var issuerId = requestData.EmitId;
            var internalId = Guid.NewGuid();

            var requestDataWithInternalId = requestData with
            {
                InternalDocumentId = internalId.ToString()
            };

            await orderReportsRepository.SaveAsync(requestDataWithInternalId, default);

            var result = await RequestReportBase(
                requestDataWithInternalId,
                fileName,
                sendingDate,
                issuerId,
                userId,
                internalId,
                requestSender.SendReeRepReportRequest,
                ActionLogType.RequestReeRep,
                ReportType.ReeRepNotSign);

            return result;
        }
        public async Task<Result> RequestReport(GenerateDividendListRequest requestData, DateTime sendingDate, string userId)
        {
            var fileName = "Дивидендный список";
            var issuerId = requestData.IssuerId;
            var internalId = Guid.NewGuid();

            var requestDataWithInternalId = requestData with
            {
                InternalDocumentId = internalId.ToString()
            };

            await orderReportsRepository.SaveAsync(requestDataWithInternalId, default);

            var result = await RequestReportBase(
                requestDataWithInternalId,
                fileName,
                sendingDate,
                issuerId,
                userId,
                internalId,
                requestSender.SendDividendListReportRequest,
                ActionLogType.RequestDividendList,
                ReportType.DividendList);

            return result;
        }
        private async Task<Result> RequestReportBase<TRequestData>
        (
            TRequestData requestData,
            string fileName,
            DateTime sendingDate,
            int issuerId,
            string userId,
            Guid internalId,
            Func<DateTime, TRequestData, Task<Result<Guid>>> sendRequestMethod,
            ActionLogType logType,
            ReportType reportType
            )
            where TRequestData : class
        {
            var orderReportCreatingResult = OrderReport.Create(fileName, sendingDate, issuerId, internalId, reportType);

            if (!orderReportCreatingResult.IsSuccessfull) return orderReportCreatingResult;

            var methodSendingResult = MethodResultSending.ReceiveGeneratedReport;

            var setProcessingResult = await statusChangeService.SetProcessingStatus
                (userId, orderReportCreatingResult.Value, methodSendingResult);

            if (!setProcessingResult.IsSuccessfull)
            {
                await statusChangeService
                    .SetFailedStatus(userId, orderReportCreatingResult.Value, methodSendingResult);
                return setProcessingResult;
            }

            var logEvent = new UserActionLogEvent(
                Guid.Parse(userId),
                logType.Type,
                sendingDate);

            await publisher
                .SendMessageAsync(
                    JsonSerializer.Serialize(logEvent), 
                    RabbitMqAction.WriteUsersLogs, 
                    default);

            var generatingResult = await sendRequestMethod(sendingDate, requestData);

            if (!generatingResult.IsSuccessfull)
            {
                await statusChangeService
                    .SetFailedStatus(userId, orderReportCreatingResult.Value, methodSendingResult);
                return generatingResult;
            }

            var statusSuccessResult = await statusChangeService
                    .SetSuccessfullStatus(
                        userId,
                        orderReportCreatingResult.Value,
                        generatingResult.Value, methodSendingResult);

            if (!statusSuccessResult.IsSuccessfull)
            {
                await statusChangeService
                    .SetFailedStatus(userId, orderReportCreatingResult.Value, methodSendingResult);
                return statusSuccessResult;
            }

            return Result.Success();
        }
    }


    /* public class OrderReportsService : IOrderReportsService
     {
         private readonly HttpClient httpClient;

         public OrderReportsService(HttpClient httpClient)
         {
             this.httpClient = httpClient;
         }

         public async Task<Result<DocumentInfo>> LoadReportOrderFromStorage
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
         public async Task<Result<Guid>> RequestListOfShareholdersForMeetingReport(
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
         public async Task<Result<Guid>> RequestReeRepReport(
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
         public async Task<Result<Guid>> RequestDividendListReport
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
     }*/
}
