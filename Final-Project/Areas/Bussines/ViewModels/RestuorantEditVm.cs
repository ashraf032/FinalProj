using Final_Project.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Areas.Bussines.ViewModels
{
    public class RestuorantEditVm
    {
        public AppUser AppUser { get; set; }
        public Restuorant restuorant { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public List<int> CategoryIds { get; set; }

    }
}
