using HeroesAndDragons.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesAndDragons.DBData
{
    public class FakeDataSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                      serviceProvider
                      .GetRequiredService<DbContextOptions<AppDbContext>>());

            if (context.Heroes.Any()) { return; }

            var heroes = new List<Hero>
            {
                new Hero{ Name = "CoCoCo", CreatedAt = DateTime.Now, WeaponDamage = 4, Role="user" },
                new Hero{ Name = "Listening to Music", CreatedAt = DateTime.Now, WeaponDamage = 4, Role="user" },
                new Hero{ Name = "Drinking Beer", CreatedAt = DateTime.Now, WeaponDamage = 4, Role="user"},
                new Hero{ Name = "Playing Guitar", CreatedAt = DateTime.Now, WeaponDamage = 4, Role="user" },
                new Hero{ Name = "Blogging", CreatedAt = DateTime.Now, WeaponDamage = 4, Role="user" },
                new Hero{ Name = "Vlogging", CreatedAt = DateTime.Now, WeaponDamage = 4, Role="user" },
                new Hero{ Name = "Travelling", CreatedAt = DateTime.Now, WeaponDamage = 4, Role="user" },
            };
            var dragons = new List<Dragon>
            {
                new Dragon(),
                new Dragon(),
                new Dragon(),
                new Dragon(),
                new Dragon(),
                new Dragon(),
                new Dragon(),
            };
            context.Heroes.AddRange(heroes);
            context.Dragons.AddRange(dragons);
            context.SaveChanges();
        }
    }
}
