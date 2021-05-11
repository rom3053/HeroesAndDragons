using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesAndDragons.Models
{
    public class Hit : BaseModel<int>
    {
        public int HitPower { get; set; }
        public DateTime CreatedAt { get; set; }
        public int DragonId { get; set; }
        public int HeroId { get; set; }
    }
}
