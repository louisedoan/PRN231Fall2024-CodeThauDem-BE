using BusinessObjects.DTOs;
using Repositories.Repositories;
using BusinessObjects.Entities;
namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IPlaneService
    {
        public PlaneDTO CreatePlane(PlaneDTO planeCreate);
        public PlaneDTO UpdatePlane(PlaneDTO planeUpdate);
        public bool DeletePlane(int idTmp);
        public List<PlaneDTO> GetAll();
        public PlaneDTO GetById(int idTmp);
        public List<PlaneDTO> GetSuitablePlane();

    }

    public class PlaneService : IPlaneService
    {
        private readonly IPlaneRepository _planeRepository;

        public PlaneService(IPlaneRepository planeRepository)
        {
            _planeRepository = planeRepository;
        }

        public PlaneDTO CreatePlane(PlaneDTO planeCreate)
        {
            var plane = new Plane
            {
                PlaneCode = planeCreate.PlaneCode,
                TotalSeats = planeCreate.TotalSeats,
                Status = planeCreate.Status
            };

            _planeRepository.Create(plane);
            _planeRepository.Save();

            return new PlaneDTO
            {
                PlaneId = plane.PlaneId,
                PlaneCode = plane.PlaneCode,
                TotalSeats = plane.TotalSeats,
                Status = plane.Status
            };
        }

        public PlaneDTO UpdatePlane(PlaneDTO planeUpdate)
        {
            var plane = _planeRepository.Get(planeUpdate.PlaneId);
            if (plane == null) return null;

            plane.PlaneCode = planeUpdate.PlaneCode;
            plane.TotalSeats = planeUpdate.TotalSeats;
            plane.Status = planeUpdate.Status;

            _planeRepository.Update(plane);
            _planeRepository.Save();

            return new PlaneDTO
            {
                PlaneId = plane.PlaneId,
                PlaneCode = plane.PlaneCode,
                TotalSeats = plane.TotalSeats,
                Status = plane.Status
            };
        }

        public bool DeletePlane(int idTmp)
        {
            var plane = _planeRepository.Get(idTmp);
            if (plane == null) return false;

            _planeRepository.Delete(plane);
            _planeRepository.Save();
            return true;
        }

        public List<PlaneDTO> GetAll()
        {
            var planes = _planeRepository.Get().ToList();
            return planes.Select(plane => new PlaneDTO
            {
                PlaneId = plane.PlaneId,
                PlaneCode = plane.PlaneCode,
                TotalSeats = plane.TotalSeats,
                Status = plane.Status
            }).ToList();
        }

        public PlaneDTO GetById(int idTmp)
        {
            var plane = _planeRepository.Get(idTmp);
            if (plane == null) return null;

            return new PlaneDTO
            {
                PlaneId = plane.PlaneId,
                PlaneCode = plane.PlaneCode,
                TotalSeats = plane.TotalSeats,
                Status = plane.Status
            };
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
