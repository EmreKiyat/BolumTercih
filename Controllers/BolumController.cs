using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BolumTer.App_Class;
using BolumTer.Models;

namespace BolumTer.Controllers
{
    public class BolumController : Controller
    {

        public ActionResult Index()
        {
            return View("Bolum");
        }

        public JsonResult GetBolumListBySearchText(SearchTextModel SearchObj)
        {
            tercih t = new tercih();
            return Json(t.GetBolumListBySearchText(SearchObj), JsonRequestBehavior.AllowGet);
        }


    }
}