using Final_Project.Areas.ViewModel;
using Final_Project.Dal;
using Final_Project.Models;
using Microsoft.AspNet.SignalR;
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
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class DashBoardController : Controller
    {
        private readonly WoltDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DashBoardController(WoltDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(int page=1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Orders.Where(s => s.IsOrderComlete).Count() / 4);
            DashBoardVM dashBoard = new DashBoardVM
            {
            Orders=_context.Orders.Include(o=>o.AppUser).Include(o=>o.Restuorant).Include(o=>o.Store).Where(o=>o.IsOrderComlete).Skip((page - 1) * 4).Take(4).ToList(),
            Orderstotal=_context.Orders.ToList()
            };
            return View(dashBoard);
        }
    }
}
