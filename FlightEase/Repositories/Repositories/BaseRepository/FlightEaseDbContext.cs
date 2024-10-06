using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Repositories.Repositories.BaseRepository;

public partial class FlightEaseDbContext : DbContext
{
    public FlightEaseDbContext()
    {
    }

    public FlightEaseDbContext(DbContextOptions<FlightEaseDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Flight> Flights { get; set; }

    public virtual DbSet<FlightRoute> FlightRoutes { get; set; }

    public virtual DbSet<Membership> Memberships { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Plane> Planes { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Refund> Refunds { get; set; }

    public virtual DbSet<SeatFlight> SeatFlights { get; set; }

    public virtual DbSet<User> Users { get; set; }

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DBDefault"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flight>(entity =>
        {
            entity.HasKey(e => e.FlightId).HasName("PK__Flight__8A9E148E62E53AC3");

            entity.ToTable("Flight");

            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.ArrivalTime).HasColumnType("datetime");
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");
            entity.Property(e => e.FlightStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PlaneId).HasColumnName("PlaneID");

            entity.HasOne(d => d.ArrivalLocationNavigation).WithMany(p => p.FlightArrivalLocationNavigations)
                .HasForeignKey(d => d.ArrivalLocation)
                .HasConstraintName("FK__Flight__ArrivalL__3D5E1FD2");

            entity.HasOne(d => d.DepartureLocationNavigation).WithMany(p => p.FlightDepartureLocationNavigations)
                .HasForeignKey(d => d.DepartureLocation)
                .HasConstraintName("FK__Flight__Departur__3C69FB99");

            entity.HasOne(d => d.Plane).WithMany(p => p.Flights)
                .HasForeignKey(d => d.PlaneId)
                .HasConstraintName("FK__Flight__PlaneID__3B75D760");
        });

        modelBuilder.Entity<FlightRoute>(entity =>
        {
            entity.HasKey(e => e.FlightRouteId).HasName("PK__FlightRo__812C3CDC94B105EC");

            entity.ToTable("FlightRoute");

            entity.Property(e => e.FlightRouteId).HasColumnName("FlightRouteID");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__Membersh__92A78599891C885C");

            entity.ToTable("Membership");

            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.Rank)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF0E45690C");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TripType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Order__UserID__47DBAE45");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30CF1EA2F01");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.DoB).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nationality)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__4AB81AF0");

            entity.HasOne(d => d.SeatFlight).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => new { d.SeatId, d.FlightId })
                .HasConstraintName("FK__OrderDetail__4BAC3F29");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A584AEE5865");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Payment__OrderID__4E88ABD4");
        });

        modelBuilder.Entity<Plane>(entity =>
        {
            entity.HasKey(e => e.PlaneId).HasName("PK__Plane__843E549CE1448BCE");

            entity.ToTable("Plane");

            entity.Property(e => e.PlaneId).HasColumnName("PlaneID");
            entity.Property(e => e.PlaneCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__RefreshT__658FEE8A60F8E718");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.TokenId).HasColumnName("TokenID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__RefreshTo__UserI__5441852A");
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.RefundId).HasName("PK__Refund__725AB900E02B8EBC");

            entity.ToTable("Refund");

            entity.Property(e => e.RefundId).HasColumnName("RefundID");
            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.RefundDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.OrderDetail).WithMany(p => p.Refunds)
                .HasForeignKey(d => d.OrderDetailId)
                .HasConstraintName("FK__Refund__OrderDet__5165187F");
        });

        modelBuilder.Entity<SeatFlight>(entity =>
        {
            entity.HasKey(e => new { e.SeatId, e.FlightId }).HasName("PK__Seat_Fli__C9BEF29B9C7200F2");

            entity.ToTable("Seat_Flight");

            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.Class)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Flight).WithMany(p => p.SeatFlights)
                .HasForeignKey(d => d.FlightId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Seat_Flig__Fligh__403A8C7D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACE18AFDA1");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Membership).WithMany(p => p.Users)
                .HasForeignKey(d => d.MembershipId)
                .HasConstraintName("FK__User__Membership__44FF419A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
