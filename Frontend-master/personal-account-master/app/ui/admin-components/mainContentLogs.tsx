'use client'

import TableForUserLogs from "./tableForUserLogs"
import { useEffect, useState } from "react";
import { Button } from "antd";
import FiltersForLogs from "./filtersForLogs";
import { ActionsReport } from "@/app/models/ActionsReport";
import { useSignalR } from "@/app/signalR/SignalRContext";
import { getActionsReportsList } from "@/app/services/usersService";

export default function MainContentLogs() {

    const [loading, setLoading] = useState<boolean>(false)
    // const [orderReports, setOrderReports] = useState<{
    //         totalSize: number,
    //         orderReports: ReportOrder[]
    //     }>({ totalSize: 0, orderReports: [] })
    useEffect(() => {
        
        const getActionsReports = async () => {
            setLoading(true)
            const response = await getActionsReportsList()
            if (response?.ok) {
                const reports = await response.json();
                setActionsReports(reports)
                setLoading(false)
            } else if (response?.status === 400) {
                console.error('контролируемая ошибка')
            } else {
                console.error('НЕконтролируемая ошибка')
            }
        }

        getActionsReports()
    }, [])

    const [logsTableVis, setLogsTableVis] = useState<boolean>(false);
    const [actionsReports, setActionsReports] = useState<ActionsReport[]>([])

    const addActionsReport = async (actionsReport: ActionsReport) => {
       
        setActionsReports(prev => [actionsReport, ...prev]);
    }

    // const addReportOrder = (
    //         reportOrder: ReportOrder) => {
    //         setOrderReports(prev => {
    //             // Если мы на первой странице
    //             if (pagination.current === 1) {
    //                 const newReports = 
    //                     !prev.orderReports.some(item => item.internalId === reportOrder.internalId) 
    //                         ? [reportOrder, ...prev.orderReports] 
    //                         : prev.orderReports;
    //                 // Обрезаем массив, если превысили pageSize
    //                 if (newReports.length > pagination.pageSize) {
    //                     newReports.pop();
    //                 }
    
    //                 return {
    //                     totalSize: prev.totalSize + 1,
    //                     orderReports: newReports
    //                 };
    //             }
    
    //             // Если не на первой странице, просто увеличиваем totalSize
    //             return {
    //                 ...prev,
    //                 totalSize: prev.totalSize + 1
    //             };
    //         });
    //     }

    return (
        <div className="w-full flex flex-col items-center space-y-[20px]">
            <p className="text-[24px]">Получение сведений о проведенных пользователями операций</p>
            <FiltersForLogs handleOnAdd={addActionsReport}/>
            <TableForUserLogs logsTableVis={true} actionsReports={actionsReports} loading={loading}/>
        </div>
    )
}