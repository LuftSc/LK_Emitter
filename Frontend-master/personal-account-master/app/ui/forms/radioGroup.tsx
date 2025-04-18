import { Radio } from "antd"

interface RadioBtnProps {
    firstText: string
    secondText: string
    setState: React.Dispatch<React.SetStateAction<boolean>>
}
  
export const RadioGroup = ({ 
  firstText, 
  secondText, 
  setState
}: RadioBtnProps) => {
    return (
      <Radio.Group 
        onChange={(e) => setState(e.target.value)}
        options={[
            { value: true, label: firstText, },
            { value: false, label: secondText },
        ]}
        className="text-[14px]/[18px]"
      />
    )
}