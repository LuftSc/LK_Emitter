interface Props {
    emitentName: string,
    setEmitentName: React.Dispatch<React.SetStateAction<string>>,
    name: string
}

export default function EmitentTableRow ({emitentName, setEmitentName, name}: Props) {
    return (
        <tr>
            <td className="border-[2px] border-black text-[24px] text-center bg-[#D9D9D9] cursor-pointer" 
                onClick={() => setEmitentName(name)}>
                {emitentName == name ? "Выбрано" : "Выбрать"}
            </td>
            <td className="border-[2px] border-black text-[24px] pl-[15px]">{name}</td>
        </tr>
    )
}