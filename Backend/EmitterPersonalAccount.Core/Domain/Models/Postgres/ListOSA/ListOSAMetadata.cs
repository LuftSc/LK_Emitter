using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.ListOSA
{
    public record ListOSAMetadata(
        bool ListOfPeopleRightToParticipate,
        bool ListOfPeopleRightOnPapers,
        bool ListOfPeopleRightToParticipateTwo,
        bool IsMeetingWillBeHeldByBoD,
        string MettingWillBeHeldBy,
        int MeetingNumber,
        string DecisionDate,
        string StartRegistrationTime,
        string StartMeetingTime,
        string EndRegistrationTime,
        string EndRegistrationDate,
        string MeetingPlace,
        bool IsVotingPossible,
        string AddressFilledBallots,
        string Fcs,
        string EmailAddress,
        string PhoneNumber,
        string InfoReviewingProcedure,
        bool IsParticipatingInVote,
        int AgendaNumber,
        bool IsParticipatingInVoteOnNumber,
        string EmitentRepresentative,
        bool IsRegulationOrAttorney,
        int RegulationNumber
        )
    {
    }
}
