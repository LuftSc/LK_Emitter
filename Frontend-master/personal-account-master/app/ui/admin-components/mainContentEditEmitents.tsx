'use client'

import { useState } from "react"
import { UserWithEmitters } from "@/app/models/UserWithEmitters"
import { Role } from "@/app/services/usersService"
import { Emitter } from "@/app/models/Emitter"
import SelectSearchEmitters from "./selectSearchEmitters"
import CardForEmitentInfo from "./cardForEmitentInfo"

export default function MainContentEditEmitents() {

    const [emitents, setEmitents] = useState<string[]>(['ООО "УКЗ"','ООО "МФС"'])

    const defaultUserValues = {
        id: '',
        fullName: '',
        email: '',
        phone: '',
        birthDate: '',
        passport: {series: '', number: '', dateOfIssuer: '', issuer: '', unitCode: ''},
        role: Role.User,
        emitters: []
    } as UserWithEmitters
    
    const [selectedUser, setSelectedUser] = useState<UserWithEmitters>(defaultUserValues)
    
    const addEmitters = (newEmitters: Emitter[]) => {
        setSelectedUser(prev => ({
            ...prev,
            emitters: [...prev.emitters, ...newEmitters]
        }));
    };

    const deleteEmitter = (emitterGuid: string) => {
        setSelectedUser(prev => ({
            ...prev,
            emitters: prev.emitters.filter(e => e.id !== emitterGuid)
        }))
    }

    return (
        <div className="w-full flex flex-col items-center space-y-[30px]">
            <p className="text-[24px]">Редактирование информации организаций-эмитентов</p>
            <SelectSearchEmitters role={Role.Emitter} setSelectedEmittersGuid={setEmitents}/>
            <CardForEmitentInfo/>
        </div>
    )
}