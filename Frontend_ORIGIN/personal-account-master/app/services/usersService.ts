

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