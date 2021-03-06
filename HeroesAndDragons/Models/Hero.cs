using HeroesAndDragons.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesAndDragons.Models
{
    public class Hero : BaseModel<int>
    {
        public Hero()
        {

        }
        public Hero(string username)
        {
            Name = username;
            CreatedAt = DateTime.Now;
            Role = "user";
            WeaponDamage = GeneratorRandomString.random.Next(1, 6);
        }
        [StringLength(20, MinimumLength = 4)]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int WeaponDamage { get; set; }
        public string Role { get; set; }

        
    }
}
