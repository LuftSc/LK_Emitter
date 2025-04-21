'use client'

import { ConfigProvider, InputNumber } from "antd"

interface InputProps {
  placeholder: string
  setState: React.Dispatch<React.SetStateAction<number>>
}

export const InputFormNumber = ({
  setState,
  placeholder,
}: InputProps) => {

  const onChange = (value: number | null) => {
    if (value) {
      setState(value)
    }
  }

  return (
      <InputNumber
      onChange={onChange}
      controls={false}
      placeholder={placeholder} 
      className="h-[31px] w-full border-[0.5px] border-black text-[14px]/[18px] placeholder:text-[#C4C4C4] pb-[5px]"
    ></InputNumber>
  )
}
