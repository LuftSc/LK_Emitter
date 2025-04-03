import { ReportOrder } from "../models/ReportOrder";

export interface RequestListOfShareholders {
    requestData : ListOfShareholders
    emitterId: string
}

export interface ListOfShareholders {
    reportName : string,
    isSaveToStorage : boolean,
    issuerId : number,
    regOutInfo : string,
    generalReportHeader : string,
    typKls : string,
    dtMod : string,
    nomList : boolean,
    isPodr: boolean,
    viewCb: boolean,
    isCateg: boolean,
    isOneRecAllNomin: boolean,
    isCategMeeting: boolean,
    isRangeMeeting: boolean,
    dt_Begsobr : string,
    isSocr : boolean,
    isFillSchNd: boolean,
    isBirthday: boolean,
    isViewPhone: boolean,
    isViewEmail: boolean,
    isViewMeetNotify: boolean,
    isViewGenDirect: boolean,
    isViewInn: boolean,
    viewLs: boolean,
    isSignBox: boolean,
    offNumbers: boolean,
    isExcelFormat: boolean,
    currentUser : string,
    operator : string,
    controler : string,
    isViewDirect : boolean,
    isViewCtrl: boolean,
    isViewElecStamp: boolean,
    guid : string
}

export const downloadReportOrder = async (request: ReportOrder) => {
    return await fetch(`http://localhost:5000/OrderReports/download-report-order/${request.idForDownload}`, {
        method: "GET",
        credentials: 'include'
    })
    .catch(error => console.error(error))
}

export const sendRequestListOfShareholders = async (request: RequestListOfShareholders) => {
    await fetch('http://localhost:5000/OrderReports/list-of-shareholders', {
        method: 'POST',
        credentials: 'include',
        headers:{
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
}

export const getAllOrderReportsByEmitterId = async (emitterId: string) => {
    return await fetch(`http://localhost:5000/OrderReports/get-all-report-orders/${emitterId}`, {
        method: "GET",
        credentials: 'include'
    })
    .catch(error => console.error(error))
}
//http://localhost:5000/OrderReports/get-report-orders/3fa85f64-5717-4562-b3fc-2c963f66afa6?Page=1&PageSize=10
export const getOrderReportsByEmitterId = async (emitterId: string, page:number, pageSize: number) => {
    return await fetch(`http://localhost:5000/OrderReports/get-report-orders/${emitterId}?Page=${page}&PageSize=${pageSize}`, {
        method: "GET",
        credentials: 'include'
    })
    .catch(error => console.error(error))
}