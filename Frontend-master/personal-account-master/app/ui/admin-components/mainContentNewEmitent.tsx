'use client'

import { useState } from "react"
import { InputAdmin } from "./inputAdmin"
import { Button, Input } from "antd"
import Calendar from "../forms/calendar-new"
import { registerEmitter, RegisterEmitterRequest } from "@/app/services/emitterService"
import { EmitterInfo } from "@/app/models/EmitterInfo"

export default function MainContentNewEmitent() {

    const [fullName, setFullName] = useState<string>('')
    const [shortName, setShortName] = useState<string>('')
    const [innNumber, setInnNumber] = useState<string>('')
    const [jurisdiction, setJurisdiction] = useState<string>('')
    const [issuerId, setIssuerId] = useState<string>('')
    const [ogrnBy, setOgrnBy] = useState<string>('')
    const [ogrnDate, setOgrnDate] = useState<string>('')
    const [ogrnNumber, setOgrnNumber] = useState<string>('')
    const [regBy, setRegBy] = useState<string>('')
    const [regNumber, setRegNumber] = useState<string>('')
    const [regDate, setRegDate] = useState<string>('')

    const onEmitterRegistration = async () => {

        const request = {
            emitterInfo: {
                fullName: fullName,
                shortName: shortName,
                inn: innNumber,
                jurisdiction: jurisdiction,
                ogrn: {
                    number: ogrnNumber,
                    dateOfAssignment: ogrnDate,
                    issuer: ogrnBy
                },
                registration: {
                    number: regNumber,
                    registrationDate: regDate,
                    issuer: regBy
                }
            } as EmitterInfo,
            // Так плохо, надо сделать числовой инпут
            issuerId: Number(issuerId)
        } as RegisterEmitterRequest

        console.log(request)

        const response = await registerEmitter(request);

        console.log(response)
    }

    

    return (
        <div className="w-full flex flex-col items-center space-y-[20px]">
            <p className="text-[24px]">Добавление новой организации-эмитента</p>
            <div>
                <p>Основная информация</p>
                <div className="flex w-[600px] space-x-[15px]">
                    <div className="w-[240px]">
                        <InputAdmin
                            placeholder="Полное название организации"
                            setState={setFullName}
                            errorText="Может принимать только текстовые значения"
                            validation={/^[а-яА-Я ]*$/}
                        />
                    </div>
                    <div className="w-[240px]">
                        <InputAdmin
                            placeholder="Короткое название организации"
                            setState={setShortName}
                            errorText="Может принимать только текстовые значения"
                            validation={/^[а-яА-Я ]*$/}
                        />
                    </div>
                    <div className="w-[140px]">
                        <InputAdmin
                            placeholder="Номер ИНН"
                            setState={setInnNumber}
                            errorText="Поле обязательно для заполнения"
                            validation={/./}
                        />
                    </div>
                    <div className="w-[190px]">
                        <InputAdmin
                            placeholder="Юрисдикция"
                            setState={setJurisdiction}
                            errorText="Поле обязательно для заполнения"
                            validation={/./}
                        />
                    </div>
                    
                </div>
                <p className="mt-[15px]">Код эмитента (поле EMIT_ID в вашей системе)</p>
                <div className="w-[190px]">
                    <Input
                        onChange={(e) => setIssuerId(e.target.value)}
                        placeholder={'Введите ID эмитента'}
                        classNames={{ input: 'h-[31px] w-full text-[14px]/[18px] placeholder:text-[#C4C4C4] pl-[12px] pb-[5px]' }}
                    ></Input>
                </div>
            </div>
            <div>
                <p>ОГРН</p>
                <div className="flex w-[600px] space-x-[15px]">
                    <div className="w-[240px]">
                        <InputAdmin
                            placeholder="Кем выдан"
                            setState={setOgrnBy}
                            errorText="Может принимать только текстовые значения"
                            validation={/^[а-яА-Я ]*$/}
                        />
                    </div>
                    <div className="w-[140px]">
                        <Calendar placeholder="Дата выдачи" setDate={setOgrnDate} />
                    </div>
                    <div className="w-[190px]">
                        <InputAdmin
                            placeholder="Номер"
                            setState={setOgrnNumber}
                            errorText="Поле обязательно для заполнения"
                            validation={/./}
                        />
                    </div>
                </div>
            </div>
             <div>
                <p>Регистрация</p>
                <div className="flex w-[600px] space-x-[15px]">
                    <div className="w-[240px]">
                        <InputAdmin
                            placeholder="Кем выдан"
                            setState={setRegBy}
                            errorText="Может принимать только текстовые значения"
                            validation={/^[а-яА-Я ]*$/}
                        />
                    </div>
                    <div className="w-[140px]">
                        <Calendar placeholder="Дата выдачи" setDate={setRegDate} />
                    </div>
                    <div className="w-[190px]">
                        <InputAdmin
                            placeholder="Номер"
                            setState={setRegNumber}
                            errorText="Поле обязательно для заполнения"
                            validation={/./}
                        />
                    </div>
                </div>
            </div>

            <Button onClick={onEmitterRegistration}>Добавить</Button>
        </div>
    )
}