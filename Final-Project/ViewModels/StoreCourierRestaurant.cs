using Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.ViewModels
{
    public class StoreCourierRestaurant
    {
        public List<Restuorant> Restuorants { get; set; }
        public List<Store> Stores { get; set; }
        public List<AppUser> Users { get; set; }
    }
}
