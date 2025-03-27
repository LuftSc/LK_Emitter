import { Step } from "@/app/ui/forms/step"
import { InputForm } from "@/app/ui/forms/input"
import { CheckBox } from "@/app/ui/forms/checkbox"
import { RadioButton } from "@/app/ui/forms/radiobtn";
import CalendarInput from "@/app/ui/forms/calendar";

export default function Page () {
  return (
    <div className="w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
      <Step back='' next='/forms/first/step-two' >
          <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на список к ОСА</p>
          <div className="border-[0.5px] border-black rounded-[28px] pt-[23px] pl-[26px] pb-[25px] mb-[45px]">
            <p className="font-bold leading-[18px] text-sm mb-[9px]">Предоставить:</p>
            <div className="flex flex-col gap-[9px] mb-[24px]">
              <CheckBox text="Список лиц, имеющих право на участие в общем собрании акционеров" />
              <CheckBox text="Список лиц, осуществляющих права по ценным бумагам" />
              <div className="w-[594px]"><CheckBox text="Список лиц, имеющих право на участие в общем собрании акционеров, без персональных данных и данных о волеизъявлении для ознакомления (п. 4. Ст. 51 ФЗ об АО)." /></div>
            </div>
            <div className="flex">
              <p className="text-[14px]/[18px] mr-[45px]">Дата определения (фиксации) лиц</p>
              <CalendarInput calendarId="dateFix"/>
            </div>
          </div>
          <div className="border-[0.5px] border-black rounded-[28px] pt-[14px] pl-[26px] pb-[25px]">
            <p className="text-[#B82116] text-[14px]/[18px] font-bold text-center mb-[20px]">Сведения об общем собрании акционеров:</p>
            <div className="flex mb-[20px]">
              <p className="w-[229px] text-[14px]/[18px] mr-[16px]">Решение о проведении общего собрания акционеров принято:</p>
              <div className="flex flex-col gap-[10px]">
                <RadioButton name="decisionMadeBy" text="Советом директоров"/>
                <div className="flex w-[457px]">
                  <RadioButton name="decisionMadeBy" text=""/>
                  <InputForm placeholder="Введите наименование" />
                </div>
                <div className="flex">
                  <div className="flex w-[149px] mr-[20px]">
                    <p className="text-[14px]/[18px] mr-[11px]">№</p>
                    <InputForm placeholder="" />
                  </div>
                  <p className="mr-[27px]">дата принятия решения</p>
                  <CalendarInput calendarId="dateDecision"/>
                </div>
              </div>
            </div>
            <div className="flex mb-[15px]">
              <p className="text-[14px]/[18px] mr-[65px]">Вид и форма проведения собрания</p>
              <div className="mr-[45px]"><RadioButton name="typeOfMeeting" text="Годовое"/></div>
              <div className="mr-[65px]"><RadioButton name="typeOfMeeting" text="Внеочередное"/></div>
              <div className="mr-[45px]"><RadioButton name="formOfMeeting" text="Совместное присутствие"/></div>
              <div><RadioButton name="formOfMeeting" text="Заочное голосование"/></div>
            </div>
            <div className="flex">
              <p className="text-[14px]/[18px] mr-[30px]">Дата проведения собрания</p>
              <CalendarInput calendarId="dateMeet"/>
            </div>
          </div>  
      </Step>
     </div>
  );
}
