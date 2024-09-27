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

            // Remove the ODataQueryOptions parameter
            operation.Parameters = operation.Parameters
                .Where(p => !p.Name.Equals("queryOptions", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Manually add custom descriptions for OData options like $filter, $orderby, etc.
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$filter",
                In = ParameterLocation.Query,
                Description = "OData filter query (e.g., $filter=Class eq 'Economy')",
                Required = false,
                Schema = new OpenApiSchema { Type = "string" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$orderby",
                In = ParameterLocation.Query,
                Description = "OData order by query (e.g., $orderby=SeatNumber desc)",
                Required = false,
                Schema = new OpenApiSchema { Type = "string" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$top",
                In = ParameterLocation.Query,
                Description = "OData top query (e.g., $top=10)",
                Required = false,
                Schema = new OpenApiSchema { Type = "integer", Format = "int32" }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "$skip",
                In = ParameterLocation.Query,
                Description = "OData skip query (e.g., $skip=5)",
                Required = false,
                Schema = new OpenApiSchema { Type = "integer", Format = "int32" }
            });
        }
    }
}
