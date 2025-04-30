'use client'

import { useState } from "react"
import CardForUserInfo from "./cardForUserInfo"
import SearchForAdmin from "./searchForSecondPage"

export default function MainContent() {

    const [userName, setUserName] = useState<string>('')

    return (
        <div className="flex flex-col items-center">
            <SearchForAdmin setUserName={setUserName}/>
            <CardForUserInfo userName={userName} />
        </div>
    )
}