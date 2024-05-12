//Dystinct - Business(spedup)
namespace Hendel_copy.Models
{
    public class Class_Alihana_Location
    {
        //Watch watch = new Watch();
        //    MyKorzina myKorzina = new MyKorzina();
        //    BuyProducts buyProduct = new BuyProducts();

        //    var userId = _mainContext.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
        //    var user = _mainContext.Users.Find(userId);

        //    buyProduct = _mainContext.BuyProductsTable.FirstOrDefault(x => x.UserId == _mainContext.Users.Find(userId).Id);

        //    var catalogs = _mainContext.Catalogs.Where(x => x.Id == buyProduct.ProductId);
           
        //    var catalogId = _mainContext.Catalogs.FirstOrDefault(x => x.Id == buyProduct.ProductId).Id;

        //    if (catalogs != null)
        //    {
        //        myKorzina.UserId = _mainContext.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
        //        myKorzina.WatchId = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).Id;

        //        if (user != null)
        //        {
        //            myKorzina.UserName = user.Name;
        //            myKorzina.Surname = user.Surname;
        //            myKorzina.Email = user.Email;
        //            myKorzina.Password = user.Password;
        //            myKorzina.DoublePassword = user.DoublePassword;
        //            myKorzina.Role = user.Role;
        //            myKorzina.AmountBuyUser += 1;

        //            _mainContext.MyKorzinas.Add(myKorzina);
        //            await _mainContext.SaveChangesAsync();

        //            //----------------------------------------------------------------

        //            watch.UserId = myKorzina.UserId;
        //            watch.MyKorzinaId = myKorzina.Id;

        //            watch.Name = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).Name;
        //            watch.Image = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).Image;
        //            watch.Description = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).Description;
        //            watch.Price = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).Price;
        //            watch.Amount = _mainContext.Favorites.FirstOrDefault(x => x.ProductId == buyProduct.ProductId).Amount;

        //            _mainContext.Watches.Add(watch);
        //            await _mainContext.SaveChangesAsync();



        //            foreach (var catalog in catalogs)
        //            {
        //                int colAmWa = catalog.Amount;
        //                catalog.Amount -= buyProduct.CollWatch;

        //                if (catalog.Amount < 0)
        //                {
        //                    catalog.Amount = colAmWa;
        //                    return RedirectToAction("FavoritePage", "Catalog");
        //                }
        //                _mainContext.Catalogs.Update(catalog);

        //                Archive archive = new Archive();
        //                if (catalog.Amount < 1)
        //                {
        //                    archive.Name = catalog.Name;
        //                    archive.Description = catalog.Description;
        //                    archive.Price = catalog.Price;
        //                    archive.Amount = catalog.Amount;
        //                    archive.Image = catalog.Image;
        //                    archive.WatchCategory = catalog.WhichCatalog;

        //                    archive.AdminInputClassId = 1;
        //                    archive.ProductId = catalog.Id;

        //                    _mainContext.Archives.Add(archive);
        //                    _mainContext.Catalogs.Remove(catalog);

        //                }
        //            }
        //        }
        //    }


        //    if (buyProduct.ProductId != null)
        //    {
        //        var favoriteId = _mainContext.Favorites.FirstOrDefault(x=> x.ProductId == buyProduct.ProductId).ProductId;
               
        //        Favorite favorite = _mainContext.Favorites.FirstOrDefault(x=>x.ProductId == buyProduct.ProductId);
        //        BuyProducts buyProductsss =  _mainContext.BuyProductsTable.FirstOrDefault(x=> x.ProductId == buyProduct.ProductId);
        //        if (favorite != null)
        //        {
        //            _mainContext.Favorites.Remove(favorite);
        //            _mainContext.BuyProductsTable.Remove(buyProductsss);
        //            await _mainContext.SaveChangesAsync();
        //            return RedirectToAction("BuyCompete", "Catalog");
        //        }
        //        return RedirectToAction("Error");
        //    }

    }
}
