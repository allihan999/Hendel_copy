using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Azure.Core.Pipeline;
using Hendel.DAL_copy.Models;

namespace Hendel_copy.Controllers
{
    public class AccountController : Controller
    {
        public readonly MainContext _mainContext;
        public int UserId;

        public AccountController(MainContext mainContext)
        {
            _mainContext = mainContext;
        }

        [HttpGet]
        public IActionResult Registr()
        {   
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registr(RegistrModels models)
        {
            User user = await _mainContext.Users.FirstOrDefaultAsync(u => u.Name == models.Name);
            if(user == null)
            {
                user = new User
                {
                    Name = models.Name,
                    Surname = models.Surname,
                    Email = models.Email,
                    Password = models.Password,
                    DoublePassword = models.DoublePassword,
                };
                user.Role = Role.Пользователь.ToString();


                _mainContext.Users.Add(user);
                await _mainContext.SaveChangesAsync();
                Content(User.Identity.Name);


                await Authenticate(user); // аутентификация

                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "Такой аккаунт уже существует!");

            return View();
        }

        [HttpGet]
        public IActionResult Input()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Input(LoginModels models) 
        {
            User user = await _mainContext.Users.FirstOrDefaultAsync(u => u.Name == models.Name && u.Password == models.Password);
            if(user != null)
            {
                await Authenticate(user); // аутентификация
                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "Ошибка в логине или пароль");

            return View(models);
        }

        public async Task<IActionResult> Account()
        {
            _mainContext.Users.UpdateRange();
            var UserAccount = _mainContext.Users.Where(x => x.Name == User.Identity.Name).ToList();
            return View(UserAccount);
        }

        public IActionResult MyKorzinaView()
        {
            var Id = _mainContext.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
            return View(_mainContext.MyKorzinas.Where(x => x.UserId == Id).Include(t => t.KorzinaWatches).Where(x => x.UserId == Id).ToList());
       
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };
            UserId = user.Id;
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        
    }
}
