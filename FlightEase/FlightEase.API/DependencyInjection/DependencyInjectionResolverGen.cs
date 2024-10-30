using FlightEaseDB.BusinessLogic.Services;
using FlightEaseDB.Services.Services;
using Repositories.Repositories;
using Services.EmailService;
using Services.Services;
using Services.VnPay;

namespace FlightEaseDB.BusinessLogic.Generations.DependencyInjection
{
    public static class DependencyInjectionResolverGen
    {
        public static void InitializerDependencyInjection(this IServiceCollection services)
        {

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

            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IPlaneService, PlaneService>();
            services.AddScoped<IPlaneRepository, PlaneRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<ISeatService, SeatService>();

            services.AddScoped<IPasswordRepository, PasswordRepository>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IVnPayService, VnPayService>();
        }
    }
}
