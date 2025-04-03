'use client'

import locale from 'antd/locale/ru_RU';
import dayjs, { Dayjs } from 'dayjs';
import { ConfigProvider, DatePicker } from 'antd';
import { DatePickerProps } from 'antd';
import 'dayjs/locale/ru';

interface Props {
    setState: React.Dispatch<React.SetStateAction<string>>
}

export default function Calendar({setState}: Props) {

    const onChange: DatePickerProps['onChange'] = (date, dateString) => {
        console.log(date, dateString);
        setState(dateString)
    };

    dayjs.locale('ru');
    return (
        <div>
            <ConfigProvider locale={locale}>
                <DatePicker onChange={onChange} />
            </ConfigProvider>
        </div>
    )
}