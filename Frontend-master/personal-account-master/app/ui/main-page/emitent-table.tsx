"use client"

import { Emitter } from "@/app/models/Emitter";
import React, { useEffect, useState } from 'react';
import { Table, Button } from 'antd';
import type { ColumnsType } from 'antd/es/table';
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
    const [selectedRowId, setSelectedRowId] = useState<string | null>(null);
    //const [emitterId, setEmitterId] = useLocalStorage<{emitterId: string}>('emitterId', {emitterId: ""})
    useEffect(() => {
        const emitter = localStorage.getItem('emitter')
        const emitterData = emitter ? JSON.parse(emitter) : null

            setSelectedRowId(emitterData.Id);
            setEmitterName(emitterData.Name)

    }, [])
    //const { getItem, setItem } = useSimpleStorage('emitter');

    const handleSelect = (id: string, emitterName: string) => {
        setSelectedRowId(id === selectedRowId ? null : id);
        setEmitterName(emitterName)
        console.log('устанавливаем id: ' + id)
        
        localStorage.setItem('emitter', JSON.stringify({ Id: id, Name: emitterName, AuthPerson: userId }))

        //console.log(localStorage.getItem('emitter'))
        //setItem({ Id: id, Name: emitterName, AuthPerson: userId });
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
    
    /*return (
        <table className="border-collapse border-[2px] border-black">
            <thead>
              <tr className="">
                <th className="w-[240px] text-[24px]/[35px] bg-[#D9D9D9] border-[2px] border-black">Действие</th>
                <th className="w-[600px] text-[24px]/[35px] bg-[#D9D9D9] border-[2px] border-black">Название эмитента</th>
              </tr>
            </thead>
            <tbody>
              {emitents.map((emitent) => {
                    return (
                        <EmitentTableRow
                            key={emitent.email}
                            emitentName={emitentName} 
                            setEmitentName={setEmitentName} 
                            name={emitent.name}
                        />
                    );
                })}
            </tbody>
        </table>
    ) */
}