'use client'

import React from "react"
import { useState } from "react";
import { InputLogin } from "./login-input";
import LoginBtn from "./login-btn";

export default function LogInForm () {
    const [visible, setVisible] = React.useState(true)
    if(!visible) return null;

    return (
        <div className="fixed top-0 left-0 flex justify-center items-center w-screen h-screen backdrop-blur-sm">
            <div className="flex flex-col items-center w-[664px] h-[447px] border-black rounded-[28px] border-[0.5px] bg-[#F1F1F1]/75 pt-[58px]">
                <p className="text-[36px]/[47px] mb-[54px]">Личный кабинет эмитента</p>
                <div className="mb-[22px]"><InputLogin placeholder="Логин*"/></div>
                <div className="mb-[48px]"><InputLogin placeholder="Пароль*"/></div>
                <div className="mb-[27px]" onClick={() => setVisible(false)}><LoginBtn /></div>
                <p className="text-[20px]/[26px]">Забыл(а) пароль</p>
            </div>
            <div className="flex flex-col items-center w-[664px] h-[447px] border-black rounded-[28px] border-[0.5px] bg-[#F1F1F1]/75 pt-[58px]">
                <p className="text-[36px]/[47px] mb-[10px]">Введите код подтвеждения</p>
                <p className="text-[24px]/[26px] mb-[20px]">На вашу почту был отправлен пароль </p>
                <div className="mb-[48px]"><InputLogin placeholder="Код"/></div>
                <div className="mb-[27px]" onClick={() => setVisible(false)}><LoginBtn /></div>
            </div>
        </div>
    );
}