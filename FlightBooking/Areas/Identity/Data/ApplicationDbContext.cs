using FlightBooking.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace FlightBooking.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public ApplicationDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<TblRolePermissions>().HasOne(r => r.TblPermissions).WithMany(rp => rp.TblRolePermission)
                .OnDelete(DeleteBehavior.Cascade).HasForeignKey(r => r.permissionId);

        builder.Entity<TblRolePermissions>().HasOne(r => r.Role).WithMany(rp => rp.TblRolePermissions)
            .OnDelete(DeleteBehavior.Cascade).HasForeignKey(r => r.RoleId);

        builder.Entity<TblFlight>()
         .HasOne(f => f.DepartureAirport)  // Flight has one DepartureAirport
         .WithMany()  // Assuming Airport does not explicitly collect Flights departing from it
         .HasForeignKey(f => f.DepartureAirportId)
         .OnDelete(DeleteBehavior.Restrict);  // Using Restrict to avoid cascade delete issues

        builder.Entity<TblFlight>()
            .HasOne(f => f.ArrivalAirport)  // Flight has one ArrivalAirport
            .WithMany()  // Assuming Airport does not explicitly collect Flights arriving at it
            .HasForeignKey(f => f.ArrivalAirportId)
            .OnDelete(DeleteBehavior.Restrict);  // Using Restrict to avoid cascade delete issues

        builder.Entity<TblBooking>()
           .HasOne(b => b.OutboundFlight)
           .WithMany(f => f.OutboundBookings)
           .HasForeignKey(b => b.OutboundFlightId)
           .OnDelete(DeleteBehavior.Restrict);

        // Configuring the relationship between TblFlight and TblBooking for return flights
        builder.Entity<TblBooking>()
            .HasOne(b => b.ReturnFlight)
            .WithMany(f => f.ReturnBookings)
            .HasForeignKey(b => b.ReturnFlightId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuring the relationship between TblBooking and TblPassengerDetails
        builder.Entity<TblBooking>()
            .HasOne(b => b.PassengerDetails)
            .WithMany() // Assuming one passenger can have multiple bookings
            .HasForeignKey(b => b.PassengerId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());






        base.OnModelCreating(builder);   
    }

    public virtual DbSet<TblPermissions> TblPermissions { get; set; }
    public virtual DbSet<TblRolePermissions> TblRolePermissions { get; set; }
    public virtual DbSet<ApplicationRoles> ApplicationRoles { get; set; }
    public virtual DbSet<TblDestinations> TblDestinations { get; set; }
    public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
    public virtual DbSet<TblAirport> TblAirport { get; set; }
    public virtual DbSet<TblBooking> TblBooking { get; set; }
    public virtual DbSet<TblFlight> TblFlight { get; set; }
    public virtual DbSet<TblPassengerDetails> TblPassengerDetails { get; set; }
    public virtual DbSet<TblPayments> TblPayments { get; set; }
    //public virtual DbSet<TblFlightHistory> TblFlightHistory { get; set; }




    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }

}


public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);
    }
}
