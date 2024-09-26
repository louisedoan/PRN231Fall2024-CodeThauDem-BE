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
            builder.EntitySet<SeatDTO>("Seats").EntityType.HasKey(s => s.SeatId);

            services.AddControllers().AddOData(opt =>
                opt.Select().Filter().OrderBy().Expand().SetMaxTop(null).Count().AddRouteComponents("odata", builder.GetEdmModel()));
        }
    }
}
