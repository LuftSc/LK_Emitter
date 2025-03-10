using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MimeKit;

namespace EmailSender.Services
{
    public interface ISender
    {
        MimeMessage CreateMessage(string userAddress, AuthConfiramtionEvent eventData);
        Task<Result> SendAsync(string userAddress, AuthConfiramtionEvent eventData);
    }
}