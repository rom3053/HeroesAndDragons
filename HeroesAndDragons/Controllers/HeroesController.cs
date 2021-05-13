using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HeroesAndDragons.DBData;
using HeroesAndDragons.Helpers;
using HeroesAndDragons.Models;
using HeroesAndDragons.Services;
using HeroesAndDragons.ViewModels;
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
        private HeroService _heroService;
        private DragonService _dragonService;
        private HitService _hitService;
        public HeroesController(AppDbContext ctx)
        {
            _ctx = ctx;
            _heroService = new HeroService(_ctx);
            _dragonService = new DragonService(_ctx);
            _hitService = new HitService(_ctx);
        }

        //api/heroes Get All heroes
        [Authorize]
        public async Task<IActionResult> Index(
            string sortOrder,
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

            IQueryable<Hero> heroes = _heroService.GetHeroes();
            if (!String.IsNullOrEmpty(searchString))
            {
                heroes = heroes.Where(s => s.Name.StartsWith(searchString));
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
        //api/heroes/history View all hero hits
        [Authorize]
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
            
            int heroIdClaim = 0;
            bool isHeroId = GetAuthHeroId(ref heroIdClaim);
            if (!isHeroId)
            {
                return BadRequest(new { errorText = "Couldnt find a hero or a dragon" });
            }

            IQueryable<DragonHitInfo> hits = _hitService.GetHerosHits(heroIdClaim);

            hits = SortHeroHits(sortOrder, hits);

            int pageSize = 3;
            return Ok(await PaginatedList<DragonHitInfo>.CreateAsync(hits.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        private static IQueryable<DragonHitInfo> SortHeroHits(string sortOrder, IQueryable<DragonHitInfo> hits)
        {
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

            return hits;
        }
        //api/heroes/attack
        [Authorize]
        [HttpPost("attack")]
        public async Task<IActionResult> HitAsync(int dragonId)
        {
            int heroIdClaim = 0;
            bool isHeroId = GetAuthHeroId(ref heroIdClaim);
            Dragon dragon = await _dragonService.GetDragonById(dragonId);
            Hero hero = await _heroService.GetHeroByIdAsync(heroIdClaim);

            if (hero == null || !isHeroId || dragon == null)
            {
                return BadRequest(new { errorText = "Couldnt find a hero or a dragon" });
            }
            //--------------------------
            int dragonHealtPointNow = await _hitService.HeroHitDragon(dragon, hero);
            return Ok(dragonHealtPointNow);
        }

        private bool GetAuthHeroId(ref int heroIdClaim)
        {
            bool heroChecked = true;

            foreach (var claim in User.Claims)
            {
                if (claim.Type == ClaimTypes.NameIdentifier)
                {
                    heroChecked = int.TryParse(claim.Value, out heroIdClaim);
                }
            }

            return heroChecked;
        }
    }
}
