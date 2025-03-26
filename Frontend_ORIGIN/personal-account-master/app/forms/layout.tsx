export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex flex-col items-center px-[168px] pb-[55px]">
            <h1 className="min-w-[1000px] text-4xl mt-[40px] ml-[54px]">Личный кабинет Эмитента</h1>
            {children}
        </div>
    );
  }