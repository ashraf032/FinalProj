using Final_Project.Areas.Extensions;
using Final_Project.Dal;
using Final_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class SettingController : Controller
    {
        private readonly WoltDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingController(WoltDbContext context, IWebHostEnvironment env )
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            Setting setting = _context.Setting.FirstOrDefault();
            return View(setting);
        }

        public IActionResult Edit()
        {
            Setting setting = _context.Setting.FirstOrDefault();
            return View(setting);
        }
        [HttpPost]
        public IActionResult Edit(Setting setting)
        {
            Setting ExistSetting = _context.Setting.FirstOrDefault();
            if (!ModelState.IsValid) return View(ExistSetting);
            if (setting.LogoFile != null)
            {
                if (!setting.LogoFile.IsImage())
                {
                    ModelState.AddModelError("LogoFile", "The file must be an image");
                    return View();
                }
                if (!setting.LogoFile.IsSizeOk(2))
                {
                    ModelState.AddModelError("LogoFile", "The file must be max size 2mb");
                    return View();
                }

                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/image", ExistSetting.Logo);
                ExistSetting.Logo = setting.LogoFile.SaveImage(_env.WebRootPath, "assets/image");
            }

            ExistSetting.InstagramLink = setting.InstagramLink;
            ExistSetting.FacebbokLink = setting.FacebbokLink;
            ExistSetting.TwitterLink = setting.TwitterLink;
            ExistSetting.LInkedinLink = setting.LInkedinLink;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
