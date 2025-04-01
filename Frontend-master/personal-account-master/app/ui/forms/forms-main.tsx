'use client'

import Link from "next/link";
import { usePathname } from "next/navigation";
import clsx from 'clsx';
import { getOrderReportsByEmitterId, ListOfShareholders, RequestListOfShareholders, sendRequestListOfShareholders } from "@/app/services/orderReportsService";
import { Button, List } from "antd";
import { ReportOrder } from "@/app/models/ReportOrder";
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
    const [orderReports, setOrderReports] = useState<ReportOrder[]>([])
    
    const { connection } = useSignalR();
    const { startConnection } = useSignalR();

    const [emitterName, setEmitterName] = useState<string>("")
    
    

    useEffect(() => {
        
        const emitter = localStorage.getItem('emitter')
        const emitterData = emitter ? JSON.parse(emitter) : null
        

        if (emitterData) {
            setEmitterName(emitterData.Name)
            onReportOrdersTableUpdate(emitterData.Id)
        }
    }, [])

    const onReportOrdersTableUpdate = async (emitterId: string) => {
        console.log('зашли в обновление')
        const orderReportsResponse = await getOrderReportsByEmitterId(emitterId)

        if (orderReportsResponse?.ok) {
            const reports = await orderReportsResponse.json()
            console.log(reports)
            setOrderReports(reports)
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

    const onRequestListOSA = async () => {
        // Если соединения не установлено - устанавливаем
        const emitter = localStorage.getItem('emitter')
        const emitterData = emitter ? JSON.parse(emitter) : null

        const currentConnection = connection ? connection : await startConnection()
            
        currentConnection?.on('SendListOfShareholdersResult', async (documentId: string, requestDate:string) => {
            console.log('Мы зашли в подписанный метод!')
            const dateAndTime = requestDate.split('.')
            
            /*setOrderReports(prevOrders => [...prevOrders, 
                {
                    Id: documentId, 
                    fileName: `Лист участников собрания акционеров ${dateAndTime[0]}`,
                    status: 'Выполнено',
                    requestDate: requestDate
            }]); */ 
            await onReportOrdersTableUpdate(emitterData.Id);

            currentConnection.off('SendListOfShareholdersResult');
        })
        
        const defaultListOSRequest = {
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
    
        await sendRequestListOfShareholders(defaultListOSRequest)

        await onReportOrdersTableUpdate(emitterData.Id);
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

                <Table rowKey="id" columns={columns} dataSource={orderReports}
                    pagination={{
                        pageSize: 5, // Количество строк на странице
                        //showSizeChanger: true, // Показывать выбор количества строк
                        //pageSizeOptions: ['10', '20', '50', '100'], // Варианты выбора
                      }}
                />
                
            </div>
        </div>
    );
}