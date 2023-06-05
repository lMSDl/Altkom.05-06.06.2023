using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Bogus.Fakers
{
    public class ProductFaker : BaseFaker<Product>
    {
        public ProductFaker() {

            RuleFor(x => x.Name, x => x.Commerce.ProductName());
            RuleFor(x => x.Price, x => float.Parse(x.Commerce.Price()));
            RuleFor(x => x.Category, x => null);
            RuleFor(x => x.DaysToExpire, x => x.Random.Int(0, 10));
        }
    }
}
