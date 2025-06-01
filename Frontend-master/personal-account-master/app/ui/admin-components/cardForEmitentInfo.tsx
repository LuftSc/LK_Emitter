"use client"

import React, { useState } from 'react';
import { InputAdmin } from './inputAdmin';
import Calendar from '../forms/calendar-new';


export default function CardForEmitentInfo() {

    const [userName, setUserName] = useState<string>('')
    const [userEmail, setUserEmail] = useState<string>('')
    const [userPhone, setUserPhone] = useState<string>('')
    const [userId, setUserId] = useState<string>('')
    const [userBirthDate, setUserBirthDate] = useState<string>('')
    const [passSerial, setPassSerial] = useState<string>('')
    const [passDate, setPassDate] = useState<string>('')


    return (
        <>
            <div className='flex flex-col border-[0.5px] w-[600px] rounded-xl border-[#f0f0f0] py-[25px]'>
                <div className='flex flex-col items-center w-full space-y-[10px] px-[60px]'>
                    <p className='text-[28px]'>Основные данные</p>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Полное название:</span>
                        <InputAdmin errorText='' validation={/./} setState={setUserName} placeholder={'Введите полное название'} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Сокращенное наименование:</span>
                        <InputAdmin errorText='' validation={/./} setState={setUserEmail} placeholder={'Введите сокращенное наименование'} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Номер ИНН:</span>
                        <InputAdmin errorText='' validation={/./} setState={setUserPhone} placeholder={'Введите номер ИНН'} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Юрисдикция:</span>
                        <InputAdmin errorText='' validation={/./} setState={setUserId} placeholder={'Введите Юрисдикцию'} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Код эмитента:</span>
                        <InputAdmin errorText='' validation={/./} setState={setUserBirthDate} placeholder={'Введите код эмитента'} />
                    </div>
                    <p className='text-[28px]'>Данные ОГРН:</p>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Кем выдан:</span>
                        <InputAdmin errorText='' validation={/./} setState={setPassSerial} placeholder={'Введите кем выдан'} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span>Дата выдачи:</span>
                        <Calendar setDate={setUserEmail}/>
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Номер:</span>
                        <InputAdmin errorText='' validation={/./} setState={setPassDate} placeholder={'Введите номер'} />
                    </div>
                    <p className='text-[28px]'>Данные Регистрации:</p>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Кем выдан:</span>
                        <InputAdmin errorText='' validation={/./} setState={setPassSerial} placeholder={'Введите кем выдан'} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span>Дата выдачи:</span>
                        <Calendar setDate={setUserEmail}/>
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Номер:</span>
                        <InputAdmin errorText='' validation={/./} setState={setPassDate} placeholder={'Введите номер'} />
                    </div>
                </div>
            </div>
        </>
    )
}
