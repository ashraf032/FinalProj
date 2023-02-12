using Final_Project.Areas.Bussines.ViewModels;
using Final_Project.Areas.Extensions;
using Final_Project.Dal;
using Final_Project.Models;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class BussinesAccountController : Controller
    {
        private readonly WoltDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _env;

        public BussinesAccountController(WoltDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _env = env;
        }


        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserVM loginUser)
        {
            if (!ModelState.IsValid) return View();
            if (loginUser.UserName == null || loginUser.Password == null)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(loginUser.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, loginUser.Password, true, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }
            if (user.Role == "Store")
            {
                if (user.LoginStatus)
                {
                return RedirectToAction(nameof(StoreSettingEdit));

                }
                else
                {
                    return RedirectToAction("StoreAcceptChat", "chat");
                }
            }
            if (user.Role == "Courier")
            {

                return RedirectToAction("index","courier");
            }
            if (user.LoginStatus)
            {
                return RedirectToAction(nameof(RestuorantMain));

            }
            else
            {
                return RedirectToAction("RestaurantAcceptChat", "chat");
            }
            
        }
        public async Task<IActionResult> Logout()
        {
          await  _signInManager.SignOutAsync();
          return  RedirectToAction(nameof(Login));
        }
        public IActionResult CreateRestuorant()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRestuorant(RestuorantRegisterVM restuorantInfo)
        {
            ViewBag.Categories = _context.Categories.ToList();
            if (!ModelState.IsValid) return View();
            if (restuorantInfo.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The field image is required");
                return View();
            }
            if (restuorantInfo.CategoryIds == null)
            {
                ModelState.AddModelError("CategoryIds", "can not be null");
                return View();
            }
            if (!restuorantInfo.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "image file must be");
                return View();
            }
            if (!restuorantInfo.ImageFile.IsSizeOk(2))
            {
                ModelState.AddModelError("ImageFile", "The field image max size 2mb");
                return View();
            }
            AppUser appUser = new AppUser
            {
                FullName = restuorantInfo.FullName,
                UserName = restuorantInfo.UserName,
                PhoneNumber = restuorantInfo.PhoneNumber,
                Email = restuorantInfo.Email,
                Role = "Restaurant"
            };
            var Result = await _userManager.CreateAsync(appUser, restuorantInfo.Password);
            if (!Result.Succeeded)
            {
                foreach (IdentityError error in Result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            Restuorant restuorant = new Restuorant();
            restuorant.Restuorant_Categories = new List<Restuorant_Category>();
            restuorant.Name = restuorantInfo.RestuorantName;
            restuorant.Adress = restuorantInfo.Adress;
            restuorant.Title = restuorantInfo.Title;
            restuorant.Description = restuorantInfo.Description;
            restuorant.WorkTime = restuorantInfo.WorkTime;
            restuorant.PhoneNumber = restuorantInfo.PhoneNumberRestuorant;
            restuorant.Image = restuorantInfo.ImageFile.SaveImage(_env.WebRootPath, "assets/image");
            AppUser User = await _userManager.FindByNameAsync(appUser.UserName);
            restuorant.AppUserId = User.Id;
            foreach (var id in restuorantInfo.CategoryIds)
            {
                Restuorant_Category restuorant_Category = new Restuorant_Category
                {
                    Restuorant = restuorant,
                    CategoryId = id
                };
                restuorant.Restuorant_Categories.Add(restuorant_Category);
            }
            _context.Restuorants.Add(restuorant);
            _context.SaveChanges();
            Restuorant CreatedRestuorant = _context.Restuorants.FirstOrDefault(cr => cr.AppUserId == User.Id);
            User.RestuorantId = CreatedRestuorant.Id;
            await _userManager.AddToRoleAsync(User, "Restaurant");
            await _userManager.UpdateAsync(User);
           await _signInManager.SignInAsync(User, true);
            return RedirectToAction("RestaurantAcceptChat", "chat");
        }
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> RestuorantMain(int page=1)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Restuorant restuorant = _context.Restuorants.FirstOrDefault(c => c.AppUserId == user.Id);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Orders.Where(r=>r.RestuorantId==restuorant.Id&&r.IsOrderComlete).Count()/5);
            BussinesOrderVM bussinesOrderVM = new BussinesOrderVM
            {
                Orders= _context.Orders.Where(r => r.RestuorantId == restuorant.Id && r.IsOrderComlete).Skip((page - 1) * 5).Take(5).Include(o => o.AppUser).ToList(),
                OrdersTotal=_context.Orders.Where(o=>o.RestuorantId==restuorant.Id&&o.IsOrderComlete).ToList()
        };
            return View(bussinesOrderVM);
        }
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> RestuorantSettingEdit()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Campaigns = _context.Campaigns.ToList();
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Restuorant restuorant = _context.Restuorants.Include(r => r.Restuorant_Categories).FirstOrDefault(c => c.AppUserId == user.Id);
            if (restuorant==null)
            {
                return RedirectToAction("error", "home");
            }
            RestuorantEditVm restuorantEdit = new RestuorantEditVm
            {
                AppUser = user,
                restuorant = restuorant
            };
            return View(restuorantEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> RestuorantSettingEdit(RestuorantEditVm editVm)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Campaigns = _context.Campaigns.ToList();
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Restuorant restuorant = _context.Restuorants.Include(r => r.Restuorant_Categories).FirstOrDefault(c => c.AppUserId == user.Id);
            if (restuorant == null)
            {
                return RedirectToAction("error", "home");
            }
            RestuorantEditVm restuorantEdit = new RestuorantEditVm
            {
                AppUser = user,
                restuorant = restuorant
            };
            if (editVm.AppUser.UserName == null)
            {
                ModelState.AddModelError("Appuser.UserName", "UserName is required");
                return View(restuorantEdit);
            }
            if (editVm.AppUser.FullName == null)
            {
                ModelState.AddModelError("Appuser.FullName", "Fullname is required");
                return View(restuorantEdit);
            }
            if (editVm.AppUser.Email == null)
            {
                ModelState.AddModelError("Appuser.Email", "Email is required");
                return View(restuorantEdit);
            }
            if (editVm.AppUser.PhoneNumber == null)
            {
                ModelState.AddModelError("Appuser.PhoneNumber", "PhoneNumber is required");
                return View(restuorantEdit);
            }
          
            if (!ModelState.IsValid) return View(restuorantEdit);
            user.UserName = editVm.AppUser.UserName;
            user.FullName = editVm.AppUser.FullName;
            user.Email = editVm.AppUser.Email;
            user.PhoneNumber = editVm.AppUser.PhoneNumber;
            user.Email = editVm.AppUser.Email;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, true);
            restuorant.Name = editVm.restuorant.Name;
            restuorant.Title = editVm.restuorant.Title;
            restuorant.IsDeliveryFree = editVm.restuorant.IsDeliveryFree;
            restuorant.CampaignId = editVm.restuorant.CampaignId;
            restuorant.IsCampaign = editVm.restuorant.CampaignId == null ? false : true;
            restuorant.Description = editVm.restuorant.Description;
            restuorant.PhoneNumber = editVm.restuorant.PhoneNumber;
            restuorant.Adress = editVm.restuorant.Adress;
            restuorant.WorkTime = editVm.restuorant.WorkTime;
            if (editVm.CategoryIds != null)
            {
                List<Restuorant_Category> RemoveAbleCategories = restuorant.Restuorant_Categories.Where(rc => !editVm.CategoryIds.Contains(rc.CategoryId)).ToList();
                restuorant.Restuorant_Categories.RemoveAll(rc => RemoveAbleCategories.Any(rmc => rmc.Id == rc.Id));
                foreach (var categoryId in editVm.CategoryIds)
                {
                    Restuorant_Category restuorant_Category = restuorant.Restuorant_Categories.FirstOrDefault(rc => rc.CategoryId == categoryId);
                    if (restuorant_Category == null)
                    {
                        Restuorant_Category NewRestuorantCategory = new Restuorant_Category
                        {
                            RestuorantId = restuorant.Id,
                            CategoryId = categoryId
                        };
                        restuorant.Restuorant_Categories.Add(NewRestuorantCategory);
                    }
                }
            }
            if (editVm.ImageFile != null)
            {
                if (!editVm.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "image file is required");
                    return View(restuorantEdit);
                }
                if (!editVm.ImageFile.IsSizeOk(2))
                {
                    ModelState.AddModelError("ImageFile", "image file imax size  must be 2mb");
                    return View(restuorantEdit);
                }

                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/image", restuorant.Image);
                restuorant.Image = editVm.ImageFile.SaveImage(_env.WebRootPath, "assets/image");
            }
            _context.SaveChanges();
            TempData["message"] = "Data saved";
            return RedirectToAction(nameof(RestuorantSettingEdit));
        }

        [Authorize(Roles = "Restaurant,Store")]
        public async Task<IActionResult> Order()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<Order> orders = _context.Orders.Include(o=>o.AppUser).Include(o=>o.OrderItems).ThenInclude(o=>o.Product).Where(o=>o.IsCourierTaked==false&&o.RestuorantId==user.RestuorantId&&o.IsOrderComlete==false).ToList();
            return View(orders);
        }
        [HttpPost]
        public IActionResult ShowOrders(int orderid)
        {
            Order order = _context.Orders.Include(o=>o.Restuorant).Include(o=>o.AppUser).Include(o=>o.Store).Include(o => o.OrderItems).ThenInclude(oi=>oi.Product).FirstOrDefault(o => o.Id==orderid);
            //if (orderItems == null) return NotFound();
            return Json(order);
            
        }
        public IActionResult CreateStore()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStore(StoreRegisterVM storeRegister)
        {

            if (!ModelState.IsValid) return View();
            if (storeRegister.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The field image is required");
                return View();
            }
            if (!storeRegister.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "image file must be");
                return View();
            }
            if (!storeRegister.ImageFile.IsSizeOk(2))
            {
                ModelState.AddModelError("ImageFile", "The field image max size 2mb");
                return View();
            }
            AppUser appUser = new AppUser
            {
                FullName = storeRegister.FullName,
                UserName = storeRegister.UserName,
                PhoneNumber = storeRegister.PhoneNumber,
                Email = storeRegister.Email,
                Role = "Store"
            };
            var Result = await _userManager.CreateAsync(appUser, storeRegister.Password);
            if (!Result.Succeeded)
            {
                foreach (IdentityError error in Result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            Store store = new Store();
            store.Name = storeRegister.StoreName;
            store.Adress = storeRegister.Adress;
            store.Title = storeRegister.Title;
            store.Description = storeRegister.Description;
            store.WorkTime = storeRegister.WorkTime;
            store.PhoneNumber = storeRegister.PhoneNumberRestuorant;
            store.Image = storeRegister.ImageFile.SaveImage(_env.WebRootPath, "assets/image");
            AppUser User = await _userManager.FindByNameAsync(appUser.UserName);
            store.AppUserId = User.Id;

            _context.Stores.Add(store);
            _context.SaveChanges();
            Store CreatedStore = _context.Stores.FirstOrDefault(cr => cr.AppUserId == User.Id);
            User.StoreId = CreatedStore.Id;
            await _userManager.AddToRoleAsync(User, "Store");
            await _userManager.UpdateAsync(User);
            await _signInManager.SignInAsync(User, true);
            return RedirectToAction("StoreAcceptChat", "chat");
        }

        [Authorize(Roles = "Store")]
        public async Task<IActionResult> StoreMain(int page=1)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Store store = _context.Stores.FirstOrDefault(c => c.AppUserId == user.Id);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)_context.Orders.Where(o=>o.StoreId==store.Id&&o.IsOrderComlete).Count()/5);
            
            BussinesOrderVM bussinesOrderVM = new BussinesOrderVM
            {
                Orders = _context.Orders.Where(o => o.StoreId == store.Id && o.IsOrderComlete).Skip((page - 1) * 4).Take(5).Include(o => o.AppUser).ToList(),
                OrdersTotal=_context.Orders.Where(o=>o.StoreId==store.Id&&o.IsOrderComlete).ToList()
        };
            return View(bussinesOrderVM);

        }

        [Authorize(Roles = "Store")]
        public async Task<IActionResult> StoreSettingEdit()
        {
           
            ViewBag.Campaigns = _context.Campaigns.ToList();
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Store store = _context.Stores.FirstOrDefault(c => c.AppUserId == user.Id);
            StoreEditVM storeEdit = new StoreEditVM
            {
                AppUser = user,
                Store = store
            };
            return View(storeEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Store")]
        public async Task<IActionResult> StoreSettingEdit(StoreEditVM storeEditVM)
        {
            ViewBag.Campaigns = _context.Campaigns.ToList();
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Store store = _context.Stores.FirstOrDefault(c => c.AppUserId == user.Id);
            StoreEditVM storeEdit = new StoreEditVM
            {
                AppUser = user,
                Store = store
            };
            if (storeEditVM.AppUser.UserName==null)
            {
                ModelState.AddModelError("Appuser.UserName", "UserName is required");
                return View(storeEdit);
            }
            if (storeEditVM.AppUser.FullName == null)
            {
                ModelState.AddModelError("Appuser.FullName", "Fullname is required");
                return View(storeEdit);
            }
            if (storeEditVM.AppUser.Email == null)
            {
                ModelState.AddModelError("Appuser.Email", "Email is required");
                return View(storeEdit);
            }
            if (storeEditVM.AppUser.PhoneNumber == null)
            {
                ModelState.AddModelError("Appuser.PhoneNumber", "PhoneNumber is required");
                return View(storeEdit);
            }
            if (!ModelState.IsValid) return View(storeEdit);
            user.UserName = storeEditVM.AppUser.UserName;
            user.FullName = storeEditVM.AppUser.FullName;
            user.Email = storeEditVM.AppUser.Email;
            user.PhoneNumber = storeEditVM.AppUser.PhoneNumber;
            user.Email = storeEditVM.AppUser.Email;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, true);
            store.Name = storeEditVM.Store.Name;
            store.Title = storeEditVM.Store.Title;
            store.IsDeliveryFree = storeEditVM.Store.IsDeliveryFree;
            store.CampaignId = storeEditVM.Store.CampaignId;
            store.IsCampaign = storeEditVM.Store.CampaignId == null ? false : true;
            store.Description = storeEditVM.Store.Description;
            store.PhoneNumber = storeEditVM.Store.PhoneNumber;
            store.Adress = storeEditVM.Store.Adress;
            store.WorkTime = storeEditVM.Store.WorkTime;
          
            if (storeEditVM.ImageFile != null)
            {
                if (!storeEditVM.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "image file is required");
                    return View(storeEdit);
                }
                if (!storeEditVM.ImageFile.IsSizeOk(2))
                {
                    ModelState.AddModelError("ImageFile", "image file imax size  must be 2mb");
                    return View(storeEdit);
                }

                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/image", store.Image);
                store.Image = storeEditVM.ImageFile.SaveImage(_env.WebRootPath, "assets/image");
            }
            _context.SaveChanges();
            TempData["message"] = "Data saved";
            return RedirectToAction(nameof(StoreSettingEdit));
        }

    }
}
