interface ErrorToolTipProps {
    text: string
    width: string
    show: boolean
    setShow: React.Dispatch<React.SetStateAction<boolean>>
}

export const ErrorToolTip = ({ text, width, show, setShow }: ErrorToolTipProps) => {
    return (
        <div onClick={(e) => setShow(false)}
            className={show == false ? 'hidden' :
            "items-center absolute top-[-46px] text-center bg-white p-[5px] border-[0.5px] border-[#DC2424] shadow-[0_0_0_2px_rgba(255,38,5,0.06)] rounded-[10px]"}>
            <p className={width}>{text}</p>
            <div className="absolute top-[34px] left-[10px] w-6 h-3 overflow-hidden inline-block">
                <div className="h-4 w-8 bg-white border border-[#DC2424] -rotate-45 transform origin-top-left"></div>
            </div>
        </div>
    )
}