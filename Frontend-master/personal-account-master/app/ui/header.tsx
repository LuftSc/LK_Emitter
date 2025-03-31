import NavLinks from "./nav-links";
import SettingsBtn from "./settings-btn";
import LogoSvg from "./svg-icons/logo";

export default function Header() {
    return (
        <>
            <div className="flex justify-between items-center h-[120px] px-[220px]">
                <div className="flex items-center">
                    <LogoSvg />
                    <p className="text-[16px] w-[100px] text-[#BF4333] ml-[20px]">Ведение реестров компаний</p>
                </div>
                <div>
                    <SettingsBtn />
                </div>
            </div>
            <div className="flex bg-[#B82116] h-[50px] items-center space-x-[75px] pl-[220px]">
                < NavLinks />
            </div>
            <div className="flex bg-[#F1F1F1] h-[50px] items-center pl-[220px]">
                <p className="text-[#333333] text-base/[20px]">Путь пользователя</p>
            </div>
        </>
    );
}