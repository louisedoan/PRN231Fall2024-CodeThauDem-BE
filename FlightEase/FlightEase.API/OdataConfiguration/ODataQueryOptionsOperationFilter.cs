using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FlightEase.API.OdataConfiguration
{
    public class ODataQueryOptionsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                return;

            operation.Parameters = operation.Parameters
                .Where(p => !p.Name.StartsWith("$"))
                .ToList();
        }
    }
}
