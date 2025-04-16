import { CheckBox } from "@/app/ui/forms/checkbox";
import { RadioButton } from "@/app/ui/forms/radiobtn";
import { InputForm } from "@/app/ui/forms/input";
import { NavigationButtons } from "@/app/ui/forms/nav-btn";

export default function Page () {
  return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
          <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на предоставление информации из реестра</p>
          <div className="border-[0.5px] border-black rounded-[28px] pt-[29px] pl-[35px] pb-[32px]">
            <p className="text-[14px]/[18px] font-bold mb-[9px]">Описание требуемой информации:</p>
            <div className="mb-[9px]"><CheckBox text="Список владельцев ценных бумаг на дату" /></div>
            <div className="mb-[9px]"><RadioButton name="infoPeople" text="Информация о лицах, которым открыт лицевой счет, и о количестве ценных бумаг, которые учитываются на указанных лицевых счетах:"/></div>
            <div className="ml-[40px]">
                <div className="mb-[9px]"><CheckBox text="в сокращенной форме на дату "/></div>
                <div className="flex items-center mb-[9px]">
                    <CheckBox text="на которых учитывается"/>
                    <div className="w-[50px] mx-[9px]"><InputForm placeholder=""/></div>
                    <p className="text-[14px]/[18px]">и более % от Уставного капитала на дату</p>
                </div>
                <CheckBox text="в виде Справки о структуре распределения акций на дату "/>
            </div>
          </div>
          <NavigationButtons back='/forms/second/step-one' next='/forms/second/step-three' />
     </div>
  );
}
