using BaseMicroservice;
using EmailSender.Services;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using RabbitMQ.Client.Events;

namespace EmailSender.Consumers
{
    public class MessageSendingError : Error
    {
        public override string Type => nameof(MessageSendingError);
    }

    public class AuthConfirmationConsumer : BaseRabbitConsumer
    {
        private readonly ISender sender;

        
        public AuthConfirmationConsumer(string rabbitUri, string queueName, ISender sender)
            : base(rabbitUri, queueName) 
        {
            this.sender = sender;
        }
        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            var authConfiramtionEvent = EventDeserializer<AuthConfiramtionEvent>
                .Deserialize(args);

            var result = await sender
                .SendAsync(authConfiramtionEvent.Recipient, authConfiramtionEvent);

            if (!result.IsSuccessfull)
            {
                return Result.Error(new MessageSendingError() { });
            }

            return Result.Success();
        }
    }
}
