using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.PersonalData;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository userRepository;
        private readonly IEmittersRepository emittersRepository;
        private readonly IProtectDataService protectService;

        private static TextInfo textInfo = new CultureInfo("ru-RU").TextInfo;

        public UsersService(IUserRepository userRepository,
            IEmittersRepository emittersRepository, IProtectDataService protectService)
        {
            this.userRepository = userRepository;
            this.emittersRepository = emittersRepository;
            this.protectService = protectService;
        }

        public async Task<Result> EncryptAndSaveToDb(
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
            )
        {
            var stringPassDateOfIssuer = passDateOfIssuer is null
                ? DateOnly.MinValue.ToString()
                : passDateOfIssuer.ToString();

            var stringBirthDate = birthDate is null
                ? DateOnly.MinValue.ToString()
                : birthDate.ToString();

            var encryptedEmail = protectService.EncryptForSearch(email, "User.Email");
            var hashedEmail = protectService.HashForSearch(email);

            var hashedPassword = protectService.HashWithoutSearch(password);
            var encryptFullName = protectService.EncryptForSearch(fullName, "User.FullName");

            var encryptedPassport = EncryptedPassport.Create(
                protectService.EncryptForSearch(passSeries, "User.PassportSeries"),
                protectService.EncryptForSearch(passNumber, "User.PassportNumber"),
                protectService.EncryptForSearch(stringPassDateOfIssuer, "User.PassportDateOfIssuer"),
                protectService.EncryptForSearch(passIssuer, "User.PassportIssuer"),
                protectService.EncryptForSearch(passUnitCode, "User.PassportUnitCode")
            );

            var encryptedBirthDate = protectService.EncryptForSearch(stringBirthDate, "User.BirthDate");

            var words = fullName.Split(' ');

            var fullNameSearchHash = string
                .Join(";", words
                    .Select(protectService.HashForSearch));

            var encryptedPhone = protectService.EncryptForSearch(phone, "User.Phone");

            var userCreatingResult = User.Create(
                encryptedEmail,
                hashedEmail,
                hashedPassword,
                encryptFullName,
                encryptedPassport.Value,
                encryptedBirthDate,
                fullNameSearchHash,
                encryptedPhone
            );

            if (!userCreatingResult.IsSuccessfull) return userCreatingResult;

            var savingResult = await userRepository.AddWithRole(
                userCreatingResult.Value,
                role,
                emittersGuids,
                default
            );

            if (!savingResult.IsSuccessfull) return savingResult;

            return Result.Success();
        }
        public async Task<Result> UpdateUser(
            Guid userId,
            DateOnly? birthDate,
            DateOnly? passDateOfIssuer,
            string fullName = "",
            string email = "",
            string phone = "",
            string passSeries = "",
            string passNumber = "",
            string passIssuer = "",
            string passUnitCode = ""
            )
        {
            var stringPassDateOfIssuer = passDateOfIssuer is null
                ? DateOnly.MinValue.ToString()
                : passDateOfIssuer.ToString();

            var stringBirthDate = birthDate is null
                ? DateOnly.MinValue.ToString()
                : birthDate.ToString();

            var encryptedEmail = protectService.EncryptForSearch(email, "User.Email");
            var hashedEmail = protectService.HashForSearch(email);

            var encryptFullName = protectService.EncryptForSearch(fullName, "User.FullName");

            var encryptedPassport = EncryptedPassport.Create(
                protectService.EncryptForSearch(passSeries, "User.PassportSeries"),
                protectService.EncryptForSearch(passNumber, "User.PassportNumber"),
                protectService.EncryptForSearch(stringPassDateOfIssuer, "User.PassportDateOfIssuer"),
                protectService.EncryptForSearch(passIssuer, "User.PassportIssuer"),
                protectService.EncryptForSearch(passUnitCode, "User.PassportUnitCode")
            );

            var encryptedBirthDate = protectService.EncryptForSearch(stringBirthDate, "User.BirthDate");

            var words = fullName.Split(' ');

            var fullNameSearchHash = string
                .Join(";", words
                    .Select(protectService.HashForSearch));

            var encryptedPhone = protectService.EncryptForSearch(phone, "User.Phone");

            var updatingResult = await userRepository.Update(
                userId,
                encryptedEmail: encryptedEmail,
                hashedEmail: hashedEmail,
                encryptFullName: encryptFullName,
                fullNameSearchHash: fullNameSearchHash,
                encryptedBirthDate: encryptedBirthDate,
                encryptedPassport: encryptedPassport.Value,
                encryptedPhone: encryptedPhone
            );

            if (!updatingResult.IsSuccessfull) 
                return updatingResult;

            return Result.Success();
        }

        public async Task<Result<Dictionary<Guid, string>>> GetListUsesFullName(
            List<Guid> usersGuidList, 
            CancellationToken cancellationToken)
        {
            var users = await userRepository
                .ListAsync(u => usersGuidList.Contains(u.Id), cancellationToken);

            var fullNames = users
                .Select(user => textInfo.ToTitleCase
                    (protectService.Decrypt(user.EncryptedFullName, "User.FullName.Deterministic")))
                .ToList();

            var result = usersGuidList
                .Zip(fullNames, (id, fullName) =>
                    new 
                    {
                        Id = id,
                        FullName = fullName
                    })
                .ToDictionary(x => x.Id, x => x.FullName);

            return Result<Dictionary<Guid, string>>.Success(result);
        }

        public async Task<Result<DecryptedUser>> GetUserPersonalData(
            Guid userId, 
            CancellationToken cancellation)
        {
            var userGettingResult = await userRepository
                .GetUserWithRoles(userId);

            if (!userGettingResult.IsSuccessfull)
                return Result<DecryptedUser>
                    .Error(new UserNotFoundError());

            var user = userGettingResult.Value;

            var decryptedUser = new DecryptedUser(
                Id: user.Id,
                FullName: textInfo.ToTitleCase
                    (protectService.Decrypt(user.EncryptedFullName, "User.FullName.Deterministic")),
                Email: protectService.Decrypt(user.EncryptedEmail, "User.Email.Deterministic"),
                Phone: protectService.Decrypt(user.EncryptedPhone, "User.Phone.Deterministic"),
                BirthDate: DateOnly.Parse(protectService.Decrypt(user.EncryptedBirthDate, "User.BirthDate.Deterministic")),
                Passport: new DecryptedPassport(
                    Series: protectService.Decrypt(user.EncryptedPassport.Series, "User.PassportSeries.Deterministic"),
                    Number: protectService.Decrypt(user.EncryptedPassport.Number, "User.PassportNumber.Deterministic"),
                    DateOfIssuer: DateOnly.Parse(protectService.Decrypt(user.EncryptedPassport.DateOfIssue, "User.PassportDateOfIssuer.Deterministic")),
                    Issuer: protectService.Decrypt(user.EncryptedPassport.Issuer, "User.PassportIssuer.Deterministic"),
                    UnitCode: protectService.Decrypt(user.EncryptedPassport.UnitCode, "User.PassportUnitCode.Deterministic")
                    ),
                Role: (Role)user.Roles.Max(r => r.Id),
                Emitters: user.Emitters);

            return Result<DecryptedUser>.Success(decryptedUser);
        }
        public async Task<Result<List<DecryptedUser>>> SearchUsersByFullName(string searchTerm, int page, int pageSize)
        {
            var searchHash = protectService.HashForSearch(searchTerm);

            var users = await userRepository
                .GetUsersIncludeEmitters(
                    u => u.FullNameSearchHash.Contains(searchHash),
                    page,
                    pageSize,
                    default);

            var decryptedUsers = users
                .Select(u => new DecryptedUser(
                    Id: u.Id,
                    FullName: textInfo.ToTitleCase
                        (protectService.Decrypt(u.EncryptedFullName, "User.FullName.Deterministic")),
                    Email: protectService.Decrypt(u.EncryptedEmail, "User.Email.Deterministic"),
                    Phone: protectService.Decrypt(u.EncryptedPhone, "User.Phone.Deterministic"),
                    BirthDate: DateOnly.Parse(protectService.Decrypt(u.EncryptedBirthDate, "User.BirthDate.Deterministic")),
                    Passport: new DecryptedPassport(
                        Series: protectService.Decrypt(u.EncryptedPassport.Series, "User.PassportSeries.Deterministic"),
                        Number: protectService.Decrypt(u.EncryptedPassport.Number, "User.PassportNumber.Deterministic"),
                        DateOfIssuer: DateOnly.Parse(protectService.Decrypt(u.EncryptedPassport.DateOfIssue, "User.PassportDateOfIssuer.Deterministic")),
                        Issuer: protectService.Decrypt(u.EncryptedPassport.Issuer, "User.PassportIssuer.Deterministic"),
                        UnitCode: protectService.Decrypt(u.EncryptedPassport.UnitCode, "User.PassportUnitCode.Deterministic")
                        ),
                    Role: (Role)u.Roles.Max(r => r.Id),
                    Emitters: u.Emitters)
                )
                .ToList();

            return Result<List<DecryptedUser>>.Success(decryptedUsers);
        }
    }
}
