using Final_Project.Dal;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly WoltDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public FavoriteController(WoltDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> AdFavoriteRes(int id)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Restuorant restuorant = _context.Restuorants.FirstOrDefault(r=>r.Id==id);
            Favorite favorite = new Favorite
            {
                RestuorantId=restuorant.Id,
                AppUserId=user.Id
            };
            _context.Favorites.Add(favorite);
            _context.SaveChanges();
            return Json(new {status=200 });
        }

        public async Task<IActionResult> RemoveFavoriteRes(int id)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Favorite favorite = _context.Favorites.FirstOrDefault(f=>f.RestuorantId==id&&f.AppUserId==user.Id);
           
            _context.Favorites.Remove(favorite);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }


        public async Task<IActionResult> AdFavoriteStor(int id)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Store store = _context.Stores.FirstOrDefault(r => r.Id == id);
            Favorite favorite = new Favorite
            {
                StoreId = store.Id,
                AppUserId = user.Id
            };
            _context.Favorites.Add(favorite);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }

        public async Task<IActionResult> RemoveFavoriteStor(int id)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            Favorite favorite = _context.Favorites.FirstOrDefault(f => f.StoreId == id && f.AppUserId == user.Id);

            _context.Favorites.Remove(favorite);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
