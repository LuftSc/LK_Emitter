namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IProtectDataService
    {
        string Decrypt(string encryptedInput, string purpose);
        string EncryptForSearch(string input, string purpose);
        string HashForSearch(string input);
        string HashWithoutSearch(string input);
    }
}