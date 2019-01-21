using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ChossonKallah.Models;
using ChossonKallahAdmin.EF6;
using ChossonKallahAdmin.GlobalUtilities;

namespace ChossonKallah.Controllers
{
    public class AdsBannerController : Controller
    {
        public ActionResult Create()
        {
            try
            {
                GetCateoryVendor();
                return View();
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.AdsBannerError = ex.Message;
                return View();
            }
        }

        public void GetCateoryVendor()
        {
            using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
            {
                Int32? Categoryid = 0;
                var CategoryidPara = new SqlParameter("@Categoryid", Categoryid);
                var CategoryResult = context.Database.SqlQuery<Category>("sp_Categories_Id_Name_selectall @Categoryid", CategoryidPara).ToList();
                List<SelectListItem> CategoryIdName = new List<SelectListItem>();
                SelectListItem slCategory = new SelectListItem();
                slCategory.Text = "--Please Select--";
                slCategory.Value = "0";
                slCategory.Selected = true;
                CategoryIdName.Add(slCategory);
                for (int i = 0; i < CategoryResult.Count; i++)
                {
                    SelectListItem obj = new SelectListItem();

                    obj.Text = Convert.ToString(CategoryResult[i].CategoryName);
                    obj.Value = Convert.ToString(CategoryResult[i].CategoryId);
                    CategoryIdName.Add(obj);
                }
                ViewBag.Categories = CategoryIdName;
                var VendorResult = context.Database.SqlQuery<Vendor>("sp_Vendor_Id_Name_selectall").ToList();
                List<SelectListItem> VendorIdName = new List<SelectListItem>();
                SelectListItem slVendor = new SelectListItem();
                slVendor.Text = "--Please Select--";
                slVendor.Value = "0";
                slVendor.Selected = true;
                VendorIdName.Add(slVendor);
                for (int i = 0; i < VendorResult.Count; i++)
                {
                    SelectListItem obj = new SelectListItem();
                    obj.Text = Convert.ToString(VendorResult[i].VendorName);
                    obj.Value = Convert.ToString(VendorResult[i].VendorID);
                    VendorIdName.Add(obj);
                }
                ViewBag.Vendors = VendorIdName;

                List<SelectListItem> PositionName = new List<SelectListItem>();
                SelectListItem slPosition = new SelectListItem();
                slPosition.Text = "--Please Select--";
                slPosition.Value = "0";
                slPosition.Selected = true;
                PositionName.Add(slPosition);
                for (int i = 1; i < 5; i++)
                {
                    if (i == 1)
                    {
                        slPosition = new SelectListItem();
                        slPosition.Text = "Top";
                        slPosition.Value = "Top";
                        PositionName.Add(slPosition);
                    }
                    if (i == 2)
                    {
                        slPosition = new SelectListItem();
                        slPosition.Text = "Right";
                        slPosition.Value = "Right";
                        PositionName.Add(slPosition);
                    }
                    if (i == 3)
                    {
                        slPosition = new SelectListItem();
                        slPosition.Text = "Left";
                        slPosition.Value = "Left";
                        PositionName.Add(slPosition);
                    }
                    if (i == 4)
                    {
                        slPosition = new SelectListItem();
                        slPosition.Text = "Bottom";
                        slPosition.Value = "Bottom";
                        PositionName.Add(slPosition);
                    }
                }
                ViewBag.Positions = PositionName;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdsBanner Obj_AdsBanner, string command, HttpPostedFileBase AdsBannerImage)
        {
            try
            {
                string AdsBannerName = string.Empty;
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var AdsBannerNamePara = new SqlParameter("@adsbannername", Obj_AdsBanner.AdsBannerName);
                    var AdsBannerVendorIdPara = new SqlParameter("@vendorid", Obj_AdsBanner.VendorID);
                    var result = context.Database.SqlQuery<AdsBanner>("sp_AdsBanner_CheckAdminExists @adsbannername,@vendorid", AdsBannerNamePara, AdsBannerVendorIdPara).ToList();
                    if (result.Count > 0)
                    {
                        ViewBag.AdsBannerExists = "Banner name is already exists.";
                        GetCateoryVendor();
                        return View(Obj_AdsBanner);
                    }
                    else
                    {
                        GetCateoryVendor();
                        if (SessionUtilities.SaveImage(AdsBannerImage, "AdsBanner", Obj_AdsBanner.AdsBannerName))
                        {
                            AdsBannerName = Path.GetFileName(AdsBannerImage.FileName);
                            Obj_AdsBanner.AdsBannerImage = AdsBannerName;
                        }
                        Obj_AdsBanner.CreateAt = System.DateTime.Now;
                        Obj_AdsBanner.IsDeleted = false;
                        context.AdsBanners.Add(Obj_AdsBanner);
                        context.SaveChanges();
                        var adsBannersequenceIdpara = new SqlParameter("@adsbannerid", Obj_AdsBanner.AdsBannerID);
                        context.Database.ExecuteSqlCommand("sp_AdsBanner_UpdateSequence @adsbannerid", adsBannersequenceIdpara);
                        TempData["AdsBannerSuccess"] = "Record has been added successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.AdsBannerExists = ex.Message;
                return View(Obj_AdsBanner);
            }

        }

        public ActionResult Edit(Int32? AdsBannerId)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var AdsBannerIdPara = new SqlParameter("@adsbannerid", AdsBannerId);
                    AdsBanner obj_AdsBanner = context.Database.SqlQuery<AdsBanner>("sp_AdsBanner_select @adsbannerid", AdsBannerIdPara).FirstOrDefault();
                    GetCateoryVendor();
                    return View(obj_AdsBanner);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.AdsBannerExists = ex.Message;
                return View(new AdsBanner());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AdsBanner Obj_AdsBanner, HttpPostedFileBase AdsBannerImage)
        {
            try
            {
                string AdsBannerName = string.Empty;
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var AdsBannerNamePara = new SqlParameter("@adsbannername", Obj_AdsBanner.AdsBannerName);
                    var AdsBannerVendorIdPara = new SqlParameter("@vendorid", Obj_AdsBanner.VendorID);
                    var AdsBannerIdPara = new SqlParameter("@adsbannerid", Obj_AdsBanner.AdsBannerID);
                    var result = context.Database.SqlQuery<AdsBanner>("sp_AdsBanner_CheckVendorExistsUpdate @adsbannername,@vendorid,@adsbannerid", AdsBannerNamePara, AdsBannerVendorIdPara, AdsBannerIdPara).ToList();
                    if (result.Count > 0)
                    {
                        ViewBag.AdsBannerExists = "Banner name is already exists.";
                        GetCateoryVendor();
                        return View(Obj_AdsBanner);
                    }
                    else
                    {
                        var AdsGetBannerIdPara = new SqlParameter("@adsbannerid", Obj_AdsBanner.AdsBannerID);
                        AdsBanner obj_AdsBanners = context.Database.SqlQuery<AdsBanner>("sp_AdsBanner_select @adsbannerid", AdsGetBannerIdPara).FirstOrDefault();
                        if (SessionUtilities.SaveImage(AdsBannerImage, "AdsBanner", Obj_AdsBanner.AdsBannerName))
                        {
                            AdsBannerName = Path.GetFileName(AdsBannerImage.FileName);
                            Obj_AdsBanner.AdsBannerImage = AdsBannerName;
                        }
                        else
                        {
                            Obj_AdsBanner.AdsBannerImage = obj_AdsBanners.AdsBannerImage;
                        }

                        var adsbannerSequenceidPara = new SqlParameter("@adsbannerid", Obj_AdsBanner.AdsBannerID);
                        var adsbannerSequencePara = new SqlParameter("@priority", Obj_AdsBanner.Priority);
                        context.Database.ExecuteSqlCommand("sp_AdsBanner_updateSequenceEdit @priority,@adsbannerid", adsbannerSequencePara, adsbannerSequenceidPara);
                        context.AdsBanners.Attach(Obj_AdsBanner);
                        context.Entry(Obj_AdsBanner).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        TempData["AdsBannerSuccess"] = "Record has been update successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.AdsBannerExists = ex.Message;
                return View(Obj_AdsBanner);
            }
        }

        public ActionResult Delete(Int32 AdsBannerId)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var AdsBannerIdPara = new SqlParameter("@adsbannerid", AdsBannerId);
                    AdsBanner obj_AdsBanner = context.Database.SqlQuery<AdsBanner>("sp_AdsBanner_select @adsbannerid", AdsBannerIdPara).FirstOrDefault();
                    obj_AdsBanner.IsDeleted = true;
                    context.Entry(obj_AdsBanner).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    var adsbannerSequenceidPara = new SqlParameter("@adsbannerid", obj_AdsBanner.AdsBannerID);
                    context.Database.ExecuteSqlCommand("sp_AdsBanner_delete @adsbannerid", AdsBannerIdPara);
                    TempData["AdsBannerSuccess"] = "Record has been deleted successfully.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    TempData["AdsBannerError"] = ex.Message;
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
                    var result = context.Database.SqlQuery<Listview>("sp_AdsBanner_selectIndexPaging @PageSize,@PageIndex,@Search", PageSizePara, PageIndexPara, SearchPara).ToList();
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
