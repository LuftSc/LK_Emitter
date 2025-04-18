'use client'

import { InputForm } from "@/app/ui/forms/input";
import { RadioButton } from "@/app/ui/forms/radiobtn";
import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { RequestListOfShareholders } from "@/app/services/orderReportsService";
import { useState } from "react";
import { RadioGroup } from "@/app/ui/forms/radioGroup";
import { InputFormNumber } from "@/app/ui/forms/inputNumbers";

export default function Page() {

  const [isParticipatingInVote, setIsParticipatingInVote] = useState<boolean>(false)
  const [agendaNumber, setAgendaNumber] = useState<number>(0)
  const [isParticipatingInVoteOnNumber, setIsParticipatingInVoteOnNumber] = useState<boolean>(false)
  const [emitentRepresentative, setEmitentRepresentative] = useState<string>('')
  const [isRegulationOrAttorney, setIsRegulationOrAttorney] = useState<boolean>(false)
  const [regulationNumber, setRegulationNumber] = useState<number>(0)

  const onNextPageTransition = () => {
    const request = localStorage.getItem('request_listOSA')
    const requestData = request ? JSON.parse(request) as RequestListOfShareholders : null

    if (requestData) {
      requestData.forDbSaving.stepFour = {
        isParticipatingInVote: isParticipatingInVote,
        agendaNumber: agendaNumber,
        isParticipatingInVoteOnNumber: isParticipatingInVoteOnNumber,
        emitentRepresentative: emitentRepresentative,
        isRegulationOrAttorney: isRegulationOrAttorney,
        regulationNumber: regulationNumber
      }

      localStorage.setItem('request_listOSA', JSON.stringify(requestData))
    }
  }

  return (
    <div className="relative w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]">
      <div>
        <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на список к ОСА</p>
        <div className="border-[0.5px] border-black rounded-[28px] pt-[26px] pl-[26px] pb-[25px] mb-[45px]">
          <div className="flex items-center mb-[16px]">
            <p className="text-[14px] mr-[38px]">В голосовании по всем вопросам повестки дня принимают участие</p>
            <RadioGroup firstText="обыкновенные" secondText="привилегированные" setState={setIsParticipatingInVote} />
          </div>
          <div className="flex items-center">
            <p className="text-[14px] mr-[16px]">В голосовании по вопросам повестки дня</p>
            <div className="flex items-center w-[140px] mr-[25px]">
              <p className="text-[14px]/[18px] mr-[9px]">№</p>
              <InputFormNumber setState={setAgendaNumber} placeholder="" />
            </div>
            <p className="text-[14px] mr-[31px]">принимают участие</p>
            <RadioButton setState={setIsParticipatingInVoteOnNumber} text="привелигированные" />
          </div>
        </div>
        <div className="border-[0.5px] border-black rounded-[28px] pt-[26px] pl-[26px] pb-[25px]">
          <div className="flex items-center mb-[5px]">
            <p className="text-[14px] mr-[50px]">Уполномоченный представитель эмитента:</p>
            <div className="w-[424px]">
              <InputForm setState={setEmitentRepresentative} placeholder="Введите ФИО" />
            </div>
          </div>
          <div className="flex items-center">
            <RadioGroup firstText="Устав" secondText="Доверенность" setState={setIsRegulationOrAttorney} />
            <p className="text-[14px]/[18px] mr-[20px] ml-[59px]">№, от</p>
            <div className="w-[129px]"><InputFormNumber setState={setRegulationNumber} placeholder="" /></div>
          </div>
        </div>
      </div>
      <NavigationButtons back='/forms/first/step-three-shown' next='' onClick={onNextPageTransition} />
    </div>
  );
}