using System.Text.Json;
using System.Text;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Contracts;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using Microsoft.Extensions.Options;
using ExternalOrderReportsService.Configurations;

namespace ExternalOrderReportsService.Services
{
    public class RequestSender : IRequestSender
    {
        private readonly HttpClient httpClient;
        private readonly RegistratorEndpointOptions options;

        public RequestSender(HttpClient httpClient, 
            IOptions<RegistratorEndpointOptions> options)
        {
            this.httpClient = httpClient;
            this.options = options.Value;
        }

        public async Task<Result<DocumentInfo>> SendDownloadReportRequest
            (Guid reportOrderId)
        {
            //var url = $"https://localhost:7024/api/LoadPdfFromStorage?code_doc={reportOrderId}";
            var url = $"{options.Download}{reportOrderId}";
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
            GenerateListOSARequest request)
        {
            var dateString = requestDate.ToString("yyyy-MM-dd HH:mm:ss");
            // Заменить на URL вашего API: 
            // http://host:port/api/ListOfShareholdersForMeetingNotSign?r=2023-07-11 09:15:15
            //var url = $"https://localhost:7024/api/ListOfShareholdersForMeetingNotSign?r={dateString}";
            var url = $"{options.ListOfShareholders}{dateString}";

            var apiRequest = new ListOfShareholdersRequest(
                ReportName: "ListOfMeetingShareholdersCb",
                IsSaveToStorage: true,
                IssuerId: request.IssuerId,
                RegOutInfo: "1/ИСХ",
                GeneralReportHeader: "Список лиц, имеющих право голоса при принятии решений общим собранием акционеров",
                TypKls: string.Empty,
                DtMod: request.DtMod,
                NomList: request.NomList,
                IsPodr: false,
                ViewCb: true,
                IsCateg: false,
                IsOneRecAllNomin: false,
                IsCategMeeting: request.IsCategMeeting,
                IsRangeMeeting: request.IsRangeMeeting,
                Dt_Begsobr: request.Dt_Begsobr,
                IsSocr: false,
                IsFillSchNd: false,
                IsBirthday: false,
                IsViewPhone: true,
                IsViewEmail: true,
                IsViewMeetNotify: true,
                IsViewGenDirect: false,
                IsViewInn: false,
                ViewLs: false,
                IsSignBox: false,
                OffNumbers: false,
                IsExcelFormat: false,
                PrintDt: false,
                CurrentUser: "LK",
                Operator: "Кузнецов А. С.",
                Controler: string.Empty,
                IsViewDirect: false,
                IsViewCtrl: false,
                IsViewElecStamp: true,
                Guid: request.InternalDocumentId
            );

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(apiRequest),
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
            GenerateReeRepRequest request)
        {
            var dateString = requestDate.ToString("yyyy-MM-dd HH:mm:ss");
            // Заменить на URL вашего API: 
            // http://host:port/api/ReeRepNotSign?r=2023-07-11 09:15:15
            //var url = $"https://localhost:7024/api/ReeRepNotSign?r={dateString}";
            var url = $"{options.ReeRep}{dateString}";

            var apiRequest = new ReeRepNotSignRequest(
                ReportName: "ReeRep",
                IsSaveToStorage: true,
                EmitId: request.EmitId,
                SvipId: 0,
                Categ: string.Empty,
                Fields: string.Empty,
                Filter: string.Empty,
                NumStoc: 0,
                ProcUk: request.ProcUk,
                DtMod: request.DtMod,
                IsPodr: 0,
                IsCateg: 0,
                NomList: 0,
                IsZalog: 0,
                IsNullSch: 0,
                Estimation1: 0,
                Estimation2: 0,
                IsNotOblig: 1,
                IsFillSchNd: 0,
                IsFullAnketa: 0,
                IsViewBorn: 0,
                TypeReport: 0,
                IsExcludeListZl: 0,
                ListZl: string.Empty,
                IsBr: 0,
                IsControlModifyPerson: 0,
                IsTrustManager: 1,
                IsPawnGolos: 0,
                IsPawnDivid: 0,
                IsIssuerAccounts: 0,
                IsEmissionAccounts: 0,
                IsViewPhone: 0,
                IsViewEmail: 0,
                CorporateId: string.Empty,
                IsClosedAccount: 0,
                IsViewMeetNotify: 0,
                OneProcMode: request.OneProcMode,
                IsBenef: 0,
                IsAgent: 0,
                ProcCat: 0,
                IsReestr: false,
                Operator: "Кузнецов А. С.",
                Controler: string.Empty,
                IsViewCtrl: false,
                IsViewGenDirect: false,
                IsViewUk: false,
                IsZl: false,
                IsViewInn: false,
                IsPcateg: false,
                IsCheckGroupCb: false,
                IsViewDirect: false,
                ViewGroupCb: string.Empty,
                Diagn: string.Empty,
                PrintDt: string.Empty,
                StrParams: string.Empty,
                IsRiskEst: false,
                SpisZl: string.Empty,
                IsPrintDtRegIssueOfSecurities: false,
                GUID: request.InternalDocumentId,
                IsPrintUk: false,
                GeneralReportHeader: "Реестр зарегистрированных лиц",
                RegOutInfo: "1/ИСХ",
                IsViewElecStamp: true,
                CurrentUser: "LK"
            );

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(apiRequest),
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
            GenerateDividendListRequest request)
        {
            var dateString = requestDate.ToString("yyyy-MM-dd HH:mm:ss");
            // Заменить на URL вашего API: 
            // http://host:port/api/ReportAboutDividendListNotSignController?r=2023-07-11 09:15:15
            //var url = $"https://localhost:7024/api/ReportAboutDividendListNotSignController?r={dateString}";
            var url = $"{options.DividendList}{dateString}";

            var apiRequest = new ReportAboutDividendListNotSignRequest(
                ReportName: "DividendList",
                IssuerId: request.IssuerId,
                IsReestr: false,
                DivPtr: 0,
                IsPodr: false,
                IsBr: false,
                TypPers: string.Empty,
                PostMan: string.Empty,
                IsGroupTypNal: false,
                IsBirthday: false,
                IsRate: false,
                IsOrderCoowner: false,
                IsPostMan: false,
                DtClo: request.DtClo,
                IsAnnotation: false,
                IsPrintNalog: false,
                IsEstimationoN: false,
                IsExcelFormat: false,
                Operator: "Кузнецов А. С.",
                Controler: string.Empty,
                IsViewCtrl: false,
                IsViewGenDirect: false,
                IsViewInn: false,
                IsViewOgrn: false,
                IsViewAddress: false,
                Guid: request.InternalDocumentId,
                IsViewPrintUk: false,
                GeneralReportHeader: "Список лиц, имеющих право на получение доходов по ценным бумагам",
                RegOutInfo: "1/ИСХ",
                PrintDt: false,
                IsViewElecStamp: true,
                CurrentUser: "LK"
            );

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(apiRequest),
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
