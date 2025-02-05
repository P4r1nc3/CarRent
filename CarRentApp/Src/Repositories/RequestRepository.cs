using CarRentApp.Src.Contexts;
using CarRentApp.Src.Models;

namespace CarRentApp.Src.Repositories
{
    public class RequestRepository
    {
        private readonly DatabaseContext _dbContext;
        private readonly CarRepository _carRepository;
        private readonly UserRepository _userRepository;

        public RequestRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            _carRepository = new CarRepository(dbContext);
            _userRepository = new UserRepository(dbContext);
        }

        public void CreateRequest(int carId, int userId, DateTime startDate, DateTime endDate, bool isAccepted)
        {
            Models.Car car = _carRepository.GetCar(carId);
            User user = _userRepository.GetUser(userId);

            Request newRequest = new Request
            {
                CarId = carId,
                UserId = userId,
                Car = car,
                User = user,
                StartDate = startDate,
                EndDate = endDate,
                IsAccepted = isAccepted
            };

            _dbContext.Requests.Add(newRequest);
            _dbContext.SaveChanges();
        }

        public Request GetRequest(int requestId)
        {
            Request? request = _dbContext.Requests.FirstOrDefault(r => r.Id == requestId);
            return request == null ? throw new KeyNotFoundException($"Request with ID {requestId} not found.") : request;
        }

        public List<Request> GetRequests()
        {
            return _dbContext.Requests.ToList();
        }

        public void UpdateRequest(int requestId, int carId, int userId, DateTime startDate, DateTime endDate, bool isAccepted)
        {
            Request? request = _dbContext.Requests.FirstOrDefault(r => r.Id == requestId);
            if (request != null)
            {
                request.CarId = carId;
                request.UserId = userId;
                request.StartDate = startDate;
                request.EndDate = endDate;
                request.IsAccepted = isAccepted;

                _dbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Request with ID {requestId} not found.");
            }
        }

        public void RemoveRequest(int requestId)
        {
            Request? request = _dbContext.Requests.FirstOrDefault(r => r.Id == requestId);
            if (request != null)
            {
                _dbContext.Requests.Remove(request);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Request with ID {requestId} not found.");
            }
        }
    }
}
