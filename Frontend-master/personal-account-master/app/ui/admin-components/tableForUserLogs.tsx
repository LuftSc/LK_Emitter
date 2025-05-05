"use client"

import { Button, Pagination, Table } from "antd/es";
import { ColumnsType } from "antd/es/table";
import { Document } from "@/app/models/Document";
import { DocumentDownloadLink } from "../documents-page/download-btn";
import { useEffect, useState } from "react";
import { getDocumentsByPageByIssuerId } from "@/app/services/documentsService";
import { useSignalR } from "@/app/signalR/SignalRContext";

interface Props {
    logsTableVis: boolean
}

export default function TableForUserLogs({logsTableVis}: Props) {

    const [documents, setDocuments] = useState<Document[]>([])

    const [paginationDocuments, setPaginationDocuments] = useState<{
        totalSize: number,
        documents: Document[]
    }>({ totalSize: 0, documents: [] })

    const [emitterInfo, setEmitterInfo] = useState<{
        Id: string,
        Name: string,
        AuthPerson: string,
        IssuerId: number
    }>
        ({ Id: "", Name: "", AuthPerson: "", IssuerId: 0 })

    const [pagination, setPagination] = useState({
        current: 1,
        pageSize: 10
    })

    const [loading, setLoading] = useState<boolean>(false)

    const { connection } = useSignalR();
    const { startConnection } = useSignalR();

    useEffect(() => {
        const emitter = localStorage.getItem('emitter')
        const emitterData = emitter ? JSON.parse(emitter) : null

        if (emitterData) {
            setPaginationDocuments(prev => ({
                ...prev, documents: []
            }))
            setEmitterInfo(emitterData)

            console.log("кидаем запрос на отчёты по id: " + emitterData.IssuerId)
            getDocumentsByPage(emitterData.IssuerId, pagination)

            subscribeOnReceiveDocuments()
        }

        return () => {
            subscribeOnReceiveDocuments(true)
        };
    }, [])

    useEffect(() => {
        setPagination(prev => ({ ...prev, total: paginationDocuments.totalSize }))
    }, [paginationDocuments])


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

    const formatDate = (dateTime: string): string => {
        const splitDate = dateTime.split('T');
        const date = splitDate[0];
        const splitTime = splitDate[1].split('.')
        const time = splitTime[0];

        return `${time} ${date}`
    }

    const columns: ColumnsType<Document> = [
        {
            title: 'Название',
            dataIndex: 'title',
            key: 'title',
            width: '3   0%',
            render: (value) => value.slice(0, 30) + '...'
        },
        {
            title: 'Тип файла',
            dataIndex: 'fileExtnsion',
            key: 'fileExtnsion',
            width: '20%',
        },
        {
            title: 'Дата загрузки',
            dataIndex: 'uploadDate',
            key: 'uploadDate',
            width: '20%',
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
            width: '15%',
            key: 'size',
        },
        {
            title: "Скачать",
            key: "download",
            render: (_, record) => (
                <DocumentDownloadLink documentInfo={record} />
            ),
        },
    ]
    return (
        <Table
            className={logsTableVis == true ? "w-full": "hidden"}
            rowKey="id"
            dataSource={paginationDocuments.documents}
            columns={columns}
            loading={loading}
            pagination={pagination}

            onChange={(newPagination, filters, sorter) => getDocumentsByPage(emitterInfo.IssuerId, newPagination)}
        />

    )
}