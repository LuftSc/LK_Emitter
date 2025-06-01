export default function Layout({ children }: { children: React.ReactNode }) {
    return (
        <div className="flex flex-col items-center px-[168px] py-[55px]">
            {children}
        </div>
    );
  }