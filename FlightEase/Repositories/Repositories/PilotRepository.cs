
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;

namespace Repositories.Repositories
{

    public partial interface IPilotRepository : IBaseRepository<Pilot>
    {
    }
    public partial class PilotRepository : BaseRepository<Pilot>, IPilotRepository
    {
        public PilotRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}


