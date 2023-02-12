using Final_Project.Models;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roloemanager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roloemanager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roloemanager = roloemanager;
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Index(int page = 1)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)_userManager.Users.Count() / 5);
            List<AppUser> admins = _userManager.Users.Where(u => u.Role == "Admin" || u.Role == "SuperAdmin").Skip((page - 1) * 5).Take(5).ToList();
            ViewBag.Roles = _roloemanager.Roles.ToList();
            return View(admins);
        }

        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserVM login)
        {
            if (login.UserName == null)
            {
                ModelState.AddModelError("", "name or pasword incorrect");
                return View();
            }
            if (login.Password == null)
            {
                ModelState.AddModelError("", "name or pasword incorrect");
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(login.UserName);
            if (!ModelState.IsValid) return View();
            if (user == null)
            {
                ModelState.AddModelError("", "username or pasword incorrect");
                return View();
            }

            if (!(user.Role == "Admin" || user.Role == "SuperAdmin"))
            {
                ModelState.AddModelError("", "Username or pasword incorrect");
                return View();
            }
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Username or pasword incorrect");
                    return View();
                }
            }



            return RedirectToAction("index", "setting");
        }

        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AdminCreate()
        {
            ViewBag.Roles = _roloemanager.Roles.ToList();

            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminCreate(CreateAdminVM register)
        {
            ViewBag.Roles = _roloemanager.Roles.ToList();
            if (!ModelState.IsValid) return View();
            AppUser appuser = new AppUser
            {
                FullName = register.FullName,
                Email = register.Email,
                UserName = register.UserName,
                Role = register.Role,
                PhoneNumber = register.PhoneNumber

            };

            IdentityResult result = await _userManager.CreateAsync(appuser, register.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }
            //await _userManager.CreateAsync(appuser, "Ahad1234");
            await _userManager.AddToRoleAsync(appuser, register.Role);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [Authorize(Roles = "Admin SuperAdmin")]
        public async Task<IActionResult> EditAdmin(string id)
        {
            AppUser Admins = await _userManager.FindByIdAsync(id);
            EditAdminVM editAdminVm = new EditAdminVM
            {
                UserName = Admins.UserName,
                FullName = Admins.FullName,
                Email = Admins.Email,
                Role = Admins.Role,
                PhoneNumber = Admins.PhoneNumber
            };
            ViewBag.Roles = _roloemanager.Roles.ToList();
            return View(editAdminVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Amin SuperAdmin")]
        public async Task<IActionResult> EditAdmin(string id, EditAdminVM adminvm)
        {
            ViewBag.Roles = _roloemanager.Roles.ToList();
            AppUser Admins = await _userManager.FindByIdAsync(id);
            EditAdminVM eadmin = new EditAdminVM
            {
                UserName = Admins.UserName,
                FullName = Admins.FullName,
                Email = Admins.Email,
                Role = Admins.Role,
                PhoneNumber = Admins.PhoneNumber

            };
            if (!ModelState.IsValid) return View(eadmin);
            if (adminvm.UserName == null)
            {
                ModelState.AddModelError("UserName", "required");
                return View(eadmin);
            }

            if (adminvm.FullName == null)
            {
                ModelState.AddModelError("FullName", "required");
                return View(eadmin);
            }

            if (adminvm.Email == null)
            {
                ModelState.AddModelError("Email", "required");
                return View(eadmin);
            }

            Admins.UserName = adminvm.UserName;
            Admins.FullName = adminvm.FullName;
            Admins.Email = adminvm.Email;
            Admins.Role = adminvm.Role;
            Admins.PhoneNumber = adminvm.PhoneNumber;
            await _userManager.UpdateAsync(Admins);


            if(!string.IsNullOrWhiteSpace(adminvm.CurrentPassword))
            {
                if (adminvm.Password==null)
                {
                    ModelState.AddModelError("", "Please Add Password");
                    return View(adminvm);
                }
                IdentityResult Result = await _userManager.ChangePasswordAsync(Admins, adminvm.CurrentPassword, adminvm.Password);
                if (!Result.Succeeded)
                {
                    foreach (var item in Result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);

                    }
                    return View(eadmin);
                }

            }


            return RedirectToAction("index", "account");
        }
    }
}
