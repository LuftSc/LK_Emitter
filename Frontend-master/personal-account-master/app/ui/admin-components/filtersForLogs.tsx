'use client'

import { useState } from "react";
import UserSearchFilters from "./userSearchFilters";
import Calendar from "../forms/calendar-new";
import { Button } from "antd";
import { RadioGroup } from "../forms/radioGroup";
import SelectForUserRoles from "./selectForUserRoles";


export default function FiltersForLogs() {

    const [userName, setUserName] = useState<string>('')
    const [startDate, setStartDate] = useState<string>('')
    const [endDate, setEndDate] = useState<string>('')
    const [role, setRole] = useState<string>('')

    return (
        <div className="w-full flex flex-col items-center border-[0.5px] rounded-[10px] py-[20px] space-y-[20px]">
            <p className="text-[20px] italic">Выберите фильтры для получения нужной информации</p>
            <div className="flex space-x-[20px]">
                <div className="">
                    <p>ФИО пользователя</p>
                    <UserSearchFilters setUserName={setUserName} />
                </div>
                <div>
                    <p>Роль пользователя</p>
                    <SelectForUserRoles setNewRole={setRole}/> 
                </div>               
            </div>
            <div className="flex items-end space-x-[20px]">
                <div className="">
                    <p>Начальная дата</p>
                    <Calendar setDate={setStartDate}/>
                </div>
                <div>
                    <p>Конечная дата</p>
                    <Calendar setDate={setEndDate} />
                </div>
                <Button className="">Получить сведения</Button>
            </div>
        </div>
    )
}