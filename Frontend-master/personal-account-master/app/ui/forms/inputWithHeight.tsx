type InputProps = {
    placeholder: string
  }
  
  export const InputFormHeight = ({
    placeholder,
  }: InputProps) => {
    return (
      <input
        type="text"
        placeholder={placeholder}
        className="h-full w-full border-[0.5px] border-black text-[14px]/[18px] placeholder:text-[#C4C4C4] pl-[12px]"
      ></input>
    )
  }
  