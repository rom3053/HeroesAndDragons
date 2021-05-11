using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesAndDragons.Models
{
    public class Hero : BaseModel<int>
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int WeaponDamage { get; set; }
        public string Role { get; set; }
    }
}
