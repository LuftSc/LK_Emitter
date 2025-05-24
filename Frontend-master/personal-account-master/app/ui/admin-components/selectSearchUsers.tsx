"use client"

import { UserWithEmitters } from "@/app/models/UserWithEmitters";
import { Role, searchUsers } from "@/app/services/usersService";
import { Select, Spin } from "antd";
import { useEffect, useState } from "react";

interface Props {
    user?: UserWithEmitters
    setSelectedUser: React.Dispatch<React.SetStateAction<UserWithEmitters>>

}

export default function SelectSearchUsers ({user, setSelectedUser}: Props) {

    const [users, setUsers] = useState<UserWithEmitters[]>([])
    
    const [fetching, setFetching] = useState(false);
    const [searchTerm, setSearchTerm] = useState('');

    
    
    const fetchUsers = async (term:string) => {
        if (searchTerm.length < 2) return;

        setFetching(true);
        try {
            const response = await searchUsers(term)
            
            const data = await response?.json();
            console.log(data)
            setUsers(data);

        } finally {
        setFetching(false);
        }
    }

    return (
        <Select
            showSearch
            filterOption={false}
            onSearch={(value) => {
                setSearchTerm(value);
                fetchUsers(value);
            }}
            notFoundContent={fetching ? <Spin size="small" /> : null}
            options={users.map((user) => ({
                value: user.id,
                label: `${user.fullName} [${Role[user.role]}]`,
            }))}
            onChange={(userId) => {
                const selectedUser = users.find(user => user.id === userId)
                if (selectedUser) setSelectedUser(selectedUser)
            }}
            className="w-[550px]"
            placeholder="Начните вводить фамилию, имя или отчество"
        />
    )
}