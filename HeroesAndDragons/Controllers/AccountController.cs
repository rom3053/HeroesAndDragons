using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using HeroesAndDragons.Models;
using HeroesAndDragons.DBData;
using HeroesAndDragons.Services;
using System.Text.RegularExpressions;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeroesAndDragons.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private HeroService _heroService;

        public AccountController(AppDbContext ctx)
        {
            _ctx = ctx;
            _heroService = new HeroService(_ctx);
        }

        [HttpPost("/createhero")]
        public async Task<IActionResult> CreateHero(string username)
        {
            string pattern = @"^(?=[a-zA-Z0-9\s]{4,20}$)";
            var rx = new Regex(pattern);
            if (rx.IsMatch(username))
            {
                Hero hero = await _heroService.GetHeroByNameAsync(username);
                if (hero == null)
                {
                    hero = await _heroService.CreateHeroAsync(username);
                    return await TokenAsync(username);
                }
            }
            return BadRequest(new { errorText = "Invalid username." });
        }
        // POST api/<AccountController>
        [HttpPost("/token")]
        public async Task<IActionResult> TokenAsync(string username)
        {
            ClaimsIdentity identity = await GetIdentityAsync(username);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: Config.AuthOptions.ISSUER,
                    audience: Config.AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(Config.AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(Config.AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                token = encodedJwt,
                username = identity.Name
            };

            return Ok(response);
        }

        private async Task<ClaimsIdentity> GetIdentityAsync(string username)
        {
            Hero person = await _heroService.GetHeroByNameAsync(username);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Name),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role),
                    new Claim(ClaimTypes.NameIdentifier, person.Id.ToString())
                };

                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            
            return null;
        }
    }
}
