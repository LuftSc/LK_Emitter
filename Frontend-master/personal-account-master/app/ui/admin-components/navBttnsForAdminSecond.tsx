'use client'

import { DoubleLeftOutlined } from "@ant-design/icons"
import { Button } from "antd"
import { redirect } from "next/navigation"

export default function AdminNavButtonsSecond() {
        return (
            <div className="flex w-full h-[36px] items-center justify-between mb-[20px]">
                <div className="text-[28px]">Управление пользователями</div>
                <Button type="default" onClick={(e) => redirect('/admin/first')} icon={<DoubleLeftOutlined />} iconPosition="start">История действий пользователей</Button>
            </div>
        )
    }