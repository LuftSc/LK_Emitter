'use client'

import React from 'react';
import { AppstoreOutlined, MailOutlined, SettingOutlined } from '@ant-design/icons';
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
    key: 'group',
    label: 'Действия над пользователями',
    children: [
      {
        key: '/admin/first',
        label: 'Добавление пользователя'
      },
      {
        key: '/admin/second',
        label: 'Добавление эмитента'
      },
      {
        key: '/admin/third',
        label: 'Редактирование пользователей'
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
        defaultSelectedKeys={['logs']}
        defaultOpenKeys={['logs']}
        mode="inline"
        items={items}
    />
  )
}
