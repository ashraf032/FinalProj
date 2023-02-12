using Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Areas.ViewModel
{
    public class DashBoardVM
    {
        public List<Order> Orders { get; set; }
        public List<Order> Orderstotal { get; set; }
    }
}
