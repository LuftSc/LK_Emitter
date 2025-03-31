'use client'

import { Input } from "antd";

interface Props {
    placeholder: string
}

export const SettingsInput = ({placeholder}: Props) => {

  return (
    <Input placeholder={placeholder} classNames={{input: 'w-full h-[50px]'}} />
  )
}
