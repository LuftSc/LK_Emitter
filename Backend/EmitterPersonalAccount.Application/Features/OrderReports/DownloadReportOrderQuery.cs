﻿using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.OrderReports
{
    public sealed class DownloadReportOrderQuery : Query<DocumentInfo>
    {
        public Guid UserId { get; set; }  
        public Guid ReportOrderId { get; set; }
        public ReportType ReportType { get; set; }
    }

    public sealed class DownloadReportOrderQueryHandler
        : QueryHandler<DownloadReportOrderQuery, DocumentInfo>
    {
        private readonly IRpcClient rpcClient;

        public DownloadReportOrderQueryHandler(IRpcClient rpcClient)
        {
            this.rpcClient = rpcClient;
        }
        public override async Task<Result<DocumentInfo>> Handle
            (DownloadReportOrderQuery request, 
            CancellationToken cancellationToken)
        {
            var message = JsonSerializer
                .Serialize(Tuple.Create(
                    request.UserId, 
                    request.ReportOrderId, 
                    request.ReportType));

            var result = await rpcClient.CallAsync<DocumentInfo>(message,
                RabbitMqAction.DownloadReportOrder, cancellationToken);

            if (!result.IsSuccessfull) return result;

            return Success(result.Value);
        }
    }
}
