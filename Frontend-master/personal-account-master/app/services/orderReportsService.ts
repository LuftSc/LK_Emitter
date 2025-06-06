import { ReportOrder } from "../models/ReportOrder";

export interface ListOSAReportGeneratingData {
    issuerId: number // код эмитента
    dtMod: string, // Дата фиксации с 1 формы | Строка ФОРМАТА: ГГГГ-ММ-ДД
    nomList: boolean, // Флажок на форме
    isCategMeeting: boolean, // флажок с формы 1
    isRangeMeeting: boolean, // флажок с формы 1
    // true - заседание\ false - заочное
    dt_Begsobr: string, // Дата проведения собрания с формы | Строка ФОРМАТА: ГГГГ-ММ-ДД
}

export interface ListOSASavingToDbData {
    stepOne: {
        listOfPeopleRightToParticipate: boolean // Первый чекбокс с "Список лиц, имеющих право на участие в общем собрании акционеров"
        listOfPeopleRightOnPapers: boolean // Второй чекбокс с "Список лиц, осуществляющих права по ценным бумагам"
        listOfPeopleRightToParticipateTwo: boolean // Третий чекбокс с "Список лиц, имеющих право на участие в общем собрании акционеров,  без персональных данных"
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

export interface RegistryInfoGeneratingData {
    emitId: number // код эмитента
    procUk: number // цифра из поля процентов на 2 странице формы
    nomList: boolean // флажок на раскрытие списков НД
    dtMod: string // Дата на которую необходимо предоставить информацию
    oneProcMode: boolean // флажок: "в сокращённой форме на дату"
}



export interface RegistryInfoSavingToDbData {
    stepOne: {
        fullName: string // полное наименование эмитента
    }
    stepTwo: {
        listPaperOwners: boolean // Чекбокс "Список владельцев ценных бумаг"
        infoPeopleWithOpenAccount: boolean // Радио "Информация о людях, которым открыт лицевой счет"
        listFundPersentageOwners: boolean // Чекбокс "Список лиц, владеющих % от Уставного капитала"
        certificateAboutStructure: boolean // Чекбокс "Справка о структуре распределения акций"
        includeInfoShown: boolean // Чекбокс "включая сведения о лицах..."
    }
}

export interface RequestInfoFromRegistry {
    forReportGenerating: RegistryInfoGeneratingData
    forDbSaving: RegistryInfoSavingToDbData
}

export interface ListOfEntitledGeneratingData {
    issuerId: number // код эмитента
    dtClo: string // Дата на которую необходимо предоставить информацию
}

export interface CertificateOnDate {
    dtMod: string // Дата на которую оформляется справка
    fcsName: string // Наименование/ФИО
    ogrnPassport: string // ОГРН/Паспорт
    another: boolean // Чекбокс на Иное
    anotherText: string // Иное
    section61: boolean // статья 6.1
    section51: boolean // статья 51
    section30: boolean // статья 30
    section20: boolean // статья 20
    section17: boolean // статья 17 
    anotherSection: boolean // Чекбокс на Иное после статей
    anotherSectionText: string // Иное после статей
    emitentRepresentative: string // Уполномоченный представитель
    isRegulationOrAttorney: boolean // 3 флажок Устав/Доверенность
    regulationNumber: number // номер Устава или Доверенности
}

export interface ListOfEntitledSavingToDbData {
    stepOne: {
        fullEmName: string // Полное наименование эмитента
        decidingAuthority: string // Орган, управления эмитента, принявший решение...
        dateOfProtocol: string // Дата протокола
        numberOfProtocol: number // Номер протокола
    }
    stepTwo: {
        papersCategory: string // Категории ценных бумаг
        formOfPayment: string // Форма выплат
        paymentForOne: string // Выплачиваемый доход на одну акцию
        paymentForAll: string // Выплачиваемый доход, общий объем
        dateOfPayment: string // Дата выплаты
        fullOfficialName: string // Полное оф. наименование агента(ов)
        placeOfAgents: string // Место нахождения агента(ов)
        mailAddress: string // Почтовый адрес агента(ов)
        includeCalcInfo: boolean // Включить инорфмацию о расчете налога
    }
    stepThree: {
        emitentRepresentative: string // Уполномоченный представитель
        isRegulationOrAttorney: boolean // 3 флажок Устав/Доверенность
        regulationNumber: number // номер Устава или Доверенности
    }
}

export interface RequestListOfEntitled {
    forReportGenerating: ListOfEntitledGeneratingData
    forDbSaving: ListOfEntitledSavingToDbData
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
    reportName: string,
    isSaveToStorage: boolean,
    issuerId: number,
    regOutInfo: string,
    generalReportHeader: string,
    typKls: string,
    dtMod: string,
    nomList: boolean,
    isPodr: boolean,
    viewCb: boolean,
    isCateg: boolean,
    isOneRecAllNomin: boolean,
    isCategMeeting: boolean,
    isRangeMeeting: boolean,
    dt_Begsobr: string,
    isSocr: boolean,
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
    currentUser: string,
    operator: string,
    controler: string,
    isViewDirect: boolean,
    isViewCtrl: boolean,
    isViewElecStamp: boolean,
    guid: string
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
    return await fetch(`http://localhost:5000/OrderReports/download-report-order/${request.idForDownload}?type=${request.type}`, {
        method: "GET",
        credentials: 'include'
    })
        .catch(error => console.error(error))
}

export const sendRequestListOfShareholders = async (request: any) => {
    await fetch('http://localhost:5000/OrderReports/list-of-shareholders', {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
}

export const sendRequestReeRep = async (request: any) => {
    await fetch('http://localhost:5000/OrderReports/ree-rep', {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
}

export const sendRequestDividendList = async (request: any) => {
    await fetch('http://localhost:5000/OrderReports/dividend-list', {
        method: 'POST',
        credentials: 'include',
        headers: {
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
export const getOrderReportsByEmitterId = async (issuerId: number, page: number, pageSize: number) => {
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

// ЛИСТ УЧАСТНИКОВ СОБРАНИЯ АКЦИОНЕРОВ

/*

requestListOSA: {
    forReportGenerating: {
        issuerId: string // код эмитента
        dtMod: string, // Дата фиксации с 1 формы | Строка ФОРМАТА: ГГГГ-ММ-ДД
        nomList: boolean, // Флажок на форме
        isCategMeeting: boolean, // флажок с формы 1
        isRangeMeeting: boolean, // флажок с формы 1 // true - заседание\ false - заочное
        dt_Begsobr: string, // Дата проведения собрания с формы | Строка ФОРМАТА: ГГГГ-ММ-ДД
    }
    forDbSaving: {
        stepOne: {
            listOfPeopleRightToParticipate: boolean // Первый чекбокс с "Список лиц, имеющих право на участие в общем собрании акционеров"
            listOfPeopleRightOnPapers: boolean // Второй чекбокс с "Список лиц, осуществляющих права по ценным бумагам"
            listOfPeopleRightToParticipateTwo: boolean // Третий чекбокс с "Список лиц, имеющих право на участие в общем собрании акционеров,  без персональных данных"
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
}

issuerId: string // код эмитента
dtMod: string, // Дата фиксации с 1 формы | Строка ФОРМАТА: ГГГГ-ММ-ДД
nomList: boolean, // Флажок на форме
isCategMeeting: boolean, // флажок с формы 1
isRangeMeeting: boolean, // флажок с формы 1 // true - заседание\ false - заочное
dt_Begsobr: string, // Дата проведения собрания с формы | Строка ФОРМАТА: ГГГГ-ММ-ДД
listOfPeopleRightToParticipate: boolean // Первый чекбокс с "Список лиц, имеющих право на участие в общем собрании акционеров"
listOfPeopleRightOnPapers: boolean // Второй чекбокс с "Список лиц, осуществляющих права по ценным бумагам"
listOfPeopleRightToParticipateTwo: boolean // Третий чекбокс с "Список лиц, имеющих право на участие в общем собрании акционеров,  без персональных данных"
isMeetingWillBeHeldByBoD: boolean // Флажок с 1 формы "Советом директоров"
mettingWillBeHeldBy: string // Строка под флажком, если "Советом директоров" true, 
meetingNumber: number // номер под инпутом с плейсхолдером "Введите наименование"
decisionDate: string
startRegistrationTime: string // Время начало регистрации
startMeetingTime: string // Время начало собрания
endRegistrationTime: string // Время окончания приема бюллетеней
endRegistrationDate: string // Дата окончания приема бюллетеней
meetingPlace: string // Место проведения собрания
isVotingPossible: boolean // флажок "Методы голосования"
addressFilledBallots: string // Адрес заполненных бюллетеней
fcs: string // ФИО
emailAddress: string // email
phoneNumber: string // номер телефона
infoReviewingProcedure: string // Порядок ознакомления с информацией
isParticipatingInVote: boolean // 1 флажок "В голосовании принимают участие.."
agendaNumber: number // Номер повестки дня
isParticipatingInVoteOnNumber: boolean // 2 флажок 
emitentRepresentative: string // Уполномоченный представитель
isRegulationOrAttorney: boolean // 3 флажок Устав/Доверенность
regulationNumber: number // номер Устава или Доверенности
*/