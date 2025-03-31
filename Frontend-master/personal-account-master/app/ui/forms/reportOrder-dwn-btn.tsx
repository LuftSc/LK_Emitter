import { ReportOrder } from "@/app/models/ReportOrder";
import { downloadReportOrder } from "@/app/services/orderReportsService";

interface Props {
    reportOrder: ReportOrder
}

export const ReportOrderDownloadLink = ({reportOrder}:Props) => {
    const handleDownload = async () => {
        const response = await downloadReportOrder(reportOrder)

        if (response?.ok) {
            let fileName = "Распоряжение"
            // Извлекаем имя файла из заголовка Content-Disposition
            const contentDisposition = response.headers.get('Content-Disposition');
            // Проверяем наличие filename*
            const filenameStarMatch = contentDisposition?.match(/filename\*=(?:UTF-8'')?(.+)/i);
            if (filenameStarMatch && filenameStarMatch[1]) {
                // Декодируем значение filename*
                fileName = decodeURIComponent(filenameStarMatch[1]);
            } else {
                // Если filename* отсутствует, используем filename
                const filenameMatch = contentDisposition?.match(/filename="?(.+)"?/i);
                if (filenameMatch && filenameMatch[1]) {
                    fileName = filenameMatch[1];
                }
            }

            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            
            // Создаем временную ссылку для скачивания
            const link = document.createElement("a");
            link.href = url;
            link.download = `${fileName}`; // Имя файла для сохранения
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            // Освобождаем память
            window.URL.revokeObjectURL(url);
        } else if (response?.status === 400) {
            console.error(`подконтрольная ошибка в процессе генерации ссылки для скачивания распоряжения`)
        } else {
            console.error(`неизваестная ошибка в процессе генерации ссылки для скачивания распоряжения`)
        }   
      };
    
      return (
        <button onClick={handleDownload}>
          Распоряжение: {reportOrder.fileName}
        </button>
      );
}