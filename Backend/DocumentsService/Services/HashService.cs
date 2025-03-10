using EmitterPersonalAccount.Core.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace DocumentsService.Services
{
    public class HashService : IHashService
    {
        public string ComputeHash(byte[] content)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(content);

                var sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                    // "x2" означает hex в нижнем регистре
                }

                return sb.ToString();
            }
        }
    }
}
