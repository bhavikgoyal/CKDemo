using ChossonKallah.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ChossonKallah.Controllers
{
    public class BusinessGalleryController : Controller
    {
        DataTable dt = new DataTable();



        public ActionResult Create()
        {
            using (BusinessGalleryCtl db = new BusinessGalleryCtl())
            {
                Session["CreatePreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
                return View();
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BusinessGalleryClass Obj_BusinessGallery, string command)
        {

            using (BusinessGalleryCtl db = new BusinessGalleryCtl())
            {
                if (ModelState.IsValid)
                {
                    db.insert(Obj_BusinessGallery);
                    if (command.ToLower().Trim() == "save")
                    {
                        string sesionval = Convert.ToString(Session["CreatePreviousURL"]);
                        if (!string.IsNullOrEmpty(sesionval))
                        {
                            Session.Remove("CreatePreviousURL");
                            return Redirect(sesionval);
                        }
                        else
                            return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.Clear();
                        return View();
                    }
                }

                return View(Obj_BusinessGallery);
            }
        }



        public ActionResult Edit(Int32? Businessgalleryid)
        {

            using (BusinessGalleryCtl db = new BusinessGalleryCtl())
            {
                BusinessGalleryClass obj_BusinessGallery = db.selectById(Businessgalleryid);
                Session["EditPreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
                return View(obj_BusinessGallery);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BusinessGalleryClass Obj_BusinessGallery)
        {
            using (BusinessGalleryCtl db = new BusinessGalleryCtl())
            {
                if (ModelState.IsValid)
                {
                    db.update(Obj_BusinessGallery);
                    string sesionval = Convert.ToString(Session["EditPreviousURL"]);
                    if (!string.IsNullOrEmpty(sesionval))
                    {
                        Session.Remove("EditPreviousURL");
                        return Redirect(sesionval);
                    }
                    else
                        return RedirectToAction("Index");
                }
                return View(Obj_BusinessGallery);
            }
        }



        public ActionResult Details(Int32? Businessgalleryid)
        {

            using (BusinessGalleryCtl db = new BusinessGalleryCtl())
            {
                BusinessGalleryClass obj_BusinessGallery = db.selectById(Businessgalleryid);
                return View(obj_BusinessGallery);
            }
        }



        public ActionResult Delete(Int32? Businessgalleryid)
        {
            using (BusinessGalleryCtl db = new BusinessGalleryCtl())
            {
                db.delete(Businessgalleryid);
                return RedirectToAction("Index");
            }
        }


        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public JsonResult SelectGalleryImages(Int32? Businessid)
        {
            using (BusinessGalleryCtl db = new BusinessGalleryCtl())
            {
                return Json(db.SelectGalleryImages(Businessid), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Index(string Businessid)
        {
            return View();
        }


        //[HttpPost]
        //public ActionResult UploadImages()
        //{
        //    Int32? BusinessId = 0;
        //    BusinessDirectoryClass obj_BusinessDirectory = new BusinessDirectoryClass();
        //    using (BusinessDirectoryCtl dbbdc = new BusinessDirectoryCtl())
        //    {
        //        try
        //        {
        //            for (int i = 0; i < Request.Form.Count; i++)
        //            {
        //                if (Request.Form.Keys[i] == "Businessid")
        //                {
        //                    BusinessId = Convert.ToInt32(Request.Form[i]);
        //                    if (BusinessId == 0)
        //                    {
        //                        Session.Remove("NewListingGallery");
        //                        List<BusinessGalleryClass> lstGallery = new List<BusinessGalleryClass>();
                                
        //                        for (int k = 0; k < Request.Form.Count; k++)
        //                        {
        //                            if (Request.Form.Keys[k] != "Businessid" && Request.Form.Keys[k] != "BusinessName")
        //                            {
        //                                if (!Directory.Exists(Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + Convert.ToString(Request.Form["BusinessName"]) + "/")))
        //                                {
        //                                    Directory.CreateDirectory(Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + Convert.ToString(Request.Form["BusinessName"]) + "/"));
        //                                }
        //                                string actFolder = Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + Convert.ToString(Request.Form["BusinessName"]) + "/");

        //                                if (Convert.ToString(Request.Form[k]).Contains("WebsiteImages/BusinessGallery"))
        //                                {

        //                                }
        //                                else
        //                                {
        //                                    byte[] imageBytes = Convert.FromBase64String(Request.Form[k].ToString().Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "").Replace(" ", "+"));
        //                                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
        //                                    ms.Write(imageBytes, 0, imageBytes.Length);
        //                                    System.Drawing.Image image2 = System.Drawing.Image.FromStream(ms, true);
        //                                    image2.Save(actFolder + Request.Form.Keys[k]);
        //                                }
        //                                BusinessGalleryClass bgc = new BusinessGalleryClass();
        //                                bgc.Businessid = BusinessId;
        //                                bgc.Imagename = Request.Form.Keys[k];
        //                                bgc.Sequence = i + 1;
        //                                lstGallery.Add(bgc);
        //                            }
        //                        }
        //                        Session["NewListingGallery"] = lstGallery;

        //                        return Json("Success", JsonRequestBehavior.AllowGet);
        //                    }
        //                    obj_BusinessDirectory = dbbdc.selectById(Convert.ToInt32(Request.Form[i]));
        //                }
        //            }
        //            using (BusinessGalleryCtl db = new BusinessGalleryCtl())
        //            {
        //                dt = db.selectByBusinessId(BusinessId);
        //                if (dt.Rows.Count > 0)
        //                {
        //                    for (int i = 0; i < dt.Rows.Count; i++)
        //                    {
        //                        string ImageName = Convert.ToString(dt.Rows[i]["ImageName"]);
        //                        Boolean HaveDelte = true;
        //                        for (int k= 0; k < Request.Form.Count; k++) {
        //                            if (ImageName.Trim() == Request.Form.Keys[k].Trim()) {
        //                                HaveDelte = false;
        //                            }
        //                        }

        //                        if (HaveDelte && System.IO.File.Exists(Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + obj_BusinessDirectory.Businessname + "/" + ImageName)))
        //                        {
        //                            System.IO.File.Delete(Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + obj_BusinessDirectory.Businessname + "/" + ImageName));
        //                        }
        //                    }
        //                    db.delete(BusinessId);
        //                }
        //                for (int i = 0; i < Request.Form.Count; i++)
        //                {
        //                    if (Request.Form.Keys[i] != "Businessid" && Request.Form.Keys[i] != "BusinessName")
        //                    {
        //                        if (!Directory.Exists(Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + obj_BusinessDirectory.Businessname + "/")))
        //                        {
        //                            Directory.CreateDirectory(Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + obj_BusinessDirectory.Businessname + "/"));
        //                        }
        //                        string actFolder = Server.MapPath("~/WebsiteImages/" + "BusinessGallery/" + obj_BusinessDirectory.Businessname + "/");

        //                        if (Convert.ToString(Request.Form[i]).Contains("WebsiteImages/BusinessGallery"))
        //                        {

        //                        }
        //                        else {
        //                            byte[] imageBytes = Convert.FromBase64String(Request.Form[i].ToString().Replace("data:image/png;base64,", "").Replace("data:image/jpg;base64,", "").Replace("data:image/jpeg;base64,", "").Replace(" ", "+"));
        //                            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
        //                            ms.Write(imageBytes, 0, imageBytes.Length);
        //                            System.Drawing.Image image2 = System.Drawing.Image.FromStream(ms, true);
        //                            image2.Save(actFolder + Request.Form.Keys[i]);
        //                        }
        //                        BusinessGalleryClass bgc = new BusinessGalleryClass();
        //                        bgc.Businessid = BusinessId;
        //                        bgc.Imagename = Request.Form.Keys[i];
        //                        bgc.Sequence = i + 1;
        //                        db.insert(bgc);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json("Exception: " + ex.Message, JsonRequestBehavior.AllowGet);
        //        }
        //        //return View("Index");
        //    }
        //    return Json("Success",JsonRequestBehavior.AllowGet);
        //}
    }
}
