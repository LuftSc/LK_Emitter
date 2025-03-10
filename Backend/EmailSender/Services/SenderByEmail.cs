//using AuthService.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit.Text;
using MimeKit;
using System.Runtime;
//using MailKit.Net.Smtp;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using EmailSender.Configuration;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace EmailSender.Services
{
    public class AuthSendMessageError : Error
    {
        public override string Type => nameof(AuthSendMessageError);
    }
    public class SenderByEmail : ISender
    {
        private readonly MessageSettings settings;

        public SenderByEmail(IOptions<MessageSettings> settings)
        {
            this.settings = settings.Value;
        }

        public async Task<Result> SendAsync(string userAddress, AuthConfiramtionEvent eventData)
        {
            try
            {
                var message = CreateMessage(userAddress, eventData);

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(settings.Host, settings.Port, settings.SecureSocketOption);
                    await client.AuthenticateAsync(settings.SmtpEmailAddress, settings.SmtpPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Error(new AuthSendMessageError());
            }
        }

        public MimeMessage CreateMessage(string userAddress, AuthConfiramtionEvent eventData)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Личный кабинет эмитента", settings.SmtpEmailAddress));
            emailMessage.To.Add(new MailboxAddress(null, userAddress));

            if (eventData.MessageContent.Subject is not null)
                emailMessage.Subject = eventData.MessageContent.Subject;

            emailMessage.Body = new TextPart(TextFormat.Text)
            {
                Text = eventData.MessageContent.Text
            };

            return emailMessage;
        }
    }
}
