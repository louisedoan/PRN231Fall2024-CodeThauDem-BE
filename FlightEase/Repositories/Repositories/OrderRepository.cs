
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;

namespace FlightEaseDB.Repositories.Repositories
{

    public partial interface IOrderRepository :IBaseRepository<Order>
    {
    }
    public partial class OrderRepository :BaseRepository<Order>, IOrderRepository
    {
         public OrderRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}


