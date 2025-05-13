'use client'

import TableForUserLogs from "./tableForUserLogs"
import { useState } from "react";
import { Button } from "antd";
import FiltersForLogs from "./filtersForLogs";

export default function MainContentLogs() {

    const [logsTableVis, setLogsTableVis] = useState<boolean>(false);

    return (
        <div className="w-full flex flex-col items-center space-y-[20px]">
            <p className="text-[24px]">Получение сведений о проведенных пользователями операций</p>
            <FiltersForLogs />
            <TableForUserLogs logsTableVis={true}/>
        </div>
    )
}