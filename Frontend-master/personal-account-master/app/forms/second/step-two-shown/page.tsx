'use client'

import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { CheckBox } from "@/app/ui/forms/checkbox";
import { useState } from "react";
import { RequestInfoFromRegistry } from "@/app/services/orderReportsService";
import Calendar from "@/app/ui/forms/calendar-new";
import { InputFormNumber } from "@/app/ui/forms/inputNumbers";

export default function Page() {

  /*const [procUk, setProcUk] = useState<number>(0)
  const [dtMod, setDtMod] = useState<string>('')

  const [listPaperOwners, setListPaperOwners] = useState<boolean>(false)
  const [infoPeopleWithOpenAccount, setInfoPeopleWithOpenAccount] = useState<boolean>(false)
  const [includeInfo, setIncludeInfo] = useState<boolean>(false)
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
        nomList: true, // флажок на раскрытие списков НД
        dtMod: dtMod, // Дата на которую необходимо предоставить информацию
        oneProcMode: false
      }
      requestData.forDbSaving.stepTwo = {
        listPaperOwners: listPaperOwners, // Чекбокс "Список владельцев ценных бумаг"
        listFundPersentageOwners: listFundPersentageOwners, // Чекбокс "Список лиц, владеющих % от Уставного капитала"
        includeInfo: includeInfo, // Чекбокс "включая сведения о лицах..."
        infoPeopleWithOpenAccount: infoPeopleWithOpenAccount, // Чекбокс "Информация о людях, которым открыт лицевой счет"
        certificateAboutStructure: certificateAboutStructure // Чекбокс "Справка о структуре распределения акций"
      }

      localStorage.setItem('request_regInfo', JSON.stringify(requestData))
    }
  }

  return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
      <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на предоставление информации из реестра</p>
      <div className="border-[0.5px] border-black rounded-[28px] p-[26px] mb-[45px]">
        <p className="text-[14px]/[18px] font-bold mb-[9px]">Описание требуемой информации:</p>
        <div className="flex items-center mb-[9px]">
          <p className="text-[14px]/[18px] mr-[30px]">Дата, на которую необходимо предоставить информацию</p>
          <Calendar setDate={setDtMod} />
        </div>
        <div className="mb-[9px]"><CheckBox setState={setListPaperOwners} text="Спиок владельцев ценных бумаг" /></div>
        <div className="flex items-center mb-[4px]">
          <CheckBox setState={setListFundPersentageOwners} text="Список лиц, владеющих" />
          <div className="w-[50px] mx-[9px]"><InputFormNumber setState={setProcUk} placeholder="" /></div>
          <p className="text-[14px]/[18px]">и более % от Уставного капитала на дату</p>
        </div>
        <div className="flex items-center ml-[22px]">
          <CheckBox setState={setIncludeInfo} text="" />
          <p className="text-[12px]/[16px]">включая сведения о лицах, в интересах которых осуществляются права по ценным бумагам (при наличии)</p>
        </div>
      </div>
      <div className="border-[0.5px] border-black rounded-[28px] p-[26px]">
        <p className="text-[14px]/[18px] font-bold italic mb-[9px]">Нижеуказанная информация предоставляется:</p>
        <div className="flex mb-[9px]">
          <div className="min-w-[8px] h-[8px] rounded-full bg-black inline-block mt-[6px] mr-[5px]" />
          <p className="text-[14px]/[18px] italic">при наличии у реестродержателя на указанную дату списка владельцев, учитывающих свои ценные бумаги у номинального держателя, с учетом данного списка;</p>
        </div>
        <div className="flex mb-[12px]">
          <div className="min-w-[8px] h-[8px] rounded-full bg-black inline-block mt-[6px] mr-[5px]" />
          <p className="text-[14px]/[18px] italic">при отсутствии - с учетом лиц, которым открыты счета депо (список депонентов НД)</p>
        </div>
        <div className="mb-[9px]"><CheckBox setState={setInfoPeopleWithOpenAccount} text="Информация о лицах, которым открыт лицевой счет" /></div>
        <div className="mb-[9px]"><CheckBox setState={setInfoPeopleShort} text="Информация о лицах в сокращенной форме" /></div>
        <CheckBox setState={setCertificateAboutStructure} text="Справка о структуре распределения акций" />
      </div>
      <NavigationButtons back='/forms/second/step-one' next='/forms/second/step-three' onClick={onNextPageTransition} />
    </div>
  ); */
}
