using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using BolumTer.Models;
using BolumTer.App_Class;


namespace BolumTer.Controllers
{
    public class IletisimController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Feedback FormData)
        {

            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                dbOp dbOp = new dbOp();
                dbOp.writeIletisim(FormData.localId, FormData.EMail, FormData.comment);
                return RedirectToAction("eywllh");
            }

            ViewBag.ErrMessage = "Hata: Resimdeki harfleri doğru girmelisin!";
            return View();
        }

        public ActionResult eywllh()
        {
            return View();
        }


    }
}
