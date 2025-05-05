'use client'

import Link from "next/link";
import { usePathname } from "next/navigation";
import clsx from 'clsx';

const links = [
    { name: 'Распоряжение Эмитента на список к ОСА', href: '/forms/first/step-one' },
    { name: 'Распоряжение Эмитента на предоставление информации из реестра', href: '/forms/second/step-one' },
    { name: 'Распоряжение Эмитента о предоставлении Списка лиц , имеющих право на получение доходов по ценным бумагам', href: '/forms/third/step-one' },
    { name: 'Справка о состоянии лицевого счета зарегистрированного лица на определенную дату', href: '/forms/fourth' },
];

export default function FormsMainFirst() {

    const pathname = usePathname();

    return (
        <div className="border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[25px] px-[60px] py-[60px]">
            <div className="mb-[54px]">
                <h2 className="text-xl/[26px] font-bold mb-5">5.2 Распоряжения на предоставление информации Эмитенту для общего собрания акционеров</h2>
                <Link
                    key={links[0].name}
                    href={links[0].href}
                    className={clsx(pathname === links[0].href)}>
                    <p className="text-[#B82116] text-base/[21px] font-bold">{links[0].name}</p>
                </Link>
            </div>
            <div className="mb-[54px]">
                <h2 className="text-xl/[26px] font-bold mb-5">5.3 Распоряжения на предоставление информации Эмитенту</h2>
                <Link
                    key={links[1].name}
                    href={links[1].href}
                    className={clsx(pathname === links[1].href)}>
                    <p className="text-[#B82116] text-base/[21px] font-bold mb-[18px]">{links[1].name}</p>
                </Link>
                <Link
                    key={links[2].name}
                    href={links[2].href}
                    className={clsx(pathname === links[2].href)}>
                    <p className="text-[#B82116] text-base/[21px] font-bold">{links[2].name}</p>
                </Link>
            </div>
            <div>
                <h2 className="text-xl/[26px] font-bold mb-5">Справки о состоянии лицевого счета</h2>
                <Link
                    key={links[3].name}
                    href={links[3].href}
                    className={clsx(pathname === links[3].href)}>
                    <p className="text-[#B82116] text-base/[21px] font-bold">{links[3].name}</p>
                </Link>
            </div>
        </div>
    );
}