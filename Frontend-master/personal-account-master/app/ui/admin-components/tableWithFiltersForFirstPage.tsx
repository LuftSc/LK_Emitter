'use client'
import React, { useRef, useState } from 'react'
import { SearchOutlined } from '@ant-design/icons'
import type { InputRef, TableColumnsType, TableColumnType } from 'antd/es'
import { Button, Input, Space, Table } from 'antd/es'
import type { FilterDropdownProps } from 'antd/es/table/interface'
import Highlighter from 'react-highlight-words'

interface DataType {
    key: string;
    emitentName: string;
    representativeName: string;
    date: string;
    operationType: string;
}

type DataIndex = keyof DataType;

const data: DataType[] = [
    {
        key: '1',
        emitentName: 'ООО ТАКОЕ-ТО',
        representativeName: 'Иван Иванов Иванович',
        date: '27-05-2024',
        operationType: 'Загрузка документа',
    },
    {
        key: '2',
        emitentName: 'ООО ДРУГОЕ',
        representativeName: 'Илья Ильев Ильевич',
        date: '07-02-2026',
        operationType: 'Скачивание документа',
    },
    {
        key: '3',
        emitentName: 'ООО СОВСЕМ ДРУГОЕ',
        representativeName: 'Андрей Андреев Андреевич',
        date: '21-42-2321',
        operationType: 'Отправка документа',
    },
    {
        key: '4',
        emitentName: 'ООО ТАКОЕ-ТО',
        representativeName: 'Николай Николаев Николаевич',
        date: '20-02-2224',
        operationType: 'Загрузка документа',
    },
];

export default function TableForDocumentsInAdmin() {

    const [searchText, setSearchText] = useState('');
    const [searchedColumn, setSearchedColumn] = useState('');
    const searchInput = useRef<InputRef>(null);

    const handleSearch = (
        selectedKeys: string[],
        confirm: FilterDropdownProps['confirm'],
        dataIndex: DataIndex,
    ) => {
        confirm();
        setSearchText(selectedKeys[0]);
        setSearchedColumn(dataIndex);
    };

    const handleReset = (clearFilters: () => void) => {
        clearFilters();
        setSearchText('');
    };

    const getColumnSearchProps = (dataIndex: DataIndex): TableColumnType<DataType> => ({
        filterDropdown: ({ setSelectedKeys, selectedKeys, confirm, clearFilters, close }) => (
            <div style= {{ padding: 8 }} onKeyDown = {(e) => e.stopPropagation()}>
                <Input
                    ref={ searchInput }
                    placeholder = {`Найти`}
                    value = { selectedKeys[0]}
                    onChange = {(e) => setSelectedKeys(e.target.value ? [e.target.value] : [])}
                    onPressEnter = {() => handleSearch(selectedKeys as string[], confirm, dataIndex)}
                    style = {{ marginBottom: 8, display: 'block' }}
                />
                <Space>
                <Button
                  type="primary"
                    onClick = {() => 
                        {
                            handleSearch(selectedKeys as string[], confirm, dataIndex)
                            console.log(selectedKeys as string[])
                        }
                    }
                    icon = {< SearchOutlined />}
                    size = "small"
                    style = {{ width: 90 }}
                >
                    Поиск
                </Button>
                <Button
                    onClick = {() => clearFilters && handleReset(clearFilters)}
                    size = "small"
                    style = {{ width: 90 }}
                >
                    Сбросить
                </Button>
                < Button
                    type = "link"
                    size = "small"
                    onClick = {() => {
                        confirm({ closeDropdown: false });
                        setSearchText((selectedKeys as string[])[0]);
                        setSearchedColumn(dataIndex);
                    }}
                >
                    Отфильтровать
                </Button>
                < Button
                    type = "link"
                    size = "small"
                    onClick = {() => {
                        close();
                    }}
                >
                    Закрыть
                </Button>
                </Space>
            </div>
        ),
        filterIcon: (filtered: boolean) => (
            <SearchOutlined style= {{ color: filtered ? '#1677ff' : undefined }} />
        ),
        onFilter: (value, record) =>
            record[dataIndex]
                .toString()
                .toLowerCase()
                .includes((value as string).toLowerCase()),
            filterDropdownProps: {
                onOpenChange(open) {
                    if (open) {
                        setTimeout(() => searchInput.current?.select(), 100)
                    }
                }
        },
        render: (text) =>
            searchedColumn === dataIndex ? (
                <Highlighter
                  highlightStyle={{ backgroundColor: '#ffc069', padding: 0 }}
                  searchWords={[searchText]}
                  autoEscape
                  textToHighlight={text ? text.toString() : ''}
                />
              ) : (text)
        })

const columns: TableColumnsType<DataType> = [
    {
        title: 'Название Эмитента',
        dataIndex: 'emitentName',
        key: 'emitentName',
        width: '30%',
        ...getColumnSearchProps('emitentName'),
    },
    {
        title: 'Имя Представителя',
        dataIndex: 'representativeName',
        key: 'representativeName',
        width: '30%',
        ...getColumnSearchProps('representativeName'),
    },
    {
        title: 'Дата операции',
        dataIndex: 'date',
        key: 'date',
        width: '10%',
        ...getColumnSearchProps('date'),
    },
    {
        title: 'Тип операции',
        dataIndex: 'operationType',
        key: 'operationType',
        ...getColumnSearchProps('operationType'),
    },
    Table.SELECTION_COLUMN,
]

const [select, setSelect] = useState({
    selectedRowKeys: []
});

const { selectedRowKeys } = select

const rowSelection = {
    selectedRowKeys,
    onChange: (newSelects: any) => {
      setSelect({
        ...select,
        selectedRowKeys: newSelects
      });
    }
}

return (
    <div>
        <Table columns={ columns } rowSelection={rowSelection} dataSource = { data } />
        <Button onClick={(e) => selectedRowKeys.map((value) => console.log(value))}>Показать</Button>
    </div>
)
      
}
