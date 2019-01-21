using ChossonKallah.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using ChossonKallahAdmin.GlobalUtilities;
using ChossonKallahAdmin.EF6;
using System.Data.SqlClient;
using System.Linq;

namespace ChossonKallah.Controllers
{
    public class WebsiteBannerController : Controller
    {

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WebsiteBanner Obj_WebsiteBanner, string command, HttpPostedFileBase BannerImage)
        {
            try
            {
                string BannerName = string.Empty;
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    if (SessionUtilities.SaveImage(BannerImage, "Banner"))
                    {
                        BannerName = Path.GetFileName(BannerImage.FileName);
                        Obj_WebsiteBanner.BannerImage = BannerName;
                    }
                    Obj_WebsiteBanner.CreatedOn = System.DateTime.Now;
                    Obj_WebsiteBanner.IsDeleted = false;
                    context.WebsiteBanners.Add(Obj_WebsiteBanner);
                    context.SaveChanges();
                    var WebsitebannerSequenceidPara = new SqlParameter("@websitebannerid", Obj_WebsiteBanner.WebsiteBannerID);
                    context.Database.ExecuteSqlCommand("sp_WebsiteBanner_UpdateSequence @websitebannerid", WebsitebannerSequenceidPara);
                    TempData["WebsiteBannerSuccess"] = "Record has been added successfully.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.WebsiteBannerError = ex.Message;
                return View(Obj_WebsiteBanner);
            }
            //try
            //{
            //    string BannerName = string.Empty;
            //    using (WebsiteBannerCtl db = new WebsiteBannerCtl())
            //    {
            //        if (SessionUtilities.SaveImage(BannerImage, "Banner"))
            //        {
            //            BannerName = Path.GetFileName(BannerImage.FileName);
            //            Obj_WebsiteBanner.BannerImage = BannerName;
            //        }
            //        db.insert(Obj_WebsiteBanner);
            //        TempData["WebsiteBannerSuccess"] = "Record has been added successfully.";
            //    }
            //    return RedirectToAction("Index");

            //}
            //catch (Exception ex)
            //{
            //    return View(Obj_WebsiteBanner);
            //}
        }

        public ActionResult Edit(Int32? Websitebannerid)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var WebsitebanneridPara = new SqlParameter("@Websitebannerid", Websitebannerid);
                    WebsiteBanner obj_WebsiteBanner = context.Database.SqlQuery<WebsiteBanner>("sp_WebsiteBanner_select @Websitebannerid", WebsitebanneridPara).FirstOrDefault();
                    return View(obj_WebsiteBanner);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.WebsiteBannerError = ex.Message;
                return View(new Admin());
            }
            //using (WebsiteBannerCtl db = new WebsiteBannerCtl())
            //{
            //    WebsiteBannerClass obj_WebsiteBanner = db.selectById(Websitebannerid);
            //    return View(obj_WebsiteBanner);
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WebsiteBanner Obj_WebsiteBanner, HttpPostedFileBase BannerImage)
        {
            try
            {
                string BannerName = string.Empty;
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var WebsitebanneridPara = new SqlParameter("@Websitebannerid", Obj_WebsiteBanner.WebsiteBannerID);
                    WebsiteBanner obj_WebsiteBannerResult = context.Database.SqlQuery<WebsiteBanner>("sp_WebsiteBanner_select @Websitebannerid", WebsitebanneridPara).FirstOrDefault();
                    if (SessionUtilities.SaveImage(BannerImage, "Banner"))
                    {
                        BannerName = Path.GetFileName(BannerImage.FileName);
                        Obj_WebsiteBanner.BannerImage = BannerName;
                    }
                    else
                    {
                        Obj_WebsiteBanner.BannerImage = obj_WebsiteBannerResult.BannerImage;
                    }
                    var WebsitebannerSequenceidPara = new SqlParameter("@Websitebannerid", Obj_WebsiteBanner.WebsiteBannerID);
                    var WebsitebannerSequencePara = new SqlParameter("@Sequence", Obj_WebsiteBanner.Sequence);
                    context.Database.ExecuteSqlCommand("sp_WebsiteBanner_updateSequenceEdit @Sequence,@Websitebannerid", WebsitebannerSequencePara,WebsitebannerSequenceidPara);
                    context.WebsiteBanners.Attach(Obj_WebsiteBanner);
                    context.Entry(Obj_WebsiteBanner).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    TempData["WebsiteBannerSuccess"] = "Record has been update successfully.";
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.WebsiteBannerError = ex.Message;
                return View(Obj_WebsiteBanner);
            }
            //try
            //{
            //    string BannerName = string.Empty;
            //    using (WebsiteBannerCtl db = new WebsiteBannerCtl())
            //    {
            //        if (SessionUtilities.SaveImage(BannerImage, "Banner"))
            //        {
            //            BannerName = Path.GetFileName(BannerImage.FileName);
            //            Obj_WebsiteBanner.BannerImage = BannerName;
            //        }
            //        db.update(Obj_WebsiteBanner);
            //        TempData["WebsiteBannerSuccess"] = "Record has been updated successfully.";
            //    }
            //    return RedirectToAction("Index");
            //}
            //catch (Exception ex)
            //{
            //    return View(Obj_WebsiteBanner);
            //}
        }

        public ActionResult Delete(Int32? Websitebannerid)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var WebsitebanneridPara = new SqlParameter("@Websitebannerid", Websitebannerid);
                    WebsiteBanner obj_WebsiteBanner = context.Database.SqlQuery<WebsiteBanner>("sp_WebsiteBanner_select @Websitebannerid", WebsitebanneridPara).FirstOrDefault();
                    obj_WebsiteBanner.IsDeleted = true;
                    context.Entry(obj_WebsiteBanner).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    var WebsitebannerSequenceidPara = new SqlParameter("@Websitebannerid", Websitebannerid);
                    context.Database.ExecuteSqlCommand("sp_WebsiteBanner_delete @Websitebannerid", WebsitebannerSequenceidPara);
                    TempData["WebsiteBannerSuccess"] = "Record has been deleted successfully.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    TempData["WebsiteBannerError"] = ex.Message;
                return RedirectToAction("Index");
            }
            //using (WebsiteBannerCtl db = new WebsiteBannerCtl())
            //{
            //    db.delete(Websitebannerid);
            //    TempData["WebsiteBannerSuccess"] = "Record has been deleted successfully.";
            //    return RedirectToAction("Index");
            //}
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Indexpaging(Int64 PageSize, Int64 PageIndex, string Search)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var PageSizePara = new SqlParameter("@PageSize", PageSize);
                    var PageIndexPara = new SqlParameter("@PageIndex", PageIndex);
                    var SearchPara = new SqlParameter("@Search", Search);
                    var result = context.Database.SqlQuery<Listview>("sp_WebsiteBanner_selectIndexPaging @PageSize,@PageIndex,@Search", PageSizePara, PageIndexPara, SearchPara).ToList();
                    return Json(SessionUtilities.ConvertDataTableTojSonString(SessionUtilities.LINQResultToDataTable(result)), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(string.Format("Exception {0}", ex.Message), JsonRequestBehavior.AllowGet);
            }
            //using (WebsiteBannerCtl db = new WebsiteBannerCtl())
            //{
            //    return Json(db.selectIndexPaging(PageSize, PageIndex, Search), JsonRequestBehavior.AllowGet);
            //}
        }
    }
}
