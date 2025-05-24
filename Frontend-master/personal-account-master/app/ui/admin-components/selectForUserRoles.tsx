'use client'

import { Role } from '@/app/services/usersService';
import { Select } from 'antd';

interface Props {
    setNewRole: React.Dispatch<React.SetStateAction<Role>>
    placeholder?: string
}

export default function SelectForUserRoles({setNewRole, placeholder}: Props) {

    const onChange = (value: Role) => {
        setNewRole(value)
    }

    // const defaultValue = 'Представитель эмитента'
    return (
        <Select
            showSearch
            onChange={onChange}
            // defaultValue={defaultValue}
            className='w-full'
            placeholder={placeholder || "Выберите роль пользователя"}
            filterOption={(input, option) =>
                (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
            }
            options={[
                {value: Role.User, label: 'Пользователь'},
                {value: Role.Emitter, label: 'Представитель эмитента'},
                {value: Role.Registrator, label: 'Сотрудник регистратора'},
                {value: Role.Admin, label: 'Администратор'},
            ]}
        />
    )
}
