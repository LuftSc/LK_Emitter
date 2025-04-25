'use client'

import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { CheckBox } from "@/app/ui/forms/checkbox";
import { InputForm } from "@/app/ui/forms/input";
import { useState } from "react";
import Calendar from "@/app/ui/forms/calendar-new";
import { RadioGroup } from "@/app/ui/forms/radioGroup";
import { InputFormNumber } from "@/app/ui/forms/inputNumbers";
import { CertificateOnDate } from "@/app/services/orderReportsService";

export default function Page() {
  const [dtMod, setDtMod] = useState<string>('')
  const [fcsName, setFcsName] = useState<string>('')
  const [ogrnPassport, setOgrnPassport] = useState<string>('')
  const [another, setAnother] = useState<boolean>(false)
  const [anotherText, setAnotherText] = useState<string>('')
  const [section61, setSection61] = useState<boolean>(false)
  const [section51, setSection51] = useState<boolean>(false)
  const [section30, setSection30] = useState<boolean>(false)
  const [section20, setSection20] = useState<boolean>(false)
  const [section17, setSection17] = useState<boolean>(false)
  const [anotherSection, setAnotherSection] = useState<boolean>(false)
  const [anotherSectionText, setAnotherSectionText] = useState<string>('')
  const [emitentRepresentative, setEmitentRepresentative] = useState<string>('')
  const [isRegulationOrAttorney, setIsRegulationOrAttorney] = useState<boolean>(false)
  const [regulationNumber, setRegulationNumber] = useState<number>(0)

  const onNextPageTransition = () => {

      const requestData = {
        dtMod: dtMod, // Дата на которую оформляется справка
        fcsName: fcsName, // Наименование/ФИО
        ogrnPassport: ogrnPassport, // ОГРН/Паспорт
        another: another, // Чекбокс на Иное
        anotherText: anotherText, // Иное
        section61: section61, // статья 6.1
        section51: section51,// статья 51
        section30: section30, // статья 30
        section20: section20, // статья 20
        section17: section17, // статья 17 
        anotherSection: anotherSection, // Чекбокс на Иное после статей
        anotherSectionText: anotherSectionText, // Иное после статей
        emitentRepresentative: emitentRepresentative, // Уполномоченный представитель
        isRegulationOrAttorney: isRegulationOrAttorney, // 3 флажок Устав/Доверенность
        regulationNumber: regulationNumber // номер Устава или Доверенности
      } as CertificateOnDate

      localStorage.setItem('request_certificateOnDate', JSON.stringify(requestData))
    }


  return (
    <div className="relative w-[1104px] h-[944px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
      <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Справка о состоянии лицевого счета зарегистрированного лица на определенную дату</p>
      <div className="border-[0.5px] border-black rounded-[28px] pt-[21px] pl-[26px] pb-[26px] mb-[45px]">
        <div className="flex items-center mb-[9px]">
          <p className="text-[14px]/[18px] font-bold mr-[26px]">Справка о состоянии лицевого счета зарегистрированного лица на дату</p>
          <Calendar setDate={setDtMod} />
        </div>
        <div className="ml-[39px] mb-[9px]">
          <div className="flex items-center mb-[9px]">
            <p className="text-[14px]/[18px] mr-[50px]">Наименование/ФИО</p>
            <div className="w-[424px]"><InputForm setState={setFcsName} placeholder="" /></div>
          </div>
          <div className="flex items-center">
            <p className="text-[14px]/[18px] mr-[88px]">ОГРН/Паспорт</p>
            <div className="w-[424px]"><InputForm setState={setOgrnPassport} placeholder="" /></div>
          </div>
        </div>
        <div className="flex items-center">
          <CheckBox setState={setAnother} text="Иное:" />
          <div className="w-[407px] ml-[26px]"><InputForm setState={setAnotherText} placeholder="" /></div>
        </div>
      </div>
      <div className="border-[0.5px] border-black rounded-[28px] p-[26px] mb-[45px]">
        <p className="max-w-[795px] text-[14px]/[18px] font-bold mb-[7px]">Данная информация необходима Эмитенту для исполнения следующих требований законодательства Российской Федерации
          (выбрать предложенный вариант или указать свой, руководствуясь нормативными правовыми актами Российской Федерации):</p>
        <div className="max-w-[920px] mb-[5px]">
          <CheckBox setState={setSection61} text="статья 6.1 Федерального закона от 07.08.2001 №115-ФЗ 'О противодействии легализации (отмыванию) доходов, полученных преступным путем, и финансированию терроризма';" />
          <CheckBox setState={setSection51} text="статья 51 Федерального закона от 05.04.2013 N 44-ФЗ 'О контрактной системе в сфере закупок товаров, работ, услуг для обеспечения государственных и муниципальных нужд';" />
          <CheckBox setState={setSection30} text="статья 30 Федерального закона от 22.04.1996 №39-ФЗ 'О рынке ценных бумаг';" />
          <CheckBox setState={setSection20} text="статья 20 Федерального закона от 30.12.2004 №214-ФЗ 'Об участии в долевом строительстве многоквартирных домов и иных объектов недвижимости и о внесении изменений в некоторые законодательные акты РФ' (только для эмитентов - застройщиков);" />
          <CheckBox setState={setSection17} text="статья 17 Федеральный закон от 08.03.2022 N 46-ФЗ (ред. от 14.03.2022) 'О внесении изменений в отдельные законодательные акты Российской Федерации'" />
        </div>
        <div className="flex items-center">
          <CheckBox setState={setAnotherSection} text="Иное:" />
          <div className="w-[557px] ml-[26px]"><InputForm setState={setAnotherSectionText} placeholder="" /></div>
        </div>
      </div>
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
      <NavigationButtons back='' next='/forms' onClick={onNextPageTransition} />
    </div>
  )
}
