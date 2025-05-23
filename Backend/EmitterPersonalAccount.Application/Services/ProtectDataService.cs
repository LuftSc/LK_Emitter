using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Services
{
    public class ProtectDataService : IProtectDataService
    {
        private readonly IDataProtectionProvider provider;
        private readonly IPasswordHasher passwordHasher;

        public ProtectDataService(IDataProtectionProvider provider,
            IPasswordHasher passwordHasher)
        {
            this.provider = provider;
            this.passwordHasher = passwordHasher;
        }

        public string EncryptForSearch(string input, string purpose)
        {
            if (input is null || input.Length == 0) return string.Empty;

            var protector = provider.CreateProtector(purpose + ".Deterministic");
            return protector.Protect(input.ToLower().Trim());
        }
        public string HashForSearch(string input)
        {
            if (input is null || input.Length == 0) return string.Empty;

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input.ToLower());
            return Convert.ToBase64String(sha256.ComputeHash(bytes));
        }
        public string HashWithoutSearch(string input)
        {
            if (input is null || input.Length == 0) return string.Empty;
            return passwordHasher.Generate(input);
        }
        public string Decrypt(string encryptedInput, string purpose)
        {
            if (encryptedInput.Length == 0) return string.Empty;

            var protector = provider.CreateProtector(purpose);
            return protector.Unprotect(encryptedInput);
        }
    }
}
