using ClosedXML.Excel;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditService.Services;

public class ExcelService : IExcelService
{
    public async Task<Result<byte[]>> WriteLogsToExcelFile(List<ActionDTO> logs)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Действия пользователей");

            // Заголовки
            worksheet.Cell(1, 1).Value = "Фамилия";
            worksheet.Column(1).Width = 15;

            worksheet.Cell(1, 2).Value = "Имя";
            worksheet.Column(2).Width = 12;

            worksheet.Cell(1, 3).Value = "Отчество";
            worksheet.Column(3).Width = 18;

            worksheet.Cell(1, 4).Value = "Действие";
            worksheet.Column(4).Width = 55;

            worksheet.Cell(1, 5).Value = "Время";
            worksheet.Column(5).Width = 15;

            worksheet.Cell(1, 6).Value = "Дополнительная информация";
            worksheet.Column(6).Width = 25;

            // Данные
            for (int i = 0; i < logs.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = logs[i].Surname;
                worksheet.Cell(i + 2, 2).Value = logs[i].Name;
                worksheet.Cell(i + 2, 3).Value = logs[i].Patronymic;
                worksheet.Cell(i + 2, 4).Value = logs[i].ActionType;
                worksheet.Cell(i + 2, 5).Value = logs[i].Timestamp;
                worksheet.Cell(i + 2, 6).Value = logs[i].AdditionalInformation;
            }

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);

                /*var file = new DocumentInfo()
                {
                    FileName = $"Действия пользователей {DateTime.Now
                        .ToUniversalTime()
                        .AddHours(5)
                        .ToString("yyyy-MM-dd HH:mm")}",
                    Content = stream.ToArray(),
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                };*/

                return Result<byte[]>
                    .Success(stream.ToArray());
            }
        }
    }
}
