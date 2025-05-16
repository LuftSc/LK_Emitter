
import MainContentNewEmitent from "@/app/ui/admin-components/mainContentNewEmitent";
import MainContentSecond from "@/app/ui/admin-components/mainContentSecond";
import AdminNavButtonsSecond from "@/app/ui/admin-components/navBttnsForAdminSecond";
import { RegisterUser } from "@/app/ui/admin-components/RegisterUser";



export default function Page() {

    return (
        <div className="py-[60px] px-[160px]">
            <MainContentNewEmitent />
            <div className="text-[28px]">Регистрация нового пользователя</div>
            <RegisterUser />
        </div>
    )
}
