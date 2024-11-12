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
                await UpdateExpiredOrdersAsync();

                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }

        private async Task UpdateFlightStatusesAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var flightRepository = scope.ServiceProvider.GetRequiredService<IFlightRepository>();
                var planeRepository = scope.ServiceProvider.GetRequiredService<IPlaneRepository>();
                var seatRepository = scope.ServiceProvider.GetRequiredService<ISeatRepository>();

                // Update flights that need to be set to "InUse"
                var flightsToInUse = await flightRepository.Get()
                    .Where(f => f.DepartureTime <= DateTime.Now && f.FlightStatus == FlightStatus.Available.ToString())
                    .ToListAsync();

                foreach (var flight in flightsToInUse)
                {
                    flight.FlightStatus = FlightStatus.InUse.ToString();
                    flightRepository.Update(flight);
                }

                // Update flights that need to be set to "Done"
                var flightsToDone = await flightRepository.Get()
                    .Where(f => f.ArrivalTime <= DateTime.Now && f.FlightStatus == FlightStatus.InUse.ToString())
                    .ToListAsync();

                foreach (var flight in flightsToDone)
                {
                    flight.FlightStatus = FlightStatus.Done.ToString();
                    flightRepository.Update(flight);

                    // Retrieve the plane associated with this flight and set it to "Available"
                    var plane = await planeRepository.Get().FirstOrDefaultAsync(p => p.PlaneId == flight.PlaneId);
                    if (plane != null)
                    {
                        plane.Status = PlaneStatus.Available.ToString();
                        planeRepository.Update(plane);
                    }

                    // Update all seats of this plane to "Available"
                    var seats = await seatRepository.Get()
                        .Where(s => s.PlaneId == flight.PlaneId)
                        .ToListAsync();

                    foreach (var seat in seats)
                    {
                        seat.Status = "Available";
                        seatRepository.Update(seat);
                    }
                }

                // Save changes to the repositories
                await flightRepository.SaveAsync();
                await planeRepository.SaveAsync();
                await seatRepository.SaveAsync();
            }
        }


        private async Task UpdateExpiredOrdersAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var orderDetailRepository = scope.ServiceProvider.GetRequiredService<IOrderDetailRepository>();
                var seatRepository = scope.ServiceProvider.GetRequiredService<ISeatRepository>();

                // Get orders that have been pending for more than 15 minutes
                var expiredOrders = await orderRepository.Get()
                    .Where(o => o.Status == "Pending" && o.OrderDate <= DateTime.Now.AddMinutes(-15))
                    .ToListAsync();

                foreach (var order in expiredOrders)
                {
                    // Update each OrderDetail of the expired order to "Cancel"
                    var orderDetails = await orderDetailRepository.Get()
                        .Where(od => od.OrderId == order.OrderId)
                        .ToListAsync();

                    foreach (var orderDetail in orderDetails)
                    {
                        orderDetail.Status = "Cancel";
                        orderDetailRepository.Update(orderDetail);

                        // Update the associated Seat to "Available"
                        var seat = await seatRepository.Get().FirstOrDefaultAsync(s => s.SeatId == orderDetail.SeatId);
                        if (seat != null)
                        {
                            seat.Status = "Available";
                            seatRepository.Update(seat);
                        }
                    }

                    // Update the order status to reflect the cancellation if necessary
                    order.Status = "Cancelled";
                    orderRepository.Update(order);
                }

                // Save changes to the repositories
                await orderRepository.SaveAsync();
                await orderDetailRepository.SaveAsync();
                await seatRepository.SaveAsync();
            }
        }

    }
}
