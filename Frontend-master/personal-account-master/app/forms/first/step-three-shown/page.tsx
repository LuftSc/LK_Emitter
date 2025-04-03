import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { InputForm } from "@/app/ui/forms/input"
import { InputFormHeight } from "@/app/ui/forms/inputWithHeight";

export default function Page () {
  return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
            <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на список к ОСА</p>
            <div className="border-[0.5px] border-black rounded-[28px] p-[26px]">
                <div className="flex mb-[10px]">
                    <p className="max-w-[256px] text-[14px]/[18px] mr-[154px]">Контактные лица эмитента для обращения владельцев ценных бумаг</p>
                    <div className="w-full">
                        <div className="flex items-center mb-[10px]">
                            <p className="min-w-[122px] text-[14px]/[18px] text-right mr-[18px]">ФИО</p>
                            <InputForm placeholder=""/>
                        </div>
                        <div className="flex items-center mb-[10px]">
                            <p className="min-w-[122px] text-[14px]/[18px] text-right mr-[18px]">Электронный адрес</p>
                            <InputForm placeholder=""/>
                        </div>
                        <div className="flex items-center">
                            <p className="min-w-[122px] text-[14px]/[18px] text-right mr-[18px]">Телефон</p>
                            <InputForm placeholder="+7( _ _ _ ) _ _ _ - _ _ - _ _"/>
                        </div>
                    </div>
                </div>
                <div className="flex items-center">
                    <p className="max-w-[343px] text-[14px]/[18px] mr-[38px]">Порядок ознакомления с информацией (материалами), подлежащей (подлежащими) предоставлению при подготовке к проведению общего собрания участников (акционеров) эмитента, и адрес (адреса), по которому (которым) с ней можно ознакомиться:</p>
                    <div className="flex flex-col w-full items-center">
                        <div className="w-full h-[70px] mb-[10px]"><InputFormHeight placeholder=""/></div>
                        <p className="text-[12px]/[16px] text-[#464343]">(Заполнение обязательно при наличии счета НД Центрального депозитария)</p>
                    </div>
                </div>
            </div>
            <NavigationButtons back='/forms/first/step-two-shown' next='/forms/first/step-four-shown'/>
     </div>
  );
}
