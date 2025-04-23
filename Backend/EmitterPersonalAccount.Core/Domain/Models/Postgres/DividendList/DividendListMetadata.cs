using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.DividendList
{
    public record DividendListMetadata(
        string FullEmName, // Полное наименование эмитента
        string DecidingAuthority, // Орган, управления эмитента, принявший решение...
        string DateOfProtocol, // Дата протокола
        int NumberOfProtocol, // Номер протокола
        string PapersCategory, // Категории ценных бумаг
        string FormOfPayment, // Форма выплат
        string PaymentForOne, // Выплачиваемый доход на одну акцию
        string PaymentForAll, // Выплачиваемый доход, общий объем
        string DateOfPayment, // Дата выплаты
        string FullOfficialName, // Полное оф. наименование агента(ов)
        string PlaceOfAgents, // Место нахождения агента(ов)
        string MailAddress, // Почтовый адрес агента(ов)
        bool IncludeCalcInfo, // Включить информацию о расчете налога
        string EmitentRepresentative, // Уполномоченный представитель
        bool IsRegulationOrAttorney, // 3 флажок Устав/Доверенность
        int RegulationNumber
        )
    {
    }
}
