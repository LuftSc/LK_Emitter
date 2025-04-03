using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Services
{
    public static class ClaimService
    {
        public static Result<string> Get(HttpContext context, string type)
        {
            var isClaimExist = context.User.HasClaim(c => c.Type == type);

            if (!isClaimExist)
                return Result<string>.Error(new UserClaimNotFoundError());

            var claim = context.User.FindFirst(CustomClaims.UserId).Value;

            if (claim is null)
                return Result<string>.Error(new UserClaimGettingError());

            return Result<string>.Success(claim);
        }

        public static Result<string> Get(HubCallerContext context, string type)
        {
            var isClaimExist = context.User.HasClaim(c => c.Type == type);

            if (!isClaimExist)
                return Result<string>.Error(new UserClaimNotFoundError());

            var claim = context.User.FindFirst(CustomClaims.UserId).Value;

            if (claim is null)
                return Result<string>.Error(new UserClaimGettingError());

            return Result<string>.Success(claim);
        }
    }
    public class UserClaimNotFoundError : Error
    {
        public override string Type => nameof(UserClaimNotFoundError);
    }
    public class UserClaimGettingError : Error
    {
        public override string Type => nameof(UserClaimGettingError);
    }
}
