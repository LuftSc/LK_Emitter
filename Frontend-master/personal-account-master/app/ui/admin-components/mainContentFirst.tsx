'use client'

import TableForDocumentsInAdmin from "./tableWithFiltersForFirstPage"
import TableForUserLogs from "./tableForUserLogs"
import { useState } from "react";
import { Button } from "antd";

export default function MainContentFirst() {

    const [logsTableVis, setLogsTableVis] = useState<boolean>(false);

    return (
        <div className="w-full flex flex-col items-center">
            <TableForDocumentsInAdmin/>
            <Button className="mb-[20px]" onClick={(e) => setLogsTableVis(true)}>{logsTableVis == false ? "Показать" : "Обновить"}</Button>
            <TableForUserLogs logsTableVis={logsTableVis}/>
        </div>
    )
}