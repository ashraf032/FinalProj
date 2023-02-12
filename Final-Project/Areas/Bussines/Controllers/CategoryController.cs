using Final_Project.Dal;
using Final_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Areas.Bussines.Controllers
{
    [Area("bussines")]
    [Authorize(Roles = "Store, Restaurant")]
    public class CategoryController : Controller
    {
        private readonly WoltDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

     
        public CategoryController(WoltDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
        }
        public async Task<IActionResult> Index(int page=1)
        {
           
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user.Role=="Store")
            {
                Store store = _context.Stores.FirstOrDefault(r => r.AppUserId == user.Id);
                ViewBag.CurrentPage = page;
                ViewBag.TotalPage = Math.Ceiling((decimal)_context.ProductCategories.Where(c =>c.StoreId==store.Id).Count()/4);
                List<ProductCategory> productCategoriess = _context.ProductCategories.Where(pc => pc.StoreId == store.Id).Skip((page - 1) * 4).Take(4).ToList();
                return View(productCategoriess);
            }
            Restuorant restuorant = _context.Restuorants.FirstOrDefault(r => r.AppUserId == user.Id);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.ProductCategories.Where(c => c.RestuorantId == restuorant.Id).Count() / 4);
            List<ProductCategory> productCategories = _context.ProductCategories.Where(pc=>pc.RestuorantId==restuorant.Id).Skip((page - 1) * 4).Take(4).ToList();
            return View(productCategories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategory category)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user.Role=="Store")
            {
                Store store = _context.Stores.FirstOrDefault(r => r.AppUserId == user.Id);
                ProductCategory StoreCategory = new ProductCategory
                {
                    Name = category.Name,
                    StoreId = store.Id

                };
                _context.ProductCategories.Add(StoreCategory);
                
            }
            else
            {
            Restuorant restuorant = _context.Restuorants.FirstOrDefault(r => r.AppUserId == user.Id);
            ProductCategory NewCategory = new ProductCategory
            {
                Name = category.Name,
                RestuorantId = restuorant.Id
                
            };
            _context.ProductCategories.Add(NewCategory);

            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
                

        }

        public IActionResult Edit(int id)
        {
            ProductCategory category = _context.ProductCategories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(ProductCategory category)
        {
            ProductCategory ExsistCategory = _context.ProductCategories.FirstOrDefault(c => c.Id == category.Id);
            if (ExsistCategory == null) return NotFound();
            if (!ModelState.IsValid) return View(ExsistCategory);
            ExsistCategory.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            ProductCategory category = _context.ProductCategories.FirstOrDefault(pc => pc.Id == id);
            if (category == null) return Json(new { status = 404 });
            _context.ProductCategories.Remove(category);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
