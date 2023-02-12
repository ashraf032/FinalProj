
using Final_Project.Dal;
using Final_Project.Models;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly WoltDbContext _context;

        public HomeController(WoltDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Sliders = _context.Sliders.ToList(),
                RestuorantsDeliveryFree = _context.Restuorants.Where(r => r.IsDeliveryFree&&r.ResStatus).ToList(),
                RestuorantsCampaign = _context.Restuorants.Include(r=>r.Campaign).Where(r=>r.CampaignId!=null&&r.ResStatus).ToList(),
                RestaurantSweet = _context.Restuorants.Include(r=>r.Restuorant_Categories).ThenInclude(rc=>rc.Category).Where(r=>r.ResStatus).ToList(),
                Stores=_context.Stores.Where(s=>s.StoreStatus).ToList()
                
            };
            return View(homeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var result = from x in _context.Restuorants.Where(r=>r.ResStatus) select x;
                result = result.Where(x => x.Name.Contains(content));
                return Json(await result.AsNoTracking().ToListAsync());
            }
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
     
    }
}
