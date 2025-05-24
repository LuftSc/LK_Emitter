'use client'

import React from 'react';
import type { MenuProps } from 'antd';
import { Menu } from 'antd';
import { redirect } from 'next/navigation';

type MenuItem = Required<MenuProps>['items'][number];

const items: MenuItem[] = [
  {
    key: '/admin',
    label: 'Проведенные операции',

  },
  {
    key: 'group1',
    label: 'Действия над пользователями',
    children: [
      {
        key: '/admin/first',
        label: 'Добавление пользователя'
      },
      {
        key: '/admin/third',
        label: 'Редактирование пользователей'
      },
    ]
  },
  {
    key: 'group2',
    label: 'Действия над эмитентами',
    children: [
            {
        key: '/admin/second',
        label: 'Добавление эмитента'
      },
      {
        key: '/admin/fourth',
        label: 'Редактирование эмитентов'
      },
    ]
  },
];

export default function AdminNavMenu () {

  const onSelect: MenuProps['onSelect'] = (e) => {
    redirect(e.key)
  }

  return (
    <Menu
        className='w-[280px]'
        onSelect={onSelect}
        defaultSelectedKeys={['/admin']}
        defaultOpenKeys={['/admin']}
        mode="inline"
        items={items}
    />
  )
}
