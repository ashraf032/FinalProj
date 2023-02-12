using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public List<Restuorant_Category> Restuorant_Categories { get; set; }
    }
}
