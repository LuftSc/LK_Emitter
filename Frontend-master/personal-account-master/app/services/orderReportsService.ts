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
        listOfPeopleRightToParticipate: string // Первый чекбокс с "Список лиц, имеющих право на участие в общем собрании акционеров"
        listOfPeopleRightOnPapers: string // Второй чекбокс с "Список лиц, осуществляющих права по ценным бумагам"
        listOfPeopleRightToParticipateTwo: string // Третий чекбокс с "Список лиц, имеющих право на участие в общем собрании акционеров,  без персональных данных"
        isMeetingWillBeHeldByBoD: boolean // Флажок с 1 формы "Советом директоров"
        mettingWillBeHeldBy: string // Строка под флажком, если "Советом директоров" true, 
        // то она будет пустая, если нет, то тут должно быть название органа, 
        // который инициировал проведенеи собрания

        meetingNumber: number // номер под инпутом с плейсхолдером "Введите наименование"
        decisionDate: string
    }
    stepTwo: {
        startRegistrationTime: string // Время начало регистрации
        startMeetingTime: string // Время начало собрания
        endRegistrationTime: string // Время окончания приема бюллетеней
        endRegistrationDate: string // Дата окончания приема бюллетеней
        meetingPlace: string // Место проведения собрания
        isVotingPossible: boolean // флажок "Методы голосования"
        addressFilledBallots: string // Адрес заполненных бюллетеней
    }
    stepThree: {
        fcs: string // ФИО
        emailAddress: string // email
        phoneNumber: string // номер телефона
        infoReviewingProcedure: string // Порядок ознакомления с информацией
    }
    stepFour: {
        isParticipatingInVote: boolean // 1 флажок "В голосовании принимают участие.."
        agendaNumber: number // Номер повестки дня
        isParticipatingInVoteOnNumber: boolean // 2 флажок 
        emitentRepresentative: string // Уполномоченный представитель
        isRegulationOrAttorney: boolean // 3 флажок Устав/Доверенность
        regulationNumber: number // номер Устава или Доверенности
    }
}

export interface RequestListOfShareholders {
    forReportGenerating: ListOSAReportGeneratingData
    forDbSaving: ListOSASavingToDbData
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