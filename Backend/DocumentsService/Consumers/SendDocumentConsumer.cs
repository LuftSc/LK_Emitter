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
        public SendDocumentConsumer(string rabbitUri, IServiceProvider provider)
            : base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            Console.WriteLine("Зашли в консьюмер документов");
            Result response;

            var sendDocumentEvent = EventDeserializer<SendDocumentEvent>
                .Deserialize(args);

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

            Console.Write("отправка прошла успешно");
            return Result.Success();
        }
    }

    public class SendingDocumentError : Error
    {
        public override string Type => nameof(SendingDocumentError);
    }
}
