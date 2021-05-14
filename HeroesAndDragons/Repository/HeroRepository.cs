using HeroesAndDragons.DBData;
using HeroesAndDragons.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesAndDragons.Services
{
    public class HeroRepository
    {
        private readonly AppDbContext _ctx;
        public HeroRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<Hero> GetHeroByIdAsync(int heroId)
        {
            return await _ctx.Heroes.FindAsync(heroId);
        }
        public async Task<Hero> GetHeroByNameAsync(string username)
        {
            return await _ctx.Heroes.FirstOrDefaultAsync(x => x.Name == username);
        }
        public async Task<Hero> CreateHeroAsync(string username)
        {
            Hero hero = new Hero(username);
            await _ctx.Heroes.AddAsync(hero);
            await _ctx.SaveChangesAsync();
            return hero;
        }
        public IQueryable<Hero> GetHeroes()
        {
            return from s in _ctx.Heroes
                   select s;
        }
    }
}
