using BusinessObjects.DTOs;
using Repositories.Repositories;

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
            var exitSeat = _seatRepository.Get().SingleOrDefault(x => x.SeatId == seatUpdate.SeatId);
            if (exitSeat != null) {
                exitSeat.SeatNumber = seatUpdate.SeatNumber;
                exitSeat.Status = seatUpdate.Status;
                exitSeat.Class = seatUpdate.Class;
                
                _seatRepository.Update(exitSeat);
                _seatRepository.Save();
                return seatUpdate;
            }
            throw new Exception("Seat not found.");
        }

        public bool DeleteSeat(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<SeatDTO> GetAll() 
        {
            var seats = _seatRepository.Get().ToList();
            var seatDTOs = seats.Select(seat => new SeatDTO
            {
                SeatId = seat.SeatId,
                SeatNumber = seat.SeatNumber,
                Class = seat.Class,
                Status = seat.Status,
                PlaneId = seat.PlaneId
            }).ToList();
            return seatDTOs;
        }

        public SeatDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }

    }

}
