'use client'

import { DoubleRightOutlined } from "@ant-design/icons"
import { Button } from "antd"
import { redirect } from "next/navigation"

export default function AdminNavButtonsFirst() {
        return (
            <div className="flex w-full h-[36px] items-center justify-between mb-[20px]">
                <div className="text-[24px] bold">История действий пользователей</div>
                <Button type="default" onClick={(e) => redirect('/admin/second')} icon={<DoubleRightOutlined />} iconPosition="end">Управление пользователями</Button>
            </div>
        )
    }