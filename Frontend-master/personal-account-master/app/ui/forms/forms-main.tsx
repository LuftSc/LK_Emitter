'use client'

import Link from "next/link";
import { usePathname } from "next/navigation";
import clsx from 'clsx';
import { DividendListRequest, getAllOrderReportsByEmitterId, getOrderReportsByEmitterId, ListOfShareholdersRequest, ReeRepRequest, RequestDividendList_OLD, RequestListOfShareholders, RequestListOfShareholders_OLD, RequestReeRep_OLD, sendRequestDividendList, sendRequestListOfShareholders, sendRequestReeRep } from "@/app/services/orderReportsService";
import { Button, List } from "antd";
import { ReportOrder, ReportOrderStatus } from "@/app/models/ReportOrder";
import { useEffect, useState } from "react";
import { ReportOrderDownloadLink } from "./reportOrder-dwn-btn";
import { useSignalR } from "@/app/signalR/SignalRContext";
import Table, { ColumnsType } from "antd/es/table";
import { HubConnection } from "@microsoft/signalr";

const links = [
    { name: 'Распоряжение Эмитента на список к ОСА', href: '/forms/first/step-one' },
    { name: 'Распоряжение Эмитента на предоставление информации из реестра', href: '/forms/second/step-one' },
    { name: 'Распоряжение Эмитента о предоставлении Списка лиц , имеющих право на получение доходов по ценным бумагам', href: '/forms/third/step-one' },
    { name: 'Справка о состоянии лицевого счета зарегистрированного лица на определенную дату', href: '/forms/fourth' },
];

export default function FormsMain() {
    const [orderReports, setOrderReports] = useState<{
        totalSize: number,
        orderReports: ReportOrder[]
    }>({ totalSize: 0, orderReports: [] })

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

        await subscribeOnReceiveListReports()
        //await subscribeOnReceiveReport()

        const orderReportsResponse = await
            getOrderReportsByEmitterId(issuerId, pagination.current, pagination.pageSize)

        if (orderReportsResponse?.ok) {
            console.log('Отправили запрос на отчёты')

        } else if (orderReportsResponse?.status === 400) {
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
                const newReports = [reportOrder, ...prev.orderReports];

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

        if (currentConnection)
            console.log(isSubscribed(currentConnection, 'ReceiveReport'))

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

    const onRequestDividendList = async () => {
        // Если соединения не установлено - устанавливаем
        const emitter = localStorage.getItem('emitter')
        const emitterData = emitter ? JSON.parse(emitter) : null

        await subscribeOnReceiveReport();

        if (emitterData) {
            const defaultDividendListRequest = {
                requestData: {
                    reportName: "",
                    issuerId: emitterData.IssuerId,
                    divPtr: 0,
                    isPodr: false,
                    isBr: false,
                    typPers: "",
                    postMan: "",
                    isGroupTypNal: false,
                    isBirthday: false,
                    isRate: false,
                    isOrderCoowner: false,
                    isPostMan: false,
                    regOutInfo: "",
                    generalReportHeader: "",
                    dtClo: "2000-01-01",
                    isAnnotation: false,
                    isPrintNalog: false,
                    isEstimationoN: false,
                    isExcelFormat: false,
                    isViewGenDirect: false,
                    isViewPrintUk: false,
                    isViewInn: false,
                    isViewOgrn: false,
                    isViewAddress: false,
                    printDt: false,
                    operator: "",
                    controler: "",
                    isViewCtrl: false,
                    isViewElecStamp: false,
                    guid: ""
                } as DividendListRequest
            } as RequestDividendList_OLD
            console.log(defaultDividendListRequest)
            await sendRequestDividendList(defaultDividendListRequest)
        }
    }

    const onRequestReeRep = async () => {
        const emitter = localStorage.getItem('emitter')
        const emitterData = emitter ? JSON.parse(emitter) : null

        const currentConnection = connection ? connection : await startConnection()

        await subscribeOnReceiveReport();

        if (emitterData) {
            const defaultReeRepRequest = {
                requestData: {
                    reportName: "",
                    isSaveToStorage: false,
                    emitId: emitterData.IssuerId,
                    svipId: 0,
                    categ: "",
                    fields: "",
                    filter: "",
                    numStoc: 0,
                    procUk: 0,
                    dtMod: "2000-01-01", // Особое условие
                    isPodr: 0,
                    isCateg: 0,
                    nomList: 0,
                    isZalog: 0,
                    isNullSch: 0,
                    estimation1: 0,
                    estimation2: 0,
                    isNotOblig: 0,
                    isFillSchNd: 0,
                    isFullAnketa: 0,
                    isViewBorn: 0,
                    typeReport: 0,
                    isExcludeListZl: 0,
                    listZl: "",
                    isBr: 0,
                    isControlModifyPerson: 0,
                    isTrustManager: 0,
                    isPawnGolos: 0,
                    isPawnDivid: 0,
                    isIssuerAccounts: 0,
                    isEmissionAccounts: 0,
                    isViewPhone: 0,
                    isViewEmail: 0,
                    corporateId: "",
                    isClosedAccount: 0,
                    isViewMeetNotify: 0,
                    oneProcMode: false,
                    isBenef: 0,
                    isAgent: 0,
                    procCat: 0,
                    isReestr: false,
                    operator: "",
                    controler: "",
                    isViewCtrl: false,
                    isViewGenDirect: false,
                    isViewUk: false,
                    isZl: false,
                    isViewInn: false,
                    isPcateg: false,
                    isCheckGroupCb: false,
                    isViewDirect: false,
                    viewGroupCb: "",
                    diagn: "",
                    printDt: "",
                    strParams: "",
                    isRiskEst: false,
                    spisZl: "",
                    isPrintDtRegIssueOfSecurities: false,
                    guid: "",
                    isPrintUk: false,
                    generalReportHeader: "",
                    regOutInfo: "",
                    isViewElecStamp: false,
                    currentUser: ""
                } as ReeRepRequest
            } as RequestReeRep_OLD

            console.log(defaultReeRepRequest)

            await sendRequestReeRep(defaultReeRepRequest)
        }
    }
    const onRequestListOSA = async () => {
        const emitter = localStorage.getItem('emitter')
        const emitterData = emitter ? JSON.parse(emitter) : null

        const currentConnection = connection ? connection : await startConnection()

        await subscribeOnReceiveReport();

        const defaultListOSRequest = {
            requestData: {
                reportName: "string",
                isSaveToStorage: true,
                issuerId: emitterData ? emitterData.IssuerId : 9999,
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
            } as ListOfShareholdersRequest
        } as RequestListOfShareholders_OLD

        console.log(defaultListOSRequest)
        await sendRequestListOfShareholders(defaultListOSRequest)
    }

    const columns: ColumnsType<ReportOrder> = [
        {
            title: 'Тип распоряжения',
            dataIndex: 'fileName',
            key: 'fileName',
            width: 300,
        },
        {
            title: 'Дата запроса',
            dataIndex: 'requestDate',
            key: 'requestDate',
            render: (date) => formatDate(date),
            width: 150
        },
        {
            title: 'Статус выполнения',
            dataIndex: 'status',
            key: 'status',
            render: (status) => formatStatus(status),
            width: 100
        },
        {
            title: "Ссылка для скачивания",
            key: "download",
            render: (_, record) => (
                <ReportOrderDownloadLink reportOrder={record} />
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
            <div className="mb-[54px]">
                <h2 className="text-xl/[26px] font-bold mb-5">5.3 Распоряжения на предоставление информации Эмитенту</h2>
                <Link
                    key={links[1].name}
                    href={links[1].href}
                    className={clsx(pathname === links[1].href)}>
                    <p className="text-[#B82116] text-base/[21px] font-bold mb-[18px]">{links[1].name}</p>
                </Link>
                <Link
                    key={links[2].name}
                    href={links[2].href}
                    className={clsx(pathname === links[2].href)}>
                    <p className="text-[#B82116] text-base/[21px] font-bold">{links[2].name}</p>
                </Link>
            </div>
            <div>
                <h2 className="text-xl/[26px] font-bold mb-5">Справки о состоянии лицевого счета</h2>
                <Link
                    key={links[3].name}
                    href={links[3].href}
                    className={clsx(pathname === links[3].href)}>
                    <p className="text-[#B82116] text-base/[21px] font-bold mb-[18px]">{links[3].name}</p>
                </Link>
            </div>
            <div className="mt-[54px] mr-[50px]">
                <h2 className="text-xl/[26px] font-bold mb-5">Распоряжения по эмитенту {emitterName}</h2>
                <Button onClick={onRequestListOSA}>Запросить тестовый лист участников собрания</Button>
                <br />
                <Button onClick={onRequestReeRep}>Запросить информацию из реестра</Button>
                <br />
                <Button onClick={onRequestDividendList}> Запросить дивидендный список</Button>

                <Table rowKey="internalId" columns={columns} dataSource={orderReports.orderReports}
                    pagination={pagination}
                    loading={loading}
                    onChange={(newPagination, filters, sorter) => getOrderReportsByPage(emitterId, newPagination)}
                />

            </div>
        </div>
    );
}