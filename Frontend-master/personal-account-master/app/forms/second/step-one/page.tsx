'use client'
import { InputForm } from "@/app/ui/forms/input"
import { useState } from "react";
import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import ShowListsRadio from "@/app/ui/forms/showLists-radio";

export default function Page () {

  const [showLists, setShowLists] = useState<boolean>(false)

  return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
          <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на предоставление информации из реестра</p>
          <div className="flex items-center border-[0.5px] border-black rounded-[28px] pt-[29px] pl-[35px] pb-[32px]">
            <p className="text-[14px] mr-[46px]">Полное наименование элемента</p>
            <div className="w-[424px]">
              <InputForm placeholder="Введите наименование"/>
            </div>
          </div>
          <ShowListsRadio setShowLists={setShowLists}/>
          <NavigationButtons back='' next={showLists == true ? '/forms/second/step-two-shown' : '/forms/second/step-two' } />
     </div>
  );
}
