'use client'

import { Input } from "antd"
import { useState } from "react"
import { ErrorToolTip } from "../errortoolTip"

interface InputProps {
    placeholder: string
    setState: React.Dispatch<React.SetStateAction<string>>
    errorText: string
    validation: any
}

export const InputAdmin = ({
    errorText,
    validation,
    setState,
    placeholder,
}: InputProps) => {

    const [error, setError] = useState<boolean>(false)
    const [show, setShow] = useState<boolean>(false)
    const onChange = (e: any) => {
        let value = e.target.value
        if (validation.test(value)) {
            setState(value)
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
        <div className="w-full relative">
            <Input
                status={error == true ? 'error' : ''}
                onChange={onChange}
                placeholder={placeholder}
                classNames={{ input: 'h-[31px] w-full text-[14px]/[18px] placeholder:text-[#C4C4C4] pl-[12px] pb-[5px]' }}
            ></Input>
            <ErrorToolTip show={show} setShow={setShow} text={errorText} />
        </div>
    )
}
