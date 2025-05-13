'use client'

import { CheckBox } from "@/app/ui/forms/checkbox";
import { RadioButton } from "@/app/ui/forms/radiobtn";
import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { RequestInfoFromRegistry, sendRequestReeRep } from "@/app/services/orderReportsService";
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

  const onNextPageTransition = async () => {
    const request = localStorage.getItem('request_regInfo')
    const requestData = request ? JSON.parse(request) as RequestInfoFromRegistry : null
    const emitter = localStorage.getItem('emitter')
    const emitterData = emitter ? JSON.parse(emitter) : null

    if (requestData) {
      requestData.forReportGenerating = {
        emitId: emitterData.IssuerId, // код эмитента
        procUk: procUk, // цифра из поля процентов на 2 странице формы
        nomList: false, // флажок на раскрытие списков НД
        dtMod: dtMod || '', // Дата на которую необходимо предоставить информацию
        oneProcMode: infoPeopleShort
      }
      requestData.forDbSaving.stepTwo = {
        listPaperOwners: listPaperOwners, // Чекбокс "Список владельцев ценных бумаг"
        infoPeopleWithOpenAccount: infoPeopleWithOpenAccount, // Радио "Информация о людях, которым открыт лицевой счет"
        listFundPersentageOwners: listFundPersentageOwners, // Чекбокс "Список лиц, владеющих % от Уставного капитала"
        certificateAboutStructure: certificateAboutStructure, // Чекбокс "Справка о структуре распределения акций"
        includeInfoShown: false
      }

      localStorage.setItem('request_regInfo', JSON.stringify(requestData))

      await onRequestReeRep();
    }
  }

  const onRequestReeRep = async () => {
    const request = localStorage.getItem('request_regInfo')
    const requestInfo = request ? JSON.parse(request) as RequestInfoFromRegistry : null

    if (requestInfo) {
        const regInfoRequest = {
          requestData: {
            emitId: requestInfo.forReportGenerating.emitId, // код эмитента
            procUk: requestInfo.forReportGenerating.procUk, // цифра из поля процентов на 2 странице формы
            nomList: requestInfo.forReportGenerating.nomList, // флажок на раскрытие списков НД
            dtMod: requestInfo.forReportGenerating.dtMod, // Дата на которую необходимо предоставить информацию
            oneProcMode: requestInfo.forReportGenerating.oneProcMode,
            fullName: requestInfo.forDbSaving.stepOne.fullName, // полное наименование эмитента
            listPaperOwners: requestInfo.forDbSaving.stepTwo.listPaperOwners, // Чекбокс "Список владельцев ценных бумаг"
            infoPeopleWithOpenAccount: requestInfo.forDbSaving.stepTwo.infoPeopleWithOpenAccount, // Радио "Информация о людях, которым открыт лицевой счет"
            listFundPersentageOwners: requestInfo.forDbSaving.stepTwo.listFundPersentageOwners, // Чекбокс "Список лиц, владеющих % от Уставного капитала"
            certificateAboutStructure: requestInfo.forDbSaving.stepTwo.certificateAboutStructure, // Чекбокс "Справка о структуре распределения акций"
            includeInfoShown: false, // Чекбокс "включая сведения о лицах..."
            certificateAboutState: requestInfo.forDbSaving.stepTwo.certificateAboutStructure, // Чекбокс о Справке о состоянии лицевого счета
          }
        }
        console.log(regInfoRequest)
        await sendRequestReeRep(regInfoRequest);
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
          <div className="mb-[9px]">
            <CheckBox setState={setInfoPeopleShort} text="в сокращенной форме на дату" />
            <span className="text-[14px]/[18px]">{dtMod}</span>
          </div>
          <div className="flex items-center mb-[9px]">
            <CheckBox setState={setListFundPersentageOwners} text="на которых учитывается" />
            <div className="w-[50px] mx-[9px]"><InputFormNumber setState={setProcUk} placeholder="" /></div>
            <p className="text-[14px]/[18px]">и более % от Уставного капитала на дату {dtMod}</p>
          </div>
          <CheckBox setState={setCertificateAboutStructure} text="в виде Справки о структуре распределения акций на дату " />
          <span className="text-[14px]/[18px]">{dtMod}</span>
        </div>
      </div>
      <NavigationButtons back='/forms/second/step-one' next='/forms/mainSecond' onClick={onNextPageTransition} />
    </div>
  );
}
