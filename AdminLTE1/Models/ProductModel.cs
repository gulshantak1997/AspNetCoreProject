using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{
    public class ProductModel
    {
        public List<Product> FindAll()
        {
            return new List<Product>
            { new Product
            {
                Id="p01",
                Name="Toy",
                Photo="thumb1.jpg",
                Price=4.5,
                Quantity=2
            },
             new Product
            {
                Id="p02",
                Name="Kids Gun",
                Photo="thumb2.jpg",
                Price=5,
                Quantity=3
            },
              new Product
            {
                Id="p03",
                Name="Kids Camera",
                Photo="thumb3.jpg",
                Price=10,
                Quantity=3
            }
            };
        }
    }
}
