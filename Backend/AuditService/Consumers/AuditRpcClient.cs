using BaseMicroservice;

namespace AuditService.Consumers
{
    public class AuditRpcClient : RpcClientBase
    {
        public AuditRpcClient(string rabbitURI) : base(rabbitURI)
        {
        }
    }
}
