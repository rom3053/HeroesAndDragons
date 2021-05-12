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
                new Hero{ Name = "Cooking", CreatedAt = DateTime.Now, WeaponDamage = 4 },
                new Hero{ Name = "Listening to Music", CreatedAt = DateTime.Now, WeaponDamage = 4 },
                new Hero{ Name = "Drinking Beer", CreatedAt = DateTime.Now, WeaponDamage = 4},
                new Hero{ Name = "Playing Guitar", CreatedAt = DateTime.Now, WeaponDamage = 4 },
                new Hero{ Name = "Blogging", CreatedAt = DateTime.Now, WeaponDamage = 4 },
                new Hero{ Name = "Vlogging", CreatedAt = DateTime.Now, WeaponDamage = 4 },
                new Hero{ Name = "Travelling", CreatedAt = DateTime.Now, WeaponDamage = 4 },
            };
            var dragons = new List<Dragon>
            {
                new Dragon{ Name = "DragonBIK", CreatedAt = DateTime.Now, MaxHealthPoint = 400 },
                new Dragon{ Name = "Dragonasda", CreatedAt = DateTime.Now, MaxHealthPoint = 400 },
                new Dragon{ Name = "Flame Dragon", CreatedAt = DateTime.Now, MaxHealthPoint = 400 },
                new Dragon{ Name = "Dragon2312", CreatedAt = DateTime.Now, MaxHealthPoint = 300 },
                new Dragon{ Name = "DragonAFAf", CreatedAt = DateTime.Now, MaxHealthPoint = 100 },
                new Dragon{ Name = "Dragon12efe", CreatedAt = DateTime.Now, MaxHealthPoint = 400 },
                new Dragon{ Name = "DragonCGE", CreatedAt = DateTime.Now, MaxHealthPoint = 100 },
                new Dragon{ Name = "DragonBweg3", CreatedAt = DateTime.Now, MaxHealthPoint = 40 },
                new Dragon{ Name = "DragonCwg3g4fsdf", CreatedAt = DateTime.Now, MaxHealthPoint = 80 },
            };
            context.Heroes.AddRange(heroes);
            context.Dragons.AddRange(dragons);
            context.SaveChanges();
        }
    }
}
