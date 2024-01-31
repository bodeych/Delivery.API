using Delivery.API.Application.Interfaces;
using Delivery.API.Domain.Entities;
using Delivery.API.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;


namespace Delivery.API.Infrastructure;

public sealed class DataContext : DbContext, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.UserId).HasColumnName("user_id");

            builder.OwnsOne(
                o => o.Pickup,
                sa =>
                {
                    sa.Property(p => p.Latitude).HasColumnName("pickup_latitude");
                    sa.Property(p => p.Longitude).HasColumnName("pickup_longitude");
                });

            builder.OwnsOne(
                o => o.Dropoff,
                sa =>
                {
                    sa.Property(p => p.Latitude).HasColumnName("dropoff_latitude");
                    sa.Property(p => p.Longitude).HasColumnName("dropoff_longitude");
                });

            builder.Property(e => e.Distance)
                .HasConversion(
                    d => d.Meters,
                    i => Distance.FromMeters(i))
                .HasColumnName("distance");

            builder.Property(x => x.Cost).HasColumnName("cost");
        });

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.UserId).ValueGeneratedNever();
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.Username).HasColumnName("username");
            builder.Property(x => x.Password).HasColumnName("password");
            builder.Property(x => x.AccessToken).HasColumnName("access_token");
            builder.Property(x => x.RefreshToken).HasColumnName("refresh_token");
        });
    }
}
    
    
    