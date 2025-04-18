
import TextArea from "antd/lib/input/TextArea"

type InputProps = {
    placeholder: string
    setState: React.Dispatch<React.SetStateAction<string>>
  }
  
  export const InputTextArea = ({
    placeholder,
    setState
  }: InputProps) => {
    return (
      <TextArea 
      placeholder={placeholder} 
      rows={3}
      onChange={(e)=>setState(e.target.value)}
      className="border-[0.5px] border-black text-[14px]/[18px] placeholder:text-[#C4C4C4]"
      />
    )
  }
  