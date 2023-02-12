using Final_Project.Dal;
using Final_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Areas.Bussines.Services
{
    public class LayoutServicesBussines
    {
        private readonly WoltDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public LayoutServicesBussines(WoltDbContext context, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
          
        }


        public  Restuorant GetRestuorant()
        {
            AppUser appUser = _userManager.FindByNameAsync(_contextAccessor.HttpContext.User.Identity.Name).Result;
            Restuorant restuorant = _context.Restuorants.FirstOrDefault(r=>r.AppUserId==appUser.Id);
            return restuorant;
        }
        public Store GetStore()
        {
            AppUser appUser = _userManager.FindByNameAsync(_contextAccessor.HttpContext.User.Identity.Name).Result;
            Store store = _context.Stores.FirstOrDefault(s=>s.AppUserId==appUser.Id);
            return store;
        }
    }
}
