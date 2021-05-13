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

namespace HeroesAndDragons.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DragonsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly DragonService _dragonService;

        public DragonsController(AppDbContext context)
        {
            _context = context;
            _dragonService = new DragonService(_context);
        }

        // GET: api/Dragons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dragon>>> GetDragons()
        {
            return await _context.Dragons.ToListAsync();
        }
        [HttpGet("dragonpages")]
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

            var dragons = _context.Dragons.Select(d=>d);

            int filterHP;
            if (!String.IsNullOrEmpty(searchString) && int.TryParse(searchString, out filterHP))
            {
                switch (currentFilter)
                {
                    case "max_HP": dragons = dragons.Where(f => f.MaxHealthPoint == filterHP);
                        break;
                    case "current_HP": dragons = dragons.Where(f => f.CurrentHealthPoint == filterHP);
                        break;
                    default:
                        break;
                }
            }

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

            int pageSize = 3;
            return Ok(await PaginatedList<Dragon>.CreateAsync(dragons.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        // GET: api/Dragons/search
        [HttpGet("search")]
        public async Task<ActionResult<Dragon>> GetDragon(int id)
        {
            var dragon = await _context.Dragons.FindAsync(id);

            if (dragon == null)
            {
                return NotFound();
            }

            return dragon;
        }
        //api/Dragons/journeytodragon 
        //CreateDragon
        [HttpPost("journeytodragon")]
        public async Task<ActionResult<Dragon>> JourneyToDragon()
        {
            return await _dragonService.CreateDragon();
        }
        // PUT: api/Dragons/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDragon(int id, Dragon dragon)
        {
            if (id != dragon.Id)
            {
                return BadRequest();
            }

            _context.Entry(dragon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DragonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Dragons
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Dragon>> PostDragon(Dragon dragon)
        {
            _context.Dragons.Add(dragon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDragon", new { id = dragon.Id }, dragon);
        }

        // DELETE: api/Dragons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Dragon>> DeleteDragon(int id)
        {
            var dragon = await _context.Dragons.FindAsync(id);
            if (dragon == null)
            {
                return NotFound();
            }

            _context.Dragons.Remove(dragon);
            await _context.SaveChangesAsync();

            return dragon;
        }

        private bool DragonExists(int id)
        {
            return _context.Dragons.Any(e => e.Id == id);
        }
    }
}
