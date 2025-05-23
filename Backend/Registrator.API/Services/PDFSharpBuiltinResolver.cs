using PdfSharp.Fonts;
using PdfSharp.WPFonts;

namespace Registrator.API.Services
{
    public class PDFSharpBuiltinResolver : IFontResolver
    {
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Все запросы перенаправляем на SegoeWP
            string fontName = "SegoeWP"; // Базовое имя

            // Учитываем жирность/курсив (если есть варианты в FontDataHelper)
            if (isBold && FontDataHelper.SegoeWPBold != null)
                fontName = "SegoeWPBold";

            return new FontResolverInfo(fontName);
        }

        public byte[] GetFont(string faceName)
        {
            // Возвращаем данные шрифта из FontDataHelper
            return faceName switch
            {
                "SegoeWP" => FontDataHelper.SegoeWP,
                "SegoeWPBold" => FontDataHelper.SegoeWPBold,
                _ => FontDataHelper.SegoeWP // Fallback
            };
        }
    }
}
