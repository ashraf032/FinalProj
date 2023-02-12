using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string AppUserId { get; set; }
        public string ReciveUserId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime Date { get; set; }

    }
}
