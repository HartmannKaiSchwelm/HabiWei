using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HabiWei.Models;

namespace HabiWei.Data
{
    public class HabiWeiContext : DbContext
    {
        public HabiWeiContext (DbContextOptions<HabiWeiContext> options)
            : base(options)
        {
        }

        public DbSet<HabiWei.Models.Habit> Habits { get; set; } = default!;
        public DbSet<HabiWei.Models.WeightEntry> WeightEntries { get; set; } = default!;
    }
}
