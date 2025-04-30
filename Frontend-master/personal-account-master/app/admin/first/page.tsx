import AdminNavButtonsFirst from "@/app/ui/admin-components/navBttnsForAdminFirst";
import TableForDocumentsInAdmin from "@/app/ui/admin-components/tableWithFiltersForFirstPage";

export default function Page() {
    return (
        <div className="py-[60px] px-[100px]">
            <AdminNavButtonsFirst />
            <TableForDocumentsInAdmin />
        </div>
    )
}