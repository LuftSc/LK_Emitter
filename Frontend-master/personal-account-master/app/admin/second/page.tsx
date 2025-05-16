"use client"

import MainContentSecond from "@/app/ui/admin-components/mainContentSecond";
import AdminNavButtonsSecond from "@/app/ui/admin-components/navBttnsForAdminSecond";
import { RegisterUser } from "@/app/ui/admin-components/RegisterUser";


export default function Page() {

    return (
        <div className="py-[60px] px-[160px]">
            <AdminNavButtonsSecond />
            <MainContentSecond />
            <div className="text-[28px]">Регистрация нового пользователя</div>
            <RegisterUser />
        </div>
    )
}
