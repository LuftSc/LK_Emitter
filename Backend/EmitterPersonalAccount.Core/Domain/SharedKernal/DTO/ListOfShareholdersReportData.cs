using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.DTO
{
    public record ListOfShareholdersReportData(
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
}
