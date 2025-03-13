using Microsoft.AspNetCore.StaticFiles;

namespace DocumentsService
{
    public class MIMETypeMapper
    {
        public static string GetMimeType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (provider.TryGetContentType(fileName, out var mimeType))
            {
                return mimeType;
            }

            // Если MIME-тип не найден, возвращаем стандартный тип
            return "application/octet-stream";
        }
    }
}
