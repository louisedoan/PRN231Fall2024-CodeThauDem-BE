using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Repositories.Repositories.BaseRepository
{
    public static class DependencyDB
    {
        public static void DependencyDBInit(this IServiceCollection services)
        {
            services.AddScoped<DbContext, FlightEaseDbContext>();

        }
    }
}
