import { ConfigProvider, Radio } from "antd"

interface RadioBtnProps {
  text: string
  setState: React.Dispatch<React.SetStateAction<boolean>>
}

export const RadioButton = ({
  text,
  setState,
}: RadioBtnProps) => {
  return (
    <ConfigProvider
      theme={{ token: { colorText: 'rgba(0,0,0,1)' } }}>
      <Radio onChange={() => setState(true)} className="text-[14px]/[18px]">{text}</Radio>
    </ConfigProvider>
  )
}