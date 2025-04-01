import { RadioButton } from "../ui/forms/radiobtn";
import SettingsCheckbox from "../ui/settings-page/settings-checkbox";
import { SettingsInput } from "../ui/settings-page/settings-input";
import { SettingsInputPassword } from "../ui/settings-page/settings-input-password";
import SettingsSelect from "../ui/settings-page/settings-select";

export default function Page () {
    return (
        <main>
            <div className="py-[50px] flex flex-col items-center">
                <h1 className="mb-[30px] text-[34px] bold">Настройки</h1>
                <div className="flex w-[950px] justify-between mb-[30px]">
                    <div>
                        <p className="text-center bold text-[24px] mb-[20px]">Персональные данные</p>
                        <div className="w-[620px] h-[290px] flex justify-between">
                            <div className="w-[290px] flex flex-col justify-between">
                                <SettingsInput placeholder="Фамилия*" />
                                <SettingsInput placeholder="Отчество*" />
                                <SettingsInput placeholder="Дата рождения*" />
                                <SettingsSelect placeholder="Пол*" />
                            </div>
                            <div className="w-[290px] flex flex-col justify-between">
                                <SettingsInput placeholder="Имя*" />
                                <div className="h-[50px] flex items-center"><SettingsCheckbox /></div>
                                <SettingsInput placeholder="Мобильный телефон*" />
                                <SettingsInput placeholder="Адрес электронной почты*" />
                            </div>
                        </div>
                    </div>
                    <div>
                        <p className="text-center bold text-[24px] mb-[20px]">Паспортные данные</p>
                        <div className="w-[290px] h-[290px] flex flex-col justify-between">
                            <div className="flex justify-between">
                                <div className="w-[125px]"><SettingsInput placeholder="Серия*" /></div>
                                <div className="w-[125px]"><SettingsInput placeholder="Номер*" /></div>
                            </div>
                            <SettingsInput placeholder="Дата выдачи*" />
                            <SettingsInput placeholder="Кем выдан*" />
                            <SettingsInput placeholder="Место рождения*" />
                        </div>
                    </div>
                </div>
                <div className="flex w-[950px] justify-between">
                    <div>
                        <p className="text-center bold text-[24px] mb-[20px]">Пароль от аккаунта</p>
                        <div className="w-[290px] justify-between">
                            <SettingsInputPassword placeholder="Пароль*" />
                        </div>
                    </div>
                </div>
            </div>
        </main>
    )
}