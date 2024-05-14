using Hendel_copy.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace Hendel_copy.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> logger;
        public readonly MainContext _mainContext;
        public int UserId;

        public AccountController(MainContext mainContext, ILogger<HomeController> logger)
        {
            _mainContext = mainContext;
            this.logger = logger;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckEmail(string email)
        {
            var user = _mainContext.Users.Where(x => x.Name.Length > 0).ToList();
            foreach (var item in user)
            {
                if (email == item.Email)
                    return Json(false);
            }
            return Json(true);
        }

        public int Result;

        public IActionResult Revolted()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Revolted(RevoltedViewModels models)
        {
            var revoltedViewModels = _mainContext.RevoltedViews.FirstOrDefault(x => x.Email.Length > 0);
            if (revoltedViewModels != null)
            {
                _mainContext.RevoltedViews.Remove(revoltedViewModels);
            }

            var user = _mainContext.Users.FirstOrDefault(x => x.Email == models.Email);
            if (user != null)
            {
                List<RevoltedViewModels> RevoltedViewModels = new List<RevoltedViewModels>();
                RevoltedView revoltedViews = new RevoltedView();

                int a = 0;
                int b = 0;

                RevoltedViewModels.Clear();
                if (models != null)
                {
                    int _min = 1000;
                    int _max = 9999;
                    Random _rdm = new Random();
                    int result = _rdm.Next(_min, _max);
                    models.PSW = result;
                    revoltedViews.Email = models.Email;
                    revoltedViews.PSW = models.PSW;
                    revoltedViews.RevoltedPassword = 0;
                    revoltedViews.NewPassword = Convert.ToString(a);
                    revoltedViews.ConfirmPassword = Convert.ToString(b);


                    _mainContext.RevoltedViews.Add(revoltedViews);
                    await _mainContext.SaveChangesAsync();

                    //-----------------Уведомление_на_почту-----------------
                    try
                    {
                        MailMessage message = new MailMessage();
                        message.IsBodyHtml = true;
                        message.From = new MailAddress("alikhan.iskhadzhiyev@bk.ru", "Hendel");
                        message.To.Add($"{models.Email}");
                        message.Subject = "Пароль для восстановления";
                        message.Body = "<div>Пароль для восстановления<div>" +
                                       $"<div>{result}<div>";

                        //message.Attachments.Add(new Attachment("путь к файлу"));

                        using (SmtpClient client = new SmtpClient("smpt.mail.ru"))
                        {
                            client.Credentials = new NetworkCredential("alikhan.iskhadzhiyev@bk.ru", "YxuTJTfqXSBten7UNRaM\r\n");
                            client.Port = 587;
                            client.EnableSsl = true;
                            client.Send(message);

                            logger.LogInformation("Сообщение отправлено успешно!");
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e.GetBaseException().Message);
                    }
                    return RedirectToAction("PageRevol", "Account");
                }
            }
            else
            {
                ModelState.AddModelError("", "Такого аккаунта не существует!");
            }

            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckPassword(int revoltedPassword)
        {
            var revoltedViewModels = _mainContext.RevoltedViews.Where(x => x.Email.Length > 0);
            foreach (var item in revoltedViewModels)
            {
                if (revoltedPassword != item.PSW)
                    return Json(false);
            }
            return Json(true);
        }

        public IActionResult PageRevol()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PageRevol(RevoltedViewModels models)
        {
            //Вытащить из models пароли и поместить в revolm
            var revoltedViewModels = _mainContext.RevoltedViews.FirstOrDefault(x => x.Email.Length > 0);
            if (revoltedViewModels != null)
            {
                _mainContext.RevoltedViews.Remove(revoltedViewModels);
            }

            revoltedViewModels.NewPassword = models.NewPassword;
            revoltedViewModels.ConfirmPassword = models.ConfirmPassword;
            _mainContext.RevoltedViews.Update(revoltedViewModels);

            var user = _mainContext.Users.FirstOrDefault(x => x.Email == revoltedViewModels.Email);

            user.Password = models.ConfirmPassword.ToString();

            _mainContext.Users.Update(user);
            _mainContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }



        public IActionResult BuyProduct()
        {
            var user = _mainContext.Users.FirstOrDefault(x => x.Name == User.Identity.Name);
            return View(_mainContext.BuyProductsTable.Where(x => x.UserId == user.Id).ToList());
        }

        //Дописать код
        public async Task<IActionResult> DeleteProductZakaz(int id)
        {
            BuyProducts buyProductsss = _mainContext.BuyProductsTable.FirstOrDefault(x => x.Id == id);
            if (buyProductsss != null)
            {
                _mainContext.BuyProductsTable.Remove(buyProductsss);
                await _mainContext.SaveChangesAsync();
                return RedirectToAction("Catalog", "Catalog");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Registr()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registr([FromForm] RegistrModelsProject models)
        {

            User user = await _mainContext.Users.FirstOrDefaultAsync(u => u.Email == models.Email);
            if (user == null)
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
                user.UserNumberOrder = 0;

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
        public async Task<IActionResult> Input(LoginModelsClass models)
        {

            User user = await _mainContext.Users.FirstOrDefaultAsync(u => u.Name == models.Name && u.Password == models.Password);
            if (user != null)
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
            var result = _mainContext.MyKorzinas.Where(x => x.UserId == Id).Include(t => t.KorzinaWatches).Where(x => x.UserId == Id).ToList();

            foreach (var item in result)
            {
                if (item.UserNumberOrder < 1)
                {
                    return View();
                }
            }
            return View(result.OrderByDescending(x => x.UserNumberOrder));

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
