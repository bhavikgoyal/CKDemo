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

namespace ChossonKallah.Controllers
{
    public class VendorController : Controller
    {
        DataTable dt = new DataTable();
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vendor Obj_Vendor, string command)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var EmailPara = new SqlParameter("@Email", Obj_Vendor.VendorEmail);
                    var result = context.Database.SqlQuery<Vendor>("sp_Vendor_CheckAdminExists @Email", EmailPara).ToList();
                    if (result.Count > 0)
                    {
                        ViewBag.VendorExists = "Email is already exists.";
                        return View(Obj_Vendor);
                    }
                    else
                    {
                        Obj_Vendor.CreatedAt = System.DateTime.Now;
                        Obj_Vendor.IsDeleted = false;
                        context.Vendors.Add(Obj_Vendor);
                        context.SaveChanges();
                        TempData["VendorSuccess"] = "Record has been added successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.VendorExists = ex.Message;
                return View(Obj_Vendor);
            }

        }

        public ActionResult Edit(Int32? VendorId)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var VendorIdPara = new SqlParameter("@vendorid", VendorId);
                    Vendor obj_Vendor = context.Database.SqlQuery<Vendor>("sp_Vendor_select @vendorid", VendorIdPara).FirstOrDefault();
                    return View(obj_Vendor);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.VendorExists = ex.Message;
                return View(new Vendor());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vendor Obj_Vendor)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var EmailPara = new SqlParameter("@Email", Obj_Vendor.VendorEmail);
                    var VendorIdPara = new SqlParameter("@vendorid", Obj_Vendor.VendorID);
                    var result = context.Database.SqlQuery<Vendor>("sp_Vendor_CheckVendorExistsUpdate  @vendorid,@Email", VendorIdPara, EmailPara).ToList();
                    if (result.Count > 0)
                    {
                        ViewBag.VendorExists = "Email is already exists.";
                        return View(Obj_Vendor);
                    }
                    else
                    {
                        context.Vendors.Attach(Obj_Vendor);
                        context.Entry(Obj_Vendor).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        TempData["VendorSuccess"] = "Record has been update successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.VendorExists = ex.Message;
                return View(Obj_Vendor);
            }
        }

        public ActionResult Delete(Int32 VendorId)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var VendorIdPara = new SqlParameter("@vendorid", VendorId);
                    Vendor obj_Vendor = context.Database.SqlQuery<Vendor>("sp_Vendor_select @vendorid", VendorIdPara).FirstOrDefault();
                    obj_Vendor.IsDeleted = true;
                    context.Entry(obj_Vendor).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    TempData["VendorSuccess"] = "Record has been deleted successfully.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    TempData["VendorError"] = ex.Message;
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
                    var result = context.Database.SqlQuery<Listview>("sp_Vendor_selectIndexPaging @PageSize,@PageIndex,@Search", PageSizePara, PageIndexPara, SearchPara).ToList();
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
