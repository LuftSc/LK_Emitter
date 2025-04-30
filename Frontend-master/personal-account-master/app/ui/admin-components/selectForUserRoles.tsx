'use client'

import { Select } from 'antd';

export default function SelectForUserRoles() {

    const defaultValue = 'Представитель эмитента'
    return (
        <Select
            showSearch
            defaultValue={defaultValue}
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
