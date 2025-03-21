import React from "react";
import { downloadDocument } from "../services/documents";
import { DocumentInfo } from "../models/Document";

interface Props {
    documentInfo: DocumentInfo
}

export const DocumentDownloadLink = ({documentInfo} : Props) => {
  const handleDownload = async () => {
    try {
        const response = await fetch(`http://localhost:5144/Documents/download/${documentInfo.id}`, {
            method: "GET",
        });

        if (!response.ok) {
            throw new Error("Ошибка при загрузке файла");
        }

        const blob = await response.blob();
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
        } catch (error) {
        console.error("Ошибка:", error);
        }
  };

  return (
    <button onClick={handleDownload}>
      Скачать файл
    </button>
  );
};