using ChossonKallah.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Web.Mvc;
using ChossonKallahAdmin.GlobalUtilities;
using ChossonKallahAdmin.EF6;
using System.Data.SqlClient;
using System.Linq;

namespace ChossonKallah.Controllers
{
    public class BusinessReviewController : Controller
    {
        public ActionResult Create(Int32? Businessid)
        {
            BusinessReview bs = new BusinessReview();
            bs.BusinessID = Businessid;
            return View(bs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BusinessReview Obj_BusinessReview, string command)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    Obj_BusinessReview.AddedOn = System.DateTime.Now;
                    Obj_BusinessReview.IsDeleted = false;
                    context.BusinessReviews.Add(Obj_BusinessReview);
                    context.SaveChanges();
                    TempData["BusinessReviewSuccess"] = "Record has been added successfully.";
                    return RedirectToAction("Index", new { Businessid = Obj_BusinessReview.BusinessID });
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.BusinessReviewError = ex.Message;
                return View(Obj_BusinessReview);
            }
            //using (BusinessReviewCtl db = new BusinessReviewCtl())
            //{

            //    db.insert(Obj_BusinessReview);
            //    return RedirectToAction("Index", new { Businessid = Obj_BusinessReview.Businessid });
            //}
        }

        public ActionResult Edit(Int32? Businessreviewid)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var BusinessreviewidPara = new SqlParameter("@Businessreviewid", Businessreviewid);
                    BusinessReview Obj_BusinessReview = context.Database.SqlQuery<BusinessReview>("sp_BusinessReview_select @Businessreviewid", BusinessreviewidPara).FirstOrDefault();
                    return View(Obj_BusinessReview);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.BusinessReviewError = ex.Message;
                return View();
            }
            //using (BusinessReviewCtl db = new BusinessReviewCtl())
            //{
            //    BusinessReviewClass obj_BusinessReview = db.selectById(Businessreviewid);
            //    return View(obj_BusinessReview);
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BusinessReview Obj_BusinessReview)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    context.BusinessReviews.Attach(Obj_BusinessReview);
                    context.Entry(Obj_BusinessReview).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    TempData["BusinessReviewSuccess"] = "Record has been update successfully.";
                    return RedirectToAction("Index", new { Businessid = Obj_BusinessReview.BusinessID });
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.BusinessReviewError = ex.Message;
                return View(Obj_BusinessReview);
            }
            //using (BusinessReviewCtl db = new BusinessReviewCtl())
            //{
            //    db.update(Obj_BusinessReview);
            //    return RedirectToAction("Index", new { Businessid = Obj_BusinessReview.Businessid });
            //}
        }

        public ActionResult Delete(Int32? Businessreviewid)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var BusinessreviewidPara = new SqlParameter("@Businessreviewid", Businessreviewid);
                    BusinessReview obj_BusinessReview = context.Database.SqlQuery<BusinessReview>("sp_BusinessReview_select @Businessreviewid", BusinessreviewidPara).FirstOrDefault();
                    obj_BusinessReview.IsDeleted = true;
                    context.Entry(obj_BusinessReview).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    TempData["BusinessReviewSuccess"] = "Record has been deleted successfully.";
                    return RedirectToAction("Index", new { Businessid = obj_BusinessReview.BusinessID });
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.BusinessReviewError = ex.Message;
                return View();
            }
            //using (BusinessReviewCtl db = new BusinessReviewCtl())
            //{
            //    db.delete(Businessreviewid);
            //    return RedirectToAction("Index");
            //}
        }

        public ActionResult Index(Int32? Businessid)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var BusinessidPara = new SqlParameter("@Businessid", Businessid);
                    var obj_BusinessDirectory = context.Database.SqlQuery<BusinessDirectory>("sp_BusinessDirectory_select @Businessid", BusinessidPara).FirstOrDefault();
                    ViewBag.ListingName = obj_BusinessDirectory.BusinessName;
                    return View();
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.BusinessReview = ex.Message;
                return View();

            }
            //string listingName = "";
            //using (BusinessDirectoryCtl dbLIsting = new BusinessDirectoryCtl())
            //{
            //    System.Data.DataTable dt = dbLIsting.selectdatatable(Businessid);
            //    if (dt.Rows.Count > 0)
            //    {
            //        listingName = Convert.ToString(dt.Rows[0]["BusinessName"]);
            //    }
            //}
            //ViewBag.ListingName = listingName;
            //return View();
        }

        [HttpGet]
        public ActionResult Indexpaging(Int64 PageSize, Int64 PageIndex, string Search, string Businessid)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var PageSizePara = new SqlParameter("@PageSize", PageSize);
                    var PageIndexPara = new SqlParameter("@PageIndex", PageIndex);
                    var SearchPara = new SqlParameter("@Search", Search);
                    var BusinessIdPara = new SqlParameter("@Businessid", Businessid);
                    var result = context.Database.SqlQuery<Listview>("sp_BusinessReview_selectIndexPaging @Businessid,@PageSize,@PageIndex,@Search", BusinessIdPara, PageSizePara, PageIndexPara, SearchPara).ToList();
                    return Json(SessionUtilities.ConvertDataTableTojSonString(SessionUtilities.LINQResultToDataTable(result)), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(string.Format("Exception {0}", ex.Message), JsonRequestBehavior.AllowGet);
            }
            //using (BusinessReviewCtl db = new BusinessReviewCtl())
            //{
            //    return Json(db.selectIndexPaging(PageSize, PageIndex, Search, Businessid), JsonRequestBehavior.AllowGet);
            //}
        }
    }
}