using EmitterPersonalAccount.Application.Infrastructure.CacheManagment;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Hubs
{
    public class ResultsHubProxy : Hub
    {
        private static readonly ConcurrentDictionary<string, HubConnection> 
            connections = new();
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache distributedCache;

        //private readonly IHubContext<ResultsHubProxy> hubContext;
        //private HubConnection _resultsServiceConnection;
        public ResultsHubProxy(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ResultHubService");
            //this.distributedCache = distributedCache;
            // this.hubContext = hubContext;
        }
        public async Task<Result> EmitterSelected(string emitterId)
        {
            if (string.IsNullOrEmpty(emitterId))
                return Result.Error(new SelectedEmitterIdIsEmptyError());
            // Получить user ID
            // 1. Получаем JWT из Cookies запроса SignalR
            /*var httpContext = Context.GetHttpContext();
            var jwtToken = httpContext.Request.Cookies["tasty-cookies"]; // Или из заголовка

            // 2. Добавляем JWT в заголовок Authorization
            var request = new HttpRequestMessage(HttpMethod.Post, "/resultsHub");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            request.Content = JsonContent.Create(new { EmitterId = emitterId });

            // 3. Отправляем запрос в NotificationService
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();*/

            /*try
            {
                await EnsureConnectionActive();*/
                //await _resultsServiceConnection.InvokeAsync("EmitterSelected", emitterId);
           /* }
            catch (Exception ex)
            {
                // Отправляем ошибку клиенту
                //await hubContext.Clients. .SendAsync("Error", ex.Message);
                Console.WriteLine(ex.Message);
            }*/

            if (connections.TryGetValue(Context.ConnectionId, out var hubConnection))
                await hubConnection.InvokeAsync("EmitterSelected", emitterId);

            return Result.Success();
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var jwtToken = httpContext.Request.Cookies["tasty-cookies"];
            //var userId = GetUserIdFromJwt(jwtToken);

            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:5001/results-hub?access_token={jwtToken}") // Токен в URL
                .Build();

            //Context.Items["JwtToken"] = jwtToken;

            connections.TryAdd(Context.ConnectionId, hubConnection);

            await hubConnection.StartAsync();
            await base.OnConnectedAsync();

            // Передаём токен в NotificationService через HTTP-заголовок
            //var request = new HttpRequestMessage(HttpMethod.Post, "/results/connect");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            //await _httpClient.SendAsync(request);
           // await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            /*var httpContext = Context.GetHttpContext();
            var jwtToken = httpContext.Request.Cookies["tasty-cookies"];

            // Передаём токен в NotificationService через HTTP-заголовок
            var hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost/results-hub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(jwtToken);
                })
                .Build();*/
            connections.TryRemove(Context.ConnectionId, out var hubConnection);
            if (hubConnection is not null) await hubConnection.DisposeAsync();
            await base.OnDisconnectedAsync(exception);
        }
        /*private async Task EnsureConnectionActive()
        {
            if (_resultsServiceConnection == null)
                throw new InvalidOperationException("Соединение не инициализировано");

            if (_resultsServiceConnection.State != HubConnectionState.Connected)
            {
                await _resultsServiceConnection.StartAsync();
            }
        }*/
        private string GetUserIdFromJwt(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            return token.Claims.First(c => c.Type == "userId").Value;
        }
    }
    /*public record ConnectionInfo(
        string CurrentEmitterId,
        HubConnection HubConnection
        )
    { }*/
}
