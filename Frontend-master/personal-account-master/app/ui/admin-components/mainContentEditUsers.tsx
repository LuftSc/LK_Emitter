'use client'

import { useState } from "react"
import CardForUserInfo from "./cardForUserInfo"
import SearchForAdmin from "./searchForSecondPage"
import ListOfEmitents from "./listOfEmitents"

export default function MainContentEditUsers() {

    const [userName, setUserName] = useState<string>('')
    const [newRole, setNewRole] = useState<string>('')

    return (
        <div className="w-full flex flex-col items-center">
            <p className="text-[24px] mb-[20px]">Редактирование информации существующих пользователей</p>
            <SearchForAdmin setUserName={setUserName}/>
            <CardForUserInfo userName={userName} setNewRole={setNewRole}/>
            <ListOfEmitents newRole={newRole}/>
        </div>
    )
}