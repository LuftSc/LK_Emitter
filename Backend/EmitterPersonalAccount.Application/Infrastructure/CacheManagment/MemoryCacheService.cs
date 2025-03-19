using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Infrastructure.CacheManagment
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache cache;

        public MemoryCacheService(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public Result<TResult> GetValue<TResult>(string key)
            where TResult : class
        {
            ArgumentNullException.ThrowIfNullOrEmpty(key,
                "Key for get value in memory cache can not be empty or null!");

            var isExist = cache
                .TryGetValue(key, out TResult? foundValue);

            if (!isExist) return Result<TResult>
                    .Error(new MemoryCacheMissError());

            if (foundValue is null) return Result<TResult>
                    .Error(new InvalidTypeCastError());

            return Result<TResult>.Success(foundValue);
        }

        public Result SetValue<TValue>(string key, TValue value)
            where TValue : class
        {
            ArgumentNullException.ThrowIfNullOrEmpty(key,
                "Key for set value in memory cache can not be empty or null!");

            ArgumentNullException.ThrowIfNull(value,
                "Value for set value in memory cache can not be null!");

            cache.Set(key, value);

            return Result.Success();
        }
        public Result RemoveValue(string key)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(key,
                "Key for remove value in memory cache can not be empty or null!");

            cache.Remove(key);

            return Result.Success();
        }
    }

    public class MemoryCacheMissError : Error
    {
        public override string Type => nameof(MemoryCacheMissError);
    }

    public class InvalidTypeCastError : Error
    {
        public override string Type => nameof(InvalidTypeCastError);
    }
}
