using BaseMicroservice;
using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using RabbitMQ.Client.Events;

namespace DocumentsService.Consumers
{
    public class DeleteDocumentConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public DeleteDocumentConsumer(string rabbitUri, IServiceProvider provider)
            :base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            var command = EventDeserializer<DeleteDocumentCommand>
                .Deserialize(args);

            Result serviceResult;

            using (var scope = provider.CreateScope()) 
            {
                var documentService = scope.ServiceProvider
                    .GetRequiredService<IDocumentsService>();

                serviceResult = await documentService
                    .DeleteDocumentById(command.DocumentId, default);
            }

            return serviceResult;
        }
    }
}
