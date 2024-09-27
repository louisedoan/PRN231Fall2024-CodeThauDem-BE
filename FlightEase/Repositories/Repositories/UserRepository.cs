
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;

namespace Repositories.Repositories
{

    public partial interface IUserRepository : IBaseRepository<User>
    {
       
    }
    public partial class UserRepository : BaseRepository<User>, IUserRepository
    {
      
        public UserRepository(DbContext dbContext) : base(dbContext)
        {
            
        }
    }
}


