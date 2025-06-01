'use client'

import { SetStateAction, useState } from "react";
import UserSearchFilters from "./userSearchFilters";
import Calendar from "../forms/calendar-new";
import { Button, Checkbox, CheckboxProps } from "antd";
import SelectForUserRoles from "./selectForUserRoles";
import { generateActionsReport, GenerateActionsReportFilters, Role } from "@/app/services/usersService";
import { ActionsReport } from "@/app/models/ActionsReport";
import { useSignalR } from "@/app/signalR/SignalRContext";
import SelectSearchUsers from "./selectSearchUsers";
import { UserWithEmitters } from "@/app/models/UserWithEmitters";

interface Props {
    handleOnAdd: (actionsReport: ActionsReport) => void
}

export default function FiltersForLogs({handleOnAdd} : Props) {
    const defaultUserValues = {
        id: '',
        fullName: '',
        email: '',
        phone: '',
        birthDate: '',
        passport: {series: '', number: '', dateOfIssuer: '', issuer: '', unitCode: ''},
        role: Role.User,
        emitters: []
    } as UserWithEmitters
    
    const [selectedUser, setSelectedUser] = 
        useState<UserWithEmitters>(defaultUserValues)

    const [userName, setUserName] = useState<string>('')
    const [startDate, setStartDate] = useState<string>('')
    const [endDate, setEndDate] = useState<string>('')
    const [role, setRole] = useState<Role>(1)

    const [checked, setChecked] = useState(true);
    const [disabled, setDisabled] = useState(false);

    const { connection } = useSignalR();
    const { startConnection } = useSignalR();

    const onClick = async () => {
        
        const currentConnection = connection ? connection : await startConnection()
        currentConnection?.on('ReceiveActionsReport',
            (actionsReport: ActionsReport) => {
                
                handleOnAdd(actionsReport)
                currentConnection.off('ReceiveActionsReport')
            })
        
        const filters = {
            userId: selectedUser.id === '' ? null : selectedUser.id,
            startDate: startDate === '' ? null : startDate,
            endDate: endDate === '' ? null : endDate
        } as GenerateActionsReportFilters

        console.log(filters)
        const response = await generateActionsReport(filters);
       
    }

    return (
        <div className="w-full flex flex-col items-center border-[0.5px] rounded-[10px] py-[20px] space-y-[20px]">
            <p className="text-[20px] italic">Выберите фильтры для получения нужной информации</p>
            <div className="flex space-x-[20px]">
                <div className="">
                    <p>ФИО пользователя</p>
                    {/* <UserSearchFilters setUserName={setUserName} /> */}
                    <SelectSearchUsers setSelectedUser={setSelectedUser} />
                </div>
                {/* <div>
                    <p>Роль пользователя</p>
                    <SelectForUserRoles setNewRole={setRole}/> 
                </div>                
                
                
                
                
                */}
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
                <Button className="" onClick={onClick}>Получить сведения</Button>
            </div>
        </div>
    )
}