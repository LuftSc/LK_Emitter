// Словарь для маппинга типа ошибки, пришедшего с бэкэнда для
// отображения на пользовательских элементах

export const errorMessages : Record<string,string> = {
    WrongUserPasswordError: 'Неверный пароль пользователя',
    UserNotFoundError: 'Такого пользователя нет в системе. Вам нужно зарегистрироваться',
    UserNonAuthentificatedError: 'Пользователь не аутентифицирован'
}