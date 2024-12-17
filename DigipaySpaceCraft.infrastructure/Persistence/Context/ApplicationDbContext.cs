using DigipaySpaceCraft.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace DigipaySpaceCraft.infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public ApplicationDbContext() : base()
        {
        }

        public DbSet<WeatherRequest> WeatherRequests { get; set; }
        public DbSet<WeatherHourlyData> WeatherHourlyData { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            ConfigureWeatherRequestEntity(modelBuilder);
            ConfigureWeatherHourlyDataEntity(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void ConfigureWeatherRequestEntity(ModelBuilder modelBuilder)
        {

            var entity = modelBuilder.Entity<WeatherRequest>();


            entity.HasKey(w => w.Id)
                  .HasAnnotation("SqlServer:Clustered", true);

            entity.Property(w => w.Id)
                  .ValueGeneratedOnAdd();

            entity.Property(w => w.GenerationTimeMs)
                  .IsRequired();



            entity.HasMany(w => w.HourlyWeatherData)
                  .WithOne()
                  .HasForeignKey("WeatherRequestId")
                  .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureWeatherHourlyDataEntity(ModelBuilder modelBuilder)
        {

            var entity = modelBuilder.Entity<WeatherHourlyData>();

            entity.Property(h => h.Timestamp)
                  .IsRequired();

            entity.Property(w => w.Id)
               .ValueGeneratedOnAdd();

            entity.Property(h => h.Temperature)
                  .IsRequired();

        }
    }
}
