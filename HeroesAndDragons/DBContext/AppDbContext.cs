using HeroesAndDragons.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesAndDragons.DBData
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Hero> Heroes { get; set; }
        public DbSet<Dragon> Dragons { get; set; }
        public DbSet<Hit> Hits { get; set; }
    }
}
