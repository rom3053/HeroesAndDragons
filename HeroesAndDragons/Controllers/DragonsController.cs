using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeroesAndDragons.DBData;
using HeroesAndDragons.Models;
using HeroesAndDragons.Helpers;
using HeroesAndDragons.Services;
using Microsoft.AspNetCore.Authorization;

namespace HeroesAndDragons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DragonsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly DragonRepository _dragonService;

        public DragonsController(AppDbContext context)
        {
            _context = context;
            _dragonService = new DragonRepository(_context);
        }

        // GET: api/Dragons
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDragonPages(string sortOrder,
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

            var dragons = _dragonService.GetDragons();

            int filterHP;
            bool isFilterHP = int.TryParse(searchString, out filterHP);
            if (!String.IsNullOrEmpty(searchString) && isFilterHP)
            {
                dragons = FiltrationDragons(currentFilter, dragons, filterHP);
            }

            dragons = SortDragons(sortOrder, dragons);

            int pageSize = 3;
            return Ok(await PaginatedList<Dragon>.CreateAsync(dragons.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        private static IQueryable<Dragon> FiltrationDragons(string currentFilter, IQueryable<Dragon> dragons, int filterHP)
        {
            switch (currentFilter)
            {
                case "max_HP":
                    dragons = dragons.Where(f => f.MaxHealthPoint == filterHP);
                    break;
                case "current_HP":
                    dragons = dragons.Where(f => f.CurrentHealthPoint == filterHP);
                    break;
                default:
                    break;
            }

            return dragons;
        }

        private static IQueryable<Dragon> SortDragons(string sortOrder, IQueryable<Dragon> dragons)
        {
            switch (sortOrder)
            {
                case "name_desc":
                    dragons = dragons.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    dragons = dragons.OrderBy(s => s.CreatedAt);
                    break;
                case "date_desc":
                    dragons = dragons.OrderByDescending(s => s.CreatedAt);
                    break;
                default:
                    dragons = dragons.OrderBy(s => s.Name);
                    break;
            }

            return dragons;
        }
        
        // GET: api/Dragons/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Dragon>> GetDragon(int id)
        {
            var dragon = await _dragonService.GetDragonById(id);

            if (dragon == null)
            {
                return NotFound();
            }

            return dragon;
        }
        //api/Dragons/journeytodragon 
        //CreateDragon
        [Authorize]
        [HttpPost("journeytodragon")]
        public async Task<ActionResult<Dragon>> JourneyToDragon()
        {
            return await _dragonService.CreateDragon();
        }
    }
}
