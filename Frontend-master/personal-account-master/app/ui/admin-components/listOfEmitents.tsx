'use client'

import { List } from "antd";
import AddEmitentName from "./addEmitentSelect";
import { useState } from "react";
import { Role } from "@/app/services/usersService";

const data = [
    'ООО такое-то',
    'ОАО другое',
    'Организация с таким-то именем',
    'Эмитент номер тысяча двадцать пять и три сотых'
]

interface Props {
    newRole: Role
}

export default function ListOfEmitents({newRole}: Props) {

    const [emitentName, setEmitentName] = useState<string>('')

    return (
        <List
            className={newRole == Role.Emitter ? "w-[550px]" : "hidden"}
            header={<div className="font-semibold">Список эмитентов, к которым будет прикреплён пользователь:</div>}
            footer={<AddEmitentName setEmitentName={setEmitentName} />}
            bordered
            dataSource={data}
            renderItem={(item) => (
                <List.Item>
                  {item}
                </List.Item>
            )}
        />
    )
}