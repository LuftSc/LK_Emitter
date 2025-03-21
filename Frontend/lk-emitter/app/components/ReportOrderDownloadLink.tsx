import { ReportOrderInfo } from "../models/ReportOrderInfo";

interface Props {
    reportOrderInfo: ReportOrderInfo
}

export const ReportOrderDownloadLink = ({reportOrderInfo}:Props) => {
    const handleDownload = async () => {
        try {
            console.log(reportOrderInfo.fileName)
            const response = await fetch(`http://localhost:5144/OrderReports/download-report-order/${reportOrderInfo.reportOrderId}?fileName=${reportOrderInfo.fileName}`, {
                method: "GET",
                credentials: 'include'
            });
    
            if (!response.ok) {
                throw new Error("Ошибка при загрузке файла");
            }
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
            } catch (error) {
            console.error("Ошибка:", error);
            }
      };
    
      return (
        <button onClick={handleDownload}>
          Распоряжение: {reportOrderInfo.fileName}
        </button>
      );
}