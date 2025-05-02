'use client'

import { useState } from "react"
import TableForDocumentsInAdmin from "./tableWithFiltersForFirstPage"

export default function MainContentFirst() {

    return (
        <div className="w-full flex flex-col items-center">
            <TableForDocumentsInAdmin />
        </div>
    )
}