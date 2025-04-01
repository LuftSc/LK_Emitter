import { Radio } from 'antd'

interface Props {
    setShowLists:  React.Dispatch<React.SetStateAction<boolean>>
}

export default function ShowListsRadio ({setShowLists}: Props) {
    return (
        <Radio onChange={() => setShowLists(true)}>Раскрывать списки НД</Radio>
    )
}