
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.BaseRepository;

namespace FlightEaseDB.Repositories.Repositories
{

    public partial interface IPaymentRepository :IBaseRepository<Payment>
    {
    }
    public partial class PaymentRepository :BaseRepository<Payment>, IPaymentRepository
    {
         public PaymentRepository(DbContext dbContext) : base(dbContext)
         {
         }
    }
}


