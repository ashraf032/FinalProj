using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class Store
    {
        public int Id { get; set; }
        [StringLength(30)]
        [Required]
        public string Name { get; set; }
        [StringLength(100)]
        [Required]
        public string Title { get; set; }
        public string Image { get; set; }
        [StringLength(100)]
        [Required]
        public string Adress { get; set; }
        [StringLength(100)]
        [Required]
        public string Description { get; set; }
        [Required]
        [StringLength(13)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(15)]
        public string WorkTime { get; set; }
        public bool IsCampaign { get; set; }
        public bool IsDeliveryFree { get; set; }
        public bool StoreStatus { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<Product> Products { get; set; }
        public string AppUserId { get; set; }
        public int? CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public List<OrderItems> OrderItems { get; set; }
        public List<Order> Orders { get; set; }
        public List<Favorite> Favorites { get; set; }

    }
}
