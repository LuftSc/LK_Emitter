using Registrator.API.Endpoints;

namespace Registrator.API.Extensions
{
    public static class ApiExtensions
    {
        public static void AddMappedEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapApiEndpoints();
        }
    }
}
