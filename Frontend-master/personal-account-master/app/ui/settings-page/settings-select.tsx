'use client'

import { Select } from "antd";

interface Props {
    placeholder: string
}

export default function SettingsSelect({placeholder}: Props) {
    return (
        <Select
        placeholder={placeholder}
        className="w-full h-[50px]"
        options={[
          { value: 'Мужской', label: 'Мужской' },
          { value: 'Женский', label: 'Женский' },
        ]}
        />
    )
}