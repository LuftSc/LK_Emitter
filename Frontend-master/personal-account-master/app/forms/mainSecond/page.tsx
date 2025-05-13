import FormsMainNavButtonsSecond from "@/app/ui/forms/navBttnsForSecondMain";
import FormsMainSecond from "../../ui/forms/forms-main-second";


export default function Page() {
    return (
        <div className="w-full px-[160px]">
            <div className="px-[60px]"><FormsMainNavButtonsSecond /></div>
            <FormsMainSecond />
        </div>
    );
}