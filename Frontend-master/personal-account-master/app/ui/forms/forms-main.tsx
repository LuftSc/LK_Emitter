'use client'

import Link from "next/link";
import { usePathname } from "next/navigation";
import clsx from 'clsx';
import { getAllOrderReportsByEmitterId, getOrderReportsByEmitterId, RequestListOfShareholders, sendRequestListOfShareholders } from "@/app/services/orderReportsService";
import { Button, List } from "antd";
import { ReportOrder, ReportOrderStatus } from "@/app/models/ReportOrder";
import { useEffect, useState } from "react";
import { ReportOrderDownloadLink } from "./reportOrder-dwn-btn";
import { useSignalR } from "@/app/signalR/SignalRContext";
import Table, { ColumnsType } from "antd/es/table";

const links = [
    {name: 'Распоряжение Эмитента на список к ОСА', href: '/forms/first/step-one'},
    {name: 'Распоряжение Эмитента на предоставление информации из реестра', href: '/forms/second/step-one'},
    {name: 'Распоряжение Эмитента о предоставлении Списка лиц , имеющих право на получение доходов по ценным бумагам', href: '/forms/third/step-one'},
];

export default function FormsMain () {
    //const [orderReports, setOrderReports] = useState<ReportOrder[]>([])
    const [orderReports, setOrderReports] = useState<{
        totalSize: number, 
        orderReports:ReportOrder[]
    }>({totalSize: 0, orderReports:[]})

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
        //onRequestListOSA() !
        
        const emitter = localStorage.getItem('emitter')
        const emitterData = emitter ? JSON.parse(emitter) : null

        if (emitterData) {
            setEmitterName(emitterData.Name)
            console.log(emitterData.AuthPerson)
            //onReportOrdersTableUpdate(emitterData.Id)
            //onGetReportOrders(); !
            //getOrderReportsByPage(emitterData.IssuerId, pagination) !
            setEmitterId(emitterData.IssuerId)
        }
    }, [])

    useEffect(() => {
        setPagination(prev => ({...prev, total: orderReports.totalSize}))
        console.log('обновился размер')
    }, [orderReports])

    const getOrderReportsByPage = async (issuerId: number, pagination: any) => {
        setLoading(true)
        //console.log('зашли в обновление')
        setPagination(pagination)
        const orderReportsResponse = await 
            getOrderReportsByEmitterId(issuerId, pagination.current, pagination.pageSize)

        if (orderReportsResponse?.ok) {
            //const reports = await orderReportsResponse.json()
            console.log('Отправили запрос на отчёты')
            /*setOrderReports({
                totalSize: reports.totalSize,
                orderReports: reports.orderReports
            }) */
        } else if (orderReportsResponse?.status === 400) {
            console.error('контролируемая ошибка')
        } else {
            console.error('НЕконтролируемая ошибка')
        }

        
    } 

    const formatDate = (dateTime: string) : string => {
        const splitDate = dateTime.split('T');
        const date = splitDate[0];
        const splitTime = splitDate[1].split('.')
        const time = splitTime[0];

        return `${time} ${date}`
    }

    const onGetReportOrders = async () => {
        const currentConnection = connection ? connection : await startConnection()

        currentConnection?.on('ReceiveReports', (orderReports: {totalSize: number, 
            orderReports:ReportOrder[]}) => {
                setOrderReports(orderReports)
                setLoading(false)
            })

        currentConnection?.off('ReceiveReports')
    }

    const onRequestListOSA = async () => {
        // Если соединения не установлено - устанавливаем
        //const emitter = localStorage.getItem('emitter')
        //const emitterData = emitter ? JSON.parse(emitter) : null

        const currentConnection = connection ? connection : await startConnection()

        currentConnection?.on('ReceiveListOfShareholdersResult', 
            (documentId: string, status: string, requestDate: string, idForDownload: string) => {

            if (status === ReportOrderStatus.Successfull) {
                updateReportOrder(documentId, {status: status, idForDownload: idForDownload} )
                currentConnection.off('ReceiveListOfShareholdersResult');
            } else {
                addReportOrder(documentId, status, requestDate, idForDownload, 'Лист участников собрания акционеров')
            }
        })

        const updateReportOrder = (id: string, updatedData: Partial<ReportOrder>) => {
            setOrderReports(prevState => ({
                ...prevState,
                orderReports: prevState.orderReports.map(report => 
                  report.id === id ? { ...report, ...updatedData } : report
                )
              }));
          };
    
        const addReportOrder = (documentId: string, status: string, requestDate: string, idForDownload: string, fileName: string) => {
            setOrderReports(prev => {
            // Если мы на первой странице
            if (pagination.current === 1) {
                const newReports = [{
                id: documentId, 
                fileName: fileName,
                status: status,
                requestTime: requestDate,
                idForDownload: idForDownload
            }, ...prev.orderReports];
                
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
        
        /*const defaultListOSRequest = {
            requestData: {
                reportName: "string",
                isSaveToStorage: true,
                issuerId: 0,
                regOutInfo: "string",
                generalReportHeader: "string",
                typKls: "string",
                dtMod: "2025-03-18",
                nomList: true,
                isPodr: true,
                viewCb: true,
                isCateg: true,
                isOneRecAllNomin: true,
                isCategMeeting: true,
                isRangeMeeting: true,
                dt_Begsobr: "2025-03-18",
                isSocr: true,
                isFillSchNd: true,
                isBirthday: true,
                isViewPhone: true,
                isViewEmail: true,
                isViewMeetNotify: true,
                isViewGenDirect: true,
                isViewInn: true,
                viewLs: true,
                isSignBox: true,
                offNumbers: true,
                isExcelFormat: true,
                printDt: true,
                currentUser: "string",
                operator: "string",
                controler: "string",
                isViewDirect: true,
                isViewCtrl: true,
                isViewElecStamp: true,
                guid: "string"
            } as ListOfShareholders,
            emitterId: emitterData.Id
        } as RequestListOfShareholders
    
        await sendRequestListOfShareholders(defaultListOSRequest) */
    }

    const columns : ColumnsType<ReportOrder> = [
        {
            title: 'Тип распоряжения',
            dataIndex: 'fileName',
            key: 'fileName',
            width: 300,
        },
        {
            title: 'Дата запроса',
            dataIndex: 'requestTime',
            key: 'requestTime',
            sorter: (a, b) => {
                // Преобразуем даты в timestamp и сравниваем
                const dateA = new Date(a.requestTime).getTime();
                const dateB = new Date(b.requestTime).getTime();
                return dateA - dateB;
              },
            sortDirections: ['descend', 'ascend'],
            render: (date) => formatDate(date),
            width: 150
        },
        {
            title: 'Статус выполнения',
            dataIndex: 'status',
            key: 'status',
            width: 100
        },
        {
            title: "Ссылка для скачивания",
            key: "download",
            render: (_, record) => (
                <ReportOrderDownloadLink reportOrder={record}  />
            ),
            width: 200
        }
    ]

    const pathname = usePathname();
    return (
        <div className="w-[1104px] h-[1200px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] pl-[71px] pt-[68px]">
            <div className="mb-[54px]">
                <h2 className="text-xl/[26px] font-bold mb-5">5.2 Распоряжения на предоставление информации Эмитенту для общего собрания акционеров</h2>
                <Link
                    key={links[0].name}
                    href={links[0].href}
                    className={clsx(pathname === links[0].href)}>
                    <p className="text-[#B82116] text-base/[21px] font-bold">{links[0].name}</p>
                </Link>
            </div>
            <div>
                <h2 className="text-xl/[26px] font-bold mb-5">5.3 Распоряжения на предоставление информации Эмитенту</h2>
                <div className="mb-[18px]">
                    <Link
                        key={links[1].name}
                        href={links[1].href}
                        className={clsx(pathname === links[1].href)}>
                        <p className="text-[#B82116] text-base/[21px] font-bold">{links[1].name}</p>
                    </Link>
                </div>
                <Link
                    key={links[2].name}
                    href={links[2].href}
                    className={clsx(pathname === links[2].href)}>
                    <p className="text-[#B82116] text-base/[21px] font-bold">{links[2].name}</p>
                </Link>
            </div>
            <div className="mt-[54px] mr-[50px]">
                <h2 className="text-xl/[26px] font-bold mb-5">Распоряжения по эмитенту {emitterName}</h2>
                <Button onClick={onRequestListOSA}>Запросить тестовый лист участников собрания</Button>
                <br />
                <Button>Запросить информацию из реестра</Button>
                <br />
                <Button> Запросить дивидендный список</Button>

                <Table rowKey="id" columns={columns} dataSource={orderReports.orderReports}
                    pagination={pagination}
                    loading={loading}
                    onChange={(newPagination, filters, sorter) => getOrderReportsByPage(emitterId, newPagination)}
                />
                
            </div>
        </div>
    );
}