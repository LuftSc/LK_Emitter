'use client'

import { Select } from 'antd';

interface Props {
    setNewRole: React.Dispatch<React.SetStateAction<string>>
}

export default function SelectForUserRoles({setNewRole}: Props) {

    const onChange = (value: any) => {
        setNewRole(value)
    }

    // const defaultValue = 'Представитель эмитента'
    return (
        <Select
            showSearch
            onChange={onChange}
            // defaultValue={defaultValue}
            className=''
            placeholder="Выберите роль пользователя"
            filterOption={(input, option) =>
                (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
            }
            options={[
                {value: 'noRole', label: 'Нет роли'},
                {value: 'representative', label: 'Представитель эмитента'},
                {value: 'registrator', label: 'Регистратор'},
            ]}
        />
    )
}
