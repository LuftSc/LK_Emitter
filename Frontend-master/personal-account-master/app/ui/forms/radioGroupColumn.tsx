import { Radio } from "antd"

interface RadioBtnProps {
  firstText: string
  secondText: string
  setState: React.Dispatch<React.SetStateAction<boolean>>
}

export const RadioGroupColumn = ({
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
      className="flex flex-col gap-[10px] text-[14px]/[18px] "
    />
  )
}