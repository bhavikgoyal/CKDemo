using ChossonKallah.Models;
using System;
using System.Web.Mvc;


namespace ChossonKallah.Controllers
{
    public class ContactUsDataController : Controller
    {
        //private ContactUsDataCtl db = new ContactUsDataCtl();
        //{privateVariables}



        public ActionResult Create()
        {
            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                Session["CreatePreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
                return View();
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactUsDataClass Obj_ContactUsData, string command)
        {

            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                if (ModelState.IsValid)
                {
                    db.insert(Obj_ContactUsData);
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

                return View(Obj_ContactUsData);
            }
        }



        public ActionResult Edit(Int32? Contactusdataid)
        {

            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                ContactUsDataClass obj_ContactUsData = db.selectById(Contactusdataid);
                Session["EditPreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
                return View(obj_ContactUsData);
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactUsDataClass Obj_ContactUsData)
        {
            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                if (ModelState.IsValid)
                {
                    db.update(Obj_ContactUsData);
                    string sesionval = Convert.ToString(Session["EditPreviousURL"]);
                    if (!string.IsNullOrEmpty(sesionval))
                    {
                        Session.Remove("EditPreviousURL");
                        return Redirect(sesionval);
                    }
                    else
                        return RedirectToAction("Index");
                }
                return View(Obj_ContactUsData);
            }
        }



        public ActionResult Details(Int32? Contactusdataid)
        {

            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                ContactUsDataClass obj_ContactUsData = db.selectById(Contactusdataid);
                return View(obj_ContactUsData);
            }
        }



        public ActionResult Delete(Int32? Contactusdataid)
        {
            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                db.delete(Contactusdataid);
                return RedirectToAction("Index");
            }
        }


        public ActionResult Index()
        {

            return View();
        }



        public ActionResult Indexpaging(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search));
            }
        }
        public Int32 IndexpagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                return db.selectIndexPagingCount(PageSize, PageIndex, Search);
            }
        }

        public ActionResult IndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            using (ContactUsDataCtl db = new ContactUsDataCtl())
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

            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search));
            }
        }
        public Int32 VIndexpagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                return db.selectIndexPagingCount(PageSize, PageIndex, Search);
            }
        }

        public ActionResult VIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                return PartialView(db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
            }
        }


        public ActionResult EditTableRowDelete(Int32? Contactusdataid)
        {
            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                db.delete(Contactusdataid);
                return RedirectToAction("EditTable");
            }
        }


        public ActionResult EditTable()
        {

            return View();
        }



        public ActionResult EditTablePaging(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search));
            }
        }
        public Int32 EditTablePagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {

            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                return db.selectIndexPagingCount(PageSize, PageIndex, Search);
            }
        }

        public ActionResult EditTableLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            using (ContactUsDataCtl db = new ContactUsDataCtl())
            {
                return PartialView(db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
            }
        }

        [HttpPost]
        public ActionResult SaveRecords(FormCollection model)
        {
            if (ModelState.IsValid)
            {
                using (ContactUsDataCtl db = new ContactUsDataCtl())
                {
                    var ContactusdataidArray = model.GetValues("item.Contactusdataid");
                    var NameArray = model.GetValues("item.Name");
                    var EmailArray = model.GetValues("item.Email");
                    var MessagesubjectArray = model.GetValues("item.Messagesubject");
                    var DetailedmessageArray = model.GetValues("item.Detailedmessage");
                    var AddeddatetimeArray = model.GetValues("item.Addeddatetime");
                    var AddedipArray = model.GetValues("item.Addedip");
                    for (Int32 i = 0; i < ContactusdataidArray.Length; i++)
                    {
                        ContactUsDataClass obj_update = db.selectById(Convert.ToInt32(ContactusdataidArray[i]));
                        if (!string.IsNullOrEmpty(Convert.ToString(ContactusdataidArray)))
                            obj_update.Contactusdataid = Convert.ToInt32(ContactusdataidArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(NameArray)))
                            obj_update.Name = Convert.ToString(NameArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(EmailArray)))
                            obj_update.Email = Convert.ToString(EmailArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(MessagesubjectArray)))
                            obj_update.Messagesubject = Convert.ToString(MessagesubjectArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(DetailedmessageArray)))
                            obj_update.Detailedmessage = Convert.ToString(DetailedmessageArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(AddeddatetimeArray)))
                            obj_update.Addeddatetime = Convert.ToDateTime(AddeddatetimeArray[i]);
                        if (!string.IsNullOrEmpty(Convert.ToString(AddedipArray)))
                            obj_update.Addedip = Convert.ToString(AddedipArray[i]);
                        db.update(obj_update);
                    }
                }
            }
            return RedirectToAction("EditTable");
        }

        public ActionResult EditTableRowsDelete(string records)
        {
            using (ContactUsDataCtl db = new ContactUsDataCtl())
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
