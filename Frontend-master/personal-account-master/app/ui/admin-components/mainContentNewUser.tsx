'use client'

import { useState } from "react"
import { InputAdmin } from "./inputAdmin"
import SelectForUserRoles from "./selectForUserRoles"
import ListOfEmitents from "./listOfEmitents"
import { Button } from "antd"

export default function MainContentNewUser() {

    const [email, setEmail] = useState<string>('')
    const [password, setPassword] = useState<string>('')
    const [role, setNewRole] = useState<string>('')
    const [fcs, setFcs] = useState<string>('')

    return (
        <div className="w-full flex flex-col items-center space-y-[20px]">
            <p className="text-[24px]">Создание нового пользователя</p>
            <div className="flex w-[550px] space-x-[30px]">
                <div className="w-[260px]">
                    <InputAdmin
                        placeholder="Введите электронную почту"
                        setState={setEmail}
                        errorText="Введен неверный электронный адрес"
                        validation={/@/}
                    />
                </div>
                <div className="w-[260px]">
                    <InputAdmin
                        placeholder="Введите пароль"
                        setState={setPassword}
                        errorText="Поле обязательно для заполнения"
                        validation={/./}
                    />
                </div>
            </div>
            <div className="flex w-[550px] space-x-[30px]">
                <div className="w-[260px]">
                    <InputAdmin
                        placeholder="Введите ФИО"
                        setState={setFcs}
                        errorText="Может принимать только текстовые значения"
                        validation={/^[а-яА-Я ]*$/} 
                    />
                </div>
                <div className="w-[260px]"><SelectForUserRoles setNewRole={setNewRole} /></div>
            </div>
            <ListOfEmitents newRole={role}/>
            <Button>Создать</Button>
        </div>
    )
}