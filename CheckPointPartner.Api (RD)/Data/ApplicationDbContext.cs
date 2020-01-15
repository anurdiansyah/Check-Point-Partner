using CheckPointPartner.Api_RD.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AbsenSupir.WebApp.Data
{
    /// <summary>
    ///     Defines the Entity Framework database context instance used by the application.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbQuery<AllowedCrewToCheckIn> AllowedCrewToCheckIn { get; set; }
        public DbSet<M_Crew> M_Crews { get; set; }
        public DbSet<M_RumahMakan> M_RumahMakans { get; set; }
        public DbSet<M_Pengemudi> M_Pengemudis { get; set; }
        public DbSet<M_Kenek> M_Keneks { get; set; }
        public DbSet<T_AbsenMakanSopir> T_AbsenMakanSopirs { get; set; }
        public DbSet<T_AbsenMakanSopirWithDetail> T_AbsenMakanSopirWithDetails { get; set; }
        public DbSet<T_UangJalan_CheckPoint> T_UangJalan_CheckPoints { get; set; }
        public DbSet<FaceRecognizerData> FaceRecognizerDatas { get; set; }
        public DbSet<T_CheckPoint> CheckPoints { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        ///     Configures the schema needed for the application identity framework.
        /// </summary>
        /// <param name="builder">The builder being used to construct the model for this application context.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Seed Data

            builder.Entity<IdentityRole>().HasData(
                new { Id = "1", Name = "Admin", NormalizedName = "Admin" },
                new { Id = "2", Name = "UserCheckPoint", NormalizedName = "UserCheckPoint" }
                );

            #endregion

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }


    }
}
