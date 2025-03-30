export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex flex-col items-center px-[168px] pb-[55px]">
            <div className="flex justify-between items-center w-[1000px] mt-[40px] ml-[54px] mb-[11px]">
                <h1 className="text-4xl">Документы</h1>
            </div>
            {children}
        </div>
    );
  }