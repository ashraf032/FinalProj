using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public int? RestuorantId { get; set; }
        public int? StoreId { get; set; }
        public int ProductId { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsAccept { get; set; }
        public bool IsOrderComlete { get; set; }
        public bool IsCourierFind { get; set; }
        public string CourierID { get; set; }
        public bool IsCourierTaked { get; set; }
        public bool IsCard { get; set; }
        public string Owner { get; set; }
        public int Cvv { get; set; }
        public int CardNumber { get; set; }
        public string CardMonth { get; set; }
        public string CardYear { get; set; }
        public bool OrderComleete { get; set; }
        public DateTime Date { get; set; }
        public AppUser AppUser { get; set; }
        
        public Store Store { get; set; }
        public Restuorant Restuorant { get; set; }
        public double TotalPrice { get; set; }
        public List<OrderItems> OrderItems { get; set; }
        public string Adress { get; set; }
    }
}
