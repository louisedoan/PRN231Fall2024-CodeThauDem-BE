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

    public virtual DbSet<Pilot> Pilots { get; set; }

    public virtual DbSet<Plane> Planes { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

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
            entity.HasKey(e => e.FlightId).HasName("PK__Flight__8A9E148E37684FC2");

            entity.ToTable("Flight");

            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.ArrivalTime).HasColumnType("datetime");
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");
            entity.Property(e => e.FlightRouteId).HasColumnName("FlightRouteID");
            entity.Property(e => e.FlightStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PilotId).HasColumnName("PilotID");

            entity.HasOne(d => d.FlightRoute).WithMany(p => p.Flights)
                .HasForeignKey(d => d.FlightRouteId)
                .HasConstraintName("FK__Flight__FlightRo__412EB0B6");

            entity.HasOne(d => d.Pilot).WithMany(p => p.Flights)
                .HasForeignKey(d => d.PilotId)
                .HasConstraintName("FK__Flight__PilotID__403A8C7D");
        });

        modelBuilder.Entity<FlightRoute>(entity =>
        {
            entity.HasKey(e => e.FlightRouteId).HasName("PK__FlightRo__812C3CDC671BFFA7");

            entity.ToTable("FlightRoute");

            entity.Property(e => e.FlightRouteId).HasColumnName("FlightRouteID");
            entity.Property(e => e.Duration)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Membership>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__Membersh__92A78599CCDB5381");

            entity.ToTable("Membership");

            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.Rank)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF684E18CD");

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
                .HasConstraintName("FK__Order__UserID__5070F446");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30CDCE7D282");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Flight).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.FlightId)
                .HasConstraintName("FK__OrderDeta__Fligh__5441852A");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__534D60F1");

            entity.HasOne(d => d.Seat).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.SeatId)
                .HasConstraintName("FK__OrderDeta__SeatI__5535A963");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A587F6E9E94");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.OrderDetail).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderDetailId)
                .HasConstraintName("FK__Payment__OrderDe__5812160E");
        });

        modelBuilder.Entity<Pilot>(entity =>
        {
            entity.HasKey(e => e.PilotId).HasName("PK__Pilot__B305516DA8E4472C");

            entity.ToTable("Pilot");

            entity.Property(e => e.PilotId).HasColumnName("PilotID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasMany(d => d.FlightsNavigation).WithMany(p => p.Pilots)
                .UsingEntity<Dictionary<string, object>>(
                    "PilotFlight",
                    r => r.HasOne<Flight>().WithMany()
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Pilot_Fli__Fligh__44FF419A"),
                    l => l.HasOne<Pilot>().WithMany()
                        .HasForeignKey("PilotId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Pilot_Fli__Pilot__440B1D61"),
                    j =>
                    {
                        j.HasKey("PilotId", "FlightId").HasName("PK__Pilot_Fl__4BACB02510ECE623");
                        j.ToTable("Pilot_Flight");
                        j.IndexerProperty<int>("PilotId").HasColumnName("PilotID");
                        j.IndexerProperty<int>("FlightId").HasColumnName("FlightID");
                    });
        });

        modelBuilder.Entity<Plane>(entity =>
        {
            entity.HasKey(e => e.PlaneId).HasName("PK__Plane__843E549C8062D835");

            entity.ToTable("Plane");

            entity.Property(e => e.PlaneId).HasColumnName("PlaneID");
            entity.Property(e => e.PlaneCode)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PK__RefreshT__658FEE8A58CA13A2");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.TokenId).HasColumnName("TokenID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__RefreshTo__UserI__5AEE82B9");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PK__Seat__311713D398E4DDDB");

            entity.ToTable("Seat");

            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.Class)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PlaneId).HasColumnName("PlaneID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Plane).WithMany(p => p.Seats)
                .HasForeignKey(d => d.PlaneId)
                .HasConstraintName("FK__Seat__PlaneID__398D8EEE");
        });

        modelBuilder.Entity<SeatFlight>(entity =>
        {
            entity.HasKey(e => new { e.SeatId, e.FlightId }).HasName("PK__Seat_Fli__C9BEF29B26B6D994");

            entity.ToTable("Seat_Flight");

            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.FlightId).HasColumnName("FlightID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Flight).WithMany(p => p.SeatFlights)
                .HasForeignKey(d => d.FlightId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Seat_Flig__Fligh__48CFD27E");

            entity.HasOne(d => d.Seat).WithMany(p => p.SeatFlights)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Seat_Flig__SeatI__47DBAE45");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC9FA69325");

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
                .HasConstraintName("FK__User__Membership__4D94879B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
