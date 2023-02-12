using Final_Project.Dal;
using Final_Project.Models;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Services
{
    public class LayoutServices
    {
        private readonly WoltDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public RegisterUserVM RegisterVM { get; set; }

        public LoginUserVM LogginVM { get; set; }

        public LayoutServices(WoltDbContext context, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public List<Order> GetOrder()
        {
            List<Order> order;
                order = _context.Orders.Where(o => o.IsOrderComlete == true).ToList();
            if (_contextAccessor.HttpContext.User.Identity.Name != null)
            {
            if (_contextAccessor.HttpContext.User.IsInRole("User"))
            {
                AppUser user;
                user = _userManager.FindByNameAsync(_contextAccessor.HttpContext.User.Identity.Name).Result;
                order = _context.Orders.Include(o => o.OrderItems).Where(o => o.AppUserId == user.Id && o.IsOrderComlete == false).ToList();
            }
            }



            return order;

        }
        public Setting GetSetting()
        {
            Setting setting = _context.Setting.FirstOrDefault();
            return setting;
        }
    }
}
