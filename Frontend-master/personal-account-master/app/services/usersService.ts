import { PassportInfo } from "../models/User"

export enum Role {
    User = 1,
    Emitter = 2,
    Registrator = 3,
    Admin = 4
}

export interface LoginUserRequest{
    email: string,
    password: string
}

export interface ConfirmationCodeRequest {
    email: string,
    confiramtionCode: string
}

export interface RegisterUserRequest {
    email: string,
    password: string,
    emittersGuids?: string[],
    role: number,
    fullName: string,
    phone: string
}

export interface BindUserToEmittersRequest {
    userId: string,
    emittersIdList: string[]
}

export interface UpdateUserDataRequest {
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

export const loginUser = async (request: LoginUserRequest) => {
    return await fetch('http://localhost:5000/Users/login-user', {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
    .catch(error => console.error(error))
}

export const registerUser = async (request: RegisterUserRequest) => {
    return await fetch('http://localhost:5000/Users/register', {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
    .catch(error => console.error(error))
}

export const loginUserWithout2FA = async (request: LoginUserRequest) => {
    return await fetch('http://localhost:5000/Users/login-user-without-2fa', {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
    .catch(error => console.error(error))
}

export const verifyCode = async (request: ConfirmationCodeRequest) => {
    return await fetch('http://localhost:5000/Users/verify-code', {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
    .catch(error => console.error(error))
}

export const getUserEmitters = async (page: number, pageSize:number) => {
    return await fetch(`http://localhost:5000/Users/get-binding-emitters?page=${page}&pageSize=${pageSize}`, {
        credentials: 'include'
    })
    .catch(error => console.error(error))
}

export const getCurrentUser = async () => {
    return await fetch('http://localhost:5000/Users/get-current-user', {
        credentials: "include"
    })
    .catch(error => {
        console.error(error)
    })
}

export const updateUserData = async (request: UpdateUserDataRequest) => {
return await fetch('http://localhost:5000/Users/update', {
        credentials: "include",
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
    .catch(error => {
        console.error(error)
    })
}

export const getCurrentUserPersonalData = async () => {
    return await fetch('http://localhost:5000/Users/get-personal-data-current', {
        credentials: "include"
    })
    .catch(error => {
        console.error(error)
    })
}

export const searchUsers = async (searchTerm: string) => {
    return await fetch(`http://localhost:5000/Users/search-users?searchTerm=${encodeURIComponent(searchTerm)}`, {
            credentials: 'include'
        })
        .catch(error => console.error(error))
}

export const bindUserToEmitters = async (request: BindUserToEmittersRequest) => {
    return await fetch('http://localhost:5000/Users/bind-to-emitters', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        })
        .catch(error => console.error(error))
}

export const unbindUserFromEmitter = async (userId: string, emitterId:string) => {
    return await fetch(`http://localhost:5000/Users/unbind-from-emitter?userId=${userId}&emitterId=${emitterId}`, {
            method: 'DELETE',
            credentials: 'include'
        })
        .catch(error => console.error(error))
}