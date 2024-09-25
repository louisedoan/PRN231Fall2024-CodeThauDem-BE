
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;

namespace Repositories.Repositories
{

    public partial interface ISeatRepository : IBaseRepository<Seat>
    {
    }
    public partial class SeatRepository : BaseRepository<Seat>, ISeatRepository
    {
        public SeatRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}


