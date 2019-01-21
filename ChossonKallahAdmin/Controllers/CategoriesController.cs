using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using ChossonKallah.Models;
using ChossonKallahAdmin.EF6;
using ChossonKallahAdmin.GlobalUtilities;

namespace ChossonKallah.Controllers
{
    public class CategoriesController : Controller
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
                    var Result = context.Database.SqlQuery<Category>("sp_Categories_Id_Name_selectall @Categoryid", CategoryidPara).ToList();
                    List<SelectListItem> IdName = new List<SelectListItem>();
                    for (int i = 0; i < Result.Count; i++)
                    {
                        SelectListItem obj = new SelectListItem();
                        obj.Text = Convert.ToString(Result[i].CategoryName);
                        obj.Value = Convert.ToString(Result[i].CategoryId);
                        IdName.Add(obj);
                    }
                    ViewBag.Categories = IdName;
                    return View();
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.CategoryExists = ex.Message;
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category Obj_Categories, string command)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var CategorynamePara = new SqlParameter("@Categoryname", Obj_Categories.CategoryName);
                    var result = context.Database.SqlQuery<Category>("sp_Category_CheckCategoryExists @Categoryname", CategorynamePara).ToList();
                    if (result.Count > 0)
                    {
                        ViewBag.CategoryExists = "Category name is already exists.";
                        Int32? Categoryid = 0;
                        var CategoryidPara = new SqlParameter("@Categoryid", Categoryid);
                        var Result = context.Database.SqlQuery<Category>("sp_Categories_Id_Name_selectall @Categoryid", CategoryidPara).ToList();
                        List<SelectListItem> IdName = new List<SelectListItem>();
                        for (int i = 0; i < Result.Count; i++)
                        {
                            SelectListItem obj = new SelectListItem();
                            obj.Text = Convert.ToString(Result[i].CategoryName);
                            obj.Value = Convert.ToString(Result[i].CategoryId);
                            IdName.Add(obj);
                        }
                        ViewBag.Categories = IdName;
                        return View(Obj_Categories);
                    }
                    else
                    {
                        Obj_Categories.CreatedOn = System.DateTime.Now;
                        Obj_Categories.IsDeleted = false;
                        context.Categories.Add(Obj_Categories);
                        context.SaveChanges();
                        TempData["AdminSuccess"] = "Record has been added successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.Categories = ex.Message;
                return View(Obj_Categories);
            }
        }

        public ActionResult Edit(Int32? Categoryid)
        {
            using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
            {
                try
                {
                    var CategoryidPara = new SqlParameter("@Categoryid", Categoryid);
                    var Result = context.Database.SqlQuery<Category>("sp_Categories_Id_Name_selectall @Categoryid", CategoryidPara).ToList();
                    List<SelectListItem> IdName = new List<SelectListItem>();
                    for (int i = 0; i < Result.Count; i++)
                    {
                        SelectListItem obj = new SelectListItem();
                        obj.Text = Convert.ToString(Result[i].CategoryName);
                        obj.Value = Convert.ToString(Result[i].CategoryId);
                        IdName.Add(obj);
                    }
                    ViewBag.Categories = IdName;
                    var CategoryId = new SqlParameter("@Categoryid", Categoryid);
                    var ResultCategory = context.Database.SqlQuery<Category>("sp_Categories_select @Categoryid", CategoryId).FirstOrDefault();
                    return View(ResultCategory);
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                        ViewBag.Categories = ex.Message;
                    return View();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category Obj_Categories)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var CategoryName = new SqlParameter("@CategoryName", Obj_Categories.CategoryName);
                    var CategoryId = new SqlParameter("@CategoryId", Obj_Categories.CategoryId);
                    var result = context.Database.SqlQuery<Category>("sp_Category_CheckCategoryExistsUpdate  @CategoryId,@CategoryName", CategoryId, CategoryName).ToList();
                    if (result.Count > 0)
                    {
                        ViewBag.CategoryExists = "Category name is already exists.";
                        var CategoryidPara = new SqlParameter("@Categoryid", Obj_Categories.CategoryId);
                        var Result = context.Database.SqlQuery<Category>("sp_Categories_Id_Name_selectall @Categoryid", CategoryidPara).ToList();
                        List<SelectListItem> IdName = new List<SelectListItem>();
                        for (int i = 0; i < Result.Count; i++)
                        {
                            SelectListItem obj = new SelectListItem();
                            obj.Text = Convert.ToString(Result[i].CategoryName);
                            obj.Value = Convert.ToString(Result[i].CategoryId);
                            IdName.Add(obj);
                        }
                        ViewBag.Categories = IdName;
                        return View(Obj_Categories);
                    }
                    else
                    {
                        context.Categories.Attach(Obj_Categories);
                        context.Entry(Obj_Categories).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        TempData["CategorySuccess"] = "Record has been update successfully.";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    ViewBag.CategoryExists = ex.Message;
                return View(Obj_Categories);
            }
        }


        public ActionResult Delete(Int32 CategoryId)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var CategoryIdPara = new SqlParameter("@CategoryId", CategoryId);
                    Category obj_Category = context.Database.SqlQuery<Category>("sp_Categories_select @CategoryId", CategoryIdPara).FirstOrDefault();
                    obj_Category.IsDeleted = true;
                    context.Entry(obj_Category).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    TempData["CategorySuccess"] = "Record has been deleted successfully.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("See 'EntityValidationErrors' property"))
                    TempData["CategoryError"] = ex.Message;
                return RedirectToAction("Index");
            }

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
                    var result = context.Database.SqlQuery<Listview>("sp_Categories_selectIndexPaging @PageSize,@PageIndex,@Search", PageSizePara, PageIndexPara, SearchPara).ToList();
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
