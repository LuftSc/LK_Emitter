import PdfSvg from "../svg-icons/pdf"
import SignBtn from "./sign-btn"

interface Props {
    signed: boolean,
    name: string
}

export default function DocumentsRow ({signed, name}: Props) {
    return (
        <div className="flex max-w-[800px] items-center justify-between h-[54px] bg-white mb-[20px] px-[15px]">
            <div className="flex items-center">
                <div className="mr-[10px]"><PdfSvg /></div>
                <p className="text-[24px]/[32px] mr-[25px]">{name}</p>
            </div>
            <div className="flex items-center">
                <p className="w-[109px] h-[44px] text-white bg-[#FF0004] text-[20px] text-center mr-[25px] pt-[7px] cursor-pointer">Удалить</p>
                <SignBtn signed={signed} />
                <p className={signed == true ? "w-[166px] h-[44px] text-white bg-black text-[20px] text-center pt-[7px]" : "hidden"}>Подписано</p>
            </div>
        </div>
    )
}