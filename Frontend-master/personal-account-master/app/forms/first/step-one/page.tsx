"use client"

import { NavigationButtons } from "@/app/ui/forms/nav-btn"
import { InputForm } from "@/app/ui/forms/input"
import { CheckBox } from "@/app/ui/forms/checkbox"
import { RadioButton } from "@/app/ui/forms/radiobtn";
import CalendarInput from "@/app/ui/forms/calendar";
import ShowListsRadio from "@/app/ui/forms/showLists-radio";
import { useEffect, useState } from "react";
import Calendar from "@/app/ui/forms/calendar-new";
import dayjs, { Dayjs } from 'dayjs';
import { ListOSAReportGeneratingData, ListOSASavingToDbData, RequestListOfShareholders } from "@/app/services/orderReportsService";
import { Button, DatePicker } from "antd";



export default function Page () {

  const [showLists, setShowLists] = useState<boolean>(false)
  const [isRangeMeeting, setisRangeMeeting] = useState<boolean>(false)
  const [Dt_Begsobr, setDt_Begsobr] = useState<string>('')

  const [dtMod, setDtMod] = useState<string>('')
  const [dtBegSobr, setDtBegSobr] = useState<string>('')


  const onNextPageTransition = async () => {
    const emitter = localStorage.getItem('emitter')
    const emitterData = emitter ? JSON.parse(emitter) : null

    const requestData = {
      forReportGenerating: {
        dt_Begsobr: dtBegSobr,
        dtMod: dtMod,
        nomList: showLists,
        isRangeMeeting: isRangeMeeting,
        isCategMeeting: isRangeMeeting,
        issuerId: emitterData.IssuerId
      } as ListOSAReportGeneratingData,

      forDbSaving: {
        stepOne: {
          isMeetingWillBeHeldByBoD: false,
          mettingWillBeHeldBy: "",
          meetingNumber: 0,
          decisionDate: "0000-00-00"
        },
        stepTwo: {
          startRegistrationTime: "00:00",
          endRegistrationTime: "00:00"
        }
        
      } as ListOSASavingToDbData
      
    } as RequestListOfShareholders


    localStorage.setItem('request_listOSA', JSON.stringify(requestData))
  }

  const [pageInfo, setPageInfo] = useState<{
    isRangeMeeting: boolean, 
    Dt_Begsobr: string, 
    }>({isRangeMeeting: false, Dt_Begsobr: ""})

  useEffect(() => {
      onInfoUpdate()
  }, [])
  
  const onInfoUpdate = async () => {
    setPageInfo({isRangeMeeting, Dt_Begsobr})
    console.log(pageInfo)
  }

  return (
    <div className="relative w-[1104px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]" >
          <p className="text-[#B82116] text-[15px]/[19px] font-bold text-center mb-[31px]">Распоряжение Эмитента на список к ОСА</p>
          {/*<button onClick={onInfoUpdate} className=" border border-black p-[15px]">Обновить</button>
          <Calendar setState={setDt_Begsobr}/> */}
          <div className="border-[0.5px] border-black rounded-[28px] pt-[23px] pl-[26px] pb-[25px] mb-[45px]">
            <p className="font-bold leading-[18px] text-sm mb-[9px]">Предоставить: список лиц, имеющих право голоса при принятии решений общим собранием акционеров</p>
            <div className="flex flex-col gap-[9px] mb-[24px]">
              <CheckBox text="Список лиц, имеющих право на участие в общем собрании акционеров" />
              <CheckBox text="Список лиц, осуществляющих права по ценным бумагам" />
              <div className="w-[594px]"><CheckBox text="Список лиц, имеющих право на участие в общем собрании акционеров, без персональных данных и данных о волеизъявлении для ознакомления (п. 4. Ст. 51 ФЗ об АО)." /></div>
            </div>
            <div className="flex items-center">
              <p className="text-[14px]/[18px] mr-[45px]">Дата определения (фиксации) лиц</p>
              <DatePicker
                format={{
                  format: 'YYYY-MM-DD',
                  type: 'mask',
                }}
                onChange={(date) => setDtMod(date.add(1, 'day').toISOString().slice(0, 10))}
              />
              {/*<CalendarInput calendarId="dateFix"/> */}

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
              <div className="mr-[45px]"><RadioButton  name="typeOfMeeting" text="Годовое"/></div>
              <div className="mr-[65px]"><RadioButton  name="typeOfMeeting" text="Внеочередное"/></div>
              <div className="mr-[45px]"><RadioButton  name="formOfMeeting" text="Совместное присутствие"/></div>
              <div><RadioButton name="formOfMeeting" text="Заочное голосование"/></div>
            </div>
            <div className="flex items-center">
              <p className="text-[14px]/[18px] mr-[30px]">Дата проведения собрания</p>
              {/*<CalendarInput calendarId="dateMeet"/> */}
              <DatePicker
                format={{
                  format: 'YYYY-MM-DD',
                  type: 'mask',
                }}
                onChange={(date) => {setDtBegSobr(date.add(1, 'day').toISOString().slice(0, 10))}}
              />
            </div>
          </div>
          <ShowListsRadio setShowLists={setShowLists}/>
          
          <NavigationButtons back='' next={showLists == true ? '/forms/first/step-two-shown' : '/forms/first/step-four' } onClick={onNextPageTransition}/>
          {/*<Button onClick={onListOSSend}>Отправить</Button> */}
      </div>
  );
}
