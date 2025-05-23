import { Role } from "../services/usersService"

// export interface User {
//     id: string,
//     fullName: string,
//     email: string,
//     phone: string,
//     birthDate: string,
//     passport: PassportInfo,
//     role: Role
// }

export interface User {
    id: string,
    name: string,
    surname: string,
    patronymic: string,
    email: string,
    phone: string,
    birthDate: string,
    passport: PassportInfo,
    role: Role
}

export interface PassportInfo {
    series: string,
    number: string,
    dateOfIssuer: string,
    issuer: string,
    unitCode: string
}