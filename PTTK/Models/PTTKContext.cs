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

        public PTTKContext(DbContextOptions<PTTKContext> options) : base(options)
        {
        }

    }
}
