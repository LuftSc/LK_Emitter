'use client'

import { useState } from "react"
import { InputAdmin } from "./inputAdmin"
import { Button } from "antd"
import Calendar from "../forms/calendar-new"

export default function MainContentNewEmitent() {

    const [fullName, setFullName] = useState<string>('')
    const [innNumber, setInnNumber] = useState<string>('')
    const [jurisdiction, setJurisdiction] = useState<string>('')
    const [ogrnBy, setOgrnBy] = useState<string>('')
    const [ogrnDate, setOgrnDate] = useState<string>('')
    const [ogrnNumber, setOgrnNumber] = useState<string>('')
    const [regBy, setRegBy] = useState<string>('')
    const [regNumber, setRegNumber] = useState<string>('')
    const [regDate, setRegDate] = useState<string>('')

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
            <Button>Добавить</Button>
        </div>
    )
}