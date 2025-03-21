using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal
{
    public class RabbitMqAction
    {
        public static readonly RabbitMqAction SendDocument = 
            new("document_exchange", "document.send", "document_send_queue");
        public static readonly RabbitMqAction GetDocumentInfo = 
            new("document_exchange", "document.get_info", "document_get_info_queue");
        public static readonly RabbitMqAction DownloadDocument = 
            new("document_exchange", "document.download", "document_download_queue");
        public static readonly RabbitMqAction DeleteDocument =
            new("document_exchange", "document.delete", "document_delete_queue");

        public static readonly RabbitMqAction SendEmailConfirmation = 
            new("", "email", "email");

        public static readonly RabbitMqAction RequestListOfShareholders =
            new("orderReports_exchange", "orderReports.list", "request_list_shareholders_queue");
        public static readonly RabbitMqAction RequestReeRep =
            new("orderReports_exchange", "orderReports.ree_rep", "request_ree_rep_queue");
        public static readonly RabbitMqAction RequestDividendList =
            new("orderReports_exchange", "orderReports.dividend", "request_dividend_list_queue");

        public static readonly RabbitMqAction ResultListOfShareholders =
            new("orderReports_exchange", "orderReports.list_result", "result_list_shareholders_queue");
        public static readonly RabbitMqAction ResultReeRep =
            new("orderReports_exchange", "orderReports.ree_rep_result", "result_ree_rep_queue");
        public static readonly RabbitMqAction ResultDividendList =
            new("orderReports_exchange", "orderReports.dividend_result", "result_dividend_list_queue");

        public static readonly RabbitMqAction DownloadReportOrder =
            new("orderReports_exchange", "orderReports.download", "download_report_order_queue");
        /*public static readonly RabbitMqAction ResultDownloadReportOrder =
            new("orderReports_exchange", "orderReports.result_download", "result_download_report_order_queue");*/
        

        public string ExchangeName { get; }
        public string RoutingKey { get; }
        public string QueueName { get; }

        private RabbitMqAction(string exchangeName, string routingKey, string queueName)
        {
            ExchangeName = exchangeName;
            RoutingKey = routingKey;
            QueueName = queueName;
        }
    }
}
