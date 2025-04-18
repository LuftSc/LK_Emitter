"use client"

import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { InputForm } from "@/app/ui/forms/input"
import { RadioButton } from "@/app/ui/forms/radiobtn";
import CalendarInput from "@/app/ui/forms/calendar";
import TimePicker from "@/app/ui/forms/time-picker";
import { useState } from "react";
import { RequestListOfShareholders } from "@/app/services/orderReportsService";

export default function Page () {
    const [startRegistrationTime, setStartRegistrationTime] = useState<string>("")
    const [endRegistrationTime, SetEndRegistrationTime] = useState<string>("")
    
    const onNextPageTransition = () => {
        const request = localStorage.getItem('request_listOSA')
        const requestData = request ? JSON.parse(request) as RequestListOfShareholders: null

        if (requestData) {
            requestData.forDbSaving.stepTwo = {
                startRegistrationTime: startRegistrationTime,
                endRegistrationTime: endRegistrationTime
            }
        }
    }
  
    return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
        <div>
            <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на список к ОСА</p>
            <div className="border-[0.5px] border-black rounded-[28px] pt-[17px] pl-[26px] pb-[28px]">
                <div className="flex items-center mb-[5px]">
                    <p className="w-[180px] text-[14px]/[18px] mr-[48px]">Время начала регистрации </p>
                    <TimePicker calendarId="regStart"/>
                </div>
                <div className="flex items-center mb-[13px]">
                    <p className="w-[180px] text-[14px]/[18px] mr-[48px]">Время проведения собрания</p>
                    <TimePicker calendarId="meetTime"/>
                </div>
                <div className="flex items-center mb-[25px]">
                    <p className="text-[14px]/[18px] mr-[80px]">Срок окончания приема бюллетеней и инструкций по голосованию</p>
                    <div className="mr-[28px]"><CalendarInput calendarId="deadlineDate"/></div>
                    <div className="mr-[18px]"><TimePicker calendarId="deadlineTime"/></div>
                    <p className="text-[14px]/[18px]">по мск</p>
                </div>
                <div className="flex items-center max-w-[647px] mb-[25px]">
                    <p className="min-w-[175px] text-[14px]/[18px] mr-[48px]">Место проведения собрания</p>
                    <InputForm placeholder="Введите место"/>
                </div>
                <div className="flex items-center mb-[25px]">
                    <p className="text-[14px]/[18px] mr-[72px]">Методы голосования (заполняется при наличии счета НД)</p>
                    <div className="">
                        <RadioButton name="votingMethod" text="Голосование возможно путем подачи указания номинальному держателю"/>
                        <RadioButton name="votingMethod" text="Голосование возможно путем направления заполненного бюллетеня по адресу"/>
                    </div>
                </div>
                <div className="flex max-w-[610px]">
                    <p className="min-w-[308px] text-[14px]/[18px] mr-[76px]">Адрес для направления заполненных бюллетеней</p>
                    <InputForm placeholder="Введите адрес"/>
                </div>
            </div>
        </div>
        <NavigationButtons back='/forms/first/step-one' next='/forms/first/step-three-shown' onClick={onNextPageTransition}/>
     </div>
  );
}
