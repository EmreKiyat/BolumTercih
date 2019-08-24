using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BolumTer.App_Class;
using BolumTer.Models;
using System.Web.Script.Serialization;

namespace BolumTer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetNewID(List<BolumJsonModel> myL)
        {
            try
            {
                tercih t = new tercih();
                JavaScriptSerializer js = new JavaScriptSerializer();
                Guid newId = t.GetNewId(js.Serialize(myL));
                return Json(new { newId = newId });
            }
            catch (Exception ex)
            {
                dbOp dbOp = new dbOp();
                dbOp.errLog("Home", "GetNewID", ex.Source, ex.Message);
                return Json(new { newId = -1 });
            }
        }


        public JsonResult UpdateTercihList(TercihListJsonModel tListObj) {
            try {
            tercih t = new tercih();
            JavaScriptSerializer js = new JavaScriptSerializer();
            int ret = t.UpdateTercihList(tListObj.tId, js.Serialize(tListObj.tJson));
            return Json(new { Result = ret == -1 });
            }
            catch (Exception ex)
            {
                dbOp dbOp = new dbOp();
                dbOp.errLog("Home", "UpdateTercihList", ex.Source, ex.Message);
                return Json(new { Result = false });
            }
        }


        [HttpPost]
        public JsonResult GetBolumList(FilterObjModel FilterObj)
        {
            try {
            tercih t = new tercih();
            return Json(t.GetBolumList(FilterObj), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                dbOp dbOp = new dbOp();
                dbOp.errLog("Home", "GetBolumList", ex.Source, ex.Message);
                return Json(new { Result = false });
            }
        }

        public JsonResult GetTercihListById(ListIdModel ListId)
        {
            try {
            tercih t = new tercih();
            return Json(t.GetTercihListByIdList(ListId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                dbOp dbOp = new dbOp();
                dbOp.errLog("Home", "GetTercihListById", ex.Source, ex.Message);
                return Json(new { Result = false });
            }
        }

        public JsonResult GetBolumKosul(BolumJsonModel BolumId)
        {
            try {
            tercih t = new tercih();
            return Json(t.GetBolumKosul(BolumId.Id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                dbOp dbOp = new dbOp();
                dbOp.errLog("Home", "GetBolumKosul", ex.Source, ex.Message);
                return Json(new { Result = false });
            }
        }
    }
}
