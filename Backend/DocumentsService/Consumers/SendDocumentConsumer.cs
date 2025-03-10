using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using RabbitMQ.Client.Events;

namespace DocumentsService.Consumers
{
    public class SendDocumentConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public SendDocumentConsumer(string rabbitUri, string queueName, 
            IServiceProvider provider)
            : base(rabbitUri, queueName)
        {
            this.provider = provider;
        }
        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            Result response;

            var sendDocumentEvent = EventDeserializer<SendDocumentEvent>
                .Deserialize(args);

            Console.WriteLine("Попали в DocumentConsumer");

            using (var scope = provider.CreateScope())
            {
                var documentsService = scope.ServiceProvider.GetRequiredService<IDocumentsService>();

                response = await documentsService
                    .SendToRecipientAsync(sendDocumentEvent, default);
            }

            if (!response.IsSuccessfull)
            {
                return Result.Error(new SendingDocumentError());
            }

            return Result.Success();
        }
    }

    public class SendingDocumentError : Error
    {
        public override string Type => nameof(SendingDocumentError);
    }
}
