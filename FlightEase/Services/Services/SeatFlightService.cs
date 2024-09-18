using BusinessObjects.DTOs;
using FlightEaseDB.Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface ISeatFlightService {
        public SeatFlightDTO CreateSeatFlight(SeatFlightDTO seatflightCreate);
        public SeatFlightDTO UpdateSeatFlight(SeatFlightDTO seatflightUpdate);
        public bool DeleteSeatFlight(int idTmp);
        public List<SeatFlightDTO> GetAll();
        public SeatFlightDTO GetById(int idTmp);
    }

    public class SeatFlightService : ISeatFlightService {

      private readonly ISeatFlightRepository _seatflightRepository;

        public SeatFlightService(ISeatFlightRepository seatflightRepository)
        {
            _seatflightRepository = seatflightRepository;
        }

        public SeatFlightDTO CreateSeatFlight(SeatFlightDTO seatflightCreate)
        {
            throw new NotImplementedException();
        }

        public SeatFlightDTO UpdateSeatFlight(SeatFlightDTO seatflightUpdate) 
        {
            throw new NotImplementedException();
        }

        public bool DeleteSeatFlight(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<SeatFlightDTO> GetAll() 
        {
            throw new NotImplementedException();
        }

        public SeatFlightDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }

    }

}
