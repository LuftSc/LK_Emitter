"use client"

import { Menu } from "antd"
import { MenuItemType } from "antd/es/menu/interface"
import { useSignalR } from "./SignalRContext"
import Link from "next/link"

interface Props {
    items: {
        key: string, 
        label: string, 
        href: string
    }[]
}

export const ClientNavigationMenu = ({items} : Props) => {
    const { startConnection } = useSignalR();
    const { connection } = useSignalR();

    const handleMenuClick = () => {
        if (!connection) {
            startConnection()
        }
    }

    return (
        <Menu 
            theme="light" 
            mode="horizontal" 
            items={items.map((item) => ({
                ...item,
                label: (
                  <Link href={item.href} onClick={() => handleMenuClick()}>
                    {item.label}
                  </Link>
                ),
              }))}
            style={{
            display: "flex", 
            justifyContent: "center", 
            minWidth: 0,
            fontSize: "large",
            fontWeight: "bold"
            }}
        />
    )
}