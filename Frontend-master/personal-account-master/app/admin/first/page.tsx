import MainContentFirst from "@/app/ui/admin-components/mainContentFirst";
import AdminNavButtonsFirst from "@/app/ui/admin-components/navBttnsForAdminFirst";

export default function Page() {
    return (
        <div className="py-[60px] px-[160px]">
            <AdminNavButtonsFirst />
            <MainContentFirst />
        </div>
    )
}