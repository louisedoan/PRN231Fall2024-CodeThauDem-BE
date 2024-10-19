using BusinessObjects.DTOs;
using Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IPlaneService {
        public PlaneDTO CreatePlane(PlaneDTO planeCreate);
        public PlaneDTO UpdatePlane(PlaneDTO planeUpdate);
        public bool DeletePlane(int idTmp);
        public List<PlaneDTO> GetAll();
        public PlaneDTO GetById(int idTmp);
        public List<PlaneDTO> GetSuitablePlane();

    }

    public class PlaneService : IPlaneService {

      private readonly IPlaneRepository _planeRepository;

        public PlaneService(IPlaneRepository planeRepository)
        {
            _planeRepository = planeRepository;
        }

        public PlaneDTO CreatePlane(PlaneDTO planeCreate)
        {
            throw new NotImplementedException();
        }

        public PlaneDTO UpdatePlane(PlaneDTO planeUpdate) 
        {
            throw new NotImplementedException();
        }

        public bool DeletePlane(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<PlaneDTO> GetAll() 
        {
            throw new NotImplementedException();
        }

        public PlaneDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }
        public List<PlaneDTO> GetSuitablePlane()
        {
            var plane = _planeRepository.Get();

            var planeDTO = plane.Select(x => new PlaneDTO
            {
                PlaneId = x.PlaneId,
                PlaneCode = x.PlaneCode,
                Status = x.Status,
                TotalSeats = x.TotalSeats
            }).ToList();

            return planeDTO;
        }

    }

}
