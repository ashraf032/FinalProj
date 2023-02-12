using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public int? RestuorantId { get; set; }
        public int? StoreId { get; set; }
        public AppUser AppUser { get; set; }
        public Restuorant Restuorant { get; set; }
        public Store Store { get; set; }
    }
}
