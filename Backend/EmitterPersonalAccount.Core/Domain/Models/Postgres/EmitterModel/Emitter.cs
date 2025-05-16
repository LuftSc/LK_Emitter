using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.LocationVO;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel
{
    public class Emitter : Entity<Guid>, IAggregateRoot
    {
        private Emitter() : base(Guid.NewGuid()) { }
        private Emitter(
            int issuerId,
            EmitterInfo emitterInfo,
            Location location,
            MailingAddress mailingAddress,
            bool isPersonalDocumentsReception,
            long authorizedCapital,
            bool isInformationDisclosure,
            string methodGettingInfoFromRegistry,
            string meetNotifyXML,
            BankDetails bankDetails,
            PaymentRecipient paymentRecipient,
            string fieldOfActivity,
            string additionalInformation
            ) : base(Guid.NewGuid())
        {
            IssuerId = issuerId;
            EmitterInfo = emitterInfo;
            Location = location;
            MailingAddress = mailingAddress;
            IsPersonalDocumentsReception = isPersonalDocumentsReception;
            AuthorizedCapital = authorizedCapital;
            IsInformationDisclosure = isInformationDisclosure;
            MethodGettingInfoFromRegistry = methodGettingInfoFromRegistry;
            MeetNotifyXML = meetNotifyXML;
            BankDetails = bankDetails;
            PaymentRecipient = paymentRecipient;
            FieldOfActivity = fieldOfActivity;
            AdditionalInformation = additionalInformation;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IssuerId { get; private set; }
        public EmitterInfo EmitterInfo { get; private set; }
        public Location Location { get; private set; } = Location.Empty;
        public MailingAddress MailingAddress { get; private set; } = MailingAddress.Empty;
        public bool IsPersonalDocumentsReception { get; private set; } = false;
        public long AuthorizedCapital { get; private set; }
        public bool IsInformationDisclosure { get; private set; } = false;
        public string MethodGettingInfoFromRegistry { get; private set; } = string.Empty;
        public string MeetNotifyXML { get; private set; } = null!;
        public BankDetails BankDetails { get; private set; } = BankDetails.Empty;
        public PaymentRecipient PaymentRecipient { get; private set; } = PaymentRecipient.Empty;
        public string FieldOfActivity { get; private set; } = string.Empty;
        public string AdditionalInformation { get; private set; } = null!;
        
        public List<User> Users { get; private set; } = [];
        public List<Document> Documents { get; private set; } = [];
        //public Registrator Registrator { get; private set; } = null!;
        //public List<OrderReport> OrderReports { get; private set; } = [];
        public static Result<Emitter> Create(
            int issuerId,
            EmitterInfo emitterInfo,
            Location location,
            MailingAddress mailingAddress,
            bool isPersonalDocumentsReception,
            long authorizedCapital,
            bool isInformationDisclosure,
            string methodGettingInfoFromRegistry,
            string meetNotifyXML,
            BankDetails bankDetails,
            PaymentRecipient paymentRecipient,
            string fieldOfActivity,
            string additionalInformation
            )
        {
            return Result<Emitter>.Success(new Emitter(
                issuerId,
                emitterInfo,
                location,
                mailingAddress, 
                isPersonalDocumentsReception,
                authorizedCapital,
                isInformationDisclosure, 
                methodGettingInfoFromRegistry,
                meetNotifyXML,
                bankDetails,
                paymentRecipient,
                fieldOfActivity,
                additionalInformation
                ));
        }

        /*public static Result<Emitter> Create(EmitterInfo emitterInfo)
        {
            var emitter = new Emitter();
            emitter.EmitterInfo = emitterInfo;

            return Result<Emitter>.Success(emitter);
        }*/

        public static Result<Emitter> Create(EmitterInfo emitterInfo, int issuerId)
        {
            var emitter = new Emitter();
            emitter.EmitterInfo = emitterInfo;
            emitter.IssuerId = issuerId;

            return Result<Emitter>.Success(emitter);
        }
    }
}
