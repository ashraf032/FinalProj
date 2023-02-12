using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Image { get; set; }
        [StringLength(100)]
        [Required]
        public string SliderText { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
