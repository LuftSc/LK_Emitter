'use client'

import { useEffect, useState } from "react";
import ConfirmationForm from "./ui/logIn/confirmation-popup";
import LogInForm from "./ui/logIn/login-popup";
import EmitentTable from "./ui/main-page/emitent-table";
import { Emitter } from "./models/Emitter";
import { getCurrentUser, getUserEmitters } from "./services/usersService";
import { errorMessages } from "./services/errorMessages";
import { tree } from "next/dist/build/templates/app-page";
import { useSimpleStorage } from "./hooks/useLocalStorage";

const { getItem } = useSimpleStorage('emitter');

export default function Home() {

  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [visLog, setVisLog] = useState<boolean>(false);
  const [visCon, setVisCon] = useState<boolean>(false);
  const [emitentName, setEmitentName] = useState<string>("");

  const [emitters, setEmitters] = useState<Emitter[]>([]);
  const [visEmitTable, setVisEmitTable] = useState<boolean>(false)

  const [userId, setUserId] = useState<string>("");

  const [emitterInfo, setEmitterInfo] = useState<{
    Id: string, 
    Name: string, 
    AuthPerson: string}>
    ({Id: "", Name: "", AuthPerson: ""})

  useEffect(() => {
    const emitter = getItem()
    console.log(emitter)

    const getUser = async () => {
      const userIdResponse = await getCurrentUser();
      console.log(userIdResponse)

      if (userIdResponse?.ok) {
        console.log(userIdResponse)

        setVisLog(false)
        const userGuid = await userIdResponse.json();
        setUserId(userGuid)
        await onAuthSucces()
      } else if (userIdResponse?.status === 400) {
        const error = await userIdResponse.json()
        console.log(errorMessages[error[0].type])
        setVisLog(true)
      } else {
        console.log('Неизвестная ошибка')

      }
    }; 
    getUser();
  }, [])

  const onAuthSucces = async () => {
    const emittersResponse = await getUserEmitters()
      console.log(emittersResponse)

      if (emittersResponse?.ok) { // Случай, когда запрос выполнился успешно
          const emitters = await emittersResponse.json()
          setEmitters(emitters)
          setVisEmitTable(true)
      } else if (emittersResponse?.status === 400){ // Какая-то ошибка в процессе выполнения логики
          // Сам текст ошибки: errorMessages[error[0].type]
          const error = await emittersResponse.json()
          console.log(errorMessages[error[0].type])
      } else {
          // На случай неизвестных ошибок, например, если
          // бэкэнд недоступен(не запущен) и мы пытаемся отправлять туда запросы
          console.error('Неизвестная ошибка')
      }
  }

  return (
    <main>
      <div className="px-[168px] pb-[55px]">
        <h1 className="text-4xl mt-[40px] ml-[54px]">
          Личный кабинет Эмитента
        </h1>
        <div className="flex flex-col items-center max-w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[27px]">
          <ConfirmationForm email={email} visCon={visCon} setVisCon={setVisCon} onLoginSuccess={onAuthSucces}/>
          {/*<LogInForm email={email} setEmail={setEmail} password={password} setPassword={setPassword} visLog={visLog} setVisLog={setVisLog} setVisCon={setVisCon}/> */}
          <LogInForm email={email} setEmail={setEmail} password={password} setPassword={setPassword} visLog={visLog} setVisLog={setVisLog} setVisCon={setVisCon} onLoginSuccess={onAuthSucces}/>
          <p className="max-w-[900px] text-[34px]/[44px] mb-[24px]">{emitentName}</p>
          {/*<EmitentTable emitentName={emitentName} setEmitentName={setEmitentName}/> */}
          <EmitentTable userId={userId} emitters={emitters} setEmitterName={setEmitentName} isTableVisible={visEmitTable}/>
        </div>
     </div>
    </main>
  );
}
