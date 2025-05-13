'use client'

import { useState } from "react";
import UserSearchFilters from "./userSearchFilters";
import Calendar from "../forms/calendar-new";
import { Button } from "antd";


export default function FiltersForLogs() {

    const [userName, setUserName] = useState<string>('')
    const [startDate, setStartDate] = useState<string>('')
    const [endDate, setEndDate] = useState<string>('')

    return (
        <div className="w-full flex flex-col items-center border-[0.5px] rounded-[10px] py-[20px]">
            <p className="text-[20px] mb-[20px]">Выберите фильтры для получения нужной информации</p>
            <div className="flex mb-[20px] space-x-[20px]">
                <div className="">
                    <p>ФИО пользователя</p>
                    <UserSearchFilters setUserName={setUserName} />
                </div>
                <div className="">
                    <p>Начальная дата</p>
                    <Calendar setDate={setStartDate}/>
                </div>
                <div>
                    <p>Конечная дата</p>
                    <Calendar setDate={setEndDate} />
                </div>
            </div>
            <Button className="">Получить сведения</Button>
        </div>
    )
}