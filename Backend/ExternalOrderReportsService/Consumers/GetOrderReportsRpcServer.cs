using BaseMicroservice;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.OrderReports;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System.Text.Json;

namespace ExternalOrderReportsService.Consumers
{
    public class GetOrderReportsRpcServer
        : RpcServerBase<Result<OrderReportPaginationList>>
    {
        private readonly IServiceProvider provider;

        public GetOrderReportsRpcServer(string rabbitUri, IServiceProvider provider) 
            : base(rabbitUri)
        {
            this.provider = provider;
        }

        public override async Task<Result<OrderReportPaginationList>>
            OnMessageProcessingAsync(string message)
        {
            var ev = JsonSerializer.Deserialize<GetOrderReportsEvent>(message);

            using (var scope = provider.CreateScope())
            {
                var orderReportsRepository = scope.ServiceProvider
                    .GetRequiredService<IOrderReportsRepository>();

                var getReportsResult = await orderReportsRepository
                    .GetByPage(ev.IssuerId, ev.Page, ev.PageSize);

                if (!getReportsResult.IsSuccessfull)
                    return Result<OrderReportPaginationList>
                        .Error(new OrderReportProcessingError());

                var paginationList = new OrderReportPaginationList(
                    getReportsResult.Value.Item1,
                    getReportsResult.Value.Item2
                        .Select(o => new OrderReportDTO
                            (o.ExternalStorageId, o.Id, o.FileName, o.Status,
                            o.RequestDate, ev.UserId)
                            )
                        .ToList()
                    );

                return Result<OrderReportPaginationList>.Success(paginationList);
            }
        }

        public override Task<Result<OrderReportPaginationList>> 
            OnMessageProcessingFailureAsync(Exception exception)
        {
            return Task.FromResult(Result<OrderReportPaginationList>
                .Error(new OrderReportProcessingError()));
        }
    }

    public class OrderReportProcessingError : Error
    {
        public override string Type => nameof( OrderReportProcessingError );
        
    }

}
