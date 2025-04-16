import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { CheckBox } from "@/app/ui/forms/checkbox";
import { InputForm } from "@/app/ui/forms/input";
import CalendarInput from "@/app/ui/forms/calendar";
import { RadioButton } from "@/app/ui/forms/radiobtn";

export default function Page () {
  return (
    <div className="relative w-[1104px] h-[944px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
          <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на предоставление информации из реестра</p>
          <div className="border-[0.5px] border-black rounded-[28px] pt-[21px] pl-[26px] pb-[26px] mb-[45px]">
            <div className="flex items-center mb-[9px]">
                <div className="mt-[-4px]"><CheckBox text=""/></div>
                <p className="text-[14px]/[18px] font-bold mr-[26px]">Справка о состоянии лицевого счета зарегистрированного лица на дату</p>
                <CalendarInput calendarId="stateOnDate"/>
            </div>
            <div className="ml-[39px] mb-[9px]">
                <div className="flex items-center mb-[9px]">
                    <p className="text-[14px]/[18px] mr-[50px]">Наименование/ФИО</p>
                    <div className="w-[424px]"><InputForm placeholder="" /></div>
                </div>
                <div className="flex items-center">
                    <p className="text-[14px]/[18px] mr-[88px]">ОГРН/Паспорт</p>
                    <div className="w-[424px]"><InputForm placeholder="" /></div>
                </div>
            </div>
            <div className="flex items-center">
                <CheckBox text="Иное:"/>
                <div className="w-[407px] ml-[26px]"><InputForm placeholder="" /></div>
            </div>
          </div>
          <div className="border-[0.5px] border-black rounded-[28px] p-[26px] mb-[45px]">
            <p className="max-w-[795px] text-[14px]/[18px] font-bold mb-[7px]">Данная информация необходима Эмитенту для исполнения следующих требований законодательства Российской Федерации
            (выбрать предложенный вариант или указать свой, руководствуясь нормативными правовыми актами Российской Федерации):</p>
            <div className="max-w-[920px] mb-[5px]">
                <CheckBox text="статья 6.1 Федерального закона от 07.08.2001 №115-ФЗ 'О противодействии легализации (отмыванию) доходов, полученных преступным путем, и финансированию терроризма';"/>
                <CheckBox text="статья 51 Федерального закона от 05.04.2013 N 44-ФЗ 'О контрактной системе в сфере закупок товаров, работ, услуг для обеспечения государственных и муниципальных нужд';"/>
                <CheckBox text="статья 30 Федерального закона от 22.04.1996 №39-ФЗ 'О рынке ценных бумаг';"/>
                <CheckBox text="статья 20 Федерального закона от 30.12.2004 №214-ФЗ 'Об участии в долевом строительстве многоквартирных домов и иных объектов недвижимости и о внесении изменений в некоторые законодательные акты РФ' (только для эмитентов - застройщиков);"/>
                <CheckBox text="статья 17 Федеральный закон от 08.03.2022 N 46-ФЗ (ред. от 14.03.2022) 'О внесении изменений в отдельные законодательные акты Российской Федерации'"/>
            </div>
            <div className="flex items-center">
                <CheckBox text="Иное:"/>
                <div className="w-[557px] ml-[26px]"><InputForm placeholder=""/></div>
            </div>
          </div>
          <div className="border-[0.5px] border-black rounded-[28px] pt-[21px] pl-[26px] pb-[26px]">
            <div className="flex items-center mb-[5px]">
              <p className="text-[14px]/[18px] mr-[70px]">Уполномоченный представитель эмитента:</p>
              <div className="w-[424px]"><InputForm placeholder="Введите ФИО"/></div>
            </div>
            <div className="flex items-center">
              <div className="mr-[45px]"><RadioButton name="emitentAgent" text="Устав"/></div>
              <div className="mr-[50px]"><RadioButton name="emitentAgent" text="Доверенность"/></div>
              <p className="text-[14px]/[18px] mr-[35px]">№, от</p>
              <div className="w-[129px]"><InputForm placeholder=""/></div>
            </div>
          </div>
          <NavigationButtons back='/forms/second/step-two-shown' next='' />
     </div>
  );
}