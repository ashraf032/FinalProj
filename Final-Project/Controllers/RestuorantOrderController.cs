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
    public class RestuorantOrderController : Controller
    {

        private readonly WoltDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public RestuorantOrderController(WoltDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Checkout(int id)
        {
            if (User.Identity.Name==null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            //Store store = _context.Stores.FirstOrDefault(s => s.Id == id);
            Restuorant restuorant = _context.Restuorants.FirstOrDefault(r => r.Id == id);
            if (restuorant==null)
            {
                return RedirectToAction("index","home");
            }
            OrderVM order = new OrderVM
            {
                BasketItems = _context.BasketItems.Include(b=>b.Restuorant).ThenInclude(r=>r.Campaign).Include(b=>b.Product).Where(b => b.AppUserId == user.Id && b.RestuorantId == id).ToList(),
                Restuorant=restuorant
            };
        
            return View(order);
        }
    }
}
