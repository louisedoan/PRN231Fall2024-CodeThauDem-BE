using BusinessObjects.DTOs;
using FlightEaseDB.Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface ISeatService {
        public SeatDTO CreateSeat(SeatDTO seatCreate);
        public SeatDTO UpdateSeat(SeatDTO seatUpdate);
        public bool DeleteSeat(int idTmp);
        public List<SeatDTO> GetAll();
        public SeatDTO GetById(int idTmp);
    }

    public class SeatService : ISeatService {

      private readonly ISeatRepository _seatRepository;

        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public SeatDTO CreateSeat(SeatDTO seatCreate)
        {
            throw new NotImplementedException();
        }

        public SeatDTO UpdateSeat(SeatDTO seatUpdate) 
        {
            throw new NotImplementedException();
        }

        public bool DeleteSeat(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<SeatDTO> GetAll() 
        {
            throw new NotImplementedException();
        }

        public SeatDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }

    }

}
