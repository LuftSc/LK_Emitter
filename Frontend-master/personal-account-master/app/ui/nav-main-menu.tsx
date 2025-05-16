'use client'

import React, { useState } from 'react';
import type { MenuProps } from 'antd';
import { Menu } from 'antd';

type MenuItem = Required<MenuProps>['items'][number];

const items: MenuItem[] = [
  {
    label: 'Главная',
    key: 'mail'
  },
  {
    label: 'Запросы/Поручения',
    key: 'app'
  },
  {
    label: 'Документы',
    key: 'app123'
  },
  {
    label: 'Проведенные операции',
    key: 'SubMenu',
    children: [
      {
        type: 'item',
        key: '3',
        label: 'Item 1',
      },
      {
        type: 'item',
        key: '2',
        label: 'Item 1',
      },
      {
        type: 'item',
        key: '1',
        label: 'Item 1',
      },
    ],
  },
  {
    label: 'Задать вопрос',
    key: 'app123124'
  },
  {
    label: 'Помощь',
    key: 'ap51252p'
  }
];


export default function NavLinks () {
    const [current, setCurrent] = useState('mail');

  const onClick: MenuProps['onClick'] = (e) => {
    console.log('click ', e);
    setCurrent(e.key);
  };

  return ( 
        <Menu 
            className='text-white text-base/[20px]'
            onClick={onClick} 
            selectedKeys={[current]} 
            mode="horizontal" 
            items={items} 
        /> 
    )
}