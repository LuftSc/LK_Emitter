using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.DTO
{
    public record DividendListReportData(
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
