

using BusinessObjects.Entities;
using FlightEase.Services.Services;
using FlightEaseDB.BusinessLogic.Services;
using FlightEaseDB.Repositories.Repositories;
using FlightEaseDB.Services.Services;
using Microsoft.EntityFrameworkCore;

namespace FlightEaseDB.BusinessLogic.Generations.DependencyInjection
{
    public static class DependencyInjectionResolverGen
    {
        public static void InitializerDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<DbContext, FlightEaseDbContext>();
        
            services.AddScoped<IFlightService, FlightService>();
            services.AddScoped<IFlightRepository, FlightRepository>();
        
            services.AddScoped<IFlightRouteService, FlightRouteService>();
            services.AddScoped<IFlightRouteRepository, FlightRouteRepository>();
        
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
        
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
        
            services.AddScoped<IPilotService, PilotService>();
            services.AddScoped<IPilotRepository, PilotRepository>();
        
            services.AddScoped<IPlaneService, PlaneService>();
            services.AddScoped<IPlaneRepository, PlaneRepository>();
        
       
            services.AddScoped<ISeatService, SeatService>();
            services.AddScoped<ISeatRepository, SeatRepository>();
        
            services.AddScoped<ISeatFlightService, SeatFlightService>();
            services.AddScoped<ISeatFlightRepository, SeatFlightRepository>();
        
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
