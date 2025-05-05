'use client'

import { DoubleRightOutlined } from "@ant-design/icons"
import { Button } from "antd"
import { redirect } from "next/navigation"

export default function FormsMainNavButtonsFirst() {
    return (
        <div className="flex w-full h-[36px] items-center justify-between mb-[20px]">
            <div className="text-[28px]">Создание новых распоряжений</div>
            <Button type="default" onClick={(e) => redirect('/forms/mainSecond')} icon={<DoubleRightOutlined />} iconPosition="end">Статус запросов</Button>
        </div>
    )
}