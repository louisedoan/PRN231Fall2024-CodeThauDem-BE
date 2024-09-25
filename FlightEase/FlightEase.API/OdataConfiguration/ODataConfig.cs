using BusinessObjects.DTOs;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace FlightEase.API.OdataConfiguration
{
    public static class ODataConfig
    {
        public static void AddODataConfiguration(this IServiceCollection services)
        {
       
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<FlightDTO>("Flights").EntityType.HasKey(f => f.FlightId);

            services.AddControllers().AddOData(opt =>
                opt.Select().Filter().OrderBy().SetMaxTop(100).Count().AddRouteComponents("odata", builder.GetEdmModel()));
        }
    }
}
