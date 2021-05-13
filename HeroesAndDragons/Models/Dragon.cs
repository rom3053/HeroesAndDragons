using HeroesAndDragons.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesAndDragons.Models
{
    public class Dragon : BaseModel<int>
    {
        public Dragon()
        {
            Name = GeneratorRandomString.RandomString(GeneratorRandomString.random.Next(4, 20));
            CreatedAt = DateTime.Now;
            MaxHealthPoint = GeneratorRandomString.random.Next(80, 100);
            CurrentHealthPoint = MaxHealthPoint;
        }
        [StringLength(20, MinimumLength = 4)]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MaxHealthPoint { get; set; }
        public int CurrentHealthPoint { get; set; }
    }
}
