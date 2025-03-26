'use client'

import Link from "next/link";
import { usePathname } from "next/navigation";
import clsx from 'clsx';

const links = [
  {name: 'Главная', href: '/'},
  {name: 'Запросы/Поручения', href: '/forms'},
  {name: 'Документы', href: '/userDocuments'},
  {name: 'Проведенные операции', href: '/'},
  {name: 'Задать вопрос', href: '/'},
  {name: 'Помощь', href: '/forms/hidden/firstFirst'},
];

export default function NavLinks () {
    const pathname = usePathname();
    return (
        <>
        {links.map((link) => {
            return (
                <Link
                    key={link.name}
                    href={link.href}
                    className={clsx(pathname === link.href)}>
                    <p className="text-white text-base/[20px]">{link.name}</p>
                </Link>
            );
        })}
        </>
    );
}