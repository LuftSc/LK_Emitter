export interface LoginUserRequest {
    email: string,
    password: string
}

export const loginUser = async (request: LoginUserRequest) => {
    await fetch('https://localhost:7233/Users/login-user-without-2fa',{
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
    .catch(error => console.error('Login failed: ' + error))
}

export const getCurrentUserId = async () => {
    await fetch('https://localhost:7233/Users/get-current-user', {
        credentials: 'include'
    })
    .then(response => response.json())
    .then(userId => {
        console.log('CurrentUserID: ' + userId)
    })
    .catch(error => console.error('Get current user failed: ' + error))
}