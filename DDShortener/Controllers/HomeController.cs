using DDShortener.Filters;
using DDShortener.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace DDShortener.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        DBContext _db = new DBContext();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }


        public ActionResult UrlNotFound()
        {
            return View();
        }

        public ActionResult RedirectShortURL(string shortURL)
        {
            if (string.IsNullOrEmpty(shortURL))
            {
                return RedirectToAction("UrlNotFound", "Home");
            }
            else
            {
                URL longURL = _db.Urls.Where(u => u.ShortUrl.ToLower() == shortURL.ToLower()).FirstOrDefault();
                if (longURL != null)
                {
                    longURL.NoClicks++;
                    _db.SaveChanges();
                    return Redirect(longURL.LongUrl);
                }
                else
                {
                    return RedirectToAction("UrlNotFound", "Home");
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ShortenURL(string Url)
        {
            if (string.IsNullOrEmpty(Url))
            {

            }

            string _url = Url;
            if (!_url.ToLower().StartsWith("http") && !_url.ToLower().StartsWith("https"))
            {
                _url = "http://" + _url;
            }

            if (Func.IsUrlExists(_url))
            {
                URL sUrl = new URL()
                {
                    LongUrl = _url,
                    NoClicks = 0,
                    DateCreated = DateTime.UtcNow
                };

                int _userid = WebSecurity.GetUserId(User.Identity.Name);
                if (_userid != 0)
                {
                    sUrl.UserID = _userid;
                }

                //Generate random short string for URL
                //Verify if random short code is already exiting in DB
                string _shortUrl = Func.CreateShortString();
                URL existingSURL = _db.Urls.Where(u => u.ShortUrl.ToLower() == _shortUrl.ToLower()).FirstOrDefault();
                while (existingSURL != null)
                {
                    _shortUrl = Func.CreateShortString();
                    existingSURL = _db.Urls.Where(u => u.ShortUrl.ToLower() == _shortUrl.ToLower()).FirstOrDefault();
                }

                sUrl.ShortUrl = _shortUrl;
                _db.Urls.Add(sUrl);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return View();
        }
    }
}
