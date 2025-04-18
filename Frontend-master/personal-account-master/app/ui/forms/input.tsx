'use client'

import { Input } from "antd"

type InputProps = {
  placeholder: string
}

export const InputForm = ({
  placeholder,
}: InputProps) => {

  return (
    <Input 
      placeholder={placeholder} 
      classNames={{input: 'h-[27px] w-full border-[0.5px] border-black text-[14px]/[18px] placeholder:text-[#C4C4C4] pl-[12px] '}}
    ></Input>
  )
}
