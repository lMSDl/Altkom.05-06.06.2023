using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Product : Entity
    {
        public string Name { get; set; } = string.Empty;
        public float Price { get; set; }

        public string Description => $"{Name}: {Price} zł";


        public string? Category { get; set; }
        public int DaysToExpire { get; set; }


        public bool ShouldSerializeName()
        {
            return Price > 100;
        }
    }
}
