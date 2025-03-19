using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using ExternalOrderReportsService.Contracts;
//using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace ExternalOrderReportsService.Services
{
    public class OrderReportsService
    {
        private readonly HttpClient httpClient;

        public OrderReportsService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<Result<Guid>> ListOfShareholdersForMeetingNotSign(
            DateTime r,
            ListOfShareholdersRequest request)
        {
            var dateString = r.ToString("yyyy-MM-dd HH:mm:ss");
            // Заменить на URL вашего API: 
            // http://host:port/api/ListOfShareholdersForMeetingNotSign?r=2023-07-11 09:15:15
            var url = $"https://localhost:7024/api/ListOfShareholdersForMeetingNotSign?{dateString}";

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode) return Result<Guid>
                    .Error(new GetListOfShareholdersRequestError());

            var content = await response.Content.ReadAsStringAsync();

            Guid.TryParse(content, out Guid orderId);

            return Result<Guid>.Success(orderId);
        }
    }

    public class GetListOfShareholdersRequestError : Error
    {
        public override string Type => nameof(GetListOfShareholdersRequestError);
    }
}
