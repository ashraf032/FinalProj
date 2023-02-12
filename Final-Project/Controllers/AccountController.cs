using Final_Project.Models;
using Final_Project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Final_Project.Controllers
{
 
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserVM register)
        {
            AppUser user = new AppUser
            {
                FullName=register.FullName,
                UserName=register.UserName,
                Email=register.Email,
                PhoneNumber=register.PhoneNumber,
                Role="User"
                
            };
            IdentityResult result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return Json(new { status = result.Errors });
            }
            await _userManager.AddToRoleAsync(user,"User");
            await _signInManager.SignInAsync(user, true);

            return Json(new {status=200 });  
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserVM login)
        {
            if (login.UserName == null || login.Password == null) return Json(new { status = 404 });
            AppUser user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null) return Json(new { status = 404 });
            if (user.Role != "User") return Json(new { status = 404 });
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    return Json("your account is blocked 5 minute");
                }
                else
                {
                    return Json(new { status = 404 });
                }
            }
            return Json(new { status = 200 });
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index","home");
        }
        public async Task<IActionResult> EditUser()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            EditUserVM editUser = new EditUserVM
            {
                FullName = user.FullName,
                UserName=user.UserName,
                Email=user.Email,
                PhoneNumber=user.PhoneNumber
                
            };
            return View(editUser);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserVM editUser)
        {
            AppUser ExsistUser = await _userManager.FindByNameAsync(User.Identity.Name);
            EditUserVM EditUser = new EditUserVM
            {
                FullName = ExsistUser.FullName,
                UserName = ExsistUser.UserName,
                Email = ExsistUser.Email,
                PhoneNumber = ExsistUser.PhoneNumber

            };
            if(ExsistUser.UserName!=editUser.UserName&& await _userManager.FindByNameAsync(editUser.UserName) != null)
            {
                ModelState.AddModelError("", $"The {editUser.UserName} is alrady taken");
                return View(editUser);
            }
            if (!ModelState.IsValid) return View(EditUser);
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
            return RedirectToAction(nameof(EditUser));
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPasword(string email)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Json(new { status=404});
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string Link = Url.Action(nameof(ResetPasword), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("qesref87@gmail.com", "Wolt");
            mail.To.Add(user.Email);
            mail.Subject = "Reset Password";
            mail.Body = $"<a href='{Link}'>Plaese click here for reset password<a/>";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("qesref87@gmail.com", "kxlkzdxffqavsion");
            smtp.Send(mail);
            return Json(new {status=200 });
        }

        public async Task<IActionResult> ResetPasword(string email, string token)
        {
            AppUser appuser = await _userManager.FindByEmailAsync(email);
            if (appuser == null) return BadRequest();
            AccountVM account = new AccountVM
            {
                Appuser = appuser,
                Token = token
            };
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasword(AccountVM account)
        {
            AppUser user = await _userManager.FindByEmailAsync(account.Appuser.Email);
            AccountVM model = new AccountVM
            {
                Appuser = account.Appuser,
                Token = account.Token
            };
            if (!ModelState.IsValid) return View(model);
            IdentityResult Result = await _userManager.ResetPasswordAsync(user, account.Token, account.Password);
            if (!Result.Succeeded)
            {
                foreach (var item in Result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(model);
            }
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("index", "home");
        }
    }
}
