'use client'

import React, { useState } from 'react';
import SelectForUserRoles from './selectForUserRoles';
import { Button } from 'antd';
import EditUserPopUp from './editUserPopUp';

interface Props {
    userName: string
    setNewRole: React.Dispatch<React.SetStateAction<string>>
}

export default function CardForUserInfo({userName, setNewRole}: Props) {

    const [visPopUp, setVisPopUp] = useState<boolean>(false)

    return (
        <div className={userName ? 'flex items-center border-[0.5px]  rounded-[5px] h-[60px] border-[#f0f0f0] px-[25px] mb-[10px]': 'hidden'}>
            <div className='text-[18px] w-2/3 bold'>{userName}</div>
            <div className='h-full w-[0.5px] bg-[#f0f0f0] mx-[25px]'></div>
            <div><SelectForUserRoles setNewRole={setNewRole}/></div>
            <div className='h-full w-[0.5px] bg-[#f0f0f0] mx-[25px]'></div>
            <div><Button onClick={(e) => setVisPopUp(true)} >Редактировать</Button></div>
            <EditUserPopUp visPopUp={visPopUp} setVisPopUp={setVisPopUp} />
        </div>
    )
}