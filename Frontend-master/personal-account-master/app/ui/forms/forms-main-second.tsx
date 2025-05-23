'use client'

import { getOrderReportsByEmitterId } from "@/app/services/orderReportsService";
import { ReportOrder, ReportOrderStatus } from "@/app/models/ReportOrder";
import { useEffect, useState } from "react";
import { ReportOrderDownloadLink } from "./reportOrder-dwn-btn";
import { useSignalR } from "@/app/signalR/SignalRContext";
import Table, { ColumnsType } from "antd/es/table";
import { HubConnection } from "@microsoft/signalr";

export default function FormsMainSecond() {
    const [orderReports, setOrderReports] = useState<{
        totalSize: number,
        orderReports: ReportOrder[]
    }>({ totalSize: 0, orderReports: [] })

    // const [paginationDocuments, setPaginationDocuments] = useState<{
    //             totalSize: number, 
    //             documents:Document[]
    //         }>({totalSize: 0, documents:[]})

    const { connection } = useSignalR();
    const { startConnection } = useSignalR();

    const [emitterName, setEmitterName] = useState<string>("")
    const [emitterId, setEmitterId] = useState<number>(0);

    const [loading, setLoading] = useState(false);
    const [pagination, setPagination] = useState({
        current: 1,
        pageSize: 5
    });

    useEffect(() => {

        const emitter = localStorage.getItem('emitter')
        const emitterData = emitter ? JSON.parse(emitter) : null

        if (emitterData) {
            setOrderReports(prev => ({
                ...prev, orderReports: []
            }))
            console.log(emitterId)
            setEmitterName(emitterData.Name)
            setEmitterId(emitterData.IssuerId)

            console.log("кидаем запрос на отчёты по id: " + emitterData.IssuerId)
            getOrderReportsByPage(emitterData.IssuerId, pagination)

        }
    }, [])

    useEffect(() => {
        setPagination(prev => ({ ...prev, total: orderReports.totalSize }))
        console.log('обновился размер')
    }, [orderReports])

    function isSubscribed(connection: HubConnection, methodName: string): boolean {
        // @ts-ignore - доступ к внутреннему хранилищу
        const handlers = connection._handlers;
        return handlers && handlers[methodName]?.length > 0;
    }

    const getOrderReportsByPage = async (issuerId: number, pagination: any) => {
        setLoading(true)
        setPagination(pagination)

        //await subscribeOnReceiveListReports()
        await subscribeOnReceiveReport()

        const response = await
            getOrderReportsByEmitterId(issuerId, pagination.current, pagination.pageSize)

        if (response?.ok) {
            const orderReports = await response.json();
            setOrderReports(orderReports)
            
            // const docs = await documentsResponse.json();
            // setPaginationDocuments(docs);
            setLoading(false)

        } else if (response?.status === 400) {
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

    const formatStatus = (status: number): string => {
        if (ReportOrderStatus.Successfull === status) return "Выполнено";
        else if (ReportOrderStatus.Processing === status) return "В процессе"
        else return "Возникла ошибка";
    }

    const updateReportOrder = (id: string, updatedData: Partial<ReportOrder>) => {
        setOrderReports(prevState => ({
            ...prevState,
            orderReports: prevState.orderReports.map(report =>
                report.internalId === id ? { ...report, ...updatedData } : report
            )
        }));
    };

    const addReportOrder = (
        reportOrder: ReportOrder) => {
        setOrderReports(prev => {
            // Если мы на первой странице
            if (pagination.current === 1) {
                const newReports = 
                    !prev.orderReports.some(item => item.internalId === reportOrder.internalId) 
                        ? [reportOrder, ...prev.orderReports] 
                        : prev.orderReports;
                // Обрезаем массив, если превысили pageSize
                if (newReports.length > pagination.pageSize) {
                    newReports.pop();
                }

                return {
                    totalSize: prev.totalSize + 1,
                    orderReports: newReports
                };
            }

            // Если не на первой странице, просто увеличиваем totalSize
            return {
                ...prev,
                totalSize: prev.totalSize + 1
            };
        });
    }

    const subscribeOnReceiveReport = async () => {

        const currentConnection = connection ? connection : await startConnection()

        //if (currentConnection)
           // console.log( `подписка: ${isSubscribed(currentConnection, 'ReceiveReport')}` )

        currentConnection?.on('ReceiveReport',
            (orderReport: ReportOrder) => {
                if (orderReport.status === ReportOrderStatus.Successfull) {
                    updateReportOrder(orderReport.internalId, {
                        status: orderReport.status,
                        idForDownload: orderReport.idForDownload
                    })
                    currentConnection.off('ReceiveReport');
                } else if (orderReport.status === ReportOrderStatus.Failed) {
                    updateReportOrder(orderReport.internalId, {
                        status: orderReport.status
                    })
                    currentConnection.off('ReceiveReport');
                } else {
                    addReportOrder(orderReport)
                }
            })
    }

    const subscribeOnReceiveListReports = async () => {
        const currentConnection = connection ? connection : await startConnection()

        currentConnection?.on('ReceiveReports', (orderReports: {
            totalSize: number,
            orderReports: ReportOrder[]
        }) => {
            setOrderReports(orderReports)
            setLoading(false)
            console.log(orderReports)
            currentConnection?.off('ReceiveReports')
        })

        currentConnection?.on('ReceiveReport',
            (orderReport: ReportOrder) => {
                if (orderReport.status === ReportOrderStatus.Successfull) {
                    updateReportOrder(orderReport.internalId, {
                        status: orderReport.status,
                        idForDownload: orderReport.idForDownload
                    })
                    currentConnection.off('ReceiveReport');
                } else if (orderReport.status === ReportOrderStatus.Failed) {
                    updateReportOrder(orderReport.internalId, {
                        status: orderReport.status
                    })
                    currentConnection.off('ReceiveReport');
                } else {
                    addReportOrder(orderReport)
                }
            })
    }

    const columns: ColumnsType<ReportOrder> = [
        {
            title: 'Тип распоряжения',
            dataIndex: 'fileName',
            key: 'fileName',
            width: '30%',
        },
        {
            title: 'Дата запроса',
            dataIndex: 'requestDate',
            key: 'requestDate',
            render: (date) => formatDate(date),
            width: '20%',
        },
        {
            title: 'Статус выполнения',
            dataIndex: 'status',
            key: 'status',
            render: (status) => formatStatus(status),
            width: '20%',
        },
        {
            title: "Ссылка для скачивания",
            key: "download",
            render: (_, record) => (
                <ReportOrderDownloadLink reportOrder={record} />
            ),
            width: '30%',
        }
    ]

    return (
        <div className="border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[25px] px-[60px] py-[60px]">
            <div className="">
                <h2 className="text-xl/[26px] font-bold mb-5">Распоряжения по эмитенту {emitterName}</h2>

                <Table rowKey="internalId" columns={columns} dataSource={orderReports.orderReports}
                    pagination={pagination}
                    loading={loading}
                    onChange={(newPagination, filters, sorter) => getOrderReportsByPage(emitterId, newPagination)}
                />

            </div>
        </div>
    );
}