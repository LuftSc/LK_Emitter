'use client'

import { useState } from "react"
import { InputAdmin } from "./inputAdmin"
import SelectForUserRoles from "./selectForUserRoles"
import { Button } from "antd"
import { registerUser, RegisterUserRequest, Role } from "@/app/services/usersService"
import SelectSearchEmitters from "./selectSearchEmitters"

export default function MainContentNewUser() {

    const [email, setEmail] = useState<string>('')
    const [password, setPassword] = useState<string>('')
    const [role, setNewRole] = useState<Role>(Role.User)
    const [selectedEmittersGuids, setSelectedEmittersGuids] = useState<string[]>([]);
    const [fcs, setFcs] = useState<string>('')
    const [phoneNumber, setPhoneNumber] = useState<string>('')

    const onUserRegistration = async () => {
        const request = {
            email: email,
            password: password,
            emittersGuids: selectedEmittersGuids,
            role: role,
            fullName: fcs,
            phone: phoneNumber
        } as RegisterUserRequest

        console.log(request)

        const response = await registerUser(request)

        
    }

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
                        placeholder="Введите ФИО"
                        setState={setFcs}
                        errorText="Может принимать только текстовые значения"
                        validation={/^[а-яА-Я ]*$/} 
                    />
        
                </div>
            </div>
            <div className="flex w-[550px] space-x-[30px]">
                <div className="w-[260px]">
                    <InputAdmin
                        placeholder="Введите пароль"
                        setState={setPassword}
                        errorText="Поле обязательно для заполнения"
                        validation={/./}
                    />
                </div>
                <div className="w-[260px]">
                    <InputAdmin
                        placeholder="Введите номер телефона"
                        setState={setPhoneNumber}
                        errorText="Некорректный номер"
                        validation={/^[0-9]*$/} 
                    />
                </div>
            </div>
            <div className="w-[260px]"><SelectForUserRoles setNewRole={setNewRole} /></div>
            {/*<ListOfEmitents newRole={role}/> */}
            <SelectSearchEmitters setSelectedEmittersGuid={setSelectedEmittersGuids} role={role}/>
            <Button onClick={onUserRegistration}>Создать</Button>
        </div>
    )
}