export interface EmitterInfo {
    fullName: string
    shortName: string
    inn: string
    jurisdiction: string
    ogrn: OGRN
    registration: Registration
}

interface OGRN {
    number: string
    dateOfAssignment: string
    issuer: string
}

interface Registration {
    number: string
    registrationDate: string
    issuer: string
}