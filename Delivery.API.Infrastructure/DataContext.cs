using Delivery.API.Application;
using Delivery.API.Application.Interfaces;
using Delivery.API.Domain.Orders;
using Delivery.API.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Delivery.API.Infrastructure;

public class DataContext : DbContext, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }
   public DbSet<Order> Orders => Set<Order>();
   
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(builder =>
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

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
                    i => Distance.FromMeters(i));
        });
    }

}