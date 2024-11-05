using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repositories.Repositories;

namespace Services.Services.BackgroundWorker
{
    public class FlightBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FlightBackgroundService> _logger;

        public FlightBackgroundService(IServiceProvider serviceProvider, ILogger<FlightBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateFlightStatusesAsync();
                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }

        private async Task UpdateFlightStatusesAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var flightRepository = scope.ServiceProvider.GetRequiredService<IFlightRepository>();
                var seatRepository = scope.ServiceProvider.GetRequiredService<ISeatRepository>();

                var flightsToInUse = await flightRepository.Get()
                    .Where(f => f.DepartureTime <= DateTime.Now && f.FlightStatus == FlightStatus.Available.ToString())
                    .ToListAsync();

                foreach (var flight in flightsToInUse)
                {
                    flight.FlightStatus = FlightStatus.InUse.ToString();
                    flightRepository.Update(flight);
                }

                var flightsToDone = await flightRepository.Get()
                    .Where(f => f.ArrivalTime <= DateTime.Now && f.FlightStatus == FlightStatus.InUse.ToString())
                    .ToListAsync();

                foreach (var flight in flightsToDone)
                {
                    flight.FlightStatus = FlightStatus.Done.ToString();
                    flightRepository.Update(flight);
                }


                await flightRepository.SaveAsync();
            }
        }
    }
}
