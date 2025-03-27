'use client'

interface Props {
  value: string,
  setValue: React.Dispatch<React.SetStateAction<string>>,
  placeholder: string
}

export const InputLogin = ({value, setValue, placeholder}: Props) => {

  return (
    <input
      type="text"
      placeholder={placeholder}
      onChange={(e) => setValue(e.target.value)}
      className="h-[42px] w-[286px] border-[1px] border-[#BDBDBD] rounded-[15px] text-[14px]/[18px] placeholder:opacity-[25] pl-[21px] "
      value={value}
    ></input>
  )
}
