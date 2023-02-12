using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string AppUserId { get; set; }
        public int ProductId { get; set; }
        public int? RestuorantId { get; set; }
        public int? StoreId { get; set; }
        public AppUser AppUser { get; set; }
        public Restuorant Restuorant { get; set; }
        public Store Store { get; set; }
        public Product Product { get; set; }
        public bool ISsaleComlete { get; set; }
    }
}
