using EmitterPersonalAccount.Core.Domain.Models.Postgres.ReeRep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.DTO
{
    public record GenerateReeRepRequest(
        int EmitId,                        // код эмитента
        int ProcUk,                        // цифра из поля процентов на 2 странице формы
        bool NomList,                      // флажок на раскрытие списков НД
        DateOnly DtMod,                    // Дата на которую необходимо предоставить информацию
        bool OneProcMode,
        string FullName,                   // полное наименование эмитента
        bool ListPaperOwners,              // Чекбокс "Список владельцев ценных бумаг"
        bool InfoPeopleWithOpenAccount,    // Радио "Информация о людях с открытым счетом"
        bool ListFundPercentageOwners,     // Чекбокс "Список лиц, владеющих % от УК"
        bool CertificateAboutStructure,    // Чекбокс "Справка о структуре распределения акций"
        
        bool IncludeInfoShown,                  // Чекбокс "включая сведения о лицах..."
        bool CertificateAboutState,        // Чекбокс "Справка о состоянии лицевого счета"
        
        string FcsName,                    // Наименование/ФИО
        string OgrnPassport,               // ОГРН/Паспорт
        bool Another,                      // Чекбокс на Иное
        string AnotherText,                // Текст иного
        bool Section61,                    // статья 6.1
        bool Section51,                    // статья 51
        bool Section30,                    // статья 30
        bool Section20,                    // статья 20
        bool Section17,                    // статья 17
        bool AnotherSection,               // Чекбокс на Иное после статей
        string AnotherSectionText,         // Текст иного после статей
        string EmitentRepresentative,      // Уполномоченный представитель
        bool IsRegulationOrAttorney,       // Флажок Устав/Доверенность
        int RegulationNumber,               // номер Устава или Доверенности
        
        string InternalDocumentId = ""
        )
    {
        public ReeRepMetadata ExtractMetadata() => new(
             FullName,
             ListPaperOwners,
             InfoPeopleWithOpenAccount,
             ListFundPercentageOwners,
             CertificateAboutStructure,
             IncludeInfoShown,
             CertificateAboutState,
             FcsName,
             OgrnPassport,
             Another,
             AnotherText,
             Section61,
             Section51,
             Section30,
             Section20,
             Section17,
             AnotherSection,
             AnotherSectionText,
             EmitentRepresentative, 
             IsRegulationOrAttorney,
             RegulationNumber
        );
    }
}
