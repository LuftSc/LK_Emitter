"use client"

import { NavigationButtons } from "@/app/ui/forms/nav-btn"
import { InputForm } from "@/app/ui/forms/input"
import { CheckBox } from "@/app/ui/forms/checkbox"
import ShowListsRadio from "@/app/ui/forms/showLists-radio";
import { useState } from "react";
import Calendar from "@/app/ui/forms/calendar-new";
import { RequestListOfShareholders } from "@/app/services/orderReportsService";
import { RadioGroup } from "@/app/ui/forms/radioGroup";
import { RadioGroupColumn } from "@/app/ui/forms/radioGroupColumn";
import { InputFormNumber } from "@/app/ui/forms/inputNumbers";



export default function Page() {

  const [showLists, setShowLists] = useState<boolean>(false)
  const [Dt_Begsobr, setdtBegSobr] = useState<string>('')

  const [listOfPeopleRightToParticipate, setlistOfPeopleRightToParticipate] = useState<boolean>(false)
  const [listOfPeopleRightOnPapers, setlistOfPeopleRightOnPapers] = useState<boolean>(false)
  const [listOfPeopleRightToParticipateTwo, setlistOfPeopleRightToParticipateTwo] = useState<boolean>(false)
  const [isMeetingWillBeHeldByBoD, setisMeetingWillBeHeldByBoD] = useState<boolean>(false)
  const [mettingWillBeHeldBy, setmettingWillBeHeldBy] = useState<string>('')
  const [meetingNumber, setMeetingNumber] = useState<number>(0)
  const [decisionDate, setDecisionDate] = useState<string>('')

  const [dtMod, setDtMod] = useState<string>('')
  const [isRangeMeeting, setisRangeMeeting] = useState<boolean>(false)
  const [isCategMeeting, setisCategMeeting] = useState<boolean>(false)
  const [dtBegSobr, setDtBegSobr] = useState<string>('')


  const onNextPageTransition = async () => {
    localStorage.setItem('showNDlists', showLists.toString())
    
    const emitter = localStorage.getItem('emitter')
    const emitterData = emitter ? JSON.parse(emitter) : null

    const requestData = {
      forReportGenerating: {
        dt_Begsobr: dtBegSobr,
        dtMod: dtMod,
        nomList: showLists,
        isRangeMeeting: isRangeMeeting,
        isCategMeeting: isCategMeeting,
        issuerId: emitterData.IssuerId
      },
      
      forDbSaving: {
        stepOne: {
          listOfPeopleRightToParticipate: listOfPeopleRightToParticipate,
          listOfPeopleRightOnPapers: listOfPeopleRightOnPapers,
          listOfPeopleRightToParticipateTwo: listOfPeopleRightToParticipateTwo,
          isMeetingWillBeHeldByBoD: isMeetingWillBeHeldByBoD,
          mettingWillBeHeldBy: mettingWillBeHeldBy,
          meetingNumber: meetingNumber,
          decisionDate: decisionDate
        }
      }
      
    } as RequestListOfShareholders

    localStorage.setItem('request_listOSA', JSON.stringify(requestData))
  }

  return (
    <div className="relative w-[1104px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
      <div className="">
        <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на список к ОСА</p>
        <div className="border-[0.5px] border-black rounded-[28px] pt-[23px] pl-[26px] pb-[25px] mb-[45px]">
          <p className="font-bold leading-[18px] text-sm mb-[9px]">Предоставить: список лиц, имеющих право голоса при принятии решений общим собранием акционеров</p>
          <div className="flex flex-col gap-[9px] mb-[24px]">
            <CheckBox setState={setlistOfPeopleRightToParticipate} text="Список лиц, имеющих право на участие в общем собрании акционеров" />
            <CheckBox setState={setlistOfPeopleRightOnPapers} text="Список лиц, осуществляющих права по ценным бумагам" />
            <div className="w-[594px]"><CheckBox setState={setlistOfPeopleRightToParticipateTwo} text="Список лиц, имеющих право на участие в общем собрании акционеров, без персональных данных и данных о волеизъявлении для ознакомления (п. 4. Ст. 51 ФЗ об АО)." /></div>
          </div>
          <div className="flex items-center">
            <p className="text-[14px]/[18px] mr-[45px]">Дата определения (фиксации) лиц</p>
            <Calendar setDate={setDtMod} />
          </div>
        </div>
        <div className="border-[0.5px] border-black rounded-[28px] pt-[14px] pl-[26px] pb-[25px]">
          <p className="text-[#B82116] text-[14px]/[18px] font-bold text-center mb-[20px]">Сведения об общем собрании акционеров:</p>
          <div className="flex mb-[20px]">
            <p className="w-[229px] text-[14px]/[18px] mr-[16px]">Решение о проведении общего собрания акционеров принято:</p>
            <div className="flex flex-col gap-[10px]">
              <div className="relative w-[457px]">
                <RadioGroupColumn firstText="Советом директоров " secondText="" setState={setisMeetingWillBeHeldByBoD} />
                <div className="absolute w-[457px] left-[25px] bottom-[-4px]"><InputForm setState={setmettingWillBeHeldBy} placeholder="Введите наименование" /></div>
              </div>
              <div className="flex items-center">
                <div className="flex items-center w-[149px] mr-[20px]">
                  <p className="text-[14px]/[18px] mr-[11px]">№</p>
                  <InputFormNumber setState={setMeetingNumber} placeholder="" />
                </div>
                <p className="mr-[27px]">дата принятия решения</p>
                <Calendar setDate={setDecisionDate} />
              </div>
            </div>
          </div>
          <div className="flex mb-[15px] items-center">
            <p className="text-[14px]/[18px] mr-[50px]">Вид и форма проведения собрания</p>
            <div className="mr-[40px]"><RadioGroup firstText="Годовое" secondText="Внеочередное" setState={setisCategMeeting} /></div>
            <RadioGroup firstText="Совместное присутствие" secondText="Заочное голосование" setState={setisRangeMeeting} />
          </div>
          <div className="flex items-center">
            <p className="text-[14px]/[18px] mr-[30px]">Дата проведения собрания</p>
            <Calendar setDate={setDtBegSobr} />
          </div>
        </div>
      </div>
      <ShowListsRadio setShowLists={setShowLists} />
      <NavigationButtons back='' next={showLists == true ? '/forms/first/step-two-shown' : '/forms/first/step-four'} onClick={onNextPageTransition} />
    </div>
  );
}
