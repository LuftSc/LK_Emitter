import { ReportOrder } from "../models/ReportOrder";

/*const request = {
    forReportGenerating: {
        issuerId: emitterData.IssuerId, // код эмитента
        dtMod: dtMod, // Дата фиксации с 1 формы
        nomList: showLists, // Флажок на форме
        isCategMeeting: example, // флажок с формы 1
        isRangeMeeting: isRangeMeeting, // флажок с формы 1
        // true - заседание\ false - заочное
        dt_Begsobr: dtBegSobr, // Дата проведения собрания с формы
    }
    //reportName: "ListOfMeetingShareholdersCb",
    //isSaveToStorage: true,
    issuerId: emitterData.IssuerId, // код эмитента
    //regOutInfo: "1/ИСХ",
    //generalReportHeader: "Список лиц, имеющих право голоса при принятии решений общим собранием акционеров",
    //typKls: "",
    dtMod: dtMod, // Дата фиксации с 1 формы
    nomList: showLists, // Флажок на форме
    //isPodr: false,
    //viewCb: true,
    //isCateg: false,
    //isOneRecAllNomin: false,
    isCategMeeting: example, // флажок с формы 1
    // true - годовое\ false - внеочередное

    isRangeMeeting: isRangeMeeting, // флажок с формы 1
    // true - заседание\ false - заочное
    dt_Begsobr: dtBegSobr, // Дата проведения собрания с формы
    //isSocr: false,
    //isFillSchNd: false,
    //isBirthday: false,
    //isViewPhone: true,
    //isViewEmail: true,
    //isViewMeetNotify: true,
    //isViewGenDirect: false,
    //isViewInn: false,
    //viewLs: false,
    //isSignBox: false,
    //offNumbers: false,
    //isExcelFormat: false,
    //printDt: false,
    //currentUser: "LK",
    //operator: "Кузнецов А. С.",
    //controler: "",
    //isViewDirect: false,
    //isViewCtrl: false,
    //isViewElecStamp: true,
    //guid: "" // Пустое, я на бэке его генерю
      } */

export interface ListOSAReportGeneratingData {
    issuerId: string // код эмитента
    dtMod: string, // Дата фиксации с 1 формы | Строка ФОРМАТА: ГГГГ-ММ-ДД
    nomList: boolean, // Флажок на форме
    isCategMeeting: boolean, // флажок с формы 1
    isRangeMeeting: boolean, // флажок с формы 1
    // true - заседание\ false - заочное
    dt_Begsobr: string, // Дата проведения собрания с формы | Строка ФОРМАТА: ГГГГ-ММ-ДД
}

export interface ListOSASavingToDbData {
    stepOne: {
        isMeetingWillBeHeldByBoD: boolean // Флажок с 1 формы "Советом директоров"
        mettingWillBeHeldBy: string // Строка под флажком, если "Советом директоров" true, 
        // то она будет пустая, если нет, то тут должно быть название органа, 
        // который инициировал проведенеи собрания

        meetingNumber: number // номер под инпутом с плейсхолдером "Введите наименование"
        decisionDate: string
    }
    stepTwo: {
        startRegistrationTime: string
        endRegistrationTime: string
    }
}

export interface RequestListOfShareholders {
    forReportGenerating: ListOSAReportGeneratingData
    forDbSaving: ListOSASavingToDbData
}

export interface ReeRepRequest {
    reportName: string;
    isSaveToStorage: boolean;
    emitId: number;
    svipId: number;
    categ: string;
    fields: string;
    filter: string;
    numStoc: number;
    procUk: number; 
    dtMod: string; 
    isPodr: number;
    isCateg: number;
    nomList: number;
    isZalog: number;
    isNullSch: number;
    estimation1: number;
    estimation2: number;
    isNotOblig: number;
    isFillSchNd: number;
    isFullAnketa: number;
    isViewBorn: number;
    typeReport: number;
    isExcludeListZl: number;
    listZl: string;
    isBr: number;
    isControlModifyPerson: number;
    isTrustManager: number;
    isPawnGolos: number;
    isPawnDivid: number;
    isIssuerAccounts: number;
    isEmissionAccounts: number;
    isViewPhone: number;
    isViewEmail: number;
    corporateId: string;
    isClosedAccount: number;
    isViewMeetNotify: number;
    oneProcMode: boolean;
    isBenef: number;
    isAgent: number;
    procCat: number;
    isReestr: boolean;
    operator: string;
    controler: string;
    isViewCtrl: boolean;
    isViewGenDirect: boolean;
    isViewUk: boolean;
    isZl: boolean;
    isViewInn: boolean;
    isPcateg: boolean;
    isCheckGroupCb: boolean;
    isViewDirect: boolean;
    viewGroupCb: string;
    diagn: string;
    printDt: string;
    strParams: string;
    isRiskEst: boolean;
    spisZl: string;
    isPrintDtRegIssueOfSecurities: boolean;
    guid: string;
    isPrintUk: boolean;
    generalReportHeader: string;
    regOutInfo: string;
    isViewElecStamp: boolean;
    currentUser: string;
}

export interface DividendListRequest {
    reportName: string;
    issuerId: number;
    divPtr: number;
    isPodr: boolean;
    isBr: boolean;
    typPers: string;
    postMan: string;
    isGroupTypNal: boolean;
    isBirthday: boolean;
    isRate: boolean;
    isOrderCoowner: boolean;
    isPostMan: boolean;
    regOutInfo: string;
    generalReportHeader: string;
    dtClo: string; 
    isAnnotation: boolean;
    isPrintNalog: boolean;
    isEstimationoN: boolean; 
    isExcelFormat: boolean;
    isViewGenDirect: boolean;
    isViewPrintUk: boolean;
    isViewInn: boolean;
    isViewOgrn: boolean;
    isViewAddress: boolean;
    printDt: boolean; 
    operator: string;
    controler: string; 
    isViewCtrl: boolean;
    isViewElecStamp: boolean;
    guid: string; 
}

export interface ListOfShareholdersRequest {
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

export interface RequestListOfShareholders_OLD {
    requestData: ListOfShareholdersRequest
}

export interface RequestDividendList_OLD {
    requestData: DividendListRequest
}

export interface RequestReeRep_OLD {
    requestData: ReeRepRequest
}

export const downloadReportOrder = async (request: ReportOrder) => {
    return await fetch(`http://localhost:5000/OrderReports/download-report-order/${request.idForDownload}`, {
        method: "GET",
        credentials: 'include'
    })
    .catch(error => console.error(error))
}

// ПОМЕНЯТЬ!!!!!!!!!!!
export const sendRequestListOfShareholders = async (request: RequestListOfShareholders_OLD) => {
    await fetch('http://localhost:5000/OrderReports/list-of-shareholders', {
        method: 'POST',
        credentials: 'include',
        headers:{
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
}

export const sendRequestReeRep = async (request: RequestReeRep_OLD) => {
    await fetch('http://localhost:5000/OrderReports/ree-rep', {
        method: 'POST',
        credentials: 'include',
        headers:{
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
}

export const sendRequestDividendList = async (request: RequestDividendList_OLD) => {
    await fetch('http://localhost:5000/OrderReports/dividend-list', {
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
export const getOrderReportsByEmitterId = async (issuerId: number, page:number, pageSize: number) => {
    return await fetch(`http://localhost:5000/OrderReports/get-report-orders/${issuerId}?Page=${page}&PageSize=${pageSize}`, {
        method: "GET",
        credentials: 'include'
    })
    .catch(error => console.error(error))
}


    
    /*reportName : string,
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
    guid : string */