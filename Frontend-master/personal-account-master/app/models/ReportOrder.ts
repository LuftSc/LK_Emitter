export interface ReportOrder {
    id: string,
    fileName: string,
    status: string,
    requestTime: string,
    idForDownload: string
}

export enum ReportOrderStatus {
    Successfull = 'Successfull',
    Processing = 'Processing',
    Failed = 'Failed'
}
/*Guid Id,
string FileName,
string Status,
DateTime RequestTime */