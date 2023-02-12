using Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Restuorant> RestuorantsDeliveryFree { get; set; }
        public List<Restuorant> RestuorantsCampaign { get; set; }
        public List<Restuorant> RestaurantSweet { get; set; }
        public List<Store> Stores { get; set; }
    }
}
