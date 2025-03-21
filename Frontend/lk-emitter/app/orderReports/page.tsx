"use client"

import { Button, List } from "antd";
import { useSignalR } from "../components/SignalRContext";
import { ListOfShareholders, RequestListOfShareholders, sendRequestListOfShareholders } from "../services/orderReports";
import { useState } from "react";
import { ReportOrderInfo } from "../models/ReportOrderInfo";
import { ReportOrderDownloadLink } from "../components/ReportOrderDownloadLink";

export default function OrderReports() {
    const [orderReports, setOrderReports] = useState<ReportOrderInfo[]>([])
    
    const { connection } = useSignalR();
    
    const requestListOfShareholders = async () => {
        if (connection) {
            connection.on('SendListOfShareholdersResult', async (documentId, requestDate:string) => {
                const dateAndTime = requestDate.split('.')
                
                setOrderReports(prevOrders => [...prevOrders, 
                    {
                        reportOrderId: documentId, 
                        fileName: `Лист участников собрания акционеров ${dateAndTime[0]}`
                }]);

                connection.off('SendListOfShareholdersResult');
            })
        }

        const request = {
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
            } as ListOfShareholders
        } as RequestListOfShareholders

        await sendRequestListOfShareholders(request);
    }
    
    return (
        <div>
            <Button onClick={requestListOfShareholders}>Запросить лист участников собрания</Button>
            <br />
            <Button>Запросить информацию из реестра</Button>
            <br />
            <Button> Запросить дивидендный список</Button>
            <List
                size="large"
                header={<p>Готовые распоряжения появляются здесь</p>}
                footer={<div>Это все распоряжения с этим эмитентом</div>}
                bordered
                dataSource={orderReports}
                renderItem={(item) => 
                    <List.Item>
                        {<ReportOrderDownloadLink
                            key={item.reportOrderId} // Уникальный ключ
                            reportOrderInfo={item}   // Передаем объект в компонент
                        />}
                    </List.Item>}
            />
        </div>
    )
}