'use client'

import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { InputForm } from "@/app/ui/forms/input"
import { CheckBox } from "@/app/ui/forms/checkbox";
import { RequestListOfEntitled } from "@/app/services/orderReportsService";
import { useState } from "react";
import Calendar from "@/app/ui/forms/calendar-new";

export default function Page() {

  const [papersCategory, setPapersCategory] = useState<string>('')
  const [formOfPayment, setFormOfPayment] = useState<string>('')
  const [paymentForOne, setPaymentForOne] = useState<string>('')
  const [paymentForAll, setPaymentForAll] = useState<string>('')
  const [dateOfPayment, setDateOfPayment] = useState<string>('')
  const [fullOfficialName, setFullOfficialName] = useState<string>('')
  const [placeOfAgents, setPlaceOfAgents] = useState<string>('')
  const [mailAddress, setMailAddress] = useState<string>('')
  const [includeCalcInfo, setIncludeCalcInfo] = useState<boolean>(false)

  const onNextPageTransition = () => {
    const request = localStorage.getItem('request_listEntitled')
    const requestData = request ? JSON.parse(request) as RequestListOfEntitled : null

    if (requestData) {
      requestData.forDbSaving.stepTwo = {
        papersCategory: papersCategory, // Категории ценных бумаг
        formOfPayment: formOfPayment, // Форма выплат
        paymentForOne: paymentForOne, // Выплачиваемый доход на одну акцию
        paymentForAll: paymentForAll, // Выплачиваемый доход, общий объем
        dateOfPayment: dateOfPayment, // Дата выплаты
        fullOfficialName: fullOfficialName, // Полное оф. наименование агента(ов)
        placeOfAgents: placeOfAgents, // Место нахождения агента(ов)
        mailAddress: mailAddress, // Почтовый адрес агента(ов)
        includeCalcInfo: includeCalcInfo // Включить инорфмацию о расчете налога
      }

      localStorage.setItem('request_listEntitled', JSON.stringify(requestData))
    }
  }

  return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
      <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента о предоставлении Списка лиц , имеющих право на получение доходов по ценным бумагам</p>
      <div className="border-[0.5px] border-black rounded-[28px] py-[26px] px-[40px] mb-[23px]">
        <div className="flex items-center mb-[14px]">
          <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Категории ценных бумаг, по которым выплачивается доход</p>
          <div className="w-[532px]"><InputForm setState={setPapersCategory} placeholder="Введите категории" /></div>
        </div>
        <div className="flex items-center mb-[14px]">
          <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Форма осуществления выплат доходов</p>
          <div className="w-[532px]"><InputForm setState={setFormOfPayment} placeholder="Введите форму" /></div>
        </div>
        <div className="flex items-center mb-[14px]">
          <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Выплачиваемый доход по каждому виду ценных бумаг</p>
          <div className="flex w-[532px]">
            <div className="mr-[17px] w-[256px]"><InputForm setState={setPaymentForOne} placeholder="На одну акцию" /></div>
            <div className="w-[256px]"><InputForm setState={setPaymentForAll} placeholder="Общий объем" /></div>
          </div>
        </div>
        <div className="flex items-center mb-[14px]">
          <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Дата выплаты доходов</p>
          <div className="w-[532px]"><Calendar setDate={setDateOfPayment} /></div>
        </div>
        <div className="flex items-center mb-[14px]">
          <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Полное официальное наименование агента(ов) по выплате доходов (при его (их) наличии)</p>
          <div className="w-[532px]"><InputForm setState={setFullOfficialName} placeholder="Введите форму" /></div>
        </div>
        <div className="flex items-center mb-[14px]">
          <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Место нахождения агента(ов)</p>
          <div className="w-[532px]"><InputForm setState={setPlaceOfAgents} placeholder="Место нахождения агентов" /></div>
        </div>
        <div className="flex items-center mb-[14px]">
          <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Почтовый адрес агента(ов)</p>
          <div className="w-[532px]"><InputForm setState={setMailAddress} placeholder="Почтовый адрес" /></div>
        </div>
      </div>
      <div className="flex items-center mb-[14px] ml-[40px]">
        <div className="mt-[-3px]"><CheckBox setState={setIncludeCalcInfo} text="" /></div>
        <p className="text-[16px]/[21px]">Включить информацию о расчете налога</p>
      </div>
      <p className="text-[13px] ml-[64px]">Делая отметку о включении информации о расчете налога в Список лиц, имеющих право на получение доходов по ценным бумагам, эмитент подтверждает согласие на оплату данной услуги</p>
      <NavigationButtons back="/forms/third/step-one" next="/forms/third/step-three" onClick={onNextPageTransition} />
    </div>
  );
}