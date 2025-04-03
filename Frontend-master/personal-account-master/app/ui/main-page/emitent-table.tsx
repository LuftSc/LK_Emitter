"use client"

import { Emitter } from "@/app/models/Emitter";
import React, { useEffect, useState } from 'react';
import { Table, Button } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import { useSignalR } from "@/app/signalR/SignalRContext";
//import { useSimpleStorage } from "@/app/hooks/useLocalStorage";
//import { useLocalStorage } from "@/app/hooks/useLocalStorage";


/*interface Props {
    emitentName: string,
    setEmitentName: React.Dispatch<React.SetStateAction<string>>
} */

interface Props {
    userId: string
    emitters: Emitter[]
    setEmitterName: React.Dispatch<React.SetStateAction<string>>
    isTableVisible: boolean
}

export default function EmitentTable ({userId, emitters, setEmitterName, isTableVisible}: Props) {
    // const { getItem, setItem } = useSimpleStorage('emitter');
    const [selectedRowId, setSelectedRowId] = useState<string | null>();

    const { connection } = useSignalR();
    const { startConnection } = useSignalR();
    //const [emitterId, setEmitterId] = useLocalStorage<{emitterId: string}>('emitterId', {emitterId: ""})
    useEffect(() => {
        const emitter = localStorage.getItem('emitter')
        const emitterData = emitter ? JSON.parse(emitter) : null

            if (emitterData) {
                setSelectedRowId(emitterData.Id);
                setEmitterName(emitterData.Name)
            }

    }, [])
    //const { getItem, setItem } = useSimpleStorage('emitter');

    const handleSelect = async (id: string, emitterName: string) => {
        
        const currentConnection = connection ? connection : await startConnection()
        
        setSelectedRowId(id === selectedRowId ? null : id);
        setEmitterName(emitterName)
        console.log('устанавливаем id: ' + id)
        
        const response = await currentConnection?.invoke('EmitterSelected', id)
        console.log('подключили пользователя в группу')
        console.log(response)

        localStorage.setItem('emitter', JSON.stringify({ Id: id, Name: emitterName, AuthPerson: userId }))
    };

    const columns : ColumnsType<Emitter> = [
        {
            title: 'Действие',
            key: 'action',
            render: (_, record) => (
              <Button 
                type={selectedRowId === record.id ? 'primary' : 'default'}
                onClick={() => handleSelect(record.id, record.emitterInfo.shortName)}
              >
                {selectedRowId === record.id ? 'Выбрано' : 'Выбрать'}
              </Button>
            ),
            width: 100
        },
        {
            title: 'Название эмитента',
            dataIndex: ['emitterInfo', 'fullName'],
            key: 'fullName',
            width: 700
        },
    ]

    return (
        <div>
            {
                isTableVisible && (<Table<Emitter>
                    columns={columns}
                    dataSource={emitters}
                    rowKey="id"
                    rowClassName={(record) => 
                        record.id === selectedRowId ? 'active-row' : ''
                    }
                    pagination={{
                        pageSize: 5, // Количество строк на странице
                        //showSizeChanger: true, // Показывать выбор количества строк
                        //pageSizeOptions: ['10', '20', '50', '100'], // Варианты выбора
                      }}
                />)
            }
        </div>
    )
    
}