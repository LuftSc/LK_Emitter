"use client"

import { Emitter } from "@/app/models/Emitter";
import { registerUser, RegisterUserRequest } from "@/app/services/usersService";
import { Button, Input, Select, Space, Spin } from "antd";
import { useMemo, useState } from "react";

const { Option } = Select;

export const RegisterUser = () => {
    const [email, setEmail] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [role, setRole] = useState<string>('Пользователь');


    const [emitters, setEmitters] = useState<Emitter[]>([]);
    const [fetching, setFetching] = useState(false);
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedEmittersGuids, setSelectedEmittersGuids] = useState<string[]>([]);

    const fetchEmitters = async (term:string) => {
        if (searchTerm.length < 2) return;

        setFetching(true);
        try {
          const response = await fetch(
            `http://localhost:5000/Users/search-emitters?searchTerm=${encodeURIComponent(term)}`
          );
        
          const data = await response.json();
          console.log(data)
          setEmitters(data);


        } finally {
          setFetching(false);
        }
      }

    const roles = [
        'Пользователь',
        'Представитель эмитента',
        'Сотрудник регистратора',
        'Админ'
    ];

    const onUserRegister = async () => {
        console.log(selectedEmittersGuids)

        const request = {
            email: email,
            password: password,
            emittersGuids: selectedEmittersGuids,
            role: roles.indexOf(role) + 1
        } as RegisterUserRequest

        console.log(request)
        const response = await registerUser(request)
    }

    return (
            <div style={{ display: 'flex', gap: '16px', marginBottom: '16px', flexDirection: 'column' }}>
                <Space>
                    <Input
                    placeholder="Email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    style={{ width: '200px' }}
                    />
                    <Input
                    placeholder="Пароль"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    style={{ width: '200px' }}
                    />
                    <Select
                        placeholder="Выберите роль"
                        value={role}
                        onChange={(value:any) => setRole(value)}
                        style={{ width: '424px' }} // 200px + 200px + 24px gap
                        >
                        {roles.map(role => (
                            <Option key={role} value={role}>
                            {role}
                            </Option>
                        ))}
                    </Select>
                    <Button
                    type="primary"
                    onClick={() => onUserRegister()}
                    >
                        Зарегистрировать пользователя
                    </Button>
                </Space>
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
                    value={selectedEmittersGuids}
                    onChange={setSelectedEmittersGuids}
                    style={{ width: '100%' }}
                    placeholder="Начните вводить название эмитента"
                />
            </div>
    )
}