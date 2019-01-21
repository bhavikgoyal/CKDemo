using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using ChossonKallah.Models;
using ChossonKallahAdmin.EF6;
using ChossonKallahAdmin.GlobalUtilities;

namespace ChossonKallahAdmin.Areas.Xpanel.Controllers
{
    public class LocationController : Controller
    {
        DataTable dt = new DataTable();
        public ActionResult Create()
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    ViewBag.StateData = SessionUtilities.GetState();
                    return View();
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.LocationExists = ex.Message;
                return View();
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Location Obj_Location, string command)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var LocationnamePara = new SqlParameter("@Locationname", Obj_Location.LocationName);
                    var LocationurlPara = new SqlParameter("@Locationurl", Obj_Location.LocationURL);
                    var result = context.Database.SqlQuery<Location>("sp_Location_CheckLocationExists @Locationname,@Locationurl", LocationnamePara, LocationurlPara).ToList();
                    if (result.Count > 0)
                    {
                        ViewBag.LocationExists = "Location name is already exists.";
                        ViewBag.StateData = SessionUtilities.GetState();
                        return View(Obj_Location);
                    }
                    else
                    {
                        Obj_Location.CreatedOn = System.DateTime.Now;
                        Obj_Location.IsDeleted = false;
                        context.Locations.Add(Obj_Location);
                        context.SaveChanges();
                        TempData["LocationSuccess"] = "Record has been added successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.LocationExists = ex.Message;
                return View(Obj_Location);
            }
        }

        public ActionResult Edit(Int32? Locationid)
        {
            using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
            {
                try
                {
                    var LocationidPara = new SqlParameter("@Locationid", Locationid);
                    var ResultLocation = context.Database.SqlQuery<Location>("sp_Location_select @Locationid", LocationidPara).FirstOrDefault();
                    ViewBag.StateData = SessionUtilities.GetState();
                    return View(ResultLocation);
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                        ViewBag.LocationExists = ex.Message;
                    return View();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Location Obj_Location)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var LocationidPara = new SqlParameter("@Locationid", Obj_Location.LocationId);
                    var LocationnamePara = new SqlParameter("@Locationname", Obj_Location.LocationName);
                    var LocationurlPara = new SqlParameter("@Locationurl", Obj_Location.LocationURL);
                    var result = context.Database.SqlQuery<Location>("sp_Location_CheckLocationExists_Update  @Locationid,@Locationname,@Locationurl", LocationidPara, LocationnamePara, LocationurlPara).ToList();
                    if (result.Count > 0)
                    {
                        ViewBag.LocationExists = "Location name is already exists.";
                        ViewBag.StateData = SessionUtilities.GetState();
                        return View(Obj_Location);
                    }
                    else
                    {
                        context.Locations.Attach(Obj_Location);
                        context.Entry(Obj_Location).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        TempData["LocationSuccess"] = "Record has been update successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.LocationExists = ex.Message;
                return View(Obj_Location);
            }
        }

        public ActionResult Delete(Int32? Locationid)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var LocationidPara = new SqlParameter("@Locationid", Locationid);
                    Location obj_Location = context.Database.SqlQuery<Location>("sp_Location_select @Locationid", LocationidPara).FirstOrDefault();
                    obj_Location.IsDeleted = true;
                    context.Entry(obj_Location).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    TempData["LocationSuccess"] = "Record has been deleted successfully.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    TempData["LocationError"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Indexpaging(Int64 PageSize, Int64 PageIndex, string Search)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var PageSizePara = new SqlParameter("@PageSize", PageSize);
                    var PageIndexPara = new SqlParameter("@PageIndex", PageIndex);
                    var SearchPara = new SqlParameter("@Search", Search);
                    var result = context.Database.SqlQuery<Listview>("sp_Location_selectIndexPaging @PageSize,@PageIndex,@Search", PageSizePara, PageIndexPara, SearchPara).ToList();
                    return Json(SessionUtilities.ConvertDataTableTojSonString(SessionUtilities.LINQResultToDataTable(result)), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(string.Format("Exception {0}", ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
    }

}
