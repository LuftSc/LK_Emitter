using BaseMicroservice;

namespace ExternalOrderReportsService.Publishers
{
    public class ResultPublisher : BaseRabbitPublisher
    {
        public ResultPublisher(IConfiguration configuration) 
            : base(configuration)
        {
        }
    }
}
