using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Reflection.Metadata.BlobBuilder;
using System.Drawing;
using Microsoft.CodeAnalysis;
using System.Linq;
using Hendel_copy.Models;
using Azure.Messaging;
using Hendel.DAL_copy.Models;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace Hendel_copy.Controllers
{
    public class CatalogController : Controller
    {
        public readonly MainContext _mainContext;
        private readonly ILogger<HomeController> logger;

        public CatalogController(MainContext mainContext, ILogger<HomeController> logger)
        {
            _mainContext = mainContext;
            this.logger = logger;
        }

        public IActionResult History()
        {
            return View();
        }
        public IActionResult Catalog()
        {
            var news = _mainContext.Catalogs.Where(x => x.WhichCatalog == "Мужские").ToList();
            return View(news);
        }

        public IActionResult CatalogWoman()
        {
            var result = _mainContext.Catalogs.Where(x => x.WhichCatalog == "Женские").ToList();

            foreach (var item in result)
            {
                if (item.WhichCatalog == null)
                {
                    result = _mainContext.Catalogs.OrderByDescending(x => x.Name).ToList();
                }
            }
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CatalogViewModels news)
        {
            CatalogViewModels models = new CatalogViewModels();
            List<CatalogViewModels> list = new List<CatalogViewModels>();
            list.Clear();
            list.Add(news);

            foreach (var item in list.ToList())
            {
                Catalog n = new Catalog { Name = news.Name, Description = news.Description, Amount = news.Amount, Price = news.Price, WhichCatalog = news.WhichCatalog };
                
                if (item.Image == null)
                {
                    ModelState.AddModelError("", "Выберите картинку (.png, .jpg, .jpeg)");
                }
                else if (item.Image.FileName.Contains("png") == false && item.Image.FileName.Contains("jpg") == false && item.Image.FileName.Contains("jpeg") == false)
                {
                    list.Clear();
                    ModelState.AddModelError("", "Выберите картинку (.png, .jpg, .jpeg)");                  
                }
                else
                {
                    if (news.Image != null)
                    {
                        byte[] imageData = null;
                        // считываем переданный файл в массив байтов
                        using (var binaryReader = new BinaryReader(news.Image.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)news.Image.Length);
                        }
                        n.Image = imageData;
                    }

                    _mainContext.Catalogs.Add(n);
                    await _mainContext.SaveChangesAsync();

                    if (n.WhichCatalog == "Женские")
                    {
                        list.Clear();
                        return RedirectToAction("CatalogWoman", "Catalog");
                    }
                    list.Clear();
                    return RedirectToAction("Catalog");
                }
            }
            return View();

        }

        public IActionResult FavoritePage()
        {
            var Id = _mainContext.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
            return View(_mainContext.Favorites.Where(x => x.UserId == Id).ToList());
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        public async Task<IActionResult> AddFavorite(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id != 0)
                {
                    var result = _mainContext.Favorites.FirstOrDefault(k => (k.ProductId == id && k.UserId == _mainContext.Users.FirstOrDefault(k => k.Name == User.Identity.Name).Id));
                    if (result == null)
                    {
                        Favorite favorite = new Favorite();
                        favorite.ProductId = _mainContext.Catalogs.Find(id).Id;
                        favorite.UserId = _mainContext.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
                        favorite.Image = _mainContext.Catalogs.Find(id).Image;
                        favorite.Name = _mainContext.Catalogs.Find(id).Name;
                        favorite.Description = _mainContext.Catalogs.Find(id).Description;
                        favorite.Amount = _mainContext.Catalogs.Find(id).Amount;
                        favorite.Price = _mainContext.Catalogs.Find(id).Price;
                        favorite.IsModelWindow = false;

                        _mainContext.Favorites.Add(favorite);
                        await _mainContext.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Catalog");
            }
            else
                return RedirectToAction("Registr", "Account");
        }

        public IActionResult ArchiveWatch()
        {
            if (User.Identity.IsAuthenticated)
            {
                var Id = _mainContext.AdminInputClasses.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
                return View(_mainContext.Archives.Where(x => x.AdminInputClassId == Id).ToList());
            }
            else
                return RedirectToAction("Input", "Account");
        }

        public async Task<IActionResult> AddArchive(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id != 0)
                {
                    var result = _mainContext.Archives.FirstOrDefault(k => k.ProductId == id);
                    if (result == null)
                    {
                        Archive archive = new Archive
                        {
                            ProductId = _mainContext.Catalogs.Find(id).Id,
                            AdminInputClassId = _mainContext.AdminInputClasses.FirstOrDefault(x => x.Name == User.Identity.Name).Id,
                            Image = _mainContext.Catalogs.Find(id).Image,
                            Name = _mainContext.Catalogs.Find(id).Name,
                            Description = _mainContext.Catalogs.Find(id).Description,
                            Amount = _mainContext.Catalogs.Find(id).Amount,
                            Price = _mainContext.Catalogs.Find(id).Price,
                            WatchCategory = _mainContext.Catalogs.Find(id).WhichCatalog
                        };

                        _mainContext.Archives.Add(archive);
                        await _mainContext.SaveChangesAsync();
                    }
                }

                if (id != null)
                {
                    Catalog catalog = await _mainContext.Catalogs.FindAsync(id);
                    if (catalog != null)
                    {
                        _mainContext.Catalogs.Remove(catalog);
                        await _mainContext.SaveChangesAsync();
                        return RedirectToAction("Catalog", "Catalog");
                    }
                    return RedirectToAction("Error");
                }
                else
                    return RedirectToAction("Catalog");
            }
            else
                return RedirectToAction("Input", "Account");
        }

        public async Task<IActionResult> CreateAddingFromTheArchive(int? id)
        {
            if (id != null)
            {
                Catalog catalog = new Catalog
                {
                    Name = _mainContext.Archives.Find(id).Name,
                    Image = _mainContext.Archives.Find(id).Image,
                    Description = _mainContext.Archives.Find(id).Description,
                    Price = _mainContext.Archives.Find(id).Price,
                    Amount = 5,
                    WhichCatalog = _mainContext.Archives.Find(id).WatchCategory
                };

                _mainContext.Catalogs.Add(catalog);
                await _mainContext.SaveChangesAsync();

                if (id != null)
                {
                    Archive archive = await _mainContext.Archives.FindAsync(id);
                    if (archive != null)
                    {
                        _mainContext.Archives.Remove(archive);
                        await _mainContext.SaveChangesAsync();
                        return RedirectToAction("Catalog", "Catalog");
                    }
                    return RedirectToAction("Error");
                }

                return RedirectToAction("Catalog");
            }
            return View();
        }
        public async Task<IActionResult> DeleteWatchsCatalog(int? id)
        {
            if (id != null)
            {
                Catalog catalog = await _mainContext.Catalogs.FindAsync(id);
                if (catalog != null)
                {
                    _mainContext.Catalogs.Remove(catalog);
                    await _mainContext.SaveChangesAsync();
                    return RedirectToAction("Catalog", "Catalog");
                }
                return RedirectToAction("Error");
            }
            else
                return RedirectToAction("Error");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                CatalogViewModels catalogViewModels = new CatalogViewModels();
                if (id != null)
                {
                    Catalog catalog = await _mainContext.Catalogs.FindAsync(id);
                    if (catalog != null)
                    {
                        WatchEditViewModels models = new WatchEditViewModels();
                        models.Id = catalog.Id;
                        models.Name = catalog.Name;
                        models.Description = catalog.Description;
                        models.Price = catalog.Price;
                        models.Amount = catalog.Amount;
                        models.Image = catalogViewModels.Image;
                        models.WhichCatalog = catalog.WhichCatalog;
                        return View(models);
                    }
                }
                else
                    return RedirectToAction("Error");
            }
            else
                return RedirectToAction("Input", "Account");

            return View();
        }

        public async Task<IActionResult> AddCatalog_Is_a_Archive(int? id)
        {
            CatalogViewModels catalogViewModels = new CatalogViewModels();
            if (id != null)
            {
                Archive archive = await _mainContext.Archives.FindAsync(id);
                if (archive != null)
                {
                    WatchEdit_Archive_View_Models models = new WatchEdit_Archive_View_Models();
                    models.Id = archive.Id;

                    models.ProductId = archive.ProductId;
                    models.AdminInputClassId = archive.AdminInputClassId;

                    models.Name = archive.Name;
                    models.Image = catalogViewModels.Image;
                    models.Description = archive.Description;
                    models.Price = archive.Price;
                    models.Amount = archive.Amount;
                    models.WatchCategory = archive.WatchCategory;
                    return View(models);
                }
                else
                    return RedirectToAction("Error");
            }
            else
                return RedirectToAction("Error");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(WatchEditViewModels models)
        {
            Catalog catalog = await _mainContext.Catalogs.FindAsync(models.Id);

            catalog.Name = models.Name;
            catalog.Description = models.Description;
            catalog.Price = models.Price;
            catalog.Amount = models.Amount;
            catalog.WhichCatalog = models.WhichCatalog;

            if (models.Image != null)
            {
                using (var binaryReader = new BinaryReader(models.Image.OpenReadStream()))
                {
                    catalog.Image = binaryReader.ReadBytes((int)models.Image.Length);
                }
            }
            _mainContext.Catalogs.Update(catalog);
            await _mainContext.SaveChangesAsync();
            return RedirectToAction("Catalog", "Catalog");
        }

        [HttpPost]
        public async Task<IActionResult> AddCatalog_Is_a_Archive(WatchEdit_Archive_View_Models models)
        {
            Archive archive = await _mainContext.Archives.FindAsync(models.Id);

            archive.ProductId = models.ProductId;
            archive.AdminInputClassId = models.AdminInputClassId;

            archive.Name = models.Name;
            archive.Description = models.Description;
            archive.Price = models.Price;
            archive.Amount = models.Amount;
            archive.WatchCategory = models.WatchCategory;

            Catalog catalog = new Catalog();
            catalog.Name = archive.Name;
            catalog.Description = archive.Description;
            catalog.Price = archive.Price;
            catalog.Amount = archive.Amount;
            catalog.WhichCatalog = archive.WatchCategory;
            catalog.Image = archive.Image;

            if (models.Image != null)
            {
                using (var binaryReader = new BinaryReader(models.Image.OpenReadStream()))
                {
                    catalog.Image = binaryReader.ReadBytes((int)models.Image.Length);
                }
            }

            _mainContext.Archives.Remove(archive);
            _mainContext.Catalogs.Add(catalog);

            await _mainContext.SaveChangesAsync();
            return RedirectToAction("Catalog", "Catalog");
        }

        public IActionResult ViewUser(int? id)
        {
            return View(_mainContext.MyKorzinas.OrderBy(x => x.Id).Include(t => t.KorzinaWatches).OrderBy(x => x.Id).ToList());
        }

        public int IdUser { get; set; }
      
        [HttpPost]        
        public async Task<IActionResult> CollWatchs(int collWatch, int collWatch_id)
        {
            Watch watch = new Watch();
            MyKorzina myKorzina = new MyKorzina();

            //Наш пользователь
            var user = _mainContext.Users.FirstOrDefault(t => t.Id == _mainContext.Favorites.Find(collWatch_id).UserId);
            //Нашли товар который выбрали
            var cataloGg = _mainContext.Favorites.Find(collWatch_id).ProductId;
            var catalogs = _mainContext.Catalogs.FirstOrDefault(x => x.Id == cataloGg);

            //Нужно передать товар в BuyProduct
            BuyProducts buyProducts = new BuyProducts();


            if (collWatch > catalogs.Amount)
            {
                return RedirectToAction("FavoritePage", "Catalog");
            }
            else
            {
                buyProducts.UserId = user.Id;
                buyProducts.ProductId = cataloGg;
                buyProducts.Description = catalogs.Description;
                buyProducts.Name = catalogs.Name;
                buyProducts.Image = catalogs.Image;

                buyProducts.Amount = catalogs.Amount;
                buyProducts.WhichCatalog = catalogs.WhichCatalog;
                buyProducts.Price = catalogs.Price;

                buyProducts.CollWatch = collWatch;
                buyProducts.CollPrice = catalogs.Price * collWatch;

                if (user.Name == User.Identity.Name)
                {
                    var product = _mainContext.BuyProductsTable.Where(x => x.UserId == user.Id).ToList();
                    if (product.Count >= 1)
                    {
                        return RedirectToAction("Catalog", "Catalog");
                    }
                }

                _mainContext.BuyProductsTable.Add(buyProducts);
                await _mainContext.SaveChangesAsync();
                return RedirectToAction("BuyProduct", "Account");
            }
            return View();

        }

        public async Task<IActionResult> ProductBuyUser()
        {
            Watch watch = new Watch();
            MyKorzina myKorzina = new MyKorzina();
            BuyProducts buyProduct = new BuyProducts();



            var userId = _mainContext.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
            var user = _mainContext.Users.Find(userId);

            buyProduct = _mainContext.BuyProductsTable.FirstOrDefault(x => x.UserId == _mainContext.Users.Find(userId).Id);

            var catalogs = _mainContext.Catalogs.Where(x => x.Id == buyProduct.ProductId).ToList();

            var catalogId = _mainContext.Catalogs.FirstOrDefault(x => x.Id == buyProduct.ProductId).Id;

            var result = _mainContext.BuyProductsTable.FirstOrDefault(x => x.UserId == user.Id && x.ProductId == buyProduct.ProductId);
            if (catalogs != null)
            {
                foreach (var catalog in catalogs)
                {
                    int colAmWa = catalog.Amount;
                    catalog.Amount -= buyProduct.CollWatch;

                    if (catalog.Amount < 0)
                    {
                        _mainContext.BuyProductsTable.Remove(result);
                        catalog.Amount = colAmWa;
                        await _mainContext.SaveChangesAsync();
                    }
                    _mainContext.Catalogs.Update(catalog);

                    Archive archive = new Archive();
                    if (catalog.Amount < 1)
                    {
                        archive.Name = catalog.Name;
                        archive.Description = catalog.Description;
                        archive.Price = catalog.Price;
                        archive.Amount = catalog.Amount;
                        archive.Image = catalog.Image;
                        archive.WatchCategory = catalog.WhichCatalog;

                        archive.AdminInputClassId = 1;
                        archive.ProductId = catalog.Id;

                        _mainContext.Archives.Add(archive);
                        _mainContext.Catalogs.Remove(catalog);

                    }
                }

                myKorzina.UserId = _mainContext.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
                myKorzina.WatchId = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).Id;

                if (user != null)
                {
                    myKorzina.UserName = user.Name;
                    myKorzina.Surname = user.Surname;
                    myKorzina.Email = user.Email;
                    myKorzina.Password = user.Password;
                    myKorzina.DoublePassword = user.DoublePassword;
                    myKorzina.Role = user.Role;
                    myKorzina.AmountBuyUser += 1;

                    myKorzina.AmountBuyWatch = buyProduct.CollWatch;
                    myKorzina.AllSalePrice = buyProduct.CollPrice;

                    _mainContext.MyKorzinas.Add(myKorzina);
                    await _mainContext.SaveChangesAsync();

                    //----------------------------------------------------------------

                    watch.UserId = myKorzina.UserId;
                    watch.MyKorzinaId = myKorzina.Id;

                    watch.Name = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).Name;
                    watch.Image = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).Image;
                    watch.Description = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).Description;
                    watch.Price = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).Price;
                    watch.Amount = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).Amount;

                    watch.AmountBuyWatch = buyProduct.CollWatch;
                    watch.AllSalePrice = buyProduct.CollPrice;

                    _mainContext.Watches.Add(watch);
                    await _mainContext.SaveChangesAsync();                   
                }

                //-----------------Уведомление_на_почту-----------------
                //try
                //{
                //    MailMessage message = new MailMessage();
                //    message.IsBodyHtml = true;
                //    message.From = new MailAddress("alikhan.iskhadzhiyev@bk.ru", "Hendel");
                //    message.To.Add($"{user.Email}");
                //    message.Subject = "Сообщение от Hendel";
                //    message.Body = "<div>Спасибо за покупку!<div>";

                //    //message.Attachments.Add(new Attachment("путь к файлу"));

                //    using (SmtpClient client = new SmtpClient("smpt.mail.ru"))
                //    {
                //        client.Credentials = new NetworkCredential("alikhan.iskhadzhiyev@bk.ru", "YxuTJTfqXSBten7UNRaM\r\n");
                //        client.Port = 587;
                //        client.EnableSsl = true;
                //        client.Send(message);

                //        logger.LogInformation("Сообщение отправлено успешно!");
                //    }
                //}
                //catch (Exception e)
                //{
                //    logger.LogError(e.GetBaseException().Message);
                //}
            }


            if (buyProduct.ProductId != null)
            {
                var favoriteId = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).ProductId;

                Favorite favorite = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId);
                BuyProducts buyProductsss = _mainContext.BuyProductsTable.FirstOrDefault(x => x.ProductId == buyProduct.ProductId);
                if (favorite != null)
                {
                    _mainContext.Favorites.Remove(favorite);
                    if(buyProductsss != null)
                    {
                        _mainContext.BuyProductsTable.Remove(buyProductsss);
                    }
                    await _mainContext.SaveChangesAsync();
                    return RedirectToAction("BuyCompete", "Catalog");
                }
                return RedirectToAction("Error");
            }
            return View();
        }

        public IActionResult BuyCompete()
        {
            return View();
        }

       

        public async Task<IActionResult> IsModelsWindowMethod(bool isModelWindow, int BoolWatch_id)
        {
            Favorite favorite = _mainContext.Favorites.Find(BoolWatch_id);
           
            favorite.IsModelWindow = true;
            
            isModelWindow = favorite.IsModelWindow;

            return RedirectToAction("FavoritePage", "Catalog");
        }

        public async Task<IActionResult> DeleteWatch(int? id)
        {
            if (id != null)
            {
                Favorite catalog = await _mainContext.Favorites.FindAsync(id);
                if (catalog != null)
                {
                    _mainContext.Favorites.Remove(catalog);
                    await _mainContext.SaveChangesAsync();
                    return RedirectToAction("FavoritePage");
                }
                return RedirectToAction("Error");

            }
            else
                return RedirectToAction("Error");
        }

    }
}
