
export interface Emitter {
    id: string
    emitterInfo : EmitterInfo
}

interface EmitterInfo {
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

/* Пример JSON
{
    "id": "77213b79-6a63-4a69-8b0b-64ec652baa98",
    "emitterInfo": {
    "fullName": "ОТКРЫТОЕ АКЦИОНЕРНОЕ ОБЩЕСТВО \"СЕРВЕЛАТКОНГРАТУЛАТ\"",
    "shortName": "ОАО \"СЕРВЕЛАТКОНГРАТУЛАТ\"",
    "inn": "922447711009",
    "jurisdiction": "РОССИЯ",
    "ogrn": {
        "number": "21110033399922",
        "dateOfAssignment": "2013-10-11",
        "issuer": "ИФНС РОССИИ ПО ЛЕНИНСКОМУ Р-НУ Г. ЕКАТЕРИНБУРГА"
    },
    "registration": {
        "number": null,
        "registrationDate": "0001-01-01",
        "issuer": null
    }    
}
*/