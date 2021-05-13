using HeroesAndDragons.DBData;
using HeroesAndDragons.Models;
using HeroesAndDragons.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesAndDragons.Services
{
    public class HitService
    {
        private readonly AppDbContext _ctx;
        public HitService(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public IQueryable<DragonHitInfo> GetHerosHits(int heroId)
        {
            return from h in _ctx.Hits
                   where h.HeroId == heroId
                   select new DragonHitInfo
                   {
                       DragonName = (from d in _ctx.Dragons where d.Id == h.DragonId select d.Name).FirstOrDefault(),
                       HitPower = h.HitPower,
                       CreatedAt = h.CreatedAt,
                   };
        }
        public async Task<int> HeroHitDragon(Dragon dragon, Hero hero)
        {
            Random rnd = new Random();
            int hitPower = hero.WeaponDamage + rnd.Next(1, 3);
            int dragonHealtPointNow = dragon.CurrentHealthPoint - hitPower;

            Hit heroStrike = new Hit() { CreatedAt = DateTime.Now, DragonId = dragon.Id, HeroId = hero.Id, HitPower = hitPower };

            dragon.CurrentHealthPoint = dragonHealtPointNow;
            await _ctx.Hits.AddAsync(heroStrike);
            await _ctx.SaveChangesAsync();
            return dragonHealtPointNow;
        }
        //var hitsDragonV = from h in _ctx.Hits
        //                  where h.HeroId == 2
        //                  group h by h.DragonId into hitdragon
        //                  orderby hitdragon.Key
        //                  select new
        //                  {
        //                      DragonId = hitdragon.Key,
        //                      DragonName = (from d in _ctx.Dragons where d.Id == hitdragon.Key select d.Name).FirstOrDefault(),
        //                      TotalDamage = hitdragon.Sum(x => x.HitPower)
        //                  };
    }
}
