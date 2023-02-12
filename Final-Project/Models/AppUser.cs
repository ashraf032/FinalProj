using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class AppUser:IdentityUser
    {
        [StringLength(30)]

        public string FullName { get; set; }
        public bool LoginStatus { get; set; }
        public int? RestuorantId { get; set; }
        public int? StoreId { get; set; }
        public string ConnectionId { get; set; }
        public Restuorant Restuorant { get; set; }
        public Store Store { get; set; }
        public string Role { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public List<OrderItems> OrderItems { get; set; }
        public List<Order> Orders { get; set; }
        public List<Message> Messages { get; set; }
        public List<Favorite> Favorites { get; set; }
        
    }
}
