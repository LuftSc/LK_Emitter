﻿using BaseMicroservice;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Services;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace ExternalOrderReportsService.Consumers
{
    public class DownloadReportOrderRpcServer 
        : RpcServerBase<Result<DocumentInfo>>
    {
        private readonly IServiceProvider provider;

        public DownloadReportOrderRpcServer(string rabbitUri, IServiceProvider provider) 
            : base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result<DocumentInfo>> OnMessageProcessingAsync
            (string message, BasicDeliverEventArgs args)
        {
            var downloadInfo = JsonSerializer
                .Deserialize<Tuple<Guid, Guid, ReportType>>(message);

            /*if (!isCorrectParsing) 
                return Result<DocumentInfo>
                    .Error(new StringToGuidParsingError());*/

            using (var scope = provider.CreateScope())
            {
                var orderReportsService = scope.ServiceProvider
                    .GetRequiredService<IOrderReportsService>();

                var response = await orderReportsService
                    .DownloadReport(downloadInfo.Item1, downloadInfo.Item2, downloadInfo.Item3);

                if (!response.IsSuccessfull) return response;

                return Result<DocumentInfo>.Success(response.Value);
            }
        }

      
    }

    public class DownloadingProccessError : Error
    {
        public override string Type => nameof(DownloadingProccessError);
    }

    public class StringToGuidParsingError : Error
    {
        public override string Type => nameof(StringToGuidParsingError);
    }
}
