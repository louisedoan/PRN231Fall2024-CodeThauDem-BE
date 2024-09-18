
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;

namespace FlightEaseDB.Repositories.Repositories
{

    public partial interface IMembershipRepository :IBaseRepository<Membership>
    {
    }
    public partial class MembershipRepository :BaseRepository<Membership>, IMembershipRepository
    {
         public MembershipRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}


