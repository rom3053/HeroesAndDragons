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
    public class DragonService
    {
        private readonly AppDbContext _ctx;
        public DragonService(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public IQueryable<Dragon> GetDragons()
        {
            return _ctx.Dragons.Select(d => d);
        }

        public async Task<Dragon> GetDragonById(int dragonId)
        {
            return await _ctx.Dragons.FindAsync(dragonId);
        }
        public async Task<Dragon> CreateDragon()
        {
            Dragon dragon = new Dragon();
            await _ctx.Dragons.AddAsync(dragon);
            await _ctx.SaveChangesAsync();
            return dragon;
        }
    }
}
