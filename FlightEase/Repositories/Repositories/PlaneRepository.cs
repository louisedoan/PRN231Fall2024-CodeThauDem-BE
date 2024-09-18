
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;


namespace FlightEaseDB.Repositories.Repositories
{

    public partial interface IPlaneRepository :IBaseRepository<Plane>
    {
    }
    public partial class PlaneRepository :BaseRepository<Plane>, IPlaneRepository
    {
         public PlaneRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}


