using Final_Project.Dal;
using Final_Project.Models;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Controllers
{
    public class StoreOrderController : Controller
    {
        private readonly WoltDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public StoreOrderController(WoltDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Checkout(int id)
        {
            if (User.Identity.Name == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            Store store = _context.Stores.FirstOrDefault(r => r.Id == id);
            if (store == null)
            {
                return RedirectToAction("error","home");
            }
            OrderVM order = new OrderVM
            {
                BasketItems = _context.BasketItems.Include(b=>b.Store).ThenInclude(r=>r.Campaign).Include(b => b.Product).Where(b => b.AppUserId == user.Id && b.StoreId == id).ToList(),
                Store=store
            };
         
           
            return View(order);
        }
    }
}
