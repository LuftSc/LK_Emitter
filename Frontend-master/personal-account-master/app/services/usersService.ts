

export interface LoginUserRequest{
    email: string,
    password: string
}

export interface ConfirmationCodeRequest {
    email: string,
    confiramtionCode: string
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

export const getUserEmitters = async () => {
    return await fetch('http://localhost:5000/Users/get-emitters', {
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