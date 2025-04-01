import { ReactNode } from "react"
import { NavigationButtons } from "./nav-btn"

type StepProps = {
  children: ReactNode
  back: string
  next: string
  show?: boolean
}

export const Step = ({ children, back, next, show }: StepProps) => {
  return (
    <div className='flex flex-col justify-between min-w-[500px] h-full'>
      <div>{children}</div>
      <NavigationButtons back={back} next={next} />
    </div>
  )
}
