using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HeroesAndDragons.DBData;
using HeroesAndDragons.Helpers;
using HeroesAndDragons.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeroesAndDragons.Controllers
{
    [Route("api/heroes")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        public HeroesController(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            //ViewData["CurrentSort"] = sortOrder;
            //ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            var heroes = from s in _ctx.Heroes
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                heroes = heroes.Where(s => s.Name.Contains(searchString));                      
            }
            switch (sortOrder)
            {
                case "name_desc":
                    heroes = heroes.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    heroes = heroes.OrderBy(s => s.CreatedAt);
                    break;
                case "date_desc":
                    heroes = heroes.OrderByDescending(s => s.CreatedAt);
                    break;
                default:
                    heroes = heroes.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 3;
            return Ok(await PaginatedList<Hero>.CreateAsync(heroes.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        [HttpGet("history")]
        public async Task<IActionResult> HeroHitHistory(string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            //запрос по ИД героя, получаем его удары
            //в идеале просуммировать урон чтобы без дублей
            var hits = from h in _ctx.Hits
                       where h.HeroId == 2
                       select h;
            //var hitsDragon = from h in _ctx.Hits
            //                 where h.HeroId == 2
            //                 join d in _ctx.Dragons
            //                  on h.DragonId equals d.Id 
            //                 group new {d.Name, h.HitPower} by h.DragonId into hitstats
            //                 orderby hitstats.Key
            //                 select new
            //                 {
            //                     DragonID = hitstats.Key,
            //                     DragonName = hitstats,
                                 
            //                     TotalDamage = hitstats.Sum(x => x.HitPower),
            //                 };

            var hitsDragonV = from h in _ctx.Hits
                             where h.HeroId == 2
                             group h by h.DragonId into hitdragon
                             orderby hitdragon.Key
                             select new
                             {
                                 DragonId = hitdragon.Key,
                                 TotalDamage = hitdragon.Sum(x => x.HitPower)
                             };
            foreach (var item in hitsDragonV)
            {
                Console.WriteLine(item.TotalDamage);
            }

            foreach (var item in hitsDragon)
            {
                Console.WriteLine(item.DragonID);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    hits = hits.OrderByDescending(s => s.HitPower);
                    break;
                case "Date":
                    hits = hits.OrderBy(s => s.CreatedAt);
                    break;
                case "date_desc":
                    hits = hits.OrderByDescending(s => s.CreatedAt);
                    break;
                default:
                    hits = hits.OrderBy(s => s.HitPower);
                    break;
            }

            int pageSize = 3;
            return Ok(await PaginatedList<Hit>.CreateAsync(hits.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        //[Authorize]
        [HttpGet("attackdragon")]
        public IActionResult Hit(int dragonId)
        {
            bool heroChecked = true;
            int heroId = 2;
            int heroIdClaim = 15;

            var dragon = _ctx.Dragons.FirstOrDefault(d => d.Id == dragonId);
            //-----------------------
            //foreach (var claim in User.Claims)
            //{
            //    if (claim.Type == ClaimTypes.NameIdentifier)
            //    {
            //        heroChecked = int.TryParse(claim.Value, out heroIdClaim);
            //    }
            //}

            var hero = _ctx.Heroes.FirstOrDefault(h => h.Id == heroId);
            if (hero == null || !heroChecked || dragon == null)
            {
                return BadRequest(new { errorText = "Couldnt find a hero or a dragon" });
            }
            //--------------------------
            Random rnd = new Random();
            int hitPower =  hero.WeaponDamage + rnd.Next(1, 3);
            int dragonHealtPointNow = dragon.HealthPoint - hitPower;

            Hit heroStrike = new Hit() { CreatedAt = DateTime.Now, DragonId = dragon.Id, HeroId = hero.Id, HitPower = hitPower };

            dragon.HealthPoint = dragonHealtPointNow;
            _ctx.Hits.Add(heroStrike);
            _ctx.SaveChanges();
            return Ok(dragonHealtPointNow);
        }
    }
}
