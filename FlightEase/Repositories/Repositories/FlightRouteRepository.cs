
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;

namespace FlightEaseDB.Repositories.Repositories
{

    public partial interface IFlightRouteRepository :IBaseRepository<FlightRoute>
    {
    }
    public partial class FlightRouteRepository :BaseRepository<FlightRoute>, IFlightRouteRepository
    {
         public FlightRouteRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}


