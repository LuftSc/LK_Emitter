using EmitterPersonalAccount.Core.Domain.Models.Postgres.Authorization;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
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
        private User(string email, string passwordHash) : base(Guid.NewGuid())
        {
            Email = email;
            PasswordHash = passwordHash;
        }
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        //public List<Document> Documents { get; private set; } = [];
        public List<Emitter> Emitters { get; private set; } = [];
        public Registrator? Registrator { get; set; } = null!;

        public ICollection<RoleEntity> Roles = [];
        public static Result<User> Create(string email, string passwordHash)
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
