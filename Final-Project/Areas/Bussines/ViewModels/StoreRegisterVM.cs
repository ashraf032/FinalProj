using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Areas.Bussines.ViewModels
{
    public class StoreRegisterVM
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(30)]
        public string Title { get; set; }
        [Required]
        public bool Terms { get; set; }
        [Required]
        public string StoreName { get; set; }
        public string StoreImage { get; set; }
        [Required]
        public string Adress { get; set; }
        [StringLength(100)]
        [Required]
        public string Description { get; set; }
        [Required]
        [StringLength(13)]
        public string PhoneNumberRestuorant { get; set; }
        [Required]
        [StringLength(15)]
        public string WorkTime { get; set; }
        public int? CampaignId { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public List<int> CategoryIds { get; set; }
    }
}
