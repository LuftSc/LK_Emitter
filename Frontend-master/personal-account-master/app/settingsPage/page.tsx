"use client"

import { useEffect, useState } from "react";
import { RadioButton } from "../ui/forms/radiobtn";
import SettingsCheckbox from "../ui/settings-page/settings-checkbox";
import { SettingsInput } from "../ui/settings-page/settings-input";
import { SettingsInputPassword } from "../ui/settings-page/settings-input-password";
import SettingsSelect from "../ui/settings-page/settings-select";
import { PassportInfo, User } from "../models/User";
import { getCurrentUserPersonalData, Role, updateUserData, UpdateUserDataRequest } from "../services/usersService";
import { Button } from "antd";


export default function Page () {
    useEffect(() => {
        const getUserData = async () => {
              const response = await getCurrentUserPersonalData()
              console.log(response)
        
              if (response?.ok) {
                const data = await response.json();
                setUserData(data)
              } else if (response?.status === 400) {
                const error = await response.json()
              } else {
                console.log('Неизвестная ошибка')
        
              }
            }; 
            getUserData();
    }, [])


    const defaultUserData = {
            id: "",
            name: "",
            surname: "",
            patronymic: "",
            email: "",
            phone: "",
            birthDate: "",
            passport: {
                series: "",
                number: "",
                dateOfIssuer: "",
                issuer: "",
                unitCode: ""
            } as PassportInfo,
            role: Role.User
        } as User

    const [userData, setUserData] = useState<User>(defaultUserData)

    const onSaveChanges = async () => {


        console.log(userData)
        const response = await updateUserData(userData as UpdateUserDataRequest)
    
        console.log(response)
    }

    const onChange = (field: string, value: string) => {
        setUserData(prev => ({ ...prev, [field]: value })); 
    }
    
    const onChangePassportField = (field: string, value: string) => {
        setUserData(prev => ({
            ...prev,
            passport: {
            ...prev.passport,
            [field]: value   
            }
        }));
    };

    return (
        <main>
            <div className="py-[50px] flex flex-col items-center">
                <h1 className="mb-[30px] text-[34px] bold">Настройки</h1>
                <div className="flex w-[950px] justify-between mb-[30px]">
                    <div>
                        <p className="text-center bold text-[24px] mb-[20px]">Персональные данные</p>
                        <div className="w-[620px] h-[290px] flex justify-between">
                            <div className="w-[290px] flex flex-col justify-between">
                                <SettingsInput 
                                    placeholder="Фамилия*" 
                                    value={userData.surname} 
                                    fieldName={"surname"} 
                                    handleChange={onChange}                                    
                                />
                                <SettingsInput 
                                    placeholder="Отчество*" 
                                    value={userData.patronymic} 
                                    fieldName={"patronymic"} 
                                    handleChange={onChange}
                                />
                                <SettingsInput 
                                    placeholder="Дата рождения*" 
                                    value={userData.birthDate} 
                                    fieldName={"birthDate"} 
                                    handleChange={onChange}
                                />
                                <SettingsInputPassword 
                                    placeholder="Новый пароль*" 
                                />
                            </div>
                            <div className="w-[290px] flex flex-col justify-between">
                                <SettingsInput 
                                    placeholder="Имя*"
                                    value={userData.name} 
                                    fieldName={"name"} 
                                    handleChange={onChange}
                                />
                                <div className="h-[50px] flex items-center"><SettingsCheckbox /></div>
                                <SettingsInput 
                                    placeholder="Мобильный телефон*" 
                                    value={userData.phone} 
                                    fieldName={"phone"} 
                                    handleChange={onChange}
                                />
                                <SettingsInput 
                                    placeholder="Адрес электронной почты*" 
                                    value={userData.email} 
                                    fieldName={"email"} 
                                    handleChange={onChange}
                                />
                            </div>
                        </div>
                    </div>
                    <div>
                        <p className="text-center bold text-[24px] mb-[20px]">Паспортные данные</p>
                        <div className="w-[290px] h-[290px] flex flex-col justify-between">
                            <div className="flex justify-between">
                                <div className="w-[125px]">
                                    <SettingsInput 
                                        placeholder="Серия*" 
                                        value={userData.passport.series} 
                                        fieldName={"series"} 
                                        handleChange={onChangePassportField}
                                    />
                                </div>
                                <div className="w-[125px]">
                                    <SettingsInput 
                                        placeholder="Номер*" 
                                        value={userData.passport.number} 
                                        fieldName={"number"} 
                                        handleChange={onChangePassportField}
                                    />
                                </div>
                            </div>
                            <SettingsInput 
                                placeholder="Дата выдачи*" 
                                value={userData.passport.dateOfIssuer} 
                                fieldName={"dateOfIssuer"} 
                                handleChange={onChangePassportField}/>
                            <SettingsInput 
                                placeholder="Кем выдан*" 
                                value={userData.passport.issuer} 
                                fieldName={"issuer"} 
                                handleChange={onChangePassportField}
                            />
                            <SettingsInput 
                                placeholder="Код подразделения*"
                                value={userData.passport.unitCode} 
                                fieldName={"unitCode"} 
                                handleChange={onChangePassportField} 
                            />

                            <Button onClick={onSaveChanges}>Сохранить изменения</Button>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    )
}