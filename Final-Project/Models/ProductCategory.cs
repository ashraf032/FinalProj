using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        public int? RestuorantId { get; set; }
        public int? StoreId { get; set; }
        public Restuorant Restuorant { get; set; }
        public Store Store { get; set; }
        public List<Product> Products { get; set; }
    }
}
