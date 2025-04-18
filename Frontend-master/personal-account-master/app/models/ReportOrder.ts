/*export interface ReportOrder {
    id: string,
    fileName: string,
    status: string,
    requestTime: string,
    idForDownload: string
} */

export interface ReportOrder {
    idForDownload: string,
    internalId: string,
    fileName: string,
    status: number,
    requestDate: string,
    userId: string
}


export enum ReportOrderStatus {
    Successfull = 1,
    Processing = 2,
    Failed = 3
}
/*Guid Id,
string FileName,
string Status,
DateTime RequestTime */