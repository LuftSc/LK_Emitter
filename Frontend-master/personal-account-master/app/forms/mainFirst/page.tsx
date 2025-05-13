import FormsMainFirst from "@/app/ui/forms/forms-main-first";
import FormsMainNavButtonsFirst from "@/app/ui/forms/navBttnsForFirstMain";

export default function Page() {
    return (
        <div className="w-full px-[160px]">
            <div className="px-[60px]"><FormsMainNavButtonsFirst /></div>
            <FormsMainFirst />
        </div>
    );
}