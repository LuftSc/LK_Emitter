import { ConfigProvider, Radio } from "antd"

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
    <ConfigProvider
     theme={{token: {colorText: 'rgba(0,0,0,1)'}}}>
      <Radio.Group
        onChange={(e) => setState(e.target.value)}
        options={[
          { value: true, label: firstText, },
          { value: false, label: secondText },
        ]}
        className="text-[14px]/[18px] text-black"
      />
    </ConfigProvider>
  )
}