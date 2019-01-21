using ChossonKallahAdmin.GlobalUtilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChossonKallahAdmin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string LocationName = null)
        {
            if (!string.IsNullOrEmpty(LocationName))
            {
                SessionUtilities.LocationName = LocationName;
                return RedirectToAction("Category", new { LocationName = LocationName });
            }
            else
                return View();
        }
        public ActionResult Category(string LocationName = null)
        {
            if (!string.IsNullOrEmpty(LocationName))
            {

            }
            return View();

        }
        public JsonResult GetLocations()
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var result = context.Database.SqlQuery<EF6.Location>("sp_Location_select_Front").ToList();
                    return Json(SessionUtilities.ConvertDataTableTojSonString(SessionUtilities.LINQResultToDataTable(result)), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(string.Format("Exception {0}", ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetBanner()
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var result = context.Database.SqlQuery<EF6.WebsiteBanner>("sp_WebSitebanner_select_Front").ToList();
                    return Json(SessionUtilities.ConvertDataTableTojSonString(SessionUtilities.LINQResultToDataTable(result)), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(string.Format("Exception {0}", ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCategories()
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var result = context.Database.SqlQuery<EF6.Category>("sp_Category_select_Front").ToList();
                    List<EF6.CategoryClass> LstCat = new List<EF6.CategoryClass>();
                    for (int i = 0; i < result.Count; i++)
                    {
                        EF6.CategoryClass childcat = new EF6.CategoryClass();
                        childcat.CategoryId = result[i].CategoryId;
                        childcat.CategoryName = result[i].CategoryName;
                        LstCat.Add(childcat);
                    }
                    for (int i = 0; i < LstCat.Count; i++)
                    {
                        var CategoryIdPara = new SqlParameter("@CategoryId", LstCat[i].CategoryId);
                        var SubCat = context.Database.SqlQuery<EF6.CategoryClass>("sp_Category_Sub_select_Front @CategoryId", CategoryIdPara).ToList();
                        LstCat[i].childCat = new List<EF6.CategoryClass>();
                        for (int j = 0; j < SubCat.Count; j++)
                        {
                            EF6.CategoryClass childcat = new EF6.CategoryClass();
                            childcat.CategoryId = SubCat[j].CategoryId;
                            childcat.CategoryName = SubCat[j].CategoryName;
                            LstCat[i].childCat.Add(childcat);
                        }
                    }


                    return Json(SessionUtilities.ConvertDataTableTojSonString(SessionUtilities.LINQResultToDataTable(LstCat)), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(string.Format("Exception {0}", ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetListingDataByCategory(Int32 CategoryId)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var CategoryIdPara = new SqlParameter("@CategoryId", CategoryId);
                    var result = context.Database.SqlQuery<EF6.BusinessDirectoryClass>("sp_BusinessDirectory_select_Front @CategoryId", CategoryIdPara).ToList();
                    List<EF6.BusinessDirectoryClass> LstCat = new List<EF6.BusinessDirectoryClass>();
                    for (int i = 0; i < result.Count; i++)
                    {
                        EF6.BusinessDirectoryClass childcat = new EF6.BusinessDirectoryClass();
                        childcat.BusinessName = result[i].BusinessName;
                        childcat.BusinessID = result[i].BusinessID;
                        childcat.Website = result[i].Website;
                        childcat.BusinessURL = result[i].BusinessURL;
                        childcat.PhoneNumber = result[i].PhoneNumber;
                        childcat.BusinessImage = result[i].BusinessImage;
                        childcat.LocationId = result[i].LocationId;
                        childcat.Address = result[i].Address;
                        childcat.AddressLine2 = result[i].AddressLine2;
                        childcat.City = result[i].City;
                        childcat.State = result[i].State;
                        childcat.Zipcode = result[i].Zipcode;
                        
                        childcat.IsActive = result[i].IsActive;
                        childcat.IsDeleted = result[i].IsDeleted;
                        childcat.CreatedOn = result[i].CreatedOn;
                        childcat.DeletedOn = result[i].DeletedOn;
                        childcat.PhoneNumber2 = result[i].PhoneNumber2;
                        childcat.Email = result[i].Email;
                        childcat.HasBrochure = result[i].HasBrochure;
                        childcat.IsFeatured = result[i].IsFeatured;
                        childcat.BusinessVideoURL = result[i].BusinessVideoURL;
                        childcat.BusinessLogo = result[i].BusinessLogo;
                        LstCat.Add(childcat);
                    }
                    for (int i = 0; i < LstCat.Count; i++)
                    {
                        var BusinessIdPara = new SqlParameter("@BusinessID", LstCat[i].BusinessID);
                        var SubCat = context.Database.SqlQuery<EF6.BusinessGalleryClass>("sp_Business_Gallery_select_Front @BusinessID", BusinessIdPara).ToList();
                        LstCat[i].Gallery = new List<EF6.BusinessGalleryClass>();
                        for (int j = 0; j < SubCat.Count; j++)
                        {
                            EF6.BusinessGalleryClass childcat = new EF6.BusinessGalleryClass();
                            childcat.BusinessGalleryID= SubCat[j].BusinessGalleryID;
                            childcat.BusinessID = SubCat[j].BusinessID;
                            childcat.ImageName = SubCat[j].ImageName;
                            childcat.IsActive = SubCat[j].IsActive;
                            childcat.Sequence = SubCat[j].Sequence;
                            LstCat[i].Gallery.Add(childcat);
                        }
                    }


                    return Json(SessionUtilities.ConvertDataTableTojSonString(SessionUtilities.LINQResultToDataTable(LstCat)), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(string.Format("Exception {0}", ex.Message), JsonRequestBehavior.AllowGet);
            }
        }
    }
}