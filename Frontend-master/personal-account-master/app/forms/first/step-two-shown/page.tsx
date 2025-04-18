"use client"

import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { InputForm } from "@/app/ui/forms/input"
import { useState } from "react";
import { RequestListOfShareholders } from "@/app/services/orderReportsService";
import { RadioGroupColumn } from "@/app/ui/forms/radioGroupColumn";
import Calendar from "@/app/ui/forms/calendar-new";
import Timepicker from "@/app/ui/forms/time-picker";

export default function Page () {
    const [startRegistrationTime, setStartRegistrationTime] = useState<string>("")
    const [startMeetingTime, setStartMeetingTime] = useState<string>("")
    const [endRegistrationTime, setEndRegistrationTime] = useState<string>("")
    const [endRegistrationDate, setEndRegistrationDate] = useState<string>("")
    const [meetingPlace, setMeetingPlace] = useState<string>("")
    const [isVotingPossible, setIsVotingPossible] = useState<boolean>(false)
    const [addressFilledBallots, setaddressFilledBallots] = useState<string>("")
    
    const onNextPageTransition = () => {
        const request = localStorage.getItem('request_listOSA')
        const requestData = request ? JSON.parse(request) as RequestListOfShareholders: null

        if (requestData) {
            requestData.forDbSaving.stepTwo = {
                startRegistrationTime: startRegistrationTime,
                startMeetingTime: startMeetingTime,
                endRegistrationTime: endRegistrationTime,
                endRegistrationDate: endRegistrationDate,
                meetingPlace: meetingPlace,
                isVotingPossible: isVotingPossible,
                addressFilledBallots: addressFilledBallots
            }

            localStorage.setItem('request_listOSA', JSON.stringify(requestData))
        }
    }
  
    return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
        <div>
            <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на список к ОСА</p>
            <div className="border-[0.5px] border-black rounded-[28px] pt-[17px] pl-[26px] pb-[28px]">
                <div className="flex items-center mb-[5px]">
                    <p className="w-[180px] text-[14px]/[18px] mr-[40px]">Время начала регистрации </p>
                    <Timepicker setTime={setStartRegistrationTime}/>
                </div>
                <div className="flex items-center mb-[13px]">
                    <p className="w-[180px] text-[14px]/[18px] mr-[40px]">Время проведения собрания</p>
                    <Timepicker setTime={setStartMeetingTime}/>
                </div>
                <div className="flex items-center mb-[25px]">
                    <p className="text-[14px]/[18px] mr-[40px]">Срок окончания приема бюллетеней и инструкций по голосованию</p>
                    <div className="mr-[28px]"><Calendar setDate={setEndRegistrationDate}/></div>
                    <div className="mr-[18px]"><Timepicker setTime={setEndRegistrationTime}/></div>
                    <p className="text-[14px]/[18px]">по мск</p>
                </div>
                <div className="flex items-center max-w-[647px] mb-[25px]">
                    <p className="min-w-[175px] text-[14px]/[18px] mr-[48px]">Место проведения собрания</p>
                    <InputForm setState={setMeetingPlace} placeholder="Введите место"/>
                </div>
                <div className="flex items-center mb-[25px]">
                    <p className="text-[14px]/[18px] mr-[50px]">Методы голосования (заполняется при наличии счета НД)</p>
                    <div className="">
                        <RadioGroupColumn 
                            firstText="Голосование возможно путем подачи указания номинальному держателю"
                            secondText="Голосование возможно путем направления заполненного бюллетеня по адресу"
                            setState={setIsVotingPossible}
                        />
                    </div>
                </div>
                <div className="flex items-center max-w-[610px]">
                    <p className="min-w-[308px] text-[14px]/[18px] mr-[76px]">Адрес для направления заполненных бюллетеней</p>
                    <InputForm setState={setaddressFilledBallots} placeholder="Введите адрес"/>
                </div>
            </div>
        </div>
        <NavigationButtons back='/forms/first/step-one' next='/forms/first/step-three-shown' onClick={onNextPageTransition}/>
     </div>
  );
}
