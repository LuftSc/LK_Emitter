type RadioBtnProps = {
    text: string
    name: string
    state?: boolean 
    setState?: React.Dispatch<React.SetStateAction<boolean>>
}
  
export const RadioButton = ({ text, name, state, setState}: RadioBtnProps) => {
    return (
      <div className='flex items-center'>
        {/*  onChange={() => {if(setState && state) setState(state)}} */}
        <input type="radio"  name={name} className="appearance-none min-w-[16px] h-[16px] bg-[#FFEBC8] border border-black rounded-xl before:block before:h-[10px] before:w-[10px] before:bg-black before:rounded-full before:relative before:inset-y-0.5 before:inset-x-0.5 checked:before:visible before:invisible"></input>
        <p className="ml-[9px] text-[14px]/[18px]">{text}</p>
      </div>
    )
}