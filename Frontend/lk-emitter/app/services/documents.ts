export const getAllDocumentsByUserId = async (userId: string) => {
    const response = await fetch(`https://localhost:7233/Documents/get-documents-info/${userId}`);

    return response.json();
}

export const downloadDocument = async (documentId: string) => {
    fetch('https://localhost:7233/Documents/download/ac5301c9-6e2c-4050-a6fe-445e3c711e4e')
        .then(response => {
            if (!response.ok) throw new Error('Ошибка загрузки');
            
            return response.blob();
        })
        .then(blob => {
            const url = URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'document.pdf';
            a.click();
            URL.revokeObjectURL(url);
        })
        .catch(error => console.error('Ошибка:', error));
}