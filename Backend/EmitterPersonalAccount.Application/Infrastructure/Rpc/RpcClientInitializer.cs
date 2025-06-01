using EmitterPersonalAccount.Application.Infrastructure.Consumers;
using EmitterPersonalAccount.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmitterPersonalAccount.Application.Infrastructure.Rpc
{
    public class RpcClientInitializer : IHostedService
    {
        private readonly RpcClient rpcClient;
        

        public RpcClientInitializer(IServiceProvider serviceProvider)
        {
            rpcClient = (RpcClient)serviceProvider.GetRequiredService<IRpcClient>();
            
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await rpcClient.InitializeAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            rpcClient.Dispose();
            await Task.CompletedTask;
        }
    }
}
