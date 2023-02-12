using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        [Required]
        public int CampaignPercent { get; set; }
        public List<Restuorant> Restuorants { get; set; }
    }
}
