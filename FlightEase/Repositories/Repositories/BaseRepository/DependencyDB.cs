using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Repositories.BaseRepository;

namespace Repositories.Repositories
{
    public static class DependencyDB
    {
        public static void DependencyDBInit(this IServiceCollection services)
        {
            services.AddScoped<DbContext, FlightEaseDbContext>();

        }
    }
}
