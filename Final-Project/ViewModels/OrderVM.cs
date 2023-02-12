using Final_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.ViewModels
{
    public class OrderVM
    {
        public bool IsCard { get; set; }
        public string Owner { get; set; }
        public int Cvv { get; set; }
        public int CardNumber { get; set; }
        public string CardMonth { get; set; }
        public string CardYear { get; set; }
        public bool IsDelivery { get; set; }
        public string Address { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public Restuorant Restuorant { get; set; }
        public Store Store { get; set; }

    }
}
