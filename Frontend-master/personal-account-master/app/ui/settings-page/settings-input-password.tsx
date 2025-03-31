'use client'

import { Input } from "antd";

interface Props {
    placeholder: string
}

export const SettingsInputPassword = ({placeholder}: Props) => {

  return (
    <Input.Password placeholder={placeholder} classNames={{input: 'w-full h-[40px]'}} />
  )
}
