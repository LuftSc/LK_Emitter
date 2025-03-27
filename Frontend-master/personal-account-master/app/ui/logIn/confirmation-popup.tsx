'use client'

import { useState } from "react";
import LoginBtn from "./login-btn";
import { ConfirmationInput } from "./confirmation-login";
import { ConfirmationCodeRequest, verifyCode } from "@/app/services/usersService";
import { errorMessages } from "@/app/services/errorMessages";

interface Props {
    email: string,
    visCon: boolean,
    setVisCon: React.Dispatch<React.SetStateAction<boolean>>
    onLoginSuccess: () => void;
}

export default function ConfirmationForm ({email, visCon, setVisCon, onLoginSuccess}: Props) {
    const [code, setCode] = useState<string>('')

    if(!visCon) return null;

    const onClick = async () => {
        const request = {
            email: email,
            confiramtionCode: code
        } as ConfirmationCodeRequest

        const response = await verifyCode(request)

        if (response?.ok) { // Если код подтверждения верный
            setVisCon(false)
            // Отправляем коллбэк в родительский компонент
            // о том, что аутентификация завершилась успешно
            onLoginSuccess()
        } else if (response?.status === 400) { // Если ошибка возникла в процессе выполнения логики
            const error = await response?.json()
            console.log(errorMessages[error[0].type])
        } else { // Если какая-то другая ошибка
            console.error('Неизвестная ошибка')
        }
    }

    return (
        <div className="fixed top-0 left-0 flex justify-center items-center w-screen h-screen backdrop-blur-sm">
            <div className="flex flex-col items-center w-[664px] h-[447px] border-black rounded-[28px] border-[0.5px] bg-[#F1F1F1]/75 pt-[58px]">
                <p className="text-[36px]/[47px] mb-[10px]">Введите код подтвеждения</p>
                <p className=" max-w-[500px] text-[24px]/[26px] text-center mb-[20px]">На почту: "{email}" был отправлен шестизначный код</p>
                <div className="mb-[48px]"><ConfirmationInput value={code} setValue={setCode} placeholder="Код"/></div>
                <div className="mb-[27px]" onClick={onClick}><LoginBtn /></div>
            </div>
        </div>
    );
}