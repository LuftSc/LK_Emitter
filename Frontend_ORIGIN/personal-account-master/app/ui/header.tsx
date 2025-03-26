import NavLinks from "./nav-links";

export default function Header() {
    return (
        <>
            <div className="flex h-[120px]">
                
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