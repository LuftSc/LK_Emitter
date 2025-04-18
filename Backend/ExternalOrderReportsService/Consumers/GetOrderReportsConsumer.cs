using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportService.DataAccess.Repositories;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ExternalOrderReportsService.Consumers
{
    public class GetOrderReportsConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public GetOrderReportsConsumer(string rabbitUri, 
            IServiceProvider provider) : base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            var ev = EventDeserializer<GetOrderReportsEvent>
                .Deserialize(args);

            using(var scope = provider.CreateScope())
            {
                var orderReportsRepository = scope.ServiceProvider
                    .GetRequiredService<IOrderReportsRepository>();

                var getReportsResult = await orderReportsRepository
                    .GetByPage(ev.IssuerId, ev.Page, ev.PageSize);

                if (!getReportsResult.IsSuccessfull) return getReportsResult;

                var paginationList = new OrderReportPaginationList(
                    getReportsResult.Value.Item1,
                    getReportsResult.Value.Item2
                        .Select(o => new OrderReportDTO
                            (o.ExternalStorageId, o.Id, o.FileName, o.Status,
                            o.RequestDate, ev.UserId)
                            )
                        .ToList()
                    );

                var sendClientEvent = new SendResultToClientEvent()
                {
                    MethodForResultSending = MethodResultSending.GetReports,
                    ContentJSON = JsonSerializer.Serialize
                        (new ReportsPaginationListContent(paginationList, ev.UserId))
                };
                
                var publisher = scope.ServiceProvider
                    .GetRequiredService<IRabbitMqPublisher>();

                await publisher
                    .SendMessageAsync(
                        JsonSerializer.Serialize(sendClientEvent), 
                        RabbitMqAction.SendResultToClient, 
                        default);
            }

            return Result.Success();
        }
    }
}
