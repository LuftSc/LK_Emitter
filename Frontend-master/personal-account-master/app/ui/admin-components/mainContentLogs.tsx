'use client'

import TableForUserLogs from "./tableForUserLogs"
import { useState } from "react";
import { Button } from "antd";

export default function MainContentLogs() {

    const [logsTableVis, setLogsTableVis] = useState<boolean>(false);

    return (
        <div className="w-full flex flex-col items-center">
            <p className="text-[24px] mb-[20px]">Получение сведений о проведенных пользователями операций</p>
            <Button className="mb-[20px]" onClick={(e) => setLogsTableVis(true)}>{logsTableVis == false ? "Показать" : "Обновить"}</Button>
            <TableForUserLogs logsTableVis={logsTableVis}/>
        </div>
    )
}