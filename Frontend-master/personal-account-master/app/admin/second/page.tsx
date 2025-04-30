
import MainContent from "@/app/ui/admin-components/mainContent";
import AdminNavButtonsSecond from "@/app/ui/admin-components/navBttnsForAdminSecond";



export default function Page() {
    return (
        <div className="py-[60px] px-[100px]">
            <AdminNavButtonsSecond />
            <MainContent />
        </div>
    )
}