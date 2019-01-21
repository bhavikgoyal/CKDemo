using ChossonKallah.Models;
using System;
using System.Web.Mvc;


namespace ChossonKallah.Controllers
{
    public class GeneralSettingsController : Controller
    {
        //private GeneralSettingsCtl db = new GeneralSettingsCtl();
        //{privateVariables}



        public ActionResult Create()
        {
            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                Session["CreatePreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
                return View();
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GeneralSettingsClass Obj_GeneralSettings, string command)
        {

            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                if (ModelState.IsValid)
                {
                    db.insert(Obj_GeneralSettings);
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

                return View(Obj_GeneralSettings);
            }
        }



        public ActionResult Edit(Int32? Generalsettings)
        {

            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                GeneralSettingsClass obj_GeneralSettings = db.selectById(Generalsettings);
                Session["EditPreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
                return View(obj_GeneralSettings);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GeneralSettingsClass Obj_GeneralSettings)
        {
            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                if (ModelState.IsValid)
                {
                    db.update(Obj_GeneralSettings);
                    string sesionval = Convert.ToString(Session["EditPreviousURL"]);
                    if (!string.IsNullOrEmpty(sesionval))
                    {
                        Session.Remove("EditPreviousURL");
                        return Redirect(sesionval);
                    }
                    else
                        return RedirectToAction("Index");
                }
                return View(Obj_GeneralSettings);
            }
        }



        public ActionResult Details(Int32? Generalsettings)
        {

            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                GeneralSettingsClass obj_GeneralSettings = db.selectById(Generalsettings);
                return View(obj_GeneralSettings);
            }
        }



        public ActionResult Delete(Int32? Generalsettings)
        {
            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                db.delete(Generalsettings);
                return RedirectToAction("Index");
            }
        }


        public ActionResult Index()
        {

            return View();
        }



        public ActionResult Indexpaging(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search));
            }
        }
        public Int32 IndexpagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                return db.selectIndexPagingCount(PageSize, PageIndex, Search);
            }
        }

        public ActionResult IndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
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

            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search));
            }
        }
        public Int32 VIndexpagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                return db.selectIndexPagingCount(PageSize, PageIndex, Search);
            }
        }

        public ActionResult VIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                return PartialView(db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
            }
        }


        public ActionResult EditTableRowDelete(Int32? Generalsettings)
        {
            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                db.delete(Generalsettings);
                return RedirectToAction("EditTable");
            }
        }


        public ActionResult EditTable()
        {

            return View();
        }



        public ActionResult EditTablePaging(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search));
            }
        }
        public Int32 EditTablePagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                return db.selectIndexPagingCount(PageSize, PageIndex, Search);
            }
        }

        public ActionResult EditTableLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
            {
                return PartialView(db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
            }
        }

        [HttpPost]
        public ActionResult SaveRecords(FormCollection model)
        {
            if (ModelState.IsValid)
            {
                using (GeneralSettingsCtl db = new GeneralSettingsCtl())
                {
                    var GeneralsettingsArray = model.GetValues("item.Generalsettings");
                    var KeyArray = model.GetValues("item.Key");
                    var DescrpitionArray = model.GetValues("item.Descrpition");
                    for (Int32 i = 0; i < GeneralsettingsArray.Length; i++)
                    {
                        GeneralSettingsClass obj_update = db.selectById(Convert.ToInt32(GeneralsettingsArray[i]));
                        if (!string.IsNullOrEmpty(Convert.ToString(GeneralsettingsArray)))
                            obj_update.Generalsettings = Convert.ToInt32(GeneralsettingsArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(KeyArray)))
                            obj_update.Key = Convert.ToString(KeyArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(DescrpitionArray)))
                            obj_update.Descrpition = Convert.ToString(DescrpitionArray[i]);
                        db.update(obj_update);
                    }
                }
            }
            return RedirectToAction("EditTable");
        }

        public ActionResult EditTableRowsDelete(string records)
        {
            using (GeneralSettingsCtl db = new GeneralSettingsCtl())
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
