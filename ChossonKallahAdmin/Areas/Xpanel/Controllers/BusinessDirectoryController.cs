using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChossonKallah.Models;
using ChossonKallahAdmin.EF6;
using ChossonKallahAdmin.GlobalUtilities;

namespace ChossonKallahAdmin.Areas.Xpanel.Controllers
{

    public class BusinessDirectoryController : Controller
    {
        DataTable dt = new DataTable();
        public ActionResult Create()
        {
            try
            {

                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    Int32? Categoryid = 0;
                    var CategoryidPara = new SqlParameter("@Categoryid", Categoryid);
                    var CategoryResult = context.Database.SqlQuery<Category>("sp_Categories_Id_Name_selectall @Categoryid", CategoryidPara).ToList();
                    List<SelectListItem> CategoryIdName = new List<SelectListItem>();
                    for (int i = 0; i < CategoryResult.Count; i++)
                    {
                        SelectListItem obj = new SelectListItem();
                        obj.Text = Convert.ToString(CategoryResult[i].CategoryName);
                        obj.Value = Convert.ToString(CategoryResult[i].CategoryId);
                        CategoryIdName.Add(obj);
                    }
                    ViewBag.Categories = CategoryIdName;
                    var LocationResult = context.Database.SqlQuery<Location>("sp_Location_Id_Name_selectall").ToList();
                    List<SelectListItem> LocationIdName = new List<SelectListItem>();
                    for (int i = 0; i < LocationResult.Count; i++)
                    {
                        SelectListItem obj = new SelectListItem();
                        obj.Text = Convert.ToString(LocationResult[i].LocationName);
                        obj.Value = Convert.ToString(LocationResult[i].LocationId);
                        LocationIdName.Add(obj);
                    }
                    ViewBag.Locations = LocationIdName;
                    return View();
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.BusinessDirectoryError = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BusinessDirectory Obj_BusinessDirectory, string command, HttpPostedFileBase Businessimage, HttpPostedFileBase BusinessLogo, string Categories)
        {
            try
            {
                string BusinessLogoName = string.Empty;
                string BusinessimageName = string.Empty;
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var EmailPara = new SqlParameter("@Email", Obj_BusinessDirectory.Email);
                    var result = context.Database.SqlQuery<BusinessDirectory>("sp_Admin_CheckBusinessDirectoryExists @Email", EmailPara).ToList();
                    if (result.Count > 0)
                    {
                        Int32? Categoryid = 0;
                        var CategoryidPara = new SqlParameter("@Categoryid", Categoryid);
                        var CategoryResult = context.Database.SqlQuery<Category>("sp_Categories_Id_Name_selectall @Categoryid", CategoryidPara).ToList();
                        List<SelectListItem> CategoryIdName = new List<SelectListItem>();
                        for (int i = 0; i < CategoryResult.Count; i++)
                        {
                            SelectListItem obj = new SelectListItem();
                            obj.Text = Convert.ToString(CategoryResult[i].CategoryName);
                            obj.Value = Convert.ToString(CategoryResult[i].CategoryId);
                            CategoryIdName.Add(obj);
                        }
                        ViewBag.Categories = CategoryIdName;
                        var LocationResult = context.Database.SqlQuery<Location>("sp_Location_Id_Name_selectall").ToList();
                        List<SelectListItem> LocationIdName = new List<SelectListItem>();
                        for (int i = 0; i < LocationResult.Count; i++)
                        {
                            SelectListItem obj = new SelectListItem();
                            obj.Text = Convert.ToString(LocationResult[i].LocationName);
                            obj.Value = Convert.ToString(LocationResult[i].LocationId);
                            LocationIdName.Add(obj);
                        }
                        ViewBag.Locations = LocationIdName;
                        ViewBag.BusinessDirectoryExists = "Email is already exists.";
                        return View(Obj_BusinessDirectory);
                    }
                    else
                    {
                        Int32? Categoryid = 0;
                        var CategoryidPara = new SqlParameter("@Categoryid", Categoryid);
                        var CategoryResult = context.Database.SqlQuery<Category>("sp_Categories_Id_Name_selectall @Categoryid", CategoryidPara).ToList();
                        List<SelectListItem> CategoryIdName = new List<SelectListItem>();
                        for (int i = 0; i < CategoryResult.Count; i++)
                        {
                            SelectListItem obj = new SelectListItem();
                            obj.Text = Convert.ToString(CategoryResult[i].CategoryName);
                            obj.Value = Convert.ToString(CategoryResult[i].CategoryId);
                            CategoryIdName.Add(obj);
                        }
                        ViewBag.Categories = CategoryIdName;
                        var LocationResult = context.Database.SqlQuery<Location>("sp_Location_Id_Name_selectall").ToList();
                        List<SelectListItem> LocationIdName = new List<SelectListItem>();
                        for (int i = 0; i < LocationResult.Count; i++)
                        {
                            SelectListItem obj = new SelectListItem();
                            obj.Text = Convert.ToString(LocationResult[i].LocationName);
                            obj.Value = Convert.ToString(LocationResult[i].LocationId);
                            LocationIdName.Add(obj);
                        }
                        ViewBag.Locations = LocationIdName;
                        if (SessionUtilities.SaveImage(Businessimage, "BusinessImage", Obj_BusinessDirectory.BusinessName.Trim()))
                        {
                            BusinessimageName = Path.GetFileName(Businessimage.FileName);
                            Obj_BusinessDirectory.BusinessImage = BusinessimageName;
                        }
                        if (SessionUtilities.SaveImage(BusinessLogo, "Logo", Obj_BusinessDirectory.BusinessName.Trim()))
                        {
                            BusinessLogoName = Path.GetFileName(BusinessLogo.FileName);
                            Obj_BusinessDirectory.BusinessLogo = BusinessLogoName;
                        }
                        Obj_BusinessDirectory.CreatedOn = System.DateTime.Now;
                        Obj_BusinessDirectory.IsDeleted = false;
                        context.BusinessDirectories.Add(Obj_BusinessDirectory);
                        context.SaveChanges();
                        var BusinessIdPara = new SqlParameter("@businessid", Obj_BusinessDirectory.BusinessID);
                        var CategoriesPara = new SqlParameter("@Categories", Categories);
                        context.Database.ExecuteSqlCommand("sp_BusinessDirectory_Category_insert @Categories,@businessid", CategoriesPara, BusinessIdPara);
                        List<BusinessGallery> lstGallery = new List<BusinessGallery>();

                        if (!string.IsNullOrEmpty(Convert.ToString(Session["NewListingGallery"])))
                        {
                            var BusinessGalleryidDeletePara = new SqlParameter("@businessgalleryid", Obj_BusinessDirectory.BusinessID);
                            context.Database.ExecuteSqlCommand("sp_BusinessGallery_delete @businessgalleryid", BusinessGalleryidDeletePara);
                            lstGallery = (List<BusinessGallery>)Session["NewListingGallery"];
                        }
                        BusinessGallery GalObj = new BusinessGallery();
                        for (int i = 0; i < lstGallery.Count; i++)
                        {
                            GalObj.BusinessID = Obj_BusinessDirectory.BusinessID;
                            GalObj.IsActive = true;
                            GalObj.Sequence = i + 1;
                            context.BusinessGalleries.Add(GalObj);
                            context.SaveChanges();
                        }
                        TempData["BusinessSuccess"] = "Record has been added successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                return View(Obj_BusinessDirectory);
            }
        }

        public ActionResult Edit(Int32? Businessid)
        {
            using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
            {
                try
                {

                    Int32? Categoryid = 0;
                    var CategoryidPara = new SqlParameter("@Categoryid", Categoryid);
                    var CategoryResult = context.Database.SqlQuery<Category>("sp_Categories_Id_Name_selectall @Categoryid", CategoryidPara).ToList();
                    List<SelectListItem> CategoryIdName = new List<SelectListItem>();
                    for (int i = 0; i < CategoryResult.Count; i++)
                    {
                        SelectListItem obj = new SelectListItem();
                        obj.Text = Convert.ToString(CategoryResult[i].CategoryName);
                        obj.Value = Convert.ToString(CategoryResult[i].CategoryId);
                        CategoryIdName.Add(obj);
                    }
                    ViewBag.Categories = CategoryIdName;
                    var LocationResult = context.Database.SqlQuery<Location>("sp_Location_Id_Name_selectall").ToList();
                    List<SelectListItem> LocationIdName = new List<SelectListItem>();
                    for (int i = 0; i < LocationResult.Count; i++)
                    {
                        SelectListItem obj = new SelectListItem();
                        obj.Text = Convert.ToString(LocationResult[i].LocationName);
                        obj.Value = Convert.ToString(LocationResult[i].LocationId);
                        LocationIdName.Add(obj);
                    }
                    ViewBag.Locations = LocationIdName;
                    var BusinessidPara = new SqlParameter("@Businessid", Businessid);
                    BusinessDirectory obj_BusinessDirectory = context.Database.SqlQuery<BusinessDirectory>("sp_BusinessDirectory_select @Businessid", BusinessidPara).FirstOrDefault();
                    var CategoryIdWithComma = string.Empty;
                    var BusinessCategoryidPara = new SqlParameter("@Businessid", Businessid);
                    var CategoryResultByBusiness = context.Database.SqlQuery<BusinessCategory>("sp_BusinessCategory_select @Businessid", BusinessCategoryidPara).ToList();
                    for (int i = 0; i < CategoryResultByBusiness.Count; i++)
                    {
                        CategoryIdWithComma += CategoryResultByBusiness[i].CategoryID + ",";
                    }
                    ViewBag.CategoryIdWithComma = CategoryIdWithComma;
                    return View(obj_BusinessDirectory);
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                        TempData["BusinessError"] = ex.Message;
                    return View();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BusinessDirectory Obj_BusinessDirectory, HttpPostedFileBase Businessimage, HttpPostedFileBase BusinessLogo, string Categories)
        {
            try
            {
                string BusinessLogoName = string.Empty;
                string BusinessimageName = string.Empty;
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var EmailPara = new SqlParameter("@Email", Obj_BusinessDirectory.Email);
                    var BusinessIdPara = new SqlParameter("@businessdirectoryid", Obj_BusinessDirectory.BusinessID);
                    var result = context.Database.SqlQuery<BusinessDirectory>("sp_Admin_CheckBusinessdirectoryExistsUpdate @businessdirectoryid,@Email", BusinessIdPara, EmailPara).ToList();
                    if (result.Count > 0)
                    {
                        Int32? Categoryid = 0;
                        var CategoryidPara = new SqlParameter("@Categoryid", Categoryid);
                        var CategoryResult = context.Database.SqlQuery<Category>("sp_Categories_Id_Name_selectall @Categoryid", CategoryidPara).ToList();
                        List<SelectListItem> CategoryIdName = new List<SelectListItem>();
                        for (int i = 0; i < CategoryResult.Count; i++)
                        {
                            SelectListItem obj = new SelectListItem();
                            obj.Text = Convert.ToString(CategoryResult[i].CategoryName);
                            obj.Value = Convert.ToString(CategoryResult[i].CategoryId);
                            CategoryIdName.Add(obj);
                        }
                        ViewBag.Categories = CategoryIdName;
                        var LocationResult = context.Database.SqlQuery<Location>("sp_Location_Id_Name_selectall").ToList();
                        List<SelectListItem> LocationIdName = new List<SelectListItem>();
                        for (int i = 0; i < LocationResult.Count; i++)
                        {
                            SelectListItem obj = new SelectListItem();
                            obj.Text = Convert.ToString(LocationResult[i].LocationName);
                            obj.Value = Convert.ToString(LocationResult[i].LocationId);
                            LocationIdName.Add(obj);
                        }
                        ViewBag.Locations = LocationIdName;
                        ViewBag.BusinessDirectoryExists = "Email is already exists.";
                        return View(Obj_BusinessDirectory);
                    }
                    else
                    {
                        Int32? Categoryid = 0;
                        var CategoryidPara = new SqlParameter("@Categoryid", Categoryid);
                        var CategoryResult = context.Database.SqlQuery<Category>("sp_Categories_Id_Name_selectall @Categoryid", CategoryidPara).ToList();
                        List<SelectListItem> CategoryIdName = new List<SelectListItem>();
                        for (int i = 0; i < CategoryResult.Count; i++)
                        {
                            SelectListItem obj = new SelectListItem();
                            obj.Text = Convert.ToString(CategoryResult[i].CategoryName);
                            obj.Value = Convert.ToString(CategoryResult[i].CategoryId);
                            CategoryIdName.Add(obj);
                        }
                        ViewBag.Categories = CategoryIdName;
                        var LocationResult = context.Database.SqlQuery<Location>("sp_Location_Id_Name_selectall").ToList();
                        List<SelectListItem> LocationIdName = new List<SelectListItem>();
                        for (int i = 0; i < LocationResult.Count; i++)
                        {
                            SelectListItem obj = new SelectListItem();
                            obj.Text = Convert.ToString(LocationResult[i].LocationName);
                            obj.Value = Convert.ToString(LocationResult[i].LocationId);
                            LocationIdName.Add(obj);
                        }
                        ViewBag.Locations = LocationIdName;
                        if (SessionUtilities.SaveImage(Businessimage, "BusinessImage", Obj_BusinessDirectory.BusinessName))
                        {
                            BusinessimageName = Path.GetFileName(Businessimage.FileName);
                            Obj_BusinessDirectory.BusinessImage = BusinessimageName;
                        }
                        if (SessionUtilities.SaveImage(BusinessLogo, "Logo", Obj_BusinessDirectory.BusinessName))
                        {
                            BusinessLogoName = Path.GetFileName(BusinessLogo.FileName);
                            Obj_BusinessDirectory.BusinessLogo = BusinessLogoName;
                        }
                        context.BusinessDirectories.Attach(Obj_BusinessDirectory);
                        context.Entry(Obj_BusinessDirectory).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        var BusinessCategoryIdPara = new SqlParameter("@businessid", Obj_BusinessDirectory.BusinessID);
                        var CategoriesPara = new SqlParameter("@Categories", Categories);
                        context.Database.ExecuteSqlCommand("sp_BusinessDirectory_Category_insert @Categories,@businessid", CategoriesPara, BusinessCategoryIdPara);

                        TempData["BusinessSuccess"] = "Record has been updated successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                return View(Obj_BusinessDirectory);
            }
        }

        public ActionResult Delete(Int32? Businessid)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var BusinessidPara = new SqlParameter("@Businessid", Businessid);
                    BusinessDirectory obj_BusinessDirectory = context.Database.SqlQuery<BusinessDirectory>("sp_BusinessDirectory_select @Businessid", BusinessidPara).FirstOrDefault();
                    obj_BusinessDirectory.IsDeleted = true;
                    context.Entry(obj_BusinessDirectory).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    TempData["BusinessSuccess"] = "Record has been deleted successfully.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    TempData["BusinessError"] = ex.Message;
                return RedirectToAction("Index");
            }
            //using (BusinessDirectoryCtl db = new BusinessDirectoryCtl())
            //{
            //    db.delete(Businessid);
            //    TempData["BusinessSuccess"] = "Record has been deleted successfully.";
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
                    var result = context.Database.SqlQuery<Listview>("sp_BusinessDirectory_selectIndexPaging @PageSize,@PageIndex,@Search", PageSizePara, PageIndexPara, SearchPara).ToList();
                    return Json(SessionUtilities.ConvertDataTableTojSonString(SessionUtilities.LINQResultToDataTable(result)), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(string.Format("Exception {0}", ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UploadImages()
        {
            using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
            {
                try
                {
                    BusinessDirectory obj_BusinessDirectory = new BusinessDirectory();
                    Int32? BusinessId = 0;
                    for (int i = 0; i < Request.Form.Count; i++)
                    {
                        if (Request.Form.Keys[i] == "Businessid")
                        {
                            BusinessId = Convert.ToInt32(Request.Form[i]);
                            if (BusinessId == 0)
                            {
                                Session.Remove("NewListingGallery");
                                List<BusinessGallery> lstGallery = new List<BusinessGallery>();
                                for (int k = 0; k < Request.Form.Count; k++)
                                {
                                    if (Request.Form.Keys[k] != "Businessid" && Request.Form.Keys[k] != "BusinessName")
                                    {
                                        if (!Directory.Exists(Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + Convert.ToString(Request.Form["BusinessName"].Trim()) + "/")))
                                        {
                                            Directory.CreateDirectory(Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + Convert.ToString(Request.Form["BusinessName"].Trim()) + "/"));
                                        }
                                        string actFolder = Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + Convert.ToString(Request.Form["BusinessName"].Trim()) + "/");

                                        if (Convert.ToString(Request.Form[k]).Contains("WebsiteImages/BusinessGallery"))
                                        {

                                        }
                                        else
                                        {
                                            byte[] imageBytes = Convert.FromBase64String(Request.Form[k].ToString().Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "").Replace(" ", "+"));
                                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                            ms.Write(imageBytes, 0, imageBytes.Length);
                                            System.Drawing.Image image2 = System.Drawing.Image.FromStream(ms, true);
                                            image2.Save(actFolder + Request.Form.Keys[k]);
                                        }
                                        BusinessGallery bgc = new BusinessGallery();
                                        bgc.BusinessID = BusinessId;
                                        bgc.ImageName = Request.Form.Keys[k];
                                        bgc.Sequence = i + 1;
                                        lstGallery.Add(bgc);
                                    }
                                }
                                Session["NewListingGallery"] = lstGallery;

                                return Json("Success", JsonRequestBehavior.AllowGet);
                            }
                            var BusinessidPara = new SqlParameter("@Businessid", Request.Form[i]);
                            obj_BusinessDirectory = context.Database.SqlQuery<BusinessDirectory>("sp_BusinessDirectory_select @Businessid", BusinessidPara).FirstOrDefault();
                        }
                    }
                    var BusinessGalleryidPara = new SqlParameter("@businessgalleryid", BusinessId);
                    var obj_BusinessGallery = context.Database.SqlQuery<BusinessGallery>("sp_BusinessGallery_select @businessgalleryid", BusinessGalleryidPara).ToList();
                    if (obj_BusinessGallery.Count > 0)
                    {
                        for (int i = 0; i < obj_BusinessGallery.Count; i++)
                        {
                            string ImageName = Convert.ToString(obj_BusinessGallery[i].ImageName);
                            Boolean HaveDelte = true;
                            for (int k = 0; k < Request.Form.Count; k++)
                            {
                                if (ImageName.Trim() == Request.Form.Keys[k].Trim())
                                {
                                    HaveDelte = false;
                                }
                            }

                            if (HaveDelte && System.IO.File.Exists(Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + obj_BusinessDirectory.BusinessName.Trim() + "/" + ImageName)))
                            {
                                System.IO.File.Delete(Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + obj_BusinessDirectory.BusinessName.Trim() + "/" + ImageName));
                            }
                        }
                        var BusinessGalleryidDeletePara = new SqlParameter("@businessgalleryid", BusinessId);
                        context.Database.ExecuteSqlCommand("sp_BusinessGallery_delete @businessgalleryid", BusinessGalleryidDeletePara);
                    }
                    for (int i = 0; i < Request.Form.Count; i++)
                    {
                        if (Request.Form.Keys[i] != "Businessid" && Request.Form.Keys[i] != "BusinessName")
                        {
                            if (!Directory.Exists(Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + obj_BusinessDirectory.BusinessName.Trim() + "/")))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + obj_BusinessDirectory.BusinessName.Trim() + "/"));
                            }
                            string actFolder = Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + obj_BusinessDirectory.BusinessName.Trim() + "/");

                            if (Convert.ToString(Request.Form[i]).Contains("WebsiteImages/BusinessGallery"))
                            {

                            }
                            else
                            {
                                byte[] imageBytes = Convert.FromBase64String(Request.Form[i].ToString().Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "").Replace(" ", "+"));
                                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                                ms.Write(imageBytes, 0, imageBytes.Length);
                                System.Drawing.Image image2 = System.Drawing.Image.FromStream(ms, true);
                                image2.Save(actFolder + Request.Form.Keys[i]);
                            }
                            BusinessGallery bgc = new BusinessGallery();
                            bgc.BusinessID = BusinessId;
                            bgc.ImageName = Request.Form.Keys[i];
                            bgc.Sequence = i + 1;
                            bgc.IsActive = true;
                            context.BusinessGalleries.Add(bgc);
                            context.SaveChanges();
                        }
                    }
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Exception: " + ex.Message, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public JsonResult SelectGalleryImages(Int32? Businessid)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var BusinessidPara = new SqlParameter("@Businessid", Businessid);
                    var obj_BusinessDirectory = context.Database.SqlQuery<Listview>("sp_BusinessGallery_SelectImagesByBusiness @Businessid", BusinessidPara).ToList();
                    return Json(obj_BusinessDirectory, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
