using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace EmitterPersonalAccount.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DirectiveController : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<ActionResult> GetQueues()
        {
            var httpClient = new HttpClient();
            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes("guest:guest"));
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);

            var response = await httpClient.GetAsync("http://localhost:15672/api/bindings/%2F/e/document_exchange");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var bindings = JsonSerializer.Deserialize<List<Binding>>(content);

                foreach (var binding in bindings)
                {
                    if (binding.DestinationType == "queue")
                    {
                        Console.WriteLine($"Queue: {binding.Destination}");
                    }
                }
            }
            return Ok();

        }
    }
public class Binding
{
    public string Source { get; set; }
    public string Vhost { get; set; }
    public string Destination { get; set; }
    public string DestinationType { get; set; }
    public string RoutingKey { get; set; }
}
   /* [ApiController]
    [Route("[controller]")]
    public class DirectivesController : ControllerBase
    {*//*
        public async Task<ActionResult> CreateShareholdersMettingDirective()
        {// Создаёт запрос распоряжения на предоставление информации
         // Эмитенту для общего собрания акционеров
         // асинхронно, через очередь
            return await Task.FromResult(Ok());
        }
        public async Task<ActionResult> CreateRegistryInfoDirective()
        {// Создаёт запрос распоряжения на предоставление информации
         // из реестра
            return await Task.FromResult(Ok());
        }
        public async Task<ActionResult> CreateInvestorListDirective()
        {// Создаёт запрос распоряжения на предоставление Списка лиц
         // имеющих право на получение доходов по ценным бумагам
            return await Task.FromResult(Ok());
        }*//*
    }*/
}
