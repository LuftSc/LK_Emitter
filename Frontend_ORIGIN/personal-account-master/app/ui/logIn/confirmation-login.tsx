
interface Props {
  value: string,
  setValue: React.Dispatch<React.SetStateAction<string>>,
  placeholder: string
}

export const ConfirmationInput = ({value, setValue, placeholder}: Props) => {

  return (
    <input
      type="text"
      placeholder={placeholder}
      className="h-[42px] w-[286px] border-[1px] border-[#BDBDBD] rounded-[15px] text-[14px]/[18px] placeholder:opacity-[25] pl-[21px] "
      onChange={(e) => setValue(e.target.value)}
      value={value}
    ></input>
  )
}
