'use client'

import { useState, useEffect, SetStateAction } from "react"

type InputProps = {
  placeholder: string
}

export const InputForm = ({
  placeholder,
}: InputProps) => {

  const [inputText, setInputText] = useState('');

  useEffect(() => {
      let storedValue = localStorage.getItem('inputText');
      storedValue = storedValue
      if (storedValue) {
        setInputText(storedValue)}
  },[]) 

  const onChange = (e: { target: { value: SetStateAction<string> } }) => {
    setInputText(e.target.value)
    localStorage.setItem('inputText', JSON.stringify(e.target.value))
  }

  return (
    <input
      
      type="text"
      placeholder={placeholder}
      onChange={onChange}
      className="h-[27px] w-full border-[0.5px] border-black text-[14px]/[18px] placeholder:text-[#C4C4C4] pl-[12px]"
      value={inputText}
    ></input>
  )
}
