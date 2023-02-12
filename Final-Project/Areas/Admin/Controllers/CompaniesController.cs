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
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class CompaniesController : Controller
    {
        private readonly WoltDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CompaniesController(WoltDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Restaurant()
        {
            StoreCourierRestaurant company = new StoreCourierRestaurant
            {
                Users=_userManager.Users.Where(u=>u.Role== "Restaurant").ToList(),
                Restuorants=_context.Restuorants.ToList()
            };
            return View(company);
        }
        public IActionResult Store()
        {
            StoreCourierRestaurant company = new StoreCourierRestaurant
            {
                Users = _userManager.Users.Where(u => u.Role == "Store").ToList(),
                Stores = _context.Stores.ToList()
            };
            return View(company);
        }
        public IActionResult Courier()
        {
            StoreCourierRestaurant company = new StoreCourierRestaurant
            {
                Users = _userManager.Users.Where(u => u.Role == "Courier").ToList(),
                
            };
            return View(company);
        }
        public async Task<IActionResult> ChangeStatus(string userid)
        {
            AppUser user = await _userManager.FindByIdAsync(userid);
            if (user == null) return Json(new { status = 404 });
            if (user.LoginStatus)
            {
                user.LoginStatus = false;
                await _userManager.UpdateAsync(user);
                if (user.Role == "Store")
                {
                    Store store = _context.Stores.FirstOrDefault(s => s.AppUserId == user.Id);
                    store.StoreStatus = false;
                    _context.SaveChanges();
                }
                if (user.Role == "Restaurant")
                {
                    Restuorant restaurant = _context.Restuorants.FirstOrDefault(s => s.AppUserId == user.Id);
                    restaurant.ResStatus = false;
                    _context.SaveChanges();
                }
                return Json(new { status = 200 });
            }
            else
            {
                user.LoginStatus = true;
                await  _userManager.UpdateAsync(user);
                if (user.Role == "Store")
                {
                    Store store = _context.Stores.FirstOrDefault(s => s.AppUserId == user.Id);
                    store.StoreStatus = true;
                    _context.SaveChanges();
                }
                if (user.Role == "Restaurant")
                {
                    Restuorant restaurant = _context.Restuorants.FirstOrDefault(s => s.AppUserId == user.Id);
                    restaurant.ResStatus = true;
                    _context.SaveChanges();
                }
                return Json(new { status = 201 });
            }
          

        }

        public async Task<IActionResult> Delete(string userid)
        {
            AppUser user = await _userManager.FindByIdAsync(userid);
            if (user == null) return Json(new {status=404});
            if (user.Role == "Restaurant")
            {
                Restuorant restaurant = _context.Restuorants.FirstOrDefault(r => r.AppUserId == user.Id);
                List<Message> messages = _context.Messages.Where(m=>m.AppUserId==user.Id||m.ReciveUserId==user.Id).ToList();
                List<ProductCategory> productCategories = _context.ProductCategories.Where(pc=>pc.RestuorantId==restaurant.Id).ToList();
                List<Product> products = _context.Products.Where(pc => pc.RestuorantId == restaurant.Id).ToList();
                List<Order> orders = _context.Orders.Where(pc => pc.RestuorantId == restaurant.Id).ToList();
                List<OrderItems> orderItems = _context.OrderItems.Where(pc => pc.RestuorantId == restaurant.Id).ToList();

                List<Favorite> favorites = _context.Favorites.Where(s => s.RestuorantId == restaurant.Id).ToList();


                foreach (var item in favorites)
                {
                    _context.Favorites.Remove(item);
                }
                foreach (var item in productCategories)
                {
                    _context.ProductCategories.Remove(item);
                }
                foreach (var item in messages)
                {
                    _context.Messages.Remove(item);
                }
                foreach (var item in orders)
                {
                    _context.Orders.Remove(item);
                }
                foreach (var item in orderItems)
                {
                    _context.OrderItems.Remove(item);
                }
                foreach (var item in products)
                {
                    _context.Products.Remove(item);
                }
                _context.Restuorants.Remove(restaurant);

            }
            if (user.Role == "Store")
            {
                Store store = _context.Stores.FirstOrDefault(r => r.AppUserId == user.Id);
                List<Message> messages = _context.Messages.Where(m => m.AppUserId == user.Id || m.ReciveUserId == user.Id).ToList();
                List<ProductCategory> productCategories = _context.ProductCategories.Where(pc => pc.StoreId == store.Id).ToList();
                List<Product> products = _context.Products.Where(pc => pc.StoreId == store.Id).ToList();
                List<Order> orders = _context.Orders.Where(pc => pc.StoreId == store.Id).ToList();
                List<OrderItems> orderItems = _context.OrderItems.Where(pc => pc.StoreId == store.Id).ToList();
                List<Favorite> favorites = _context.Favorites.Where(s => s.StoreId == store.Id).ToList();


                foreach (var item in favorites)
                {
                    _context.Favorites.Remove(item);
                }
                foreach (var item in productCategories)
                {
                    _context.ProductCategories.Remove(item);
                }
                foreach (var item in messages)
                {
                    _context.Messages.Remove(item);
                }
                foreach (var item in orders)
                {
                    _context.Orders.Remove(item);
                }
                foreach (var item in orderItems)
                {
                    _context.OrderItems.Remove(item);
                }
                foreach (var item in products)
                {
                    _context.Products.Remove(item);
                }
                _context.Stores.Remove(store);
            }
            _context.SaveChanges();
            await _userManager.DeleteAsync(user);
         
                   
            _context.SaveChanges();
            return Json(new {status=200 });
        }
    }
}
