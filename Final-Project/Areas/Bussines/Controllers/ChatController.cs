using Final_Project.Dal;
using Final_Project.Models;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Areas.Bussines.Controllers
{
        [Area("bussines")]
    [Authorize(Roles = "Store, Restaurant")]
    public class ChatController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly WoltDbContext _context;

        public ChatController(UserManager<AppUser> userManager,WoltDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> RestaurantAcceptChat()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            ChatVM chat = new ChatVM
            {
               User= user,
               Restuorant =_context.Restuorants.FirstOrDefault(r=>r.AppUserId==user.Id),
              
            };
            return View(chat);
        }

        public async Task<IActionResult> StoreAcceptChat()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            ChatVM chat = new ChatVM
            {
                User = user,
                Store=_context.Stores.FirstOrDefault(s=>s.AppUserId==user.Id)

            };
            return View(chat);
        }
        [HttpPost]
        public async Task<IActionResult> GetMessage( )
        {
            AppUser Admin = _userManager.Users.FirstOrDefault(u=>u.Role=="SuperAdmin");
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Message> messages = _context.Messages.Include(m => m.AppUser).ThenInclude(au=>au.Restuorant).Where(m => (m.AppUserId == user.Id && m.ReciveUserId == Admin.Id) || (m.AppUserId == Admin.Id && m.ReciveUserId == user.Id)).OrderBy(m => m.Id).ToList();

            foreach (var item in messages)
            {
                item.Date.ToString("HH:mm");
            }
            return Json(messages);
        }
    }
}
