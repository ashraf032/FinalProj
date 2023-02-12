using Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.ViewModels
{
    public class BasketItemVM
    {
        public Product Product { get; set; }
        public int Count { get; set; }
        public Double Price { get; set; }

    }
}
