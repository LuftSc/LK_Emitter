"use client"

import Link from "next/link"

type NavigationButtonsProps = {
  back: string
  next: string
  onClick?: () => void
}

export const NavigationButtons = ({
  back,
  next,
  onClick
}: NavigationButtonsProps) => {
  return (
    <div className='flex justify-between items-center absolute inset-x-[45px] bottom-[45px]'>
      <Link href={back}>
        <p className={back == '' ? 'hidden' : 'bg-[#FBCB18] border border-[#BDBDBD] rounded-2xl w-[167px] h-[41px] drop-shadow-[0_4px_4px_rgba(0,0,0,0.25)] text-center text-xl pt-[6px]'}>
        {back == '' ? "" : "Назад"}
        </p>
      </Link>
      <Link href={next} onClick={onClick || (() => {})}>
        <p className='bg-[#FBCB18] border border-[#BDBDBD] rounded-2xl w-[167px] h-[41px] drop-shadow-[0_4px_4px_rgba(0,0,0,0.25)] text-center text-xl pt-[6px]'>
          {next == '' ? "Отправить" : "Продолжить"}
        </p>
      </Link>
    </div>
  )
}
