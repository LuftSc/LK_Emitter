'use client'

import { useState } from "react"
import CardForUserInfo from "./cardForUserInfo"
import SearchForAdmin from "./searchForSecondPage"
import ListOfEmitents from "./listOfEmitents"

export default function MainContentSecond() {

    const [userName, setUserName] = useState<string>('')
    const [newRole, setNewRole] = useState<string>('')

    return (
        <div className="flex flex-col items-center">
            <SearchForAdmin setUserName={setUserName}/>
            <CardForUserInfo userName={userName} setNewRole={setNewRole}/>
            <ListOfEmitents newRole={newRole}/>
        </div>
    )
}