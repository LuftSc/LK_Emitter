'use client'

import { Input } from "antd"

interface InputProps {
  placeholder: string
  setState: React.Dispatch<React.SetStateAction<string>>
}

export const InputForm = ({
  setState,
  placeholder,
}: InputProps) => {

  return (
    <Input 
      onChange={(e)=>setState(e.target.value)}
      placeholder={placeholder} 
      classNames={{input: 'h-[31px] w-full border-[0.5px] border-black text-[14px]/[18px] placeholder:text-[#C4C4C4] pl-[12px] pb-[5px]'}}
    ></Input>
  )
}
