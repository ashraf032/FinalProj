using Final_Project.Dal;
using Final_Project.Models;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Final_Project.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly WoltDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContext;
   

        public RestaurantController(WoltDbContext context,UserManager<AppUser> userManager,IHttpContextAccessor httpContext)
        {
            _context = context;
            _userManager = userManager;
            _httpContext = httpContext;
        }
        public IActionResult Index()
        {

            List<Restuorant> Restuorants = _context.Restuorants.Include(r=>r.Campaign).Where(r=>r.ResStatus).ToList();
            return View(Restuorants);
        }
        public async Task<IActionResult> Sort(bool isdelivery, bool isdicount)
        {
            List<Restuorant> restuorants = _context.Restuorants.Where(r=>(r.IsDeliveryFree == isdelivery && r.IsCampaign == isdicount&&r.ResStatus)||(r.IsDeliveryFree&&r.ResStatus)||(r.IsCampaign&&r.ResStatus)).ToList();
            return Json(restuorants);
        }

        public async Task<IActionResult> Details(int id)
        {
            AppUser user1=new AppUser();
            if (User.Identity.IsAuthenticated)
            {
            user1 = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            Restuorant restuorant = _context.Restuorants.Where(r=>r.ResStatus).FirstOrDefault(r => r.Id == id);
            if (restuorant==null)
            {
                return RedirectToAction("error","home");
            }
            BasketVM basketVM = new BasketVM
            {
                TotalPrice = 0,
                Count = 0,
                BasketItemVMs = new List<BasketItemVM>()
            };
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                List<BasketItem> basketItem = _context.BasketItems.Include(b=>b.Restuorant).ThenInclude(r=>r.Campaign).Where(b => b.AppUserId == user.Id && b.RestuorantId == id).ToList();

                foreach (BasketItem item in basketItem)
                {
                    Product product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product!=null)
                    {
                        BasketItemVM basketItemVM = new BasketItemVM
                        {
                            Count=item.Count,
                            Product=_context.Products.FirstOrDefault(p=>p.Id==item.ProductId),
                           
                        };
                        basketItemVM.Price = item.Restuorant.CampaignId == null ? product.Price : product.Price * (100 - item.Restuorant.Campaign.CampaignPercent) / 100;
                        basketVM.Count++;
                        basketVM.TotalPrice += basketItemVM.Price * basketItemVM.Count;
                        basketVM.BasketItemVMs.Add(basketItemVM);
                    }
                }
            }
            DetailsVM details = new DetailsVM
            {
                Restuorant = _context.Restuorants.Include(r => r.ProductCategories).Include(r=>r.Campaign).Include(r => r.Products).Include(r => r.BasketItems).FirstOrDefault(r => r.Id == id),
                BasketVM = basketVM,
                Favorite = _context.Favorites.FirstOrDefault(f=>f.RestuorantId==id&&f.AppUserId==user1.Id)
            };
            return View(details);
        }
        [HttpPost]
        public async Task<IActionResult> SearchProduct(int id, string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                List<Product> Products = _context.Products.Include(p => p.productCategory).Where(p => p.RestuorantId == id && p.Name.ToLower().Contains(content.ToLower())).ToList();
                return Json(Products);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddToBassket(int id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (User.Identity.IsAuthenticated && User.IsInRole("User"))
            {
                AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(b => b.ProductId == product.Id && b.AppUserId == appUser.Id && b.RestuorantId == product.RestuorantId);
                if (basketItem==null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = appUser.Id,
                        ProductId = product.Id,
                        RestuorantId = product.RestuorantId,
                        Count = 1
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }
                _context.SaveChanges();
            }
            
            return Json(new { status = 200 });
        }

        [HttpPost]
        public async Task<IActionResult> ShowBasket( int id) {
            
            BasketVM basketVM = new BasketVM
            {
                TotalPrice = 0,
                Count = 0,
                BasketItemVMs = new List<BasketItemVM>()
            };
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                Product productEx = _context.Products.FirstOrDefault(p => p.Id == id);

                List<BasketItem> basketItem = _context.BasketItems.Include(b=>b.Restuorant).ThenInclude(r=>r.Campaign).Where(b => b.AppUserId == user.Id && b.RestuorantId == productEx.RestuorantId).ToList();

                foreach (BasketItem item in basketItem)
                {
                    Product product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product != null)
                    {
                        BasketItemVM basketItemVM = new BasketItemVM
                        {
                            Count = item.Count,
                            Product = _context.Products.FirstOrDefault(p => p.Id ==            item.ProductId),

                        };
                        basketItemVM.Price = item.Restuorant.CampaignId==null?product.Price:product.Price*(100-item.Restuorant.Campaign.CampaignPercent)/100;
                        basketVM.Count++;
                        basketVM.TotalPrice += basketItemVM.Price * basketItemVM.Count;
                        basketVM.BasketItemVMs.Add(basketItemVM);
                    }
                }
            }
            return Json(basketVM);
        }

        [HttpPost]
        public async Task<IActionResult> Decrease(int id)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
         
            BasketItem basketItem = _context.BasketItems.FirstOrDefault(b => b.ProductId == id&&b.AppUserId==user.Id);
            if (basketItem.Count==1)
            {
                _context.BasketItems.Remove(basketItem);
            }
            else
            {
                basketItem.Count--;
            }
            _context.SaveChanges();
            return Json(new { status = 200 });
        }

        public IActionResult Error()
        {
            return View();
        }
       
    }
}
