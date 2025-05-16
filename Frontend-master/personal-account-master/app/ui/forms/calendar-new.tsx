'use client'

import locale from 'antd/locale/ru_RU';
import dayjs  from 'dayjs';
import { ConfigProvider, DatePicker } from 'antd';
import 'dayjs/locale/ru';

interface Props {
    placeholder?: string,
    setDate: React.Dispatch<React.SetStateAction<string>>
}

export default function Calendar({setDate, placeholder}: Props) {
    dayjs.locale('ru');
    return (
        <div>
            <ConfigProvider locale={locale}>
                <DatePicker
                    placeholder={placeholder || 'Выберите дату'}
                    format={{
                    format: 'YYYY-MM-DD',
                    type: 'mask',
                    }}
                    onChange={(date) => setDate(date.add(1, 'day').toISOString().slice(0, 10))}
                />
            </ConfigProvider>
        </div>
    )
}