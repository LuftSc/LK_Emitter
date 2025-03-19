using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IMemoryCacheService
    {
        Result<TResult> GetValue<TResult>(string key) where TResult : class;
        Result RemoveValue(string key);
        Result SetValue<TValue>(string key, TValue value) where TValue : class;
    }
}