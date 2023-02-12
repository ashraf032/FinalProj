using Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Areas.Bussines.ViewModels
{
    public class BussinesOrderVM
    {
        public List<Order> Orders { get; set; }
        public List<Order> OrdersTotal { get; set; }
    }
}
