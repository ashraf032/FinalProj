using Final_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.ViewModels
{
    public class DetailsVM
    {
        public Restuorant Restuorant { get; set; }
        public Store Store { get; set; }
        public BasketVM BasketVM { get; set; }
        public RegisterUserVM RegisterVM { get; set; }
        public LoginUserVM loginUser { get; set; }
        public Favorite Favorite { get; set; }
    }
}
