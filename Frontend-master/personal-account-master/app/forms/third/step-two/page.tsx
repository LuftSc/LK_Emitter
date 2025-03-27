import { Step } from "@/app/ui/forms/step"
import { InputForm } from "@/app/ui/forms/input"
import CalendarInput from "@/app/ui/forms/calendar";
import { CheckBox } from "@/app/ui/forms/checkbox";

export default function Page () {
  return (
    <div className="w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
      <Step back='/forms/third/step-one' next='/forms/third/step-three' >
          <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента о предоставлении Списка лиц , имеющих право на получение доходов по ценным бумагам</p>
          <div className="border-[0.5px] border-black rounded-[28px] py-[26px] px-[40px] mb-[23px]">
            <div className="flex items-center mb-[14px]">
                <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Категории ценных бумаг, по которым выплачивается доход</p>
                <div className="w-[532px]"><InputForm placeholder="Введите категории"/></div>
            </div>
            <div className="flex items-center mb-[14px]">
                <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Форма осуществления выплат доходов</p>
                <div className="w-[532px]"><InputForm placeholder="Введите форму"/></div>
            </div>
            <div className="flex items-center mb-[14px]">
                <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Выплачиваемый доход по каждому виду ценных бумаг</p>
                <div className="flex w-[532px]">
                  <div className="mr-[17px] w-[256px]"><InputForm placeholder="На одну акцию"/></div>
                  <div className="w-[256px]"><InputForm placeholder="Общий объем"/></div>
                </div>
            </div>
            <div className="flex items-center mb-[14px]">
                <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Дата выплаты доходов</p>
                <div className="w-[532px]"><CalendarInput calendarId="dateIncomePayment"/></div>
            </div>
            <div className="flex items-center mb-[14px]">
                <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Полное официальное наименование агента(ов) по выплате доходов (при его (их) наличии)</p>
                <div className="w-[532px]"><InputForm placeholder="Введите форму"/></div>
            </div>
            <div className="flex items-center mb-[14px]">
                <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Место нахождения агента(ов)</p>
                <div className="w-[532px]"><InputForm placeholder="Место нахождения агентов"/></div>
            </div>
            <div className="flex items-center mb-[14px]">
                <p className="w-[364px] text-[14px]/[18px] mr-[40px]">Почтовый адрес агента(ов)</p>
                <div className="w-[532px]"><InputForm placeholder="Почтовый адрес"/></div>
            </div>
          </div>
          <div className="flex items-center mb-[14px] ml-[40px]">
            <div className="mt-[-3px]"><CheckBox text=""/></div>
            <p className="text-[16px]/[21px]">Включить информацию о расчете налога</p>
          </div>
          <p className="text-[13px] ml-[64px]">Делая отметку о включении информации о расчете налога в Список лиц, имеющих право на получение доходов по ценным бумагам, эмитент подтверждает согласие на оплату данной услуги</p>
      </Step>
     </div>
  );
}