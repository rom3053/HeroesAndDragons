using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesAndDragons.Models
{
    public class Dragon : BaseModel<int>
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MaxHealthPoint { get; set; }
        public int CurrentHealthPoint { get; set; }
    }
}
