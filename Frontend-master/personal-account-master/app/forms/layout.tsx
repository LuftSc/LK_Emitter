export default function RootLayout({ children }: Readonly<{ children: React.ReactNode }>) {
    return (
        <div className="w-full flex flex-col items-center py-[60px]">
            {children}
        </div>
    );
}