'use client'

import { useState } from "react";
import ConfirmationForm from "./ui/logIn/confirmation-popup";
import LogInForm from "./ui/logIn/login-popup";

export default function Home() {

  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [visLog, setVisLog] = useState<boolean>(true);
  const [visCon, setVisCon] = useState<boolean>(false);

  return (
    <main>
      <div className="px-[168px] pb-[55px]">
        <h1 className="text-4xl mt-[40px] ml-[54px]">
          Личный кабинет Эмитента
        </h1>
        <div className="max-w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] p-[45px]">
          <ConfirmationForm email={email} visCon={visCon} setVisCon={setVisCon}/>
          <LogInForm email={email} setEmail={setEmail} password={password} setPassword={setPassword} visLog={visLog} setVisLog={setVisLog} setVisCon={setVisCon}/>
        </div>
     </div>
    </main>
  );
}
