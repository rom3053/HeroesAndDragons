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
        //public async Task<IActionResult> HeroHitHistory(string sortOrder,
        //    string currentFilter,
        //    string searchString,
        //    int? pageNumber)
        //{
        //    if (searchString != null)
        //    {
        //        pageNumber = 1;
        //    }
        //    else
        //    {
        //        searchString = currentFilter;
        //    }
        //    //запрос по ИД героя, получаем его удары
        //    //в идеале просуммировать урон чтобы без дублей
        //    var heroes = from s in _ctx.Heroes
        //                 select s;

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            heroes = heroes.OrderByDescending(s => s.Name);
        //            break;
        //        case "Date":
        //            heroes = heroes.OrderBy(s => s.CreatedAt);
        //            break;
        //        case "date_desc":
        //            heroes = heroes.OrderByDescending(s => s.CreatedAt);
        //            break;
        //        default:
        //            heroes = heroes.OrderBy(s => s.Name);
        //            break;
        //    }

        //    int pageSize = 3;
        //    return Ok(await PaginatedList<Hero>.CreateAsync(heroes.AsNoTracking(), pageNumber ?? 1, pageSize));
        //}
        [Authorize]
        [HttpGet("attackdragon")]
        public IActionResult Hit(int dragonId)
        {
            int a = dragonId;
            var dragon = _ctx.Dragons.FirstOrDefault(d => d.Id == a);
            //-----------------------
            bool heroChecked = false;
            int heroId = 2;
            int heroIdClaimTest = 15;
            foreach (var claim in User.Claims)
            {
                if (claim.Type == ClaimTypes.NameIdentifier)
                {
                    heroChecked = int.TryParse(claim.Value, out heroIdClaimTest);
                }
            }
            var hero = _ctx.Heroes.FirstOrDefault(h => h.Id == heroId);
            //--------------------------
            Random rnd = new Random();
            dragon.HealthPoint -= hero.WeaponDamage + rnd.Next(1, 3);
            int hitdamage = dragon.HealthPoint;
            return Ok(heroIdClaimTest);
        }
    }
}
