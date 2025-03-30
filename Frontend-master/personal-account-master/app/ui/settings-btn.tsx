'use client'

import { redirect } from "next/navigation";
import GearSvg from "./svg-icons/gear";

export default function SettingsBtn() {
    return (
        <button onClick={() => redirect('/settingsPage')} className="flex items-center p-[10px] border-[2px] rounded-[8px] border-[#D64650] active:bg-[#D64650] text-[20px]/[26px] active:text-white text-[#D64650]">
            Настройки аккаунта
            <div className="ml-[8px]"><GearSvg /></div>
        </button>
    );
}