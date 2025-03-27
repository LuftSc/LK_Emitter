'use client'

import Link from "next/link";
import { usePathname } from "next/navigation";
import clsx from 'clsx';

const links = [
    {name: 'Распоряжение Эмитента на список к ОСА', href: '/forms/first/step-one'},
    {name: 'Распоряжение Эмитента на предоставление информации из реестра', href: '/forms/second/step-one'},
    {name: 'Распоряжение Эмитента о предоставлении Списка лиц , имеющих право на получение доходов по ценным бумагам', href: '/forms/third/step-one'},
];

export default function FormsMain () {
    const pathname = usePathname();
    return (
        <div className="w-[1104px] h-[744px] border-[0.5px] border-black rounded-[28px] bg-[#F1F1F1] mt-[23px] pl-[71px] pt-[68px]">
            <div className="mb-[54px]">
                <h2 className="text-xl/[26px] font-bold mb-5">5.2 Распоряжения на предоставление информации Эмитенту для общего собрания акционеров</h2>
                <Link
                    key={links[0].name}
                    href={links[0].href}
                    className={clsx(pathname === links[0].href)}>
                    <p className="text-[#B82116] text-base/[21px] font-bold">{links[0].name}</p>
                </Link>
            </div>
            <div>
                <h2 className="text-xl/[26px] font-bold mb-5">5.3 Распоряжения на предоставление информации Эмитенту</h2>
                <div className="mb-[18px]">
                    <Link
                        key={links[1].name}
                        href={links[1].href}
                        className={clsx(pathname === links[1].href)}>
                        <p className="text-[#B82116] text-base/[21px] font-bold">{links[1].name}</p>
                    </Link>
                </div>
                <Link
                    key={links[2].name}
                    href={links[2].href}
                    className={clsx(pathname === links[2].href)}>
                    <p className="text-[#B82116] text-base/[21px] font-bold">{links[2].name}</p>
                </Link>
            </div>
        </div>
    );
}