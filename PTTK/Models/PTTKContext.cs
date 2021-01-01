using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTTK.Models
{
    public class PTTKContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<MountainGroup> MountainGroups { get; set; }
        public DbSet<Route> Routes { get; set; }

        public PTTKContext(DbContextOptions<PTTKContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StandardRouteData>(e =>
            {
                e.Property(e => e.Difficulty)
                    .HasConversion(d => d.ToString(), d => Enum.Parse<Difficulty>(d));
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
