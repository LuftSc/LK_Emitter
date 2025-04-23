using EmitterPersonalAccount.Core.Domain.Models.Postgres.ListOSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.DTO
{
    public record GenerateListOSARequest(
        int IssuerId, // код эмитента
        string DtMod, // Дата фиксации с 1 формы | Формат: ГГГГ-ММ-ДД
        bool NomList, // Флажок на форме
        bool IsCategMeeting, // флажок с формы 1
        bool IsRangeMeeting, // флажок с формы 1 (true - заседание, false - заочное)
        string Dt_Begsobr, // Дата проведения собрания | Формат: ГГГГ-ММ-ДД
        bool ListOfPeopleRightToParticipate, // Первый чекбокс "Список лиц, имеющих право на участие"
        bool ListOfPeopleRightOnPapers, // Второй чекбокс "Список лиц, осуществляющих права по ценным бумагам"
        bool ListOfPeopleRightToParticipateTwo, // Третий чекбокс "без персональных данных"
        bool IsMeetingWillBeHeldByBoD, // Флажок "Советом директоров"
        string MettingWillBeHeldBy, // Строка под флажком, если совет директоров true
        int MeetingNumber, // номер под инпутом
        string DecisionDate,
        string StartRegistrationTime, // Время начала регистрации
        string StartMeetingTime, // Время начала собрания
        string EndRegistrationTime, // Время окончания приема бюллетеней
        string EndRegistrationDate, // Дата окончания приема бюллетеней
        string MeetingPlace, // Место проведения собрания
        bool IsVotingPossible, // флажок "Методы голосования"
        string AddressFilledBallots, // Адрес заполненных бюллетеней
        string Fcs, // ФИО
        string EmailAddress, // email
        string PhoneNumber, // номер телефона
        string InfoReviewingProcedure, // Порядок ознакомления с информацией
        bool IsParticipatingInVote, // 1 флажок "В голосовании принимают участие.."
        int AgendaNumber, // Номер повестки дня
        bool IsParticipatingInVoteOnNumber, // 2 флажок
        string EmitentRepresentative, // Уполномоченный представитель
        bool IsRegulationOrAttorney, // 3 флажок Устав/Доверенность
        int RegulationNumber, // номер Устава или Доверенности
        string InternalDocumentId = ""
        )
    {
        public ListOSAMetadata ExtractMetadata() => new(
            ListOfPeopleRightToParticipate,
            ListOfPeopleRightOnPapers,
            ListOfPeopleRightToParticipateTwo,
            IsMeetingWillBeHeldByBoD,
            MettingWillBeHeldBy,
            MeetingNumber,
            DecisionDate,
            StartRegistrationTime,
            StartMeetingTime,
            EndRegistrationTime,
            EndRegistrationDate,
            MeetingPlace,
            IsVotingPossible,
            AddressFilledBallots,
            Fcs,
            EmailAddress,
            PhoneNumber,
            InfoReviewingProcedure,
            IsParticipatingInVote,
            AgendaNumber,
            IsParticipatingInVoteOnNumber,
            EmitentRepresentative,
            IsRegulationOrAttorney,
            RegulationNumber
            );
    }
}
