
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;


namespace Repositories.Repositories
{

    public partial interface IFlightRepository : IBaseRepository<Flight>
    {
    }
    public partial class FlightRepository : BaseRepository<Flight>, IFlightRepository
    {
        public FlightRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}


