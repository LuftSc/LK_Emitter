using EmitterPersonalAccount.Core.Domain.Models.Postgres.Authorization;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.PersonalData;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MassTransit.Futures.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.Models.Postgres
{
    
    public class User : Entity<Guid>, IAggregateRoot
    {
        private User() : base(Guid.NewGuid())
        {

        }
        /*private User(
            string email, 
            string passwordHash) : base(Guid.NewGuid())
        {
            Email = email;
            PasswordHash = passwordHash;
        }*/
        private User(
            string encryptedEmail,
            string emailSearchHash,
            string passwordHash,
            string encryptedFullName,
            EncryptedPassport? encryptedPassport,
            string encryptedBirthdate = "",
            string hashedFullName = "",
            string encryptedPhone = "") : base(Guid.NewGuid())
        {
            EncryptedEmail = encryptedEmail;
            EmailSearchHash = emailSearchHash;
            PasswordHash = passwordHash;
            EncryptedFullName = encryptedFullName;
            FullNameSearchHash = hashedFullName;
            EncryptedPhone = encryptedPhone;
            EncryptedBirthDate = encryptedBirthdate;
            EncryptedPassport = encryptedPassport is null 
                ? EncryptedPassport.Empty 
                : encryptedPassport;
        }
        public string EncryptedEmail { get; private set; } = string.Empty;
        public string EmailSearchHash { get; private set; } = string.Empty;
        public string EncryptedPhone { get; private set; } = string.Empty;
        public string EncryptedBirthDate { get; private set; } = string.Empty;
        public EncryptedPassport EncryptedPassport { get; private set; } = EncryptedPassport.Empty;
        public string EncryptedFullName { get; private set; } = string.Empty;
        public string FullNameSearchHash { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        //public List<Document> Documents { get; private set; } = [];
        public List<Emitter> Emitters { get; private set; } = [];

        public ICollection<RoleEntity> Roles = [];
        /*public static Result<User> Create(string email, string passwordHash)
        {
            if (string.IsNullOrEmpty(email)) 
                return Result<User>.Error( 
                    new UserValidationError(nameof(email), 
                    $"{nameof(email)} can not be empty or null!"));

            if (string.IsNullOrEmpty(passwordHash))
                return Result<User>.Error(
                    new UserValidationError(nameof(passwordHash), 
                    $"{nameof(passwordHash)} can not be empty or null!"));

            var user = new User(email, passwordHash);

            

            return Result<User>.Success(user);
        }*/

        public static Result<User> Create(
            string encryptedEmail,
            string emailSearchHash,
            string passwordHash,
            string encryptedFullName,
            EncryptedPassport? encryptedPassport,
            string encryptedBirthdate = "",
            string hashedFullName = "",
            string encryptedPhone = ""
            )
        {
            var user = new User(
                encryptedEmail,
                emailSearchHash,
                passwordHash,
                encryptedFullName,
                encryptedPassport,
                encryptedBirthdate,
                hashedFullName,
                encryptedPhone
            );

            return Result<User>.Success(user);
        }
    }
    public class UserValidationError : Error
    {
        public UserValidationError(string fieldName, string errorText)
        {
            Data[fieldName] = errorText;
        }
        public override string Type => nameof(UserValidationError);
    }
}
