namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IHashService
    {
        string ComputeHash(byte[] content);
    }
}