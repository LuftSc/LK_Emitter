import DocumentsTable from "../ui/documents-page/documents-table";

export default function Page() {
    return (
        <main>
            <div className="w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] py-[45px] px-[80px]" >
                <p className="text-[34px]/[44px] mb-[25px]">Загруженные документы</p>
                <DocumentsTable />
            </div>
        </main>
    );
}