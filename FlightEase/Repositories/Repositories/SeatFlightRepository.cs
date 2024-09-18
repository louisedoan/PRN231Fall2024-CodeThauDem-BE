
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;

namespace FlightEaseDB.Repositories.Repositories { 

    public partial interface ISeatFlightRepository :IBaseRepository<SeatFlight>
    {
    }
    public partial class SeatFlightRepository :BaseRepository<SeatFlight>, ISeatFlightRepository
    {
         public SeatFlightRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}


