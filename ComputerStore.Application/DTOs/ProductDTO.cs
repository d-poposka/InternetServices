using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> categories { get; set; }
    }
    public class ProductWithoutCategoryDTO
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price  { get; set; }

    }
}
