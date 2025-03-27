import { Emitter } from "@/app/models/Emitter";
import React, { useEffect, useState } from 'react';
import { Table, Button } from 'antd';
import type { ColumnsType } from 'antd/es/table';

/*interface Props {
    emitentName: string,
    setEmitentName: React.Dispatch<React.SetStateAction<string>>
} */

interface Props {
    emitters: Emitter[]
    setEmitterName: React.Dispatch<React.SetStateAction<string>>
    isTableVisible: boolean
}

export default function EmitentTable ({emitters, setEmitterName, isTableVisible}: Props) {
    const [selectedRowId, setSelectedRowId] = useState<string | null>(null);

    const handleSelect = (id: string, emitterName: string) => {
        setSelectedRowId(id === selectedRowId ? null : id);
        setEmitterName(emitterName)
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
        },
        {
            title: 'Название эмитента',
            dataIndex: ['emitterInfo', 'fullName'],
            key: 'fullName',
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