using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using BolumTer.Models;
using BolumTer.App_Class;

namespace BolumTer.Controllers
{
    public class UniversiteController : Controller
    {
        //
        // GET: /Universite/

        public ActionResult Index()
        {
            return View("Universite");
        }



        public JsonResult GetUniDetay(int? uId)
        {
            tercih t = new tercih();

            return Json(t.GetUniDetay(uId), JsonRequestBehavior.AllowGet);
        }

    }
}
