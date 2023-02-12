using Final_Project.Areas.Bussines.ViewModels;
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
    [Area("Bussines")]

    public class CourierController : Controller
    {
        private readonly WoltDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CourierController(WoltDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> Index()
        {
            AppUser courier = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Order> order = _context.Orders.Include(o => o.Restuorant).Include(o => o.AppUser).Include(o => o.OrderItems).Include(o=>o.Store).Where(o=> o.IsDelivery&&o.OrderComleete&&(o.IsCourierFind==false ||o.AppUserId==courier.Id)).ToList();
            return View(order);
        }
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> Detail()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Order> orders = _context.Orders.Include(o=>o.AppUser).Where(o=>o.CourierID==user.Id).ToList();
            return View(orders);
        }
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult>ShowOrder(int orderid)
        {
            AppUser courier = await _userManager.FindByNameAsync(User.Identity.Name);
            Order order = _context.Orders.Include(o=>o.Restuorant).Include(o=>o.Store).Include(o=>o.AppUser).Include(o=>o.OrderItems).ThenInclude(oi=>oi.Product).Where(o=>o.IsDelivery).FirstOrDefault(o => o.Id == orderid);
            return Json(order);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterCourierVM register)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser
            {
                FullName = register.FullName,
                UserName = register.UserName,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
                Role = "Courier"

            };
            IdentityResult result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, "Courier");
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> Setting()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            EditCourierVM editUser = new EditCourierVM
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber

            };
            return View(editUser);
        }
        [HttpPost]
        [Authorize(Roles = "Courier")]
        public async Task<IActionResult> Setting(EditCourierVM editUser)
        {
            AppUser ExsistUser = await _userManager.FindByNameAsync(User.Identity.Name);
            EditCourierVM EditUser = new EditCourierVM
            {
                FullName = ExsistUser.FullName,
                UserName = ExsistUser.UserName,
                Email = ExsistUser.Email,
                PhoneNumber = ExsistUser.PhoneNumber

            };
            if (!ModelState.IsValid) return View(EditUser);
            if (ExsistUser.UserName != editUser.UserName && await _userManager.FindByNameAsync(editUser.UserName) != null)
            {
                ModelState.AddModelError("", $"The {editUser.UserName} is alrady taken");
                return View(EditUser);
            }
            ExsistUser.UserName = editUser.UserName;
            ExsistUser.FullName = editUser.FullName;
            ExsistUser.Email = editUser.Email;
            ExsistUser.PhoneNumber = editUser.PhoneNumber;

            if (!string.IsNullOrEmpty(editUser.CurrentPassword))
            {
                if (editUser.Password == null)
                {
                    ModelState.AddModelError("Password", "The field password is required");
                    return View(EditUser);
                }
                IdentityResult result = await _userManager.ChangePasswordAsync(ExsistUser, editUser.CurrentPassword, editUser.Password);

                if (!result.Succeeded)
                {
                    foreach (IdentityError Error in result.Errors)
                    {
                        ModelState.AddModelError("", Error.Description);

                    }
                    return View(EditUser);
                }
                await _signInManager.PasswordSignInAsync(ExsistUser, editUser.Password, true, true);
            }
            await _userManager.UpdateAsync(ExsistUser);
            await _signInManager.SignInAsync(ExsistUser, true);
            TempData["message"] = "Data saved";
            return RedirectToAction(nameof(Setting));
        }
    }
}
