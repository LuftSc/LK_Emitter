export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex flex-col items-center px-[168px] pb-[55px]">
            <div className="flex justify-between items-center w-[1000px] mt-[40px] ml-[54px] mb-[11px]">
                <h1 className="text-4xl">Документы</h1>
                <p className="w-[213px] h-[44px] text-[20px]/[31px] text-white text-center bg-[#2B90E8] pt-[5px]">Загрузить документ</p>
            </div>
            {children}
        </div>
    );
  }