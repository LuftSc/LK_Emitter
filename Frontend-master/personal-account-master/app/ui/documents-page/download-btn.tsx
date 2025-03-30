import { Document } from "@/app/models/Document";
import { downloadDocument } from "@/app/services/documentsService";
import { errorMessages } from "@/app/services/errorMessages";
import { Button } from "antd";

interface Props {
    documentInfo: Document
}

export const DocumentDownloadLink = ({documentInfo} : Props) => {
    const handleDownload = async () => {
      const downloadResponse = await downloadDocument(documentInfo)
    
      if (downloadResponse?.ok) {// Нам вернулась с бэка вся 
      // необходимая информация для загрузки документа

        try { // Пытаемся сгенерировать ссылку для скачивания
            const blob = await downloadResponse.blob();
            const url = window.URL.createObjectURL(blob);
            
            // Создаем временную ссылку для скачивания
            const link = document.createElement("a");
            link.href = url;
            link.download = `${documentInfo.title}${documentInfo.fileExtnsion}`; // Имя файла для сохранения
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);

            // Освобождаем память
            window.URL.revokeObjectURL(url);
        } catch(error) { // Если произошла ошибка во время генерации ссылки
            console.error(`Произошла ошибка во время генерации ссылки для скачивания: ${error}`)
        }
      } else if (downloadResponse?.status === 400) { // Если произошла ошибка 
      // во время получения данных для загрузки документа
        const error = await downloadResponse.json()
        // НАДО ТУТ СДЕЛАТЬ МАППИНГ ДЛЯ ПРИХОДЯЩИХ ОШИБОК
        //console.log(errorMessages[error[0].type])
        // Пока нет маппинга, просто логируем ошибку
        console.log(error)
      } else { // Неизвестная ошибка
        console.error("Неизвестная ошибка во время загрузки документа")
      }
    };
  
    return (
      <Button onClick={handleDownload} type="primary">
        Скачать файл
      </Button>
    );
  };