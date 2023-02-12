using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class Restuorant_Category
    {
        public int Id { get; set; }
        public int RestuorantId { get; set; }
        public int CategoryId { get; set; }
        public Restuorant Restuorant { get; set; }
        public Category Category { get; set; }
    }
}
