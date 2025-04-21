'use client'

import { RequestListOfEntitled } from "@/app/services/orderReportsService";
import Calendar from "@/app/ui/forms/calendar-new";
import { InputForm } from "@/app/ui/forms/input"
import { InputFormNumber } from "@/app/ui/forms/inputNumbers";
import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { useState } from "react";

export default function Page() {

  const [dtClo, setDtClo] = useState<string>('')
  const [fullEmName, setFullEmName] = useState<string>('')
  const [decidingAuthority, setDecidingAuthority] = useState<string>('')
  const [dateOfProtocol, setDateofProtocol] = useState<string>('')
  const [numberOfProtocol, setNumberOfProtocol] = useState<number>(0)

  const onNextPageTransition = async () => {
    const emitter = localStorage.getItem('emitter')
    const emitterData = emitter ? JSON.parse(emitter) : null

    const requestData = {
      forReportGenerating: {
        issuerId: emitterData.IssuerId,
        dtClo: dtClo
      },
      forDbSaving: {
        stepOne: {
          fullEmName: fullEmName, // Полное наименование эмитента
          decidingAuthority: decidingAuthority, // Орган, управления эмитента, принявший решение...
          dateOfProtocol: dateOfProtocol, // Дата протокола
          numberOfProtocol: numberOfProtocol // Номер протокола
        }
      }

    } as RequestListOfEntitled

    localStorage.setItem('request_listEntitled', JSON.stringify(requestData))
  }

  return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
      <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента о предоставлении Списка лиц , имеющих право на получение доходов по ценным бумагам</p>
      <div className="border-[0.5px] border-black rounded-[28px] py-[26px] px-[42px]">
        <div className="flex items-center mb-[14px]">
          <p className="min-w-[306px] text-[14px]/[18px] mr-[40px]">Полное наименование эмитента</p>
          <InputForm setState={setFullEmName} placeholder="Введите наименование" />
        </div>
        <div className="flex items-center mb-[14px]">
          <p className="max-w-[306px] text-[14px]/[18px] mr-[40px]">Орган управления эмитента, принявший решение о выплате доходов по ценным бумагам</p>
          <InputForm setState={setDecidingAuthority} placeholder="Введите наименование" />
        </div>
        <div className="flex items-center mb-[14px]">
          <p className="w-[306px] text-[14px]/[18px] mr-[40px]">Дата соответствующего протокола</p>
          <Calendar setDate={setDateofProtocol} />
        </div>
        <div className="flex items-center mb-[14px]">
          <p className="w-[306px] text-[14px]/[18px] mr-[40px]">Номер соответствующего протокола</p>
          <div className="w-[334px]"><InputFormNumber setState={setNumberOfProtocol} placeholder="Введите номер протокола" /></div>
        </div>
        <div className="flex items-center mb-[14px]">
          <p className="w-[306px] text-[14px]/[18px] mr-[40px]">Дата составленного списка лиц, имеющих право на получение доходов по ценным бумагам</p>
          <Calendar setDate={setDtClo} />
        </div>
      </div>
      <NavigationButtons back="" next="/forms/third/step-two" onClick={onNextPageTransition} />
    </div>
  );
}