import React from 'react';

import SelectForUserRoles from './selectForUserRoles';

interface Props {
    userName: string
    setNewRole: React.Dispatch<React.SetStateAction<string>>
}

export default function CardForUserInfo({userName, setNewRole}: Props) {
    return (
        <div className={userName ? 'flex items-center border-[0.5px] w-[550px] rounded-[5px] h-[60px] border-[#f0f0f0] px-[25px] mb-[10px]': 'hidden'}>
            <div className='text-[18px] w-2/3 bold'>{userName}</div>
            <div className='h-full w-[0.5px] bg-[#f0f0f0] mx-[25px]'></div>
            <div><SelectForUserRoles setNewRole={setNewRole}/></div>
        </div>
    )
}