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

namespace Final_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class ChatController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly WoltDbContext _context;


        public ChatController(UserManager<AppUser> userManager, WoltDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public IActionResult Index()
        {
            ChatVM chat = new ChatVM
            {
                Users = _userManager.Users.Include(u => u.Restuorant).Where(u => u.Role == "Restaurant").OrderBy(u=>u.RestuorantId).ToList(),
                StoreUsers = _userManager.Users.Include(u => u.Store).Where(u => u.Role == "Store").OrderBy(s=>s.StoreId).ToList(),
                Restuorants =_context.Restuorants.OrderBy(r=>r.Id).ToList(),
                Stores=_context.Stores.OrderBy(u=>u.Id).ToList()
                
            };
          
            return View(chat);
        }

        public async Task<IActionResult> GetMessage(string userid,string admin)
        {
            
            AppUser Admin = await _userManager.FindByNameAsync(admin);
            List<Message> messages = _context.Messages.Include(m=>m.AppUser).Where(m=>(m.AppUserId==userid&&m.ReciveUserId==Admin.Id)|| (m.AppUserId == Admin.Id && m.ReciveUserId == userid)).OrderBy(m=>m.Id).ToList();

          
            
      
            return Json(messages);
        }
    }
}
