import { InputForm } from "@/app/ui/forms/input";
import { RadioButton } from "@/app/ui/forms/radiobtn";
import { NavigationButtons } from "@/app/ui/forms/nav-btn";

export default function Page () {
  return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]">
        <div>
          <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на список к ОСА</p>
          <div className="border-[0.5px] border-black rounded-[28px] pt-[26px] pl-[26px] pb-[25px] mb-[45px]">
            <div className="flex items-center mb-[16px]">
              <p className="text-[14px] mr-[38px]">В голосовании по всем вопросам повестки дня принимают участие</p>
              <div className="mr-[40px]">
                <RadioButton name="participateFirst" text="обыкновенные" />
              </div>
              <RadioButton name="participateFirst" text="привелигированные" />
            </div>
            <div className="flex items-center">
              <p className="text-[14px] mr-[16px]">В голосовании по вопросам повестки дня</p>
              <div className="flex items-center w-[153px] mr-[25px]">
                <p className="text-[14px]/[18px] mr-[9px]">№</p>
                <InputForm placeholder="" />
              </div>
              <p className="text-[14px] mr-[37px]">принимают участие</p>
              <RadioButton name="participateSecond" text="привелигированные" />
            </div>
          </div>
          <div className="border-[0.5px] border-black rounded-[28px] pt-[26px] pl-[26px] pb-[25px]">
            <div className="flex items-center mb-[5px]">
              <p className="text-[14px] mr-[50px]">Уполномоченный представитель эмитента:</p>
              <div className="w-[424px]">
                <InputForm placeholder="Введите ФИО" />
              </div>
            </div>
            <div className="flex items-center">
              <div className="mr-[45px]"><RadioButton name="representative" text="Устав"/></div>
              <div className="mr-[45px]"><RadioButton name="representative" text="Доверенность"/></div>
              <p className="text-[14px]/[18px] mr-[20px]">№, от</p>
              <div className="w-[129px]"><InputForm placeholder="" /></div>
            </div>
          </div>
         </div>
        <NavigationButtons back='/forms/first/step-three-shown' next='' />
    </div>
  );
}