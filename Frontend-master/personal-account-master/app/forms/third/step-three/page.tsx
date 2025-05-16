'use client'

import { InputForm } from "@/app/ui/forms/input"
import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { RequestListOfEntitled, sendRequestDividendList } from "@/app/services/orderReportsService";
import { useState } from "react";
import { InputFormNumber } from "@/app/ui/forms/inputNumbers";
import { RadioGroup } from "@/app/ui/forms/radioGroup";

export default function Page() {

  const [emitentRepresentative, setEmitentRepresentative] = useState<string>('')
  const [isRegulationOrAttorney, setIsRegulationOrAttorney] = useState<boolean>(false)
  const [regulationNumber, setRegulationNumber] = useState<number>(0)

  const onNextPageTransition = async () => {
    const request = localStorage.getItem('request_listEntitled')
    const requestData = request ? JSON.parse(request) as RequestListOfEntitled : null

    if (requestData) {
      requestData.forDbSaving.stepThree = {
        emitentRepresentative: emitentRepresentative, // Уполномоченный представитель
        isRegulationOrAttorney: isRegulationOrAttorney, // 3 флажок Устав/Доверенность
        regulationNumber: regulationNumber // номер Устава или Доверенн
      }

      localStorage.setItem('request_listEntitled', JSON.stringify(requestData))
      await onRequestDividendList();
    }
  }

  const onRequestDividendList = async () => {
        const request = localStorage.getItem('request_listEntitled')
        const requestInfo = request ? JSON.parse(request) as RequestListOfEntitled : null
        if (requestInfo) {
            const divListRequest = {
              requestData: {
                issuerId: requestInfo.forReportGenerating.issuerId, // код эмитента
                dtClo: requestInfo.forReportGenerating.dtClo, // Дата на которую необходимо предоставить информацию
                fullEmName: requestInfo.forDbSaving.stepOne.fullEmName, // Полное наименование эмитента
                decidingAuthority: requestInfo.forDbSaving.stepOne.decidingAuthority, // Орган, управления эмитента, принявший решение...
                dateOfProtocol: requestInfo.forDbSaving.stepOne.dateOfProtocol, // Дата протокола
                numberOfProtocol: requestInfo.forDbSaving.stepOne.numberOfProtocol, // Номер протокола
                papersCategory: requestInfo.forDbSaving.stepTwo.papersCategory, // Категории ценных бумаг
                formOfPayment: requestInfo.forDbSaving.stepTwo.formOfPayment, // Форма выплат
                paymentForOne: requestInfo.forDbSaving.stepTwo.paymentForOne, // Выплачиваемый доход на одну акцию
                paymentForAll: requestInfo.forDbSaving.stepTwo.paymentForAll, // Выплачиваемый доход, общий объем
                dateOfPayment: requestInfo.forDbSaving.stepTwo.dateOfPayment, // Дата выплаты
                fullOfficialName: requestInfo.forDbSaving.stepTwo.fullOfficialName, // Полное оф. наименование агента(ов)
                placeOfAgents: requestInfo.forDbSaving.stepTwo.placeOfAgents, // Место нахождения агента(ов)
                mailAddress: requestInfo.forDbSaving.stepTwo.mailAddress, // Почтовый адрес агента(ов)
                includeCalcInfo: requestInfo.forDbSaving.stepTwo.includeCalcInfo, // Включить инорфмацию о расчете налога
                emitentRepresentative: requestInfo.forDbSaving.stepThree.emitentRepresentative, // Уполномоченный представитель
                isRegulationOrAttorney: requestInfo.forDbSaving.stepThree.isRegulationOrAttorney, // 3 флажок Устав/Доверенность
                regulationNumber: requestInfo.forDbSaving.stepThree.regulationNumber // номер Устава или Доверенности
              }
            }
            console.log(divListRequest)
            await sendRequestDividendList(divListRequest);
          }
    
        }

  return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
      <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента о предоставлении Списка лиц , имеющих право на получение доходов по ценным бумагам</p>
      <div className="border-[0.5px] border-black rounded-[28px] pt-[21px] pl-[26px] pb-[26px]">
        <div className="flex items-center mb-[5px]">
          <p className="text-[14px]/[18px] mr-[50px]">Уполномоченный представитель эмитента:</p>
          <div className="w-[424px]"><InputForm setState={setEmitentRepresentative} placeholder="Введите ФИО" /></div>
        </div>
        <div className="flex items-center">
          <RadioGroup firstText="Устав" secondText="Доверенность" setState={setIsRegulationOrAttorney} />
          <p className="text-[14px]/[18px] mr-[20px] ml-[59px]">№, от</p>
          <div className="w-[129px]"><InputFormNumber setState={setRegulationNumber} placeholder="" /></div>
        </div>
      </div>
  
      <NavigationButtons back="/forms/third/step-two" next='/forms/mainSecond' onClick={onNextPageTransition} />
    </div>
  );
}