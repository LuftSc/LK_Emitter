
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Registrator.API.Extensions;
using Registrator.API.Services;
using Registrator.DataAccess.Repositories;
using System;

namespace Registrator.API.Endpoints
{
    public static class ApiEndpoints
    {
        public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder app) 
        {
            app.MapPost($"api/{nameof(ListOfShareholdersForMeetingNotSign)}",
                ListOfShareholdersForMeetingNotSign);

            app.MapPost($"api/{nameof(ReeRepNotSign)}",
                ReeRepNotSign);

            app.MapPost($"api/{nameof(ReportAboutDividendListNotSignController)}",
                ReportAboutDividendListNotSignController);

            app.MapGet($"api/{nameof(LoadPdfFromStorage)}", LoadPdfFromStorage);

            return app;
        }
        private static async Task<IResult> LoadPdfFromStorage(IDirectivesRepository directivesRepository,
            [FromQuery] Guid code_doc)
        {
            var directive = await directivesRepository.FindAsync([code_doc], default);
            return Results.File(
                directive.Content, 
                directive.MIMEType, 
                directive.FileName);
        }
        private static async Task<IResult> ListOfShareholdersForMeetingNotSign(
            [FromQuery] DateTimeWrapper r,
            [FromBody] ListOfShareholdersRequest request,
            PdfGeneratorService generatorService)
        {
            if (r == null)
                return  Results.BadRequest("Invalid date format");

            return Results.Ok(await generatorService
                .GenerateListOfShareholdersForMeetingNotSign(request));
        }
        private static async Task<IResult> ReeRepNotSign(
            [FromQuery] DateTimeWrapper r,
            [FromBody] ReeRepNotSignRequest request,
            PdfGeneratorService generatorService)
        {
            if (r == null)
                return Results.BadRequest("Invalid date format");

            return Results.Ok(await generatorService
                .GenerateReeRepNotSign(request));
        }
        private static async Task<IResult> ReportAboutDividendListNotSignController(
            [FromQuery] DateTimeWrapper r,
            [FromBody] ReportAboutDividendListNotSignRequest request,
            PdfGeneratorService generatorService)
        {
            if (r == null)
                return Results.BadRequest("Invalid date format");

            return Results.Ok(await generatorService
                .GenerateReportAboutDividendListNotSign(request));
        }
    }

    public record ListOfShareholdersRequest(
        string ReportName,
        bool IsSaveToStorage,
        int IssuerId,
        string RegOutInfo,
        string GeneralReportHeader,
        string TypKls,
        DateOnly DtMod,
        bool NomList,
        bool IsPodr,
        bool ViewCb,
        bool IsCateg,
        bool IsOneRecAllNomin,
        bool IsCategMeeting,
        bool IsRangeMeeting,
        DateOnly Dt_Begsobr,
        bool IsSocr,
        bool IsFillSchNd,
        bool IsBirthday,
        bool IsViewPhone,
        bool IsViewEmail,
        bool IsViewMeetNotify,
        bool IsViewGenDirect,
        bool IsViewInn,
        bool ViewLs,
        bool IsSignBox,
        bool OffNumbers,
        bool IsExcelFormat,
        bool PrintDt,
        string CurrentUser,
        string Operator,
        string Controler,
        bool IsViewDirect,
        bool IsViewCtrl,
        bool IsViewElecStamp,
        string Guid
    )
    { }
    public record ReeRepNotSignRequest(
        string ReportName,
        bool IsSaveToStorage,
        int EmitId,
        int SvipId,
        string Categ,
        string Fields,
        string Filter,
        int NumStoc,
        decimal ProcUk,
        DateOnly DtMod,
        int IsPodr,
        int IsCateg,
        int NomList,
        int IsZalog,
        int IsNullSch,
        int Estimation1,
        int Estimation2,
        int IsNotOblig,
        int IsFillSchNd,
        int IsFullAnketa,
        int IsViewBorn,
        int TypeReport,
        int IsExcludeListZl,
        string ListZl,
        int IsBr,
        int IsControlModifyPerson,
        int IsTrustManager,
        int IsPawnGolos,
        int IsPawnDivid,
        int IsIssuerAccounts,
        int IsEmissionAccounts,
        int IsViewPhone,
        int IsViewEmail,
        string CorporateId,
        int IsClosedAccount,
        int IsViewMeetNotify,
        bool OneProcMode,
        int IsBenef,
        int IsAgent,
        int ProcCat,
        bool IsReestr,
        string Operator,
        string Controler,
        bool IsViewCtrl,
        bool IsViewGenDirect,
        bool IsViewUk,
        bool IsZl,
        bool IsViewInn,
        bool IsPcateg,
        bool IsCheckGroupCb,
        bool IsViewDirect,
        string ViewGroupCb,
        string Diagn,
        string PrintDt,
        string StrParams,
        bool IsRiskEst,
        string SpisZl,
        bool IsPrintDtRegIssueOfSecurities,
        string GUID,
        bool IsPrintUk,
        string GeneralReportHeader,
        string RegOutInfo,
        bool IsViewElecStamp,
        string CurrentUser
        )
    { }
    public record ReportAboutDividendListNotSignRequest(
        string ReportName,
        int IssuerId,
        int DivPtr,
        bool IsPodr,
        bool IsBr,
        string TypPers,
        string PostMan,
        bool IsGroupTypNal,
        bool IsBirthday,
        bool IsRate,
        bool IsOrderCoowner,
        bool IsPostMan,
        string RegOutInfo,
        string GeneralReportHeader,
        DateOnly DtClo,
        bool IsAnnotation,
        bool IsPrintNalog,
        bool IsEstimationoN,
        bool IsExcelFormat,
        bool IsViewGenDirect,
        bool IsViewPrintUk,
        bool IsViewInn,
        bool IsViewOgrn,
        bool IsViewAddress,
        bool PrintDt,
        string Operator,
        string Controler,
        bool IsViewCtrl,
        bool IsViewElecStamp,
        string Guid
        )
    { }
}
