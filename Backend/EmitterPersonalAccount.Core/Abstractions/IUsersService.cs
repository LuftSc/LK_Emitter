using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IUsersService
    {
        Task<Result> EncryptAndSaveToDb(
            string email,
            string password,
            List<Guid>? emittersGuids,
            DateOnly? birthDate,
            DateOnly? passDateOfIssuer,
            string passSeries = "",
            string passNumber = "",
            string passIssuer = "",
            string passUnitCode = "",
            Role role = Role.User,
            string fullName = "",
            string phone = ""
            );

        Task<Result<Dictionary<Guid, string>>> GetListUsesFullName(
            List<Guid> usersGuidList,
            CancellationToken cancellationToken);
        Task<Result<DecryptedUser>> GetUserPersonalData(
            Guid userId,
            CancellationToken cancellation);
        Task<Result<List<DecryptedUser>>> SearchUsersByFullName(string searchTerm, int page, int pageSize);

        Task<Result> UpdateUser(
            Guid userId,
            DateOnly? birthDate,
            DateOnly? passDateOfIssuer,
            string fullName,
            string email = "",
            string phone = "",
            string passSeries = "",
            string passNumber = "",
            string passIssuer = "",
            string passUnitCode = ""
            );


    }
}