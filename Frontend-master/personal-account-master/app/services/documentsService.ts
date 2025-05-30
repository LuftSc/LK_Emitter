import { Document } from "../models/Document";

export const downloadDocument = async (documentInfo: Document) => {
    return await fetch(`http://localhost:5000/Documents/download/${documentInfo.id}`, {
        credentials: 'include'
    })
    .catch(error => console.error(error))
}

/*export const getDocuments = async (emitterId: string) => {
    return await fetch(`http://localhost:5000/Documents/get-documents-info/${emitterId}`, {
        credentials: 'include'
    })
    .catch(error => console.error(error))
} */

export const getDocumentsByPageByIssuerId = async (issuerId: number, page:number, pageSize: number) => {
    return await fetch(`http://localhost:5000/Documents/get-documents-info/${issuerId}?Page=${page}&PageSize=${pageSize}`, {
        credentials: 'include'
    })
    .catch(error => console.error(error))
}


export const uploadDocuments = async (formData: FormData) => {
    return await fetch(`http://localhost:5000/Documents/send-documents`, {
        method: 'POST',
        credentials: 'include',
        body: formData
    })
    .catch(error => console.error(error))
}