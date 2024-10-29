using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public partial interface IPasswordRepository : IBaseRepository<PasswordResetToken>
    {

    }
    public partial class PasswordRepository : BaseRepository<PasswordResetToken>, IPasswordRepository
    {
        public PasswordRepository(DbContext dbContext) : base(dbContext)
        {


        }

    }
}
