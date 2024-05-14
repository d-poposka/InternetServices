using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerStore.Domain.Models
{
    public class Product
    {
        [Key]

        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public int  Quantity { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        //public string CategoriesName { get; set; }

        [NotMapped]
        public List<Category> categories { get; set; }
    }
}
