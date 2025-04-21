import { Checkbox } from "antd"

interface CheckboxProps {
  setState: React.Dispatch<React.SetStateAction<boolean>>
  text: string
}
  
export const CheckBox = ({ text, setState }: CheckboxProps) => {
    return (
      <Checkbox onChange={(e) => setState(e.target.checked)} className="text-[14px]/[18px]">{text}</Checkbox>
    )
}