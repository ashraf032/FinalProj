using Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.ViewModels
{
    public class ChatVM
    {
        public List<AppUser> Users { get; set; }
        public List<AppUser> StoreUsers { get; set; }
        public AppUser User { get; set; }
        public Restuorant Restuorant { get; set; }
        public List<Restuorant> Restuorants { get; set; }
        public List<Store> Stores { get; set; }
        public Store Store { get; set; }
    }
}
