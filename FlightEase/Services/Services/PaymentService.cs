using BusinessObjects.DTOs;
using FlightEaseDB.Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IPaymentService {
        public PaymentDTO CreatePayment(PaymentDTO paymentCreate);
        public PaymentDTO UpdatePayment(PaymentDTO paymentUpdate);
        public bool DeletePayment(int idTmp);
        public List<PaymentDTO> GetAll();
        public PaymentDTO GetById(int idTmp);
    }

    public class PaymentService : IPaymentService {

      private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public PaymentDTO CreatePayment(PaymentDTO paymentCreate)
        {
            throw new NotImplementedException();
        }

        public PaymentDTO UpdatePayment(PaymentDTO paymentUpdate) 
        {
            throw new NotImplementedException();
        }

        public bool DeletePayment(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<PaymentDTO> GetAll() 
        {
            throw new NotImplementedException();
        }

        public PaymentDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }

    }

}
