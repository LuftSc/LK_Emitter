import { Radio } from "antd"

interface RadioBtnProps {
    text: string
    name: string
    // value: boolean
    // setState: React.Dispatch<React.SetStateAction<boolean>>
}
  
export const RadioButton = ({ 
  text, 
  name, 
}: RadioBtnProps) => {
    return (
      <Radio name={name} className="text-[14px]/[18px]">{text}</Radio>
    )
}