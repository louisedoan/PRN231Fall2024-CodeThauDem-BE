
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;

namespace FlightEaseDB.Repositories.Repositories
{

    public partial interface IOrderDetailRepository :IBaseRepository<OrderDetail>
    {
    }
    public partial class OrderDetailRepository :BaseRepository<OrderDetail>, IOrderDetailRepository
    {
         public OrderDetailRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}


