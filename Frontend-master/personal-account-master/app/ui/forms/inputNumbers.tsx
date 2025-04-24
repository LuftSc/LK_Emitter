'use client'

import { Input} from "antd"
import { useState } from "react"
import { ErrorToolTip } from "../errortoolTip"

interface InputProps {
  placeholder: string
  setState: React.Dispatch<React.SetStateAction<number>>
}


export const InputFormNumber = ({
  setState,
  placeholder,
}: InputProps) => {

  const [error, setError] = useState<boolean>(false)
  const [show, setShow] = useState<boolean>(false)
  const onChange = (e: any) => {
    let value = e.target.value
    if (/^[0-9 ]+$/.test(value)) {
      setState(Number(value))
      setError(false)
      setShow(false)
    }
    else {
      setError(true)
      setShow(true)
    }
    console.log(value + error)
  }

  return (
    <div className="relative">
      <Input
          status={error == true ? 'error' : ''}
          onChange={onChange}
          placeholder={placeholder}
          className="h-[31px] w-full border-[0.5px] border-black text-[14px]/[18px] placeholder:text-[#C4C4C4] pb-[5px]"
        ></Input>
        <ErrorToolTip show={show} setShow={setShow} width="w-[340px]" text="Может принимать только числовые значения"/>
    </div>
  )
}
