using BusinessObjects.DTOs;
using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IOrderDetailService {
        public OrderDetailDTO CreateOrderDetail(OrderDetailDTO orderdetailCreate);
        public OrderDetailDTO UpdateOrderDetail(OrderDetailDTO orderdetailUpdate);
        public bool DeleteOrderDetail(int idTmp);
        public List<OrderDetailDTO> GetAll();
        public OrderDetailDTO GetById(int idTmp);
        ResultModel CancelTicket(int orderDetailId);

        public Task<double?> GetTotalSpendingAsync(int userId);
    }

    public class OrderDetailService : IOrderDetailService {

        private readonly IOrderDetailRepository _orderdetailRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly ISeatRepository _seatRepository;


        public OrderDetailService(IOrderDetailRepository orderdetailRepository, IFlightRepository flightRepository, ISeatRepository seatRepository)
        {
            _orderdetailRepository = orderdetailRepository;
            _flightRepository = flightRepository;
            _seatRepository = seatRepository;
        }

        public OrderDetailDTO CreateOrderDetail(OrderDetailDTO orderdetailCreate)
        {
            throw new NotImplementedException();
        }

        public OrderDetailDTO UpdateOrderDetail(OrderDetailDTO orderdetailUpdate) 
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrderDetail(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<OrderDetailDTO> GetAll() 
        {
            throw new NotImplementedException();
        }

        public OrderDetailDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }

        public ResultModel CancelTicket(int orderDetailId)
        {
            var originalOrderDetail = _orderdetailRepository.Get(orderDetailId);
            if (originalOrderDetail == null)
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Message = "Không tìm thấy vé với ID được cung cấp",
                    StatusCode = 404
                };
            }

            var flight = _flightRepository.Get(originalOrderDetail.FlightId);
            if (flight == null)
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Message = "Không tìm thấy chuyến bay liên quan đến vé",
                    StatusCode = 404
                };
            }

          
            var currentTime = DateTime.Now;
            var totalHours = (flight.DepartureTime.Value - currentTime).TotalHours;
            if (totalHours < 72)
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Message = "Vé chỉ có thể hủy trước 72 tiếng so với giờ khởi hành",
                    StatusCode = 400
                };
            }

     
            if (originalOrderDetail.Status != OrderDetailEnums.Pending.ToString() &&
                originalOrderDetail.Status != OrderDetailEnums.Success.ToString())
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Message = "Vé không thể hủy ở trạng thái này",
                    StatusCode = 400
                };
            }

          
            originalOrderDetail.Status = OrderDetailEnums.Refund.ToString();
            _orderdetailRepository.Update(originalOrderDetail);

 
            var seat = _seatRepository.Get(originalOrderDetail.SeatId);
            if (seat != null && seat.PlaneId == flight.PlaneId)
            {
            
                seat.Status = SeatEnums.Available.ToString();
                _seatRepository.Update(seat);
            }


            var refundResult = ProcessRefund(originalOrderDetail.TotalAmount);
            if (!refundResult.IsSuccess)
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Message = "Lỗi khi hoàn tiền: " + refundResult.Message,
                    StatusCode = 500
                };
            }


            _orderdetailRepository.Save();
            _seatRepository.Save();

            return new ResultModel
            {
                IsSuccess = true,
                Message = "Vé đã được hủy thành công và tiền đã được hoàn trả",
                StatusCode = 200
            };
        }
        private ResultModel ProcessRefund(double? amount)
        {
          
            if (amount > 0)
            {
                return new ResultModel
                {
                    IsSuccess = true,
                    Message = "Hoàn tiền thành công",
                    StatusCode = 200
                };
            }
            else
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Message = "Số tiền hoàn trả không hợp lệ",
                    StatusCode = 400
                };
            }
        }

        public async Task<double?> GetTotalSpendingAsync(int userId)
        {
            return await _orderdetailRepository.Get()
                .Where(od => od.Order.UserId == userId && od.Status == "success")
                .SumAsync(od => od.TotalAmount);
        }


    }

}
