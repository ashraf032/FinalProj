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
    public class SliderController : Controller
    {
        private readonly WoltDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(WoltDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider> Sliders = _context.Sliders.ToList();
            return View(Sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
         
            if (!ModelState.IsValid) return View();
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image is required");
                return View();
            }
            if(slider.ImageFile.FileName.Length>20)
            {
                ModelState.AddModelError("ImageFile", "The field SliderText must be a string with a maximum length of 20");
                return View();
            }
            if (!slider.ImageFile.IsImage())
            {
                ModelState.AddModelError("ImageFile", "The file must be an image");
                return View();
            }
            if (!slider.ImageFile.IsSizeOk(2))
            {
                ModelState.AddModelError("ImageFile", "The file must be max size 2mb");
                return View();
            }
            Slider NewSlider = new Slider();
            NewSlider.Image = slider.ImageFile.SaveImage(_env.WebRootPath, "assets/image");
            NewSlider.SliderText = slider.SliderText;
            _context.Sliders.Add(NewSlider);
            _context.SaveChanges();
           return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            Slider ExsistSlider = _context.Sliders.FirstOrDefault(es=>es.Id==slider.Id);
            if (!ModelState.IsValid) return View(ExsistSlider);
            if (slider.ImageFile != null)
            {
                if (slider.ImageFile.FileName.Length > 20)
                {
                    ModelState.AddModelError("ImageFile", "The field SliderText must be a string with a maximum length of 20");
                    return View();
                }
                if (!slider.ImageFile.IsImage())
                {
                    ModelState.AddModelError("ImageFile", "The file must be an image");
                    return View();
                }
                if (!slider.ImageFile.IsSizeOk(2))
                {
                    ModelState.AddModelError("ImageFile", "The file must be max size 2mb");
                    return View();
                }
                Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/image", ExsistSlider.Image);
                ExsistSlider.Image = slider.ImageFile.SaveImage(_env.WebRootPath, "assets/image");
            }
            ExsistSlider.SliderText = slider.SliderText;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return Json( new {status=404 });
            Helpers.Helper.DeleteImg(_env.WebRootPath, "assets/image", slider.Image);
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            return Json(new { status = 200 });
        }
    }
}
