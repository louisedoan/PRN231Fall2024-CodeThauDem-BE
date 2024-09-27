
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;

namespace Repositories.Repositories
{

    public partial interface IUserRepository : IBaseRepository<User>
    {
        Task AddUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
    }
    public partial class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly FlightEaseDbContext _context;
        public UserRepository(FlightEaseDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        #region Register
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region GetEmail

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var temp = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return temp;
        }
        #endregion
    }
}


