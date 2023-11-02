using api.src.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace api.src.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration config;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration config) : base(options)
        {
            this.config = config;
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<FundModel> Funds { get; set; }
        public DbSet<OrganizationModel> Organizations { get; set; }
        public DbSet<FundSpecializationsModel> FundSpecializations { get; set; }
        public DbSet<FundSubgroupsModel> FundSubgroups { get; set; }
        public DbSet<ActivityModel> Activities { get; set; }
        public DbSet<InvitationLinkModel> InvitationLinks { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<TrainingResultModel> TrainingResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = config.GetConnectionString("Default");
            optionsBuilder.UseSqlite(connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<UserModel>(model =>
            // {
            //     model
            //         .HasMany(e => e.EndowmentFunds)
            //         .WithMany(e => e.Benefactors)
            //         .UsingEntity(j =>
            //         {
            //             j.ToTable("UserSelectedFunds");
            //         });

            //     model
            //         .HasMany(e => e.SelectedActivities)
            //         .WithMany(e => e.Users)
            //         .UsingEntity(j =>
            //         {
            //             j.ToTable("UserSelectedActivities");
            //         });
            // });

            // modelBuilder.Entity<FundModel>(model =>
            // {
            //     model
            //         .HasMany(e => e.FundSpecializations)
            //         .WithMany(e => e.Funds)
            //         .UsingEntity(j =>
            //         {
            //             j.ToTable("FundSpecializations");
            //         });
            // });

            // modelBuilder.Entity<FundSpecializationsModel>(model =>
            // {
            //     model
            //         .HasMany(e => e.SpecializationSubgroups)
            //         .WithMany(e => e.FundSpecializations)
            //         .UsingEntity(j => j.ToTable("FundSpecializationSubroups"));
            // });

            base.OnModelCreating(modelBuilder);
        }
    }

}