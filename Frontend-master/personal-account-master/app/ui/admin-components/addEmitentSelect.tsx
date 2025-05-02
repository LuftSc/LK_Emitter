'use client'

import { Select } from 'antd';

interface Props {
    setEmitentName: React.Dispatch<React.SetStateAction<string>>
}

const data: any = [
    {
        key: '1',
        emitentName: 'ООО ТАКОЕ-ТО',
    },
    {
        key: '2',
        emitentName: 'ООО ДРУГОЕ',
    },
];

export default function AddEmitentName({setEmitentName}: Props) {



    return (
        <Select
            showSearch
            onChange={(value) => setEmitentName(value)}
            className='w-[400px] h-[36px]'
            placeholder="Добавить нового Эмитента"
            filterOption={(input, option) =>
                (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
            }
            options={(data || []).map((d: any) => ({
                value: d.emitentName,
                label: d.emitentName,
            }))}
        />
    )
}