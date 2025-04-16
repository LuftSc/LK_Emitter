"use client"

import { Button, Pagination, Table } from "antd";
//import { useSimpleStorage } from "../hooks/useLocalStorage";
import DocumentsTable from "../ui/documents-page/documents-table";
import { ColumnsType } from "antd/es/table";
import { Document } from "../models/Document";
import { DocumentDownloadLink } from "../ui/documents-page/download-btn";
import { useEffect, useState } from "react";
import { getDocuments } from "../services/documentsService";
import { errorMessages } from "../services/errorMessages";
import { UploadDocumentArea } from "../ui/documents-page/upload-area";
import { useSignalR } from "../signalR/SignalRContext";

export default function Page() {
    const [documents, setDocuments] = useState<Document[]>([])
    const [emitterInfo, setEmitterInfo] = useState<{
        Id: string, 
        Name: string, 
        AuthPerson: string,
        IssuerId: number}>
        ({Id: "", Name: "", AuthPerson: "", IssuerId: 0})

    //const { connection } = useSignalR();
    //const { startConnection } = useSignalR();

    const [uploadTableVis, setUploadTableVis] = useState<boolean>(false);

    useEffect(() => {
        onDocumentsUpdate()
    }, [])

    /*const subscribeOnDocumentsTableUpdate = async () => {
        const currentConnection = connection ? connection : await startConnection()

        currentConnection?.on('')
    } */

    const onDocumentsUpdate = async () => {
        const emitterJSON = localStorage.getItem('emitter')

        if (emitterJSON) {
            const emitter = JSON.parse(emitterJSON)
            setEmitterInfo(emitter)

            const documentsResponse = await getDocuments(emitter.Id)  
        
            if (documentsResponse?.ok) { // Если успешно получили все документы
                const documentsJson = await documentsResponse.json();
                setDocuments(documentsJson)
                console.log(documentsJson)
            } else if (documentsResponse?.status === 400) { 
                // если произошла ошибка при получении списка документов
                const error = await documentsResponse.json()
                //console.log(errorMessages[error[0].type])
                console.log(error)
            } else {
                console.error('Неизвестная ошибка при полуении списка документов по эмитенту')
            }
        }
        
    }

    const formatDate = (dateTime: string) : string => {
        const splitDate = dateTime.split('T');
        const date = splitDate[0];
        const splitTime = splitDate[1].split('.')
        const time = splitTime[0];

        return `${time} ${date}`
    }
    
    const columns : ColumnsType<Document> = [
        {
            title: 'Название',
            dataIndex: 'title',
            key: 'title',
            render: (value) => value.slice(0, 30) + '...'
        },
        {
            title: 'Тип файла',
            dataIndex: 'fileExtnsion',
            key: 'fileExtnsion',
        },
        {
            title: 'Дата загрузки',
            dataIndex: 'uploadDate',
            key: 'uploadDate',
            sorter: (a, b) => {
                // Преобразуем даты в timestamp и сравниваем
                const dateA = new Date(a.uploadDate).getTime();
                const dateB = new Date(b.uploadDate).getTime();
                return dateA - dateB;
              },
            sortDirections: ['descend', 'ascend'],
            render: (date) => formatDate(date)
        },
        {
            title: 'Размер файла',
            dataIndex: 'size',
            key: 'size',
        },
        {
            title: 'Отправитель',
            dataIndex: 'isEmitterSended',
            key: 'isEmitterSended',
            render: (value) => value ? 'Эмитент' : 'Регистратор'
        },
        {
            title: "Скачать",
            key: "download",
            render: (_, record) => (
              <DocumentDownloadLink documentInfo={record}  />
            ),
        },
    ]

    return (
        <main>
            {/* <div className="w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] py-[45px] px-[80px]" > */}
            <div className="w-[1104px] h-[1500px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] py-[45px] px-[80px]" >
                <p className="text-[34px]/[44px] mb-[25px]">Документы по эмитенту {emitterInfo.Name}</p>
                <div className="mb-[10px]" style={{ display: 'flex', justifyContent: 'flex-end' }}>
                    <Button type="primary" onClick={onDocumentsUpdate}>Обновить таблицу</Button>
                </div>
                <Table 
                    rowKey="id" 
                    dataSource={documents} 
                    columns={columns}
                />
                {/*<DocumentsTable /> */}
                {/*<Button type="primary" size="large" onClick={() => setUploadTableVis(true)}>Загрузить документы</Button>
                {
                    uploadTableVis && ( */}
                        <div>
                            <p className="text-[34px]/[44px] my-[25px]">Загруженные документы</p>
                            <UploadDocumentArea/>
                        </div>
                    
            </div>
        </main>
    );
}