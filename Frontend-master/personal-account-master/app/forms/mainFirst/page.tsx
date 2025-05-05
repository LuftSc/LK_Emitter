import FormsMainFirst from "@/app/ui/forms/forms-main-first";
import FormsMainNavButtonsFirst from "@/app/ui/forms/navBttnsForFirstMain";

export default function Page() {
    return (
        <div className="px-[160px] py-[60px]">
            <div className="px-[60px]"><FormsMainNavButtonsFirst /></div>
            <FormsMainFirst />
        </div>
    );
}