using ChossonKallah.Models;
using System;
using System.Web.Mvc;


namespace ChossonKallah.Controllers
{
    public class BusinessCategoryController : Controller
    {
        //private BusinessCategoryCtl db = new BusinessCategoryCtl();
        //{privateVariables}



        public ActionResult Create()
        {
            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                Session["CreatePreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
                return View();
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BusinessCategoryClass Obj_BusinessCategory, string command)
        {

            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                if (ModelState.IsValid)
                {
                    db.insert(Obj_BusinessCategory);
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

                return View(Obj_BusinessCategory);
            }
        }



        public ActionResult Edit(Int32? Businesscategoryid)
        {

            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                BusinessCategoryClass obj_BusinessCategory = db.selectById(Businesscategoryid);
                Session["EditPreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
                return View(obj_BusinessCategory);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BusinessCategoryClass Obj_BusinessCategory)
        {
            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                if (ModelState.IsValid)
                {
                    db.update(Obj_BusinessCategory);
                    string sesionval = Convert.ToString(Session["EditPreviousURL"]);
                    if (!string.IsNullOrEmpty(sesionval))
                    {
                        Session.Remove("EditPreviousURL");
                        return Redirect(sesionval);
                    }
                    else
                        return RedirectToAction("Index");
                }
                return View(Obj_BusinessCategory);
            }
        }



        public ActionResult Details(Int32? Businesscategoryid)
        {

            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                BusinessCategoryClass obj_BusinessCategory = db.selectById(Businesscategoryid);
                return View(obj_BusinessCategory);
            }
        }



        public ActionResult Delete(Int32? Businesscategoryid)
        {
            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                db.delete(Businesscategoryid);
                return RedirectToAction("Index");
            }
        }


        public ActionResult Index()
        {

            return View();
        }



        public ActionResult Indexpaging(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search));
            }
        }
        public Int32 IndexpagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                return db.selectIndexPagingCount(PageSize, PageIndex, Search);
            }
        }

        public ActionResult IndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                return PartialView(db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
            }
        }


        public ActionResult VIndex()
        {

            return View();
        }



        public ActionResult VIndexpaging(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search));
            }
        }
        public Int32 VIndexpagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                return db.selectIndexPagingCount(PageSize, PageIndex, Search);
            }
        }

        public ActionResult VIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                return PartialView(db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
            }
        }


        public ActionResult EditTableRowDelete(Int32? Businesscategoryid)
        {
            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                db.delete(Businesscategoryid);
                return RedirectToAction("EditTable");
            }
        }


        public ActionResult EditTable()
        {

            return View();
        }



        public ActionResult EditTablePaging(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search));
            }
        }
        public Int32 EditTablePagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                return db.selectIndexPagingCount(PageSize, PageIndex, Search);
            }
        }

        public ActionResult EditTableLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                return PartialView(db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
            }
        }

        [HttpPost]
        public ActionResult SaveRecords(FormCollection model)
        {
            if (ModelState.IsValid)
            {
                using (BusinessCategoryCtl db = new BusinessCategoryCtl())
                {
                    var BusinesscategoryidArray = model.GetValues("item.Businesscategoryid");
                    var BusinessidArray = model.GetValues("item.Businessid");
                    var CategoryidArray = model.GetValues("item.Categoryid");
                    for (Int32 i = 0; i < BusinesscategoryidArray.Length; i++)
                    {
                        BusinessCategoryClass obj_update = db.selectById(Convert.ToInt32(BusinesscategoryidArray[i]));
                        if (!string.IsNullOrEmpty(Convert.ToString(BusinesscategoryidArray)))
                            obj_update.Businesscategoryid = Convert.ToInt32(BusinesscategoryidArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(BusinessidArray)))
                            obj_update.Businessid = Convert.ToInt32(BusinessidArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(CategoryidArray)))
                            obj_update.Categoryid = Convert.ToInt32(CategoryidArray[i]);
                        db.update(obj_update);
                    }
                }
            }
            return RedirectToAction("EditTable");
        }

        public ActionResult EditTableRowsDelete(string records)
        {
            using (BusinessCategoryCtl db = new BusinessCategoryCtl())
            {
                foreach (string id in records.Trim(',').Split(','))
                {
                    if (!string.IsNullOrEmpty(id.Trim()))
                    {
                        db.delete(Convert.ToInt32(id));
                    }
                }
                return View();
            }
        }
        //{ActionResultMethod}




    }

}
