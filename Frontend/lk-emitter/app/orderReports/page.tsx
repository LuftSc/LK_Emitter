import { Button } from "antd";
import { useSignalR } from "../components/SignalRContext";
import { ListOfShareholders, RequestListOfShareholders, sendRequestListOfShareholders } from "../services/orderReports";

export default function OrderReports() {
    const { connection } = useSignalR();
    
    const requestListOfShareholders = async () => {
        if (connection) {
            connection.on('SendListOfShareholdersResult', async (documentId, requestDate) => {
                console.log(documentId)
                console.log(requestDate)
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
        </div>
    )
}