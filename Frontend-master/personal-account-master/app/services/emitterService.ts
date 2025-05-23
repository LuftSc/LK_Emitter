import { Emitter } from "../models/Emitter";
import { EmitterInfo } from "../models/EmitterInfo";

export interface RegisterEmitterRequest {
    emitterInfo : EmitterInfo
    issuerId: number
}


export const registerEmitter = async (request: RegisterEmitterRequest) => {
    return await fetch('http://localhost:5000/Emitters/register', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(request)
        })
        .catch(error => console.error(error))
}

export const searchEmitters = async (searchTerm: string) => {
    return await fetch(`http://localhost:5000/Users/search-emitters?searchTerm=${encodeURIComponent(searchTerm)}`, {
            credentials: 'include'
        })
        .catch(error => console.error(error))
}

