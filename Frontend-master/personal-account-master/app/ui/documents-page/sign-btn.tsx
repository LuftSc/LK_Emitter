'use client'

interface Props {
    signed: boolean
}

export default function SignBtn({signed}: Props) {
    return (
        <p 
            className={signed == false ? "w-[166px] h-[44px] text-white bg-[#38CB17] text-[20px] text-center pt-[7px] cursor-pointer" : "hidden"}
            onClick={() => signed == true}
        >
            Подписать ЭЦП
        </p>
    )
}