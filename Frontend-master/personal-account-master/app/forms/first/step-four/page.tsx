'use client'

import { InputForm } from "@/app/ui/forms/input";
import { RadioButton } from "@/app/ui/forms/radiobtn";
import { NavigationButtons } from "@/app/ui/forms/nav-btn";
import { RequestListOfShareholders, sendRequestListOfShareholders } from "@/app/services/orderReportsService";
import { useState } from "react";
import { RadioGroup } from "@/app/ui/forms/radioGroup";
import { InputFormNumber } from "@/app/ui/forms/inputNumbers";

export default function Page() {

  const back = localStorage.getItem('showNDlists')
  const [isParticipatingInVote, setIsParticipatingInVote] = useState<boolean>(false)
  const [agendaNumber, setAgendaNumber] = useState<number>(0)
  const [isParticipatingInVoteOnNumber, setIsParticipatingInVoteOnNumber] = useState<boolean>(false)
  const [emitentRepresentative, setEmitentRepresentative] = useState<string>('')
  const [isRegulationOrAttorney, setIsRegulationOrAttorney] = useState<boolean>(false)
  const [regulationNumber, setRegulationNumber] = useState<number>(0)

  const onNextPageTransition = async () => {
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

      await onRequestListOSA();
    }
  }

  const onRequestListOSA = async () => {
    const request = localStorage.getItem('request_listOSA')
    const requestInfo = request ? JSON.parse(request) as RequestListOfShareholders : null

    if (requestInfo) {
      if (requestInfo.forReportGenerating.nomList == true) {
        const listOSARequest = {
          requestData: {
            issuerId: requestInfo.forReportGenerating.issuerId, // код эмитента
            dtMod: requestInfo.forReportGenerating.dtMod, // Дата фиксации с 1 формы | Строка ФОРМАТА: ГГГГ-ММ-ДД
            nomList: requestInfo.forReportGenerating.nomList, // Флажок на форме
            isCategMeeting: requestInfo.forReportGenerating.isCategMeeting, // флажок с формы 1
            isRangeMeeting: requestInfo.forReportGenerating.isRangeMeeting, // флажок с формы 1 // true - заседание\ false - заочное
            dt_Begsobr: requestInfo.forReportGenerating.dt_Begsobr, // Дата проведения собрания с формы | Строка ФОРМАТА: ГГГГ-ММ-ДД
            listOfPeopleRightToParticipate: requestInfo.forDbSaving.stepOne.listOfPeopleRightToParticipate, // Первый чекбокс с "Список лиц, имеющих право на участие в общем собрании акционеров"
            listOfPeopleRightOnPapers: requestInfo.forDbSaving.stepOne.listOfPeopleRightOnPapers, // Второй чекбокс с "Список лиц, осуществляющих права по ценным бумагам"
            listOfPeopleRightToParticipateTwo: requestInfo.forDbSaving.stepOne.listOfPeopleRightToParticipateTwo, // Третий чекбокс с "Список лиц, имеющих право на участие в общем собрании акционеров,  без персональных данных"
            isMeetingWillBeHeldByBoD: requestInfo.forDbSaving.stepOne.isMeetingWillBeHeldByBoD, // Флажок с 1 формы "Советом директоров"
            mettingWillBeHeldBy: requestInfo.forDbSaving.stepOne.mettingWillBeHeldBy, // Строка под флажком, если "Советом директоров" true, 
            meetingNumber: requestInfo.forDbSaving.stepOne.meetingNumber, // номер под инпутом с плейсхолдером "Введите наименование"
            decisionDate: requestInfo.forDbSaving.stepOne.decisionDate,
            startRegistrationTime: requestInfo.forDbSaving.stepTwo.startRegistrationTime, // Время начало регистрации
            startMeetingTime: requestInfo.forDbSaving.stepTwo.startMeetingTime, // Время начало собрания
            endRegistrationTime: requestInfo.forDbSaving.stepTwo.endRegistrationTime, // Время окончания приема бюллетеней
            endRegistrationDate: requestInfo.forDbSaving.stepTwo.endRegistrationDate, // Дата окончания приема бюллетеней
            meetingPlace: requestInfo.forDbSaving.stepTwo.meetingPlace, // Место проведения собрания
            isVotingPossible: requestInfo.forDbSaving.stepTwo.isVotingPossible, // флажок "Методы голосования"
            addressFilledBallots: requestInfo.forDbSaving.stepTwo.addressFilledBallots, // Адрес заполненных бюллетеней
            fcs: requestInfo.forDbSaving.stepThree.fcs, // ФИО
            emailAddress: requestInfo.forDbSaving.stepThree.emailAddress, // email
            phoneNumber: requestInfo.forDbSaving.stepThree.phoneNumber, // номер телефона
            infoReviewingProcedure: requestInfo.forDbSaving.stepThree.infoReviewingProcedure, // Порядок ознакомления с информацией
            isParticipatingInVote: requestInfo.forDbSaving.stepFour.isParticipatingInVote, // 1 флажок "В голосовании принимают участие.."
            agendaNumber: requestInfo.forDbSaving.stepFour.agendaNumber, // Номер повестки дня
            isParticipatingInVoteOnNumber: requestInfo.forDbSaving.stepFour.isParticipatingInVoteOnNumber, // 2 флажок 
            emitentRepresentative: requestInfo.forDbSaving.stepFour.emitentRepresentative, // Уполномоченный представитель
            isRegulationOrAttorney: requestInfo.forDbSaving.stepFour.isRegulationOrAttorney, // 3 флажок Устав/Доверенность
            regulationNumber: requestInfo.forDbSaving.stepFour.regulationNumber // номер Устава или Доверенности
          }
        }
        console.log(listOSARequest)
        await sendRequestListOfShareholders(listOSARequest);
      }
      else if (requestInfo.forReportGenerating.nomList == false) {
        const listOSARequest = {
          requestData: {
            issuerId: requestInfo.forReportGenerating.issuerId, // код эмитента
            dtMod: requestInfo.forReportGenerating.dtMod, // Дата фиксации с 1 формы | Строка ФОРМАТА: ГГГГ-ММ-ДД
            nomList: requestInfo.forReportGenerating.nomList, // Флажок на форме
            isCategMeeting: requestInfo.forReportGenerating.isCategMeeting, // флажок с формы 1
            isRangeMeeting: requestInfo.forReportGenerating.isRangeMeeting, // флажок с формы 1 // true - заседание\ false - заочное
            dt_Begsobr: requestInfo.forReportGenerating.dt_Begsobr, // Дата проведения собрания с формы | Строка ФОРМАТА: ГГГГ-ММ-ДД
            listOfPeopleRightToParticipate: requestInfo.forDbSaving.stepOne.listOfPeopleRightToParticipate, // Первый чекбокс с "Список лиц, имеющих право на участие в общем собрании акционеров"
            listOfPeopleRightOnPapers: requestInfo.forDbSaving.stepOne.listOfPeopleRightOnPapers, // Второй чекбокс с "Список лиц, осуществляющих права по ценным бумагам"
            listOfPeopleRightToParticipateTwo: requestInfo.forDbSaving.stepOne.listOfPeopleRightToParticipateTwo, // Третий чекбокс с "Список лиц, имеющих право на участие в общем собрании акционеров,  без персональных данных"
            isMeetingWillBeHeldByBoD: requestInfo.forDbSaving.stepOne.isMeetingWillBeHeldByBoD, // Флажок с 1 формы "Советом директоров"
            mettingWillBeHeldBy: requestInfo.forDbSaving.stepOne.mettingWillBeHeldBy, // Строка под флажком, если "Советом директоров" true, 
            meetingNumber: requestInfo.forDbSaving.stepOne.meetingNumber, // номер под инпутом с плейсхолдером "Введите наименование"
            decisionDate: requestInfo.forDbSaving.stepOne.decisionDate,
            startRegistrationTime: '', // Время начало регистрации
            startMeetingTime: '', // Время начало собрания
            endRegistrationTime: '', // Время окончания приема бюллетеней
            endRegistrationDate: '', // Дата окончания приема бюллетеней
            meetingPlace: '', // Место проведения собрания
            isVotingPossible: false, // флажок "Методы голосования"
            addressFilledBallots: '', // Адрес заполненных бюллетеней
            fcs: '', // ФИО
            emailAddress: '', // email
            phoneNumber: '', // номер телефона
            infoReviewingProcedure: '', // Порядок ознакомления с информацией
            isParticipatingInVote: requestInfo.forDbSaving.stepFour.isParticipatingInVote, // 1 флажок "В голосовании принимают участие.."
            agendaNumber: requestInfo.forDbSaving.stepFour.agendaNumber, // Номер повестки дня
            isParticipatingInVoteOnNumber: requestInfo.forDbSaving.stepFour.isParticipatingInVoteOnNumber, // 2 флажок 
            emitentRepresentative: requestInfo.forDbSaving.stepFour.emitentRepresentative, // Уполномоченный представитель
            isRegulationOrAttorney: requestInfo.forDbSaving.stepFour.isRegulationOrAttorney, // 3 флажок Устав/Доверенность
            regulationNumber: requestInfo.forDbSaving.stepFour.regulationNumber // номер Устава или Доверенности
          }
        }
        console.log(listOSARequest)
        await sendRequestListOfShareholders(listOSARequest);
      }
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
      <NavigationButtons back={back == 'true' ? '/forms/first/step-three-shown' : '/forms/first/step-one'} next='/forms/mainSecond' onClick={onNextPageTransition} />
    </div>
  );
}