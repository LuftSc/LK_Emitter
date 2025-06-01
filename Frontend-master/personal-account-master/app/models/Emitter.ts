import { EmitterInfo } from "./EmitterInfo"

export interface Emitter {
    id: string
    emitterInfo : EmitterInfo,
    issuerId: number
}

/*
int issuerId // id в ихней системе (числом),
string fullName // короткое название,
string shortName // полное название,
string inn // ИНН,
string jurisdiction // Юрисдикция (что-то типа названия страны (РОССИЯ)),
string OGRNNumber // номер ОГРН,
DateOnly OGRNDateOfAssignment // дата выдачи ОГРН, 
string OGRNIssuer // кем выдан ОГРН,
string registrationNumber // номер Гос. регистрации,
DateOnly registrationDate // дата регистрации, 
string registrationIssuer // наименовани органа регистрации,
string country // страна КАПСОМ, 
int index // Ккакой-то индекс,
string region // область,
string city // город,
string street // улица,
string homeNumber // номер дома
string mailCountry // страна из почтового адреса, 
int mailIndex, // индекс из почтового адреса
string mailRegion, // область из почтового адреса
string mailCity, // город из почтового адреса
string mailStreet, // улица из почтового адреса
string mailHomeNumber // номер дома из почтового адреса
string phoneNumber, // номер телефона
string fax, // ФАКС     
string email, // емаил
int okopf // ОКОПФ (числвой код)
bool isPersonalDocumentsReception, // галочка "Не разрешаю принимать документы почтой, только лично"
long authorizedCapital, // Уставной капитал
bool isInformationDisclosure, // галочка "Эмитент осуществляет раскрытие информации"
string methodGettingInfoFromRegistry, // способ получения информации из реестра
string meetNotifyXML // способы доведения информации об ОСА,
string bik, // БИК
string bankName, // Наименование банка
string settlementAccount, Р/с банка
string correspondentAccount, Корр/сч
string bankINN, ИНН банка
string department, Отделение
string customerAccount, Л\с (Р\с)
string taxBenefits, налоговые льготы
string bankCountry, страна (для иностранных банков)
string recipientName, наименвоание получателя
string recipientInn, ИНН получателя
string  recipientAssignment назначение получателя
string fieldOfActivity, сфера деятельности
string additionalInformation дополнительная информация
*/
