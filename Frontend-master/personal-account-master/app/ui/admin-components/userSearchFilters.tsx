'use client'

import { Select } from 'antd';

interface Props {
    setUserName: React.Dispatch<React.SetStateAction<string>>
}

const data: any = [
    {
        key: '1',
        emitentName: 'ООО ТАКОЕ-ТО',
        representativeName: 'Иван Иванов Иванович',
    },
    {
        key: '2',
        emitentName: 'ООО ДРУГОЕ',
        representativeName: 'Илья Ильев Ильевич',
    },
];

export default function UserSearchFilters({setUserName}: Props) {



    return (
        <Select
            showSearch
            onChange={(value) => setUserName(value)}
            className='w-[320px] h-[36px'
            placeholder="Введите ФИО нужного пользователя"
            filterOption={(input, option) =>
                (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
            }
            options={(data || []).map((d: any) => ({
                value: d.representativeName,
                label: d.representativeName,
            }))}
        />
    )
}
