'use client'

import { CloseCircleFilled } from "@ant-design/icons"
import { InputAdmin } from "./inputAdmin"
import { useState } from "react"
import { Button } from "antd"

interface Props {
    visPopUp: boolean
    setVisPopUp: React.Dispatch<React.SetStateAction<boolean>>
}

export default function EditUserPopUp({ visPopUp, setVisPopUp }: Props) {

    const [email, setEmail] = useState<string>('')
    const [password, setPassword] = useState<string>('')
    const [fcs, setFcs] = useState<string>('')

    if (!visPopUp) return null

    const onClickSave = (e: any) => {
        setVisPopUp(false)
    }

    return (
        <div className="fixed top-0 left-0 flex justify-center items-center w-screen h-screen backdrop-blur-sm">
            <div className="flex flex-col items-center w-[664px] h-[447px] border-[#f9f9f9] rounded-[28px] border-[0.5px] bg-[#FFF]/85 p-[30px] shadow">
                <div className="w-full flex justify-end"><CloseCircleFilled onClick={(e) => setVisPopUp(false)} className="cursor-pointer" /></div>
                <p className="text-[28px] mb-[30px]">Редактирование информации пользователя</p>
                <div className="flex flex-col space-y-[30px] mb-[40px]">
                    <div className="w-[300px]">
                        <p>Введите электронную почту</p>
                        <InputAdmin
                            placeholder=""
                            setState={setEmail}
                            errorText="Введен неверный электронный адрес"
                            validation={/@/}
                        />
                    </div>
                    <div className="w-[300px]">
                        <p>Введите пароль</p>
                        <InputAdmin
                            placeholder=""
                            setState={setPassword}
                            errorText="Поле обязательно для заполнения"
                            validation={/./}
                        />
                    </div>
                    <div className="w-[300px]">
                        <p>Введите ФИО</p>
                        <InputAdmin
                            placeholder=""
                            setState={setFcs}
                            errorText="Может принимать только текстовые значения"
                            validation={/^[а-яА-Я ]*$/}
                        />
                    </div>
                </div>
                <Button onClick={onClickSave}>Сохранить</Button>
            </div>
        </div>
    )
}