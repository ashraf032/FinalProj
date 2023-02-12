using Final_Project.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Areas.Bussines.ViewModels
{
    public class StoreEditVM
    {
        public AppUser AppUser { get; set; }
        public Store Store { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
       
    }
}
