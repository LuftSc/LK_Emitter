'use client'

import locale from 'antd/locale/ru_RU';
import moment from 'moment-timezone';
import dayjs  from 'dayjs';
import { ConfigProvider, TimePicker } from 'antd';
import 'dayjs/locale/ru';

interface Props {
    setTime: React.Dispatch<React.SetStateAction<string>>
}

export default function Timepicker({setTime}: Props) {
    dayjs.locale('ru');
    
    return (
        <div>
            <ConfigProvider locale={locale}>
                <TimePicker
                    format='HH-mm'
                    onChange={(time) => setTime(time.add(5, 'hour').toISOString().slice(11, 16))}
                />
            </ConfigProvider>
        </div>
    )
}