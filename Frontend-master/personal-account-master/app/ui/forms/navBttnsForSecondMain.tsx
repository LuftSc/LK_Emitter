'use client'

import { DoubleLeftOutlined } from "@ant-design/icons"
import { Button } from "antd"
import { redirect } from "next/navigation"

export default function FormsMainNavButtonsSecond() {
    return (
        <div className="flex w-full h-[36px] items-center justify-between mb-[20px]">
            <div className="text-[28px]">Статус запросов</div>
            <Button type="default" onClick={(e) => redirect('/forms/mainFirst')} icon={<DoubleLeftOutlined />} iconPosition="start">Создание новых распоряжений</Button>
        </div>
    )
}