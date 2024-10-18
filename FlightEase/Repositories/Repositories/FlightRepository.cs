
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;
using System.Linq.Expressions;


namespace Repositories.Repositories
{

    public partial interface IFlightRepository : IBaseRepository<Flight>
    {
        Flight GetFlightWithLocations(Expression<Func<Flight, bool>> predicate);
    }
    public partial class FlightRepository : BaseRepository<Flight>, IFlightRepository
    {
        public FlightRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public Flight GetFlightWithLocations(Expression<Func<Flight, bool>> predicate)
        {
            return _dbContext.Set<Flight>()
                .Include(f => f.DepartureLocationNavigation)
                .Include(f => f.ArrivalLocationNavigation)
                .FirstOrDefault(predicate);
        }
    }
}


