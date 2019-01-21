using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using ChossonKallah.Models;
using ChossonKallahAdmin.EF6;
using ChossonKallahAdmin.GlobalUtilities;

namespace ChossonKallahAdmin.Areas.Xpanel.Controllers
{

    public class AdminController : Controller
    {

        DataTable dt = new DataTable();
        public string Version()
        {
            return "<h2>The Installed Mvc Version In your System Is : " + typeof(Controller).Assembly.GetName().Version.ToString() + "</h2>";
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin Obj_Admin, string command)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var EmailPara = new SqlParameter("@Email", Obj_Admin.Email);
                    var result = context.Database.SqlQuery<Admin>("sp_Admin_CheckAdminExists @Email", EmailPara).ToList();
                    if (result.Count > 0)
                    {
                        ViewBag.AdminExists = "Email is already exists.";
                        return View(Obj_Admin);
                    }
                    else
                    {
                        Obj_Admin.CreatedOn = System.DateTime.Now;
                        Obj_Admin.IsDeleted = false;
                        context.Admins.Add(Obj_Admin);
                        context.SaveChanges();
                        TempData["AdminSuccess"] = "Record has been added successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.AdminExists = ex.Message;
                return View(Obj_Admin);
            }

        }

        public ActionResult Edit(Int32? AdminId)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var AdminIdPara = new SqlParameter("@adminid", AdminId);
                    Admin obj_Admin = context.Database.SqlQuery<Admin>("sp_Admin_select @adminid", AdminIdPara).FirstOrDefault();
                    return View(obj_Admin);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.AdminExists = ex.Message;
                return View(new Admin());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin Obj_Admin)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var EmailPara = new SqlParameter("@Email", Obj_Admin.Email);
                    var AdminIdPara = new SqlParameter("@adminid", Obj_Admin.AdminId);
                    var result = context.Database.SqlQuery<Admin>("sp_Admin_CheckAdminExistsUpdate  @adminid,@Email", AdminIdPara, EmailPara).ToList();
                    if (result.Count > 0)
                    {
                        ViewBag.AdminExists = "Email is already exists.";
                        return View(Obj_Admin);
                    }
                    else
                    {
                        context.Admins.Attach(Obj_Admin);
                        context.Entry(Obj_Admin).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        TempData["AdminSuccess"] = "Record has been update successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.AdminExists = ex.Message;
                return View(Obj_Admin);
            }
        }

        public ActionResult Delete(Int32 AdminId)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var AdminIdPara = new SqlParameter("@adminid", AdminId);
                    Admin obj_Admin = context.Database.SqlQuery<Admin>("sp_Admin_select @adminid", AdminIdPara).FirstOrDefault();
                    obj_Admin.IsDeleted = true;
                    context.Entry(obj_Admin).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    TempData["AdminSuccess"] = "Record has been deleted successfully.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    TempData["AdminError"] = ex.Message;
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
                    var result = context.Database.SqlQuery<Listview>("sp_Admin_selectIndexPaging @PageSize,@PageIndex,@Search", PageSizePara, PageIndexPara, SearchPara).ToList();
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
