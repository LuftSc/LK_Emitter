'use client'

import { useState } from "react";
import { InputLogin } from "./login-input";
import LoginBtn from "./login-btn";
import { loginUser, LoginUserRequest, loginUserWithout2FA } from "@/app/services/usersService";
import { errorMessages } from "@/app/services/errorMessages";

interface Props {
    email: string,
    setEmail: React.Dispatch<React.SetStateAction<string>>,
    password: string,
    setPassword: React.Dispatch<React.SetStateAction<string>>,
    visLog: boolean,
    setVisLog: React.Dispatch<React.SetStateAction<boolean>>,
    setVisCon: React.Dispatch<React.SetStateAction<boolean>>
    onLoginSuccess: () => void;
}

export default function LogInForm ({email, setEmail, password, setPassword, visLog, setVisLog, setVisCon, onLoginSuccess}: Props) {

    if(!visLog) return null
    
    const onClick = async () => {
        const loginRequest = {
            email: email,
            password: password
        } as LoginUserRequest

        // ЗАМЕНИТЬ!!!! (иСПОЛЬЗУЕТСЯ ДЛЯ РАЗРАБОТКИ)
        //const response = await loginUserWithout2FA(loginRequest)
        const response = await loginUser(loginRequest)

        if (response?.ok) { // Случай, когда запрос выполнился успешно
            // Если пользователь найден и его пароль верный
            console.log('Всё ОК!')
            
            setVisLog(false) 
            // РАСКОММЕНТИРОВАТЬ !!!! (иСПОЛЬЗУЕТСЯ ДЛЯ РАЗРАБОТКИ)
            setVisCon(true)
            // ЗАКОММЕНТИРОВАТЬ И УБРАТЬ ИЗ ПРОПСОВ !!!! (иСПОЛЬЗУЕТСЯ ДЛЯ РАЗРАБОТКИ)
            //onLoginSuccess()
            
        } else if (response?.status === 400){ // Какая-то ошибка в процессе выполнения логики
            // Тут можно подсвечивать ошибку на форме
            const error = await response?.json()
            // Сам текст ошибки: errorMessages[error[0].type]
            console.log(errorMessages[error[0].type])
        } else {
            // На случай неизвестных ошибок, например, если
            // бэкэнд недоступен(не запущен) и мы пытаемся отправлять туда запросы
            console.error('Неизвестная ошибка')
        }
    }

    return (
        <div className="fixed top-0 left-0 flex justify-center items-center w-screen h-screen backdrop-blur-sm">
            <div className="flex flex-col items-center w-[664px] h-[447px] border-black rounded-[28px] border-[0.5px] bg-[#F1F1F1]/75 pt-[58px]">
                <p className="text-[36px]/[47px] mb-[54px]">Личный кабинет эмитента</p>
                <div className="mb-[22px]"><InputLogin value={email} setValue={setEmail} placeholder="Логин*" /></div>
                <div className="mb-[48px]"><InputLogin value={password} setValue={setPassword} placeholder="Пароль*"/></div>
                <div className="mb-[27px]" onClick={onClick} ><LoginBtn /></div>
                <p className="text-[20px]/[26px]">Забыл(а) пароль</p>
            </div>
        </div>
    )
}