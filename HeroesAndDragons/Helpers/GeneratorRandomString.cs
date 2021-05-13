using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeroesAndDragons.Helpers
{
    public static class GeneratorRandomString
    {
        public static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "qwertyuiopasdfghjklzxcvbnm";
            string rnName = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return rnName.First().ToString().ToUpper() + rnName.Substring(1);
        }
    }
}
