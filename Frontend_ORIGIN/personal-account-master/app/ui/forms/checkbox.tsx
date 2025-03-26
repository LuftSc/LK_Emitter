type CheckboxProps = {
    text: string
}
  
export const CheckBox = ({ text}: CheckboxProps) => {
    return (
      <div className='flex items-top'>
        <input type="checkbox" className="appearance-none mt-[3px] w-[12px] h-[12px] bg-[#FFEBC8] border border-black before:content-['\2713'] before:relative before:top-[-7px] checked:before:visible before:invisible"></input>
        <p className="ml-[9px] text-[14px]/[18px]">{text}</p>
      </div>
    )
}