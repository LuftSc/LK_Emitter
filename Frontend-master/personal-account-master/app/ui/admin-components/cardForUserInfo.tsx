"use client"

import React, { useEffect, useState } from 'react';

import { bindUserToEmitters, BindUserToEmittersRequest, Role, unbindUserFromEmitter } from '@/app/services/usersService';
import { UserWithEmitters } from '@/app/models/UserWithEmitters';
import { Button } from 'antd';
import SelectSearchEmitters from './selectSearchEmitters';
import { Emitter } from '@/app/models/Emitter';
import { InputAdmin } from './inputAdmin';
import SelectForUserRoles from './selectForUserRoles';
import { DownCircleOutlined, PlusCircleOutlined } from '@ant-design/icons';


interface Props {
    user: UserWithEmitters
    handleAddEmitters: (newEmitters: Emitter[]) => void
    handleDeleteEmitter: (emitterGuid: string) => void
}
export default function CardForUserInfo({ user, handleAddEmitters, handleDeleteEmitter }: Props) {

    const [userName, setUserName] = useState<string>('')
    const [userEmail, setUserEmail] = useState<string>('')
    const [userPhone, setUserPhone] = useState<string>('')
    const [userId, setUserId] = useState<string>('')
    const [userBirthDate, setUserBirthDate] = useState<string>('')
    const [role, setRole] = useState<Role>(1)
    const [passSerial, setPassSerial] = useState<string>('')
    const [passNumber, setPassNumber] = useState<string>('')
    const [passDate, setPassDate] = useState<string>('')
    const [passBy, setPassBy] = useState<string>('')
    const [passCode, setPassCode] = useState<string>('')

    const [selectedEmittersGuids, setSelectedEmittersGuids] = useState<string[]>([])
    const [selectedEmitters, setSelectedEmitters] = useState<Emitter[]>([])

    const [selectVisible, setSelectVisible] = useState<boolean>(false)
    // Функция ищет совпадения среди уже привязанных к пользователю эмитентов и тех,
    // которые админ хочет к нему привязать
    const findAlreadyAddedEmitters = () => {
        const alreadyAddedEmitters = user.emitters.map(emitter => emitter.id)
        return selectedEmittersGuids.filter(guid => alreadyAddedEmitters.includes(guid));
    }

    const onUnbindEmitter = async (emitterId: string) => {
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
        <>
            <div className={user !== undefined ? 'flex flex-col border-[0.5px] w-[600px] rounded-xl border-[#f0f0f0] py-[25px]' : 'hidden'}>
                <div className='flex flex-col items-center w-full space-y-[10px] px-[60px]'>
                    <p className='text-[28px]'>Основные данные</p>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span>ФИО:</span>
                        <InputAdmin errorText='' validation={/./} setState={setUserName} placeholder={user.fullName} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span>Email:</span>
                        <InputAdmin errorText='' validation={/./} setState={setUserEmail} placeholder={user.email} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Номер телефона:</span>
                        <InputAdmin errorText='' validation={/./} setState={setUserPhone} placeholder={user.phone} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Id в системе:</span>
                        <InputAdmin errorText='' validation={/./} setState={setUserId} placeholder={user.id} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Дата рождения:</span>
                        <InputAdmin errorText='' validation={/./} setState={setUserBirthDate} placeholder={user.birthDate} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Текущая роль:</span>
                        <SelectForUserRoles placeholder={Role[user.role]} setNewRole={setRole} />
                    </div>
                    <p className='text-[28px]'>Паспортные данные:</p>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span>Серия:</span>
                        <InputAdmin errorText='' validation={/./} setState={setPassSerial} placeholder={user.passport?.series} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span>Номер:</span>
                        <InputAdmin errorText='' validation={/./} setState={setPassNumber} placeholder={user.passport?.number} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Дата выдачи:</span>
                        <InputAdmin errorText='' validation={/./} setState={setPassDate} placeholder={user.passport?.dateOfIssuer} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Кем выдан:</span>
                        <InputAdmin errorText='' validation={/./} setState={setPassBy} placeholder={user.passport?.issuer} />
                    </div>
                    <div className='text-[18px] flex space-x-[20px] w-full'>
                        <span className='text-nowrap'>Код подразделения:</span>
                        <InputAdmin errorText='' validation={/./} setState={setPassCode} placeholder={user.passport?.unitCode} />
                    </div>
                </div>
            </div>
            <div className={user !== undefined && user.role === Role.Emitter
                ? 'flex flex-col items-center border-[0.5px] w-[600px] rounded-xl border-[#f0f0f0] p-[25px] space-y-[10px]'
                : 'hidden'}>
                    <div className='text-[28px]/[28px]'>Прикреплённые эмитенты:</div>
                {user.emitters.map(emitter => (
                    <div className='flex gap-4' key={emitter.id}>
                        <div className='text-[18px]'>{emitter.emitterInfo.shortName}</div>
                        <Button onClick={() => onUnbindEmitter(emitter.id)} danger>Открепить</Button>
                    </div>
                ))}
                <Button
                    type='default'
                    // icon={<PlusCircleOutlined />}
                    // iconPosition='end'
                    className='h-[40px]'
                    onClick={() => {
                        setSelectedEmittersGuids([])
                        setSelectVisible(!selectVisible)
                    }}>
                    Прикрепить нового эмитента
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
        </>
    )
}
