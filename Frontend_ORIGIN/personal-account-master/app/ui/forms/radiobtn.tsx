type RadioBtnProps = {
    text: string
    name: string
}
  
export const RadioButton = ({ text, name}: RadioBtnProps) => {
    return (
      <div className='flex items-center'>
        <input type="radio"  name={name} className="appearance-none min-w-[16px] h-[16px] bg-[#FFEBC8] border border-black rounded-xl before:block before:h-[10px] before:w-[10px] before:bg-black before:rounded-full before:relative before:inset-y-0.5 before:inset-x-0.5 checked:before:visible before:invisible"></input>
        <p className="ml-[9px] text-[14px]/[18px]">{text}</p>
      </div>
    )
}