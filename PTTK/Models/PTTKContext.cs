using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Models
{
    public class PTTKContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MountainGroup> MountainGroups { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<RoutePoint> RoutePoints { get; set; }
        public virtual DbSet<BadgeRank> BadgeRanks { get; set; }
        public virtual DbSet<Tour> Tours { get; set; }
        public virtual DbSet<BadgeApplication> BadgeApplications { get; set; }

        public PTTKContext()
        {
        }

        public PTTKContext(DbContextOptions<PTTKContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Route>(e => {
                e.HasOne(r => r.StartingPoint)
                .WithMany(p => p.RoutesStartingWithPoint);

                e.HasOne(r => r.EndingPoint)
                .WithMany(p => p.RoutesEndingWithPoint);
            });

            modelBuilder.Entity<StandardRouteData>(e => {
                e.Property(e => e.Difficulty)
                    .HasConversion(d => d.ToString(), d => Enum.Parse<Difficulty>(d));
            });

            modelBuilder.Entity<Badge>(e => {
                e.Property(e => e.Type)
                    .HasConversion(t => t.ToString(), t => Enum.Parse<BadgeType>(t));
            });

            modelBuilder.Entity<BadgeRank>(e => {
                e.HasOne(r => r.Badge)
                .WithMany(b => b.Ranks);
            });

            modelBuilder.Entity<Entry>(e => {
                e.HasOne(e => e.Route)
                .WithMany(r => r.Entries);

                e.HasOne(e => e.Tour)
               .WithMany(t => t.Entries);
            });

            modelBuilder.Entity<Tour>(e =>
            {
                e.HasOne(t => t.Turist)
                .WithMany(t => t.Tours);
            });


            modelBuilder.Entity<BadgeApplication>(e =>
            {
                e.Property(e => e.Status)
                .HasConversion(s => s.ToString(), s => Enum.Parse<VerificationStatus>(s));

                e.HasOne(b => b.Rank)
                .WithMany(r => r.BadgeApplications);

                e.HasOne(b => b.Turist)
                .WithMany(t => t.FiledBadgeApplications);

                e.HasOne(b => b.Leader)
                .WithMany(r => r.BadgeApplicationsAssigned);

                e.HasMany(b => b.Tours)
                .WithMany(t => t.BadgeApplications)
                .UsingEntity<Dictionary<string, object>>(
                    "badge_application_tours",
                    j => j
                    .HasOne<Tour>()
                    .WithMany()
                    .HasForeignKey("tourId"),
                    j => j
                    .HasOne<BadgeApplication>()
                    .WithMany()
                    .HasForeignKey("badgeApplicationId"));
            });

            modelBuilder.Entity<LeaderData>()
                .HasMany(l => l.PermissionsForMountainGroups)
                .WithMany(mg => mg.LeadersWithPermisions)
                .UsingEntity<Dictionary<string, object>>(
                    "leader_qualifications",
                    j => j
                        .HasOne<MountainGroup>()
                        .WithMany()
                        .HasForeignKey("mountainGroupId"),
                    j => j
                        .HasOne<LeaderData>()
                        .WithMany()
                        .HasForeignKey("leaderId"));
        }

    }
}
