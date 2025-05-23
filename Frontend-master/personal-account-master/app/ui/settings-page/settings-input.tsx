'use client'

import { PassportInfo, User } from "@/app/models/User";
import { Input } from "antd";

interface Props {
    placeholder: string,
    value: string
    fieldName: string
    handleChange: (field: string, value: string) => void
}

export const SettingsInput = ({placeholder, value, fieldName, handleChange}: Props) => {

  return (
    <Input 
      placeholder={placeholder} 
      classNames={{input: 'w-full h-[50px]'}}
      value={value}
      onChange={(e:any) => handleChange(fieldName, e.target.value)}
    />
  )
}
