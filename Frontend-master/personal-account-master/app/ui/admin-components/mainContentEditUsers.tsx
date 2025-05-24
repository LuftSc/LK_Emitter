'use client'

import { useState } from "react"
import CardForUserInfo from "./cardForUserInfo"
import ListOfEmitents from "./listOfEmitents"
import SearchForUsers from "./searchForUsers"
import { UserWithEmitters } from "@/app/models/UserWithEmitters"
import { Role } from "@/app/services/usersService"
import SelectSearchUsers from "./selectSearchUsers"
import { Emitter } from "@/app/models/Emitter"

export default function MainContentEditUsers() {

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
            <p className="text-[24px]">Редактирование информации существующих пользователей</p>
            <SelectSearchUsers user={selectedUser} setSelectedUser={setSelectedUser}/>
            {/* <SearchForUsers setUserName={setUserName}/> */}
            <CardForUserInfo user={selectedUser} handleAddEmitters={addEmitters} handleDeleteEmitter={deleteEmitter}/>
            {/* <ListOfEmitents newRole={newRole}/> */}
        </div>
    )
}