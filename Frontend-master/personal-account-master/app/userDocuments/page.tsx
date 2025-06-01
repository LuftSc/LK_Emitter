"use client"

import { Button, Pagination, Table } from "antd/es";
//import { useSimpleStorage } from "../hooks/useLocalStorage";
//import DocumentsTable from "../../ui/documents-page/documents-table";
import { ColumnsType } from "antd/es/table";
//import { Document } from "../../models/Document";
//import { DocumentDownloadLink } from "../../ui/documents-page/download-btn";
import { useEffect, useState } from "react";
import { getDocumentsByPageByIssuerId } from "../services/documentsService";
import { errorMessages } from "../services/errorMessages";
import { UploadDocumentArea } from "../ui/documents-page/upload-area";
import { useSignalR } from "../signalR/SignalRContext";
import { Role } from "../services/usersService";
import { Document } from "../models/Document";
import { DocumentDownloadLink } from "../ui/documents-page/download-btn";

export default function Page() {
    const [documents, setDocuments] = useState<Document[]>([])

    const [paginationDocuments, setPaginationDocuments] = useState<{
            totalSize: number, 
            documents:Document[]
        }>({totalSize: 0, documents:[]})
        
    const [emitterInfo, setEmitterInfo] = useState<{
        Id: string, 
        Name: string, 
        AuthPerson: string,
        IssuerId: number}>
        ({Id: "", Name: "", AuthPerson: "", IssuerId: 0})

    const [pagination, setPagination] = useState({
        current: 1,
        pageSize: 10
    })

    const [loading, setLoading] = useState<boolean>(false)

    const { connection } = useSignalR();
    const { startConnection } = useSignalR();

    //const { connection } = useSignalR();
    //const { startConnection } = useSignalR();

    const [uploadTableVis, setUploadTableVis] = useState<boolean>(false);

    useEffect(() => {
        const emitter = localStorage.getItem('emitter')
        const emitterData = emitter ? JSON.parse(emitter) : null

        if (emitterData) {
            setPaginationDocuments(prev => ({
                ...prev, documents: []
            }))
            setEmitterInfo(emitterData)

            //console.log("кидаем запрос на отчёты по id: " + emitterData.IssuerId)
            getDocumentsByPage(emitterData.IssuerId, pagination) 
            
            subscribeOnReceiveDocuments()
        }

        return () => {
            subscribeOnReceiveDocuments(true)
        };
    }, []) 

    useEffect(() => {
        setPagination(prev => ({...prev, total: paginationDocuments.totalSize}))
    }, [paginationDocuments])


    /*const subscribeOnDocumentsTableUpdate = async () => {
        const currentConnection = connection ? connection : await startConnection()

        currentConnection?.on('')
    } */
    const addDocuments = (
        receivedDocuments: Document[]) => {
        setPaginationDocuments(prev => {
            console.log("Received documents:", receivedDocuments);
        // Если мы на первой странице
            if (pagination.current === 1) {
                const newDocuments = [...receivedDocuments, ...prev.documents];
                
                // Обрезаем массив, если превысили pageSize
                if (newDocuments.length > pagination.pageSize) {
                    newDocuments.pop();
                }
                
                return {
                    totalSize: prev.totalSize + receivedDocuments.length,
                    documents: newDocuments
                };
            }
            
            // Если не на первой странице, просто увеличиваем totalSize
            return {
                ...prev,
                totalSize: prev.totalSize + receivedDocuments.length
            };
        });
    }

    const withDocumentsUploadAction = async () => {
        const currentConnection = connection ? connection : await startConnection()
        

        currentConnection?.off('ReceiveDocuments')

        await subscribeOnReceiveDocuments()
    }

    const subscribeOnReceiveDocuments = async (unSubscribe: boolean = false) => {
        const currentConnection = connection ? connection : await startConnection()
        const emitter = localStorage.getItem('emitter')
        const emitterData = emitter ? JSON.parse(emitter) : null

        await currentConnection?.invoke('EmitterSelected', emitterData.Id)


        if (unSubscribe) {
            currentConnection?.off('ReceiveDocuments')
        }
        else {
            currentConnection?.on("ReceiveDocuments", (documents: Document[]) => {
                console.log('получили доки по подписке')
                addDocuments(documents)
            });
        }
    }

    const getDocumentsByPage = async (issuerId: number, pagination: any) => {
        setLoading(true)
        setPagination(pagination)

        //await subscribeOnReceiveListReports()

        const documentsResponse = await 
            getDocumentsByPageByIssuerId(issuerId, pagination.current, pagination.pageSize)
        
        if (documentsResponse?.ok) {
            const docs = await documentsResponse.json();
            setPaginationDocuments(docs);
            setLoading(false)
            console.log(docs)
        } else if (documentsResponse?.status === 400) {
            console.error('контролируемая ошибка')
        } else {
            console.error('НЕконтролируемая ошибка')
        }
    } 

    /*const onDocumentsUpdate = async () => {
        const emitterJSON = localStorage.getItem('emitter')

        if (emitterJSON) {
            const emitter = JSON.parse(emitterJSON)
            setEmitterInfo(emitter)

            const documentsResponse = await getDocumentsByPageByIssuerId(emitter.Id)  
        
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
        
    } */

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
            render: (size) => `${size} КБ`
        },
        {
            title: 'Роль отправителя',
            dataIndex: 'senderRole',
            key: 'senderRole',
            render: (role: Role) => Role[role]
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
            <div className="w-[1104px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] py-[45px] px-[80px]" >
                <p className="text-[34px]/[44px] mb-[25px]">Документы по эмитенту {emitterInfo.Name}</p>
                {/*<div className="mb-[10px]" style={{ display: 'flex', justifyContent: 'flex-end' }}>
                    <Button type="primary" onClick={onDocumentsUpdate}>Обновить таблицу</Button>
                </div>*/}
                <Table 
                    rowKey="id" 
                    dataSource={paginationDocuments.documents} 
                    columns={columns}
                    loading={loading}
                    pagination={pagination}

                    onChange={(newPagination, filters, sorter) => getDocumentsByPage(emitterInfo.IssuerId, newPagination)}
                />
                {/*<DocumentsTable /> */}
                {/*<Button type="primary" size="large" onClick={() => setUploadTableVis(true)}>Загрузить документы</Button>
                { 
                    uploadTableVis && ( */}
                        <div>
                            <p className="text-[34px]/[44px] my-[25px]">Загруженные документы</p>
                            <UploadDocumentArea withUploadAction={withDocumentsUploadAction} />
                        </div>
            
            </div>
        </main>
    );
}