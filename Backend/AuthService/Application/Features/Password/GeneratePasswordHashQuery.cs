using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace AuthService.Application.Features.Password
{
    public sealed class GeneratePasswordHashQuery : Query<string>
    {
        public string Password { get; set; }
    }

    public class EmptyPasswordError : Error
    {
        public override string Type => nameof(EmptyPasswordError);
    }
    public sealed class GeneratePasswordHashQueryHandler : QueryHandler<GeneratePasswordHashQuery, string>
    {
        public override Task<Result<string>> Handle(GeneratePasswordHashQuery request, 
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
                return Task.FromResult(Error(new EmptyPasswordError()
                { Data = { { nameof(request.Password), "User password is empty!" } } }));

            var hash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);

            return Task.FromResult(Result<string>.Success(hash));
        }
    }
}
