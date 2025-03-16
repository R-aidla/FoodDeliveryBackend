using Microsoft.EntityFrameworkCore;
using FoodDeliveryBackend.Models;

namespace FoodDeliveryBackend.Data
{
    /// <summary>
    /// The main program database context class.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Default constructor for DbContext.
        /// </summary>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// A set of Companies from the database.
        /// </summary>
        public DbSet<Company> Companies { get; set; }

        /// <summary>
        /// A set of Weather observations from the database.
        /// </summary>
        public DbSet<WeatherObservation> WeatherObservations { get; set; }

        /// <summary>
        /// A set of Weather Timestamps from the database.
        /// </summary>
        public DbSet<WeatherTimestamp> WeatherTimestamps { get; set; }

        /// <summary>
        /// A set of Delivery Region Rules from the database.
        /// </summary>
        public DbSet<DeliveryRegionRule> DeliveryRegionRules { get; set; }

        /// <summary>
        /// Configure the model to contain a few premade placeholder values and define entities.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Aidla's Fastfood", Description = "Quick and tasty fast food." },
                new Company { Id = 2, Name = "Farmburger", Description = "Fresh, farm-to-table burgers." }
            );

            modelBuilder.Entity<DeliveryRegionRule>().HasData(
                new DeliveryRegionRule { 
                    Id = 1, 
                    RegionName = "Tallinn",
                    BaseCarCost = 4m,
                    BaseScooterCost = 3.5m,
                    BaseBikeCost = 3m,
                    MinLowTemperatureCost = 0.5m,
                    MaxLowTemperatureCost = 1m,
                    HighWindsCost = 0.5m,
                    RainyWeatherCost = 0.5m,
                    SnowyWeatherCost = 1m
                },
                new DeliveryRegionRule { 
                    Id = 2, 
                    RegionName = "Tartu",
                    BaseCarCost = 3.5m,
                    BaseScooterCost = 3m,
                    BaseBikeCost = 2.5m,
                    MinLowTemperatureCost = 0.5m,
                    MaxLowTemperatureCost = 1m,
                    HighWindsCost = 0.5m,
                    RainyWeatherCost = 0.5m,
                    SnowyWeatherCost = 1m
                },
                new DeliveryRegionRule { 
                    Id = 3, 
                    RegionName = "Pärnu", 
                    BaseCarCost = 3m, 
                    BaseScooterCost = 2.5m, 
                    BaseBikeCost = 2m, 
                    MinLowTemperatureCost = 0.5m, 
                    MaxLowTemperatureCost = 1m, 
                    HighWindsCost = 0.5m, 
                    RainyWeatherCost = 0.5m, 
                    SnowyWeatherCost = 1m 
                }
            );

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Company)
                .WithMany()
                .HasForeignKey(o => o.CompanyId);

            modelBuilder.Entity<WeatherObservation>()
                .HasOne(w => w.WeatherTimestamp)
                .WithMany()
                .HasForeignKey(w => w.WeatherTimestampId)
                .IsRequired(false);
        }
    }
}