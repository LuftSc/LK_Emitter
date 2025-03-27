import { Step } from "@/app/ui/forms/step"
import { InputForm } from "@/app/ui/forms/input"
import CalendarInput from "@/app/ui/forms/calendar";

export default function Page () {
  return (
    <div className="w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
      <Step back='' next='/forms/third/step-two' >
          <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента о предоставлении Списка лиц , имеющих право на получение доходов по ценным бумагам</p>
          <div className="border-[0.5px] border-black rounded-[28px] py-[26px] px-[42px]">
            <div className="flex items-center mb-[14px]">
                <p className="w-[306px] text-[14px]/[18px] mr-[40px]">Полное наименование эмитента</p>
                <InputForm placeholder="Введите наименование"/>
            </div>
            <div className="flex items-center mb-[14px]">
                <p className="w-[306px] text-[14px]/[18px] mr-[40px]">Орган управления эмитента, принявший решение о выплате доходов по ценным бумагам</p>
                <InputForm placeholder="Введите наименование"/>
            </div>
            <div className="flex items-center mb-[14px]">
                <p className="w-[306px] text-[14px]/[18px] mr-[40px]">Дата соответствующего протокола</p>
                <CalendarInput calendarId="dateProtocol"/>
            </div>
            <div className="flex items-center mb-[14px]">
                <p className="w-[306px] text-[14px]/[18px] mr-[40px]">Номер соответствующего протокола</p>
                <div className="w-[334px]"><InputForm placeholder="Введите номер протокола"/></div>
            </div>
            <div className="flex items-center mb-[14px]">
                <p className="w-[306px] text-[14px]/[18px] mr-[40px]">Дата составленного списка лиц, имеющих право на получение доходов по ценным бумагам</p>
                <CalendarInput calendarId="dateCreatedList"/>
            </div>
          </div>
      </Step>
     </div>
  );
}