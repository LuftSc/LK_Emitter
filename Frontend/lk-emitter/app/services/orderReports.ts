export interface RequestListOfShareholders {
    requestData : ListOfShareholders
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

export const sendRequestListOfShareholders = async (request: RequestListOfShareholders) => {
    await fetch('https://localhost:7233/OrderReports/list-of-shareholders', {
        method: 'POST',
        headers:{
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
}