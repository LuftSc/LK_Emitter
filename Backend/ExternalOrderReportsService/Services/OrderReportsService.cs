using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
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

        public OrderReportsService(IReportStatusChangeService changeStatusService,
            IRequestSender requestSender)
        {
            this.statusChangeService = changeStatusService;
            this.requestSender = requestSender;
        }
        public async Task<Result<DocumentInfo>> DownloadReport(Guid reportOrderId)
        {
            return await requestSender.SendDownloadReportRequest(reportOrderId);
        }
        public async Task<Result> RequestReport(ListOfShareholdersRequest requestData, DateTime sendingDate, string userId)
        {
            var fileName = "Лист участников собрания акционеров";
            var issuerId = requestData.IssuerId;
            var internalIdFieldName = "Guid";

            var result = await RequestReportBase(
                requestData,
                fileName,
                sendingDate,
                issuerId,
                userId,
                internalIdFieldName,
                requestSender.SendListOfShareholdersReportRequest);

            return result;
        }
        public async Task<Result> RequestReport(ReeRepNotSignRequest requestData, DateTime sendingDate, string userId)
        {
            var fileName = "Список ЗЛ";
            var issuerId = requestData.EmitId;
            var internalIdFieldName = "GUID";

            var result = await RequestReportBase(
                requestData,
                fileName,
                sendingDate,
                issuerId,
                userId,
                internalIdFieldName,
                requestSender.SendReeRepReportRequest);

            return result;
        }
        public async Task<Result> RequestReport(ReportAboutDividendListNotSignRequest requestData, DateTime sendingDate, string userId)
        {
            var fileName = "Дивидендный список";
            var issuerId = requestData.IssuerId;
            var internalIdFieldName = "Guid";

            var result = await RequestReportBase(
                requestData,
                fileName,
                sendingDate,
                issuerId,
                userId,
                internalIdFieldName,
                requestSender.SendDividendListReportRequest);

            return result;
        }

        private static T UpdateRecordProperty<T>(T record, string propertyName, object newValue)
            where T : class
        {
            // Получаем PropertyInfo по имени
            var property = typeof(T).GetProperty(propertyName);
            if (property == null)
                throw new ArgumentException($"Property '{propertyName}' not found.");

            // Получаем значения всех свойств
            var values = typeof(T).GetProperties().Select(p =>
                p.Name == propertyName ? newValue : p.GetValue(record)
            ).ToArray();

            // Получаем конструктор с нужными параметрами
            var ctor = typeof(T).GetConstructors().First();
            return (T)ctor.Invoke(values);
        }
        private async Task<Result> RequestReportBase<TRequestData>
        (
            TRequestData requestData,
            string fileName,
            DateTime sendingDate,
            int issuerId,
            string userId,
            string internalIdFieldName,
            Func<DateTime, TRequestData, Task<Result<Guid>>> sendRequestMethod
            )
            where TRequestData : class
        {
            var orderReportCreatingResult = OrderReport.Create(fileName, sendingDate, issuerId);

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
            // Эта штука немного отличается у одного из отчётов
            var updatedRequestData = UpdateRecordProperty
                (requestData,
                internalIdFieldName,
                orderReportCreatingResult.Value.Id.ToString());

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
