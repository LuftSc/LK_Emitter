'use client'

import { CheckBox } from "@/app/ui/forms/checkbox";
import { RadioButton } from "@/app/ui/forms/radiobtn";
import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { RequestInfoFromRegistry } from "@/app/services/orderReportsService";
import { useState } from "react";
import { InputFormNumber } from "@/app/ui/forms/inputNumbers";
import Calendar from "@/app/ui/forms/calendar-new";

export default function Page() {

  const [procUk, setProcUk] = useState<number>(0)
  const [dtMod, setDtMod] = useState<string>('')

  const [listPaperOwners, setListPaperOwners] = useState<boolean>(false)
  const [infoPeopleWithOpenAccount, setInfoPeopleWithOpenAccount] = useState<boolean>(false)
  const [infoPeopleShort, setInfoPeopleShort] = useState<boolean>(false)
  const [listFundPersentageOwners, setListFundPersentageOwners] = useState<boolean>(false)
  const [certificateAboutStructure, setCertificateAboutStructure] = useState<boolean>(false)

  const onNextPageTransition = () => {
    const request = localStorage.getItem('request_regInfo')
    const requestData = request ? JSON.parse(request) as RequestInfoFromRegistry : null
    const emitter = localStorage.getItem('emitter')
    const emitterData = emitter ? JSON.parse(emitter) : null

    if (requestData) {
      requestData.forReportGenerating = {
        emitId: emitterData.IssuerId, // код эмитента
        procUk: procUk, // цифра из поля процентов на 2 странице формы
        nomList: false, // флажок на раскрытие списков НД
        dtMod: dtMod // Дата на которую необходимо предоставить информацию
      }
      requestData.forDbSaving.stepTwo = {
        listPaperOwners: listPaperOwners, // Чекбокс "Список владельцев ценных бумаг"
        infoPeopleWithOpenAccount: infoPeopleWithOpenAccount, // Радио "Информация о людях, которым открыт лицевой счет"
        infoPeopleShort: infoPeopleShort, // Чекбокс "Информация о людях в сокращенной форме"
        listFundPersentageOwners: listFundPersentageOwners, // Чекбокс "Список лиц, владеющих % от Уставного капитала"
        certificateAboutStructure: certificateAboutStructure // Чекбокс "Справка о структуре распределения акций"
      }

      localStorage.setItem('request_regInfo', JSON.stringify(requestData))
    }
  }

  return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
      <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на предоставление информации из реестра</p>
      <div className="border-[0.5px] border-black rounded-[28px] pt-[29px] pl-[35px] pb-[32px]">
        <p className="text-[14px]/[18px] font-bold mb-[9px]">Описание требуемой информации:</p>
        <div className="flex items-center mb-[9px]">
          <CheckBox setState={setListPaperOwners} text="Список владельцев ценных бумаг на дату" />
          <Calendar setDate={setDtMod} />
        </div>
        <div className="mb-[9px]"><RadioButton setState={setInfoPeopleWithOpenAccount} text="Информация о лицах, которым открыт лицевой счет, и о количестве ценных бумаг, которые учитываются на указанных лицевых счетах:" /></div>
        <div className="ml-[40px]">
          <div className="mb-[9px]"><CheckBox setState={setInfoPeopleShort} text="в сокращенной форме на дату" /></div>
          <div className="flex items-center mb-[9px]">
            <CheckBox setState={setListFundPersentageOwners} text="на которых учитывается" />
            <div className="w-[50px] mx-[9px]"><InputFormNumber setState={setProcUk} placeholder="" /></div>
            <p className="text-[14px]/[18px]">и более % от Уставного капитала на дату</p>
          </div>
          <CheckBox setState={setCertificateAboutStructure} text="в виде Справки о структуре распределения акций на дату " />
        </div>
      </div>
      <NavigationButtons back='/forms/second/step-one' next='/forms/second/step-three' onClick={onNextPageTransition} />
    </div>
  );
}
