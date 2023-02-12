using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.ViewModels
{
    public class BasketVM
    {
        public List<BasketItemVM> BasketItemVMs { get; set; }
        public double TotalPrice { get; set; }
        public int Count { get; set; }
        public bool IsDelivery { get; set; }
        public bool IsAccept { get; set; }
        public bool IsOrderComlete { get; set; }
        public bool IsCourierFind { get; set; }
        public bool IsCard { get; set; }
        public string Owner { get; set; }
        public int Cvv { get; set; }
        public int CardNumber { get; set; }
        public string CardMonth { get; set; }
        public string CardYear { get; set; }
        public bool OrderComleete { get; set; }

    }
}
