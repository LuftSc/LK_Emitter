
using Microsoft.AspNetCore.Mvc.RazorPages;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Registrator.API.Endpoints;
using Registrator.DataAccess.Repositories;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Registrator.API.Services
{
    public class PdfGeneratorService
    {
        private readonly IDirectivesRepository directivesRepository;

        public PdfGeneratorService(IDirectivesRepository directivesRepository)
        {
            this.directivesRepository = directivesRepository;
        }
        public async Task<Guid> GenerateListOfShareholdersForMeetingNotSign
            (ListOfShareholdersRequest data)
        {
            var documentName = "Список акционеров для собрания";
            var document = GenerateBasePDF(documentName, data);

            return await SaveDocumentToStorage(documentName, document);
        }
        public async Task<Guid> GenerateReeRepNotSign
            (ReeRepNotSignRequest data)
        {
            var documentName = "Реестр ЗЛ";
            var document = GenerateBasePDF(documentName, data);
            
            return await SaveDocumentToStorage(documentName, document);
        }
        public async Task<Guid> GenerateReportAboutDividendListNotSign
            (ReportAboutDividendListNotSignRequest data)
        {
            var documentName = "Дивидендный список";
            var document = GenerateBasePDF(documentName, data);

            return await SaveDocumentToStorage(documentName, document);
        }
        private void WriteAllRecordFieldsInPDF<TRecord>(TRecord record, XGraphics gfx, 
            PdfDocument document, PdfPage page)
            where TRecord : class
        {
            // Получаем тип record
            Type recordType = typeof(TRecord);

            // Получаем все свойства record
            PropertyInfo[] properties = recordType.GetProperties();

            // Устанавливаем начальную позицию Y
            double currentY = 50; // Отступ сверху
            double marginBottom = 50; // Отступ снизу
            double lineHeight = 20; // Высота строки

            // Создаем шрифт
            var fontBold = new XFont("Arial", lineHeight, XFontStyleEx.Bold);
            var fontRegular = new XFont("Arial", lineHeight, XFontStyleEx.Regular);
            // Перебираем свойства и выводим их названия и значения
            foreach (var property in properties)
            {
                // Проверяем, выходит ли текст за пределы страницы
                if (currentY + lineHeight > page.Height - marginBottom)
                {
                    // Создаем новую страницу
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    currentY = 50; // Сбрасываем позицию Y
                }

                var value = property.GetValue(record);
                gfx.DrawString($"{property.Name}: {value}",
                    fontRegular, XBrushes.Black, new XPoint(50, currentY += 30));
            }
        }

        private PdfDocument GenerateBasePDF<TRecord>(string documentName, TRecord data)
            where TRecord : class
        {
            // Создаем новый PDF-документ
            var document = new PdfDocument();

            // Добавляем страницу в документ
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Добавляем текст на страницу
            gfx.DrawString(documentName,
                new XFont("Arial", 20, XFontStyleEx.Bold), XBrushes.Black, new XPoint(50, 50));

            WriteAllRecordFieldsInPDF(data, gfx, document, page);

            return document;
        }

        private async Task<Guid> SaveDocumentToStorage(string documentName, PdfDocument document)
        {
            // Сохраняем документ в MemoryStream
            using (var memoryStream = new MemoryStream())
            {
                document.Save(memoryStream);
                memoryStream.Position = 0;

                var reportDate = DateOnly.FromDateTime(DateTime.Now).ToString();

                return await directivesRepository
                    .Create(memoryStream.ToArray(),
                    $"{documentName} {reportDate}.pdf");
            }
        }
    }
}
