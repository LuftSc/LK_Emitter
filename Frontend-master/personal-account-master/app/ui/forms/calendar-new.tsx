'use client'

import locale from 'antd/locale/ru_RU';
import dayjs  from 'dayjs';
import { ConfigProvider, DatePicker } from 'antd';
import 'dayjs/locale/ru';

interface Props {
    setDate: React.Dispatch<React.SetStateAction<string>>
}

export default function Calendar({setDate}: Props) {
    dayjs.locale('ru');
    return (
        <div>
            <ConfigProvider locale={locale}>
                <DatePicker
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