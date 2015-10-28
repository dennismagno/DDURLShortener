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
            ViewBag.Title = "Home";
            IEnumerable<URL> model = new List<URL>();
            if (User.Identity.IsAuthenticated)
            {
                int _userid = WebSecurity.GetUserId(User.Identity.Name);
                model = _db.Urls.Where(u => u.UserID == _userid).OrderByDescending(u => u.DateCreated).AsEnumerable();
            }

            return View(model);
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
            string _error = "Please check your URL";

            //Make sure Url is not empty
            if (!string.IsNullOrEmpty(Url))
            {
                string _url = Url;
                if (!_url.ToLower().StartsWith("http") && !_url.ToLower().StartsWith("https"))
                {
                    _url = "http://" + _url;
                }

                //Check if long Url is already existing in database
                URL urlExists = _db.Urls.Where(u => u.LongUrl.ToLower() == _url.ToLower()).FirstOrDefault();
                if (urlExists != null)
                {
                    urlExists.ShortUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + urlExists.ShortUrl;
                    return Json(new { status = true, url = urlExists }, JsonRequestBehavior.AllowGet);
                }

                //Check if Url is valid
                if (Func.IsUrlExists(_url))
                {
                    URL sUrl = new URL()
                    {
                        LongUrl = _url,
                        NoClicks = 0,
                        DateCreated = DateTime.UtcNow
                    };

                    int _userid = WebSecurity.GetUserId(User.Identity.Name);
                    if (_userid > 0)
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

                        sUrl.ShortUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + sUrl.ShortUrl;
                        return Json(new { status = true, url = sUrl }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception exc)
                    {
                        _error = exc.Message;
                    }
                }
            } 

            return Json(new { status = false, message = _error }, JsonRequestBehavior.AllowGet);
        }
    }
}
