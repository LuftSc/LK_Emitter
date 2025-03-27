import Link from "next/link"

type NavigationButtonsProps = {
  back: string
  next: string
  home?: boolean
}

export const NavigationButtons = ({
  back,
  next,
  home,
}: NavigationButtonsProps) => {
  return (
    <div className='flex justify-between items-center'>
      <div className={back == '' ? 'visible flex h-[22px] items-center ml-[26px]' : 'hidden'}>
        <input type="radio" name='showList' className="appearance-none w-[16px] h-[16px] bg-[#FFEBC8] border border-black rounded-xl before:block before:h-[10px] before:w-[10px] before:bg-black before:rounded-full before:relative before:inset-y-0.5 before:inset-x-0.5 checked:before:visible before:invisible mr-[8px]"></input>
        <p className='text-center text-[19px]/[22px]'>Раскрывать списки НД</p>
      </div>
      <Link href={back}>
        <p className={back == '' ? 'hidden' : 'bg-[#FBCB18] border border-[#BDBDBD] rounded-2xl w-[167px] h-[41px] drop-shadow-[0_4px_4px_rgba(0,0,0,0.25)] text-center text-xl pt-[6px]'}>
        {back == '' ? "" : "Назад"}
        </p>
      </Link>
      <Link href={next}>
        <p className='bg-[#FBCB18] border border-[#BDBDBD] rounded-2xl w-[167px] h-[41px] drop-shadow-[0_4px_4px_rgba(0,0,0,0.25)] text-center text-xl pt-[6px]'>
          {next == '' ? "Отправить" : "Продолжить"}
        </p>
      </Link>
    </div>
  )
}
