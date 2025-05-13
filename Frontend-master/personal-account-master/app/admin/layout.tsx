import AdminNavMenu from "../ui/admin-components/nav-menu";

export default function RootLayout({ children }: Readonly<{ children: React.ReactNode }>) {
    return (
        <div className="flex py-[50px] px-[160px]">
            <div><AdminNavMenu /></div>
            <div className="w-full pl-[30px]">{children}</div>
        </div>
    );
}
