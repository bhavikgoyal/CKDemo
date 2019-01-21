using System;
using System.Web.Mvc;
using ChossonKallah.Models;

namespace ChossonKallah.Controllers
{
    public class CitiesController : Controller
    {
        //private CitiesCtl db = new CitiesCtl();
        //{privateVariables}



        public ActionResult Create()
        {
            using (CitiesCtl db = new CitiesCtl())
            {
                Session["CreatePreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
                return View();
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CitiesClass Obj_Cities, string command)
        {

            using (CitiesCtl db = new CitiesCtl())
            {
                if (ModelState.IsValid)
                {
                    db.insert(Obj_Cities);
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

                return View(Obj_Cities);
            }
        }



        public ActionResult Edit(Int32? Cityid)
        {

            using (CitiesCtl db = new CitiesCtl())
            {
                CitiesClass obj_Cities = db.selectById(Cityid);
                Session["EditPreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
                return View(obj_Cities);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CitiesClass Obj_Cities)
        {
            using (CitiesCtl db = new CitiesCtl())
            {
                if (ModelState.IsValid)
                {
                    db.update(Obj_Cities);
                    string sesionval = Convert.ToString(Session["EditPreviousURL"]);
                    if (!string.IsNullOrEmpty(sesionval))
                    {
                        Session.Remove("EditPreviousURL");
                        return Redirect(sesionval);
                    }
                    else
                        return RedirectToAction("Index");
                }
                return View(Obj_Cities);
            }
        }



        public ActionResult Details(Int32? Cityid)
        {

            using (CitiesCtl db = new CitiesCtl())
            {
                CitiesClass obj_Cities = db.selectById(Cityid);
                return View(obj_Cities);
            }
        }



        public ActionResult Delete(Int32? Cityid)
        {
            using (CitiesCtl db = new CitiesCtl())
            {
                db.delete(Cityid);
                return RedirectToAction("Index");
            }
        }


        public ActionResult Index()
        {

            return View();
        }



        public ActionResult Indexpaging(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (CitiesCtl db = new CitiesCtl())
            {
                return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search));
            }
        }
        public Int32 IndexpagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (CitiesCtl db = new CitiesCtl())
            {
                return db.selectIndexPagingCount(PageSize, PageIndex, Search);
            }
        }

        public ActionResult IndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            using (CitiesCtl db = new CitiesCtl())
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

            using (CitiesCtl db = new CitiesCtl())
            {
                return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search));
            }
        }
        public Int32 VIndexpagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (CitiesCtl db = new CitiesCtl())
            {
                return db.selectIndexPagingCount(PageSize, PageIndex, Search);
            }
        }

        public ActionResult VIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            using (CitiesCtl db = new CitiesCtl())
            {
                return PartialView(db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
            }
        }


        public ActionResult EditTableRowDelete(Int32? Cityid)
        {
            using (CitiesCtl db = new CitiesCtl())
            {
                db.delete(Cityid);
                return RedirectToAction("EditTable");
            }
        }


        public ActionResult EditTable()
        {

            return View();
        }



        public ActionResult EditTablePaging(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (CitiesCtl db = new CitiesCtl())
            {
                return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search));
            }
        }
        public Int32 EditTablePagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (CitiesCtl db = new CitiesCtl())
            {
                return db.selectIndexPagingCount(PageSize, PageIndex, Search);
            }
        }

        public ActionResult EditTableLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            using (CitiesCtl db = new CitiesCtl())
            {
                return PartialView(db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
            }
        }

        [HttpPost]
        public ActionResult SaveRecords(FormCollection model)
        {
            if (ModelState.IsValid)
            {
                using (CitiesCtl db = new CitiesCtl())
                {
                    var CityidArray = model.GetValues("item.Cityid");
                    var CitynameArray = model.GetValues("item.Cityname");
                    var StateidArray = model.GetValues("item.Stateid");
                    var IsactiveArray = model.GetValues("item.Isactive");
                    var IsdeletedArray = model.GetValues("item.Isdeleted");
                    var CreatedonArray = model.GetValues("item.Createdon");
                    var DeletedonArray = model.GetValues("item.Deletedon");
                    for (Int32 i = 0; i < CityidArray.Length; i++)
                    {
                        CitiesClass obj_update = db.selectById(Convert.ToInt32(CityidArray[i]));
                        if (!string.IsNullOrEmpty(Convert.ToString(CityidArray)))
                            obj_update.Cityid = Convert.ToInt32(CityidArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(CitynameArray)))
                            obj_update.Cityname = Convert.ToString(CitynameArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(StateidArray)))
                            obj_update.Stateid = Convert.ToInt32(StateidArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(IsactiveArray)))
                            obj_update.Isactive = Convert.ToBoolean(IsactiveArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(IsdeletedArray)))
                            obj_update.Isdeleted = Convert.ToBoolean(IsdeletedArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(CreatedonArray)))
                            obj_update.Createdon = Convert.ToDateTime(CreatedonArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(DeletedonArray)))
                            obj_update.Deletedon = Convert.ToDateTime(DeletedonArray[i]);
                        db.update(obj_update);
                    }
                }
            }
            return RedirectToAction("EditTable");
        }

        public ActionResult EditTableRowsDelete(string records)
        {
            using (CitiesCtl db = new CitiesCtl())
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
