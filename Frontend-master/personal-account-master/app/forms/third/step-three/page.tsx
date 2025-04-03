import { InputForm } from "@/app/ui/forms/input"
import { RadioButton } from "@/app/ui/forms/radiobtn";
import { NavigationButtons } from "@/app/ui/forms/nav-btn";

export default function Page () {
  return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
          <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента о предоставлении Списка лиц , имеющих право на получение доходов по ценным бумагам</p>
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
      <NavigationButtons back="/forms/third/step-two" next="" />
     </div>
  );
}