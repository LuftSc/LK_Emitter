using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal
{
    public class ActionLogType
    {
        public static readonly ActionLogType LoginToSystem = new("Заход в систему");
        public static readonly ActionLogType LogoutOfSystem = new("Выход из системы");
        public static readonly ActionLogType RequestDividendList= new("Запрос распоряжения: дивидендный список");
        public static readonly ActionLogType RequestListOSA = new("Запрос распоряжения: список участников собрания акционеров");
        public static readonly ActionLogType RequestReeRep = new("Запрос распоряжения: предоставление информации из реестра");
        public static readonly ActionLogType SendDocuments = new("Отправка документов");

        public static readonly ActionLogType DownloadDividendList = new("Загрузка отчёта: дивидендный список");
        public static readonly ActionLogType DownloadListOSA = new("Загрузка отчёта: список участников собрания акционеров");
        public static readonly ActionLogType DownloadReeRep = new ("Загрузка отчёта: предоставление информации из реестра");
        private ActionLogType(string type)
        {
            Type = type;
        }
        public string Type { get; }
    }
}
