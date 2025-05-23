"use client"

import { Emitter } from "@/app/models/Emitter";
import { searchEmitters } from "@/app/services/emitterService";
import { Role } from "@/app/services/usersService";
import { Select, Spin } from "antd";
import { useState } from "react";

interface Props {
    setSelectedEmittersGuid: React.Dispatch<React.SetStateAction<string[]>>
    role: Role
    setSelectedEmitters?: React.Dispatch<React.SetStateAction<Emitter[]>>
}

export default function SelectSearchEmitters ({setSelectedEmittersGuid, role, setSelectedEmitters}: Props) {
    const [emitters, setEmitters] = useState<Emitter[]>([]);
    const [fetching, setFetching] = useState(false);
    const [searchTerm, setSearchTerm] = useState('');
    
    const fetchEmitters = async (term:string) => {
        if (searchTerm.length < 2) return;

        setFetching(true);
        try {
            const response = await searchEmitters(term)
            
            const data = await response?.json();
            console.log(data)
            setEmitters(data);

        } finally {
        setFetching(false);
        }
    }

    return (
        <Select
            mode="multiple"
            showSearch
            filterOption={false}
            onSearch={(value) => {
                setSearchTerm(value);
                fetchEmitters(value);
            }}
            notFoundContent={fetching ? <Spin size="small" /> : null}
            options={emitters.map((e) => ({
                value: e.id,
                label: `${e.emitterInfo.shortName} ID=${e.issuerId}`,
            }))}
            onChange={(emittersGuids: string[]) => {
                setSelectedEmittersGuid(emittersGuids)
                
                if (setSelectedEmitters) {
                    const selectedEmitters = emitters
                        .filter(emitter => emittersGuids.includes(emitter.id))
                    setSelectedEmitters(selectedEmitters)
                }
                    
            
            }}
            className={role == Role.Emitter ? "w-[550px]" : "hidden"}
            placeholder="Начните вводить название эмитента"
        />
    )
}