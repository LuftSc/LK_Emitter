"use client"

import React, { useEffect, useState } from 'react';

import { bindUserToEmitters, BindUserToEmittersRequest, Role, unbindUserFromEmitter } from '@/app/services/usersService';
import { UserWithEmitters } from '@/app/models/UserWithEmitters';
import { Button } from 'antd';
import SelectSearchEmitters from './selectSearchEmitters';
import { Emitter } from '@/app/models/Emitter';


interface Props {
    user: UserWithEmitters
    handleAddEmitters: (newEmitters: Emitter[]) => void
    handleDeleteEmitter: (emitterGuid: string) => void
}

// export default function CardForUserInfo({userName, setNewRole}: Props) {
//     return (
//         <div className={userName ? 'flex items-center border-[0.5px] w-[550px] rounded-[5px] h-[60px] border-[#f0f0f0] px-[25px] mb-[10px]': 'hidden'}>
//             <div className='text-[18px] w-2/3 bold'>{userName}</div>
//             <div className='h-full w-[0.5px] bg-[#f0f0f0] mx-[25px]'></div>
//             <div><SelectForUserRoles setNewRole={setNewRole}/></div>
//         </div>
//     )
// }
export default function CardForUserInfo({user, handleAddEmitters, handleDeleteEmitter}: Props) {
    const [selectedEmittersGuids, setSelectedEmittersGuids] = useState<string[]>([])
    const [selectedEmitters, setSelectedEmitters] = useState<Emitter[]>([])

    const [selectVisible, setSelectVisible] = useState<boolean>(false)
    // Функция ищет совпадения среди уже привязанных к пользователю эмитентов и тех,
    // которые админ хочет к нему привязать
    const findAlreadyAddedEmitters = () => {
        const alreadyAddedEmitters = user.emitters.map(emitter => emitter.id)
        return selectedEmittersGuids.filter(guid => alreadyAddedEmitters.includes(guid));
    }

    const onUnbindEmitter = async (emitterId:string) => {
        const response = await unbindUserFromEmitter(user.id, emitterId)

        console.log(response)

        handleDeleteEmitter(emitterId)
    }

    const onBindEmitters = async () => {
        const alreadyAddedEmitters = findAlreadyAddedEmitters();
        if (alreadyAddedEmitters.length == 0) {

            const request = {
                userId: user.id,
                emittersIdList: selectedEmittersGuids
            } as BindUserToEmittersRequest

            const response = await bindUserToEmitters(request)

            console.log(response)

            handleAddEmitters(selectedEmitters)
        } else {
            const alredyAddedEmittersInfo = alreadyAddedEmitters
                .map(emitterGuid => user.emitters
                    .find(emitter => emitter.id === emitterGuid))
            
            console.log(alredyAddedEmittersInfo)
        }
    }
    return (
        <div className={user !== undefined ? 'flex flex-col border-[0.5px] w-[750px] rounded-[5px] h-[60px] border-[#f0f0f0] px-[25px] mb-[10px]': 'hidden'}>
            <div className='text-[18px] w-2/3 bold'>ФИО: {user.fullName}</div>
            <div className='text-[18px] w-2/3 bold'>Email: {user.email}</div>
            <div className='text-[18px] w-2/3 bold'>Номер телефона: {user.phone}</div>
            <div className='text-[18px] w-2/3 bold'>Id в системе: {user.id}</div>
            <div className='text-[18px] w-2/3 bold'>Текущая роль: {Role[user.role]}</div>
            <div className='text-[18px] w-2/3 bold'>Дата рождения: {user.birthDate}</div>
            <div className='flex flex-col text-[18px] w-2/3 bold'>
                Паспортные данные:
                <div className='text-[18px] w-2/3 bold'>Серия: {user.passport?.series}</div>
                <div className='text-[18px] w-2/3 bold'>Номер: {user.passport?.number}</div>
                <div className='text-[18px] w-2/3 bold'>Дата выдачи: {user.passport?.dateOfIssuer}</div>
                <div className='text-[18px] w-2/3 bold'>Кем выдан: {user.passport?.issuer}</div>
                <div className='text-[18px] w-2/3 bold'>Код подразделения: {user.passport?.unitCode}</div>
            </div>

            <div className={user !== undefined && user.role === Role.Emitter ?'text-[18px]' : 'hidden'}>
                Прикреплённые эмитенты:
            </div>
            <div className={user !== undefined && user.role === Role.Emitter 
                ? 'flex flex-col border-[0.5px] w-[750px] rounded-[5px] border-[#f0f0f0] px-[25px] mb-[10px] gp-6'
                : 'hidden'}>
                    {user.emitters.map(emitter => (
                        <div className='flex gap-4' key={emitter.id}>
                            <div className='text-[18px]'>{emitter.emitterInfo.shortName}</div>
                            <Button onClick={() => onUnbindEmitter(emitter.id)}danger>Открепить</Button>
                        </div>
                    ))}
                    <Button 
                        type='primary' 
                        className='h-[40px] mt-[20px] mb-[20px]' 
                        onClick={() => {
                            setSelectedEmittersGuids([])
                            setSelectVisible(!selectVisible) 

                            }}>
                            Прикрепить пользователя к новому эмитенту
                    </Button>
                    {selectVisible 
                        ? <div className='flex flex-col '>
                            <SelectSearchEmitters 
                                setSelectedEmittersGuid={setSelectedEmittersGuids} 
                                role={Role.Emitter} 
                                setSelectedEmitters={setSelectedEmitters}
                            /> 
                            <Button 
                                type='default' 
                                className='h-[40px] mt-[20px] mb-[20px]' 
                                onClick={onBindEmitters}>
                                    Прикрепить
                            </Button>
                         </div>
                        : <div></div>}
            </div>


            {/* <div className='h-full w-[0.5px] bg-[#f0f0f0] mx-[25px]'></div> */}
        </div>
    )
}
