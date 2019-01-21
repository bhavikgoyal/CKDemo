using System;
using System.Web.Mvc;
using ChossonKallah.Models;

namespace ChossonKallah.Controllers
{
    public class DemoController : Controller
    {
        public ActionResult AdminCreate()
        {
            return View();
        }
        public ActionResult AdminEdit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminCreate(AdminClass model)
        {
            return View("AdminIndex");
        }
        [HttpPost]
        public ActionResult AdminEdit(AdminClass model)
        {
            return View("AdminIndex");
        }
        public ActionResult AdminIndex()
        {
            return View();
        }

        public ActionResult LocationCreate()
        {
            return View();
        }
        public ActionResult LocationEdit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LocationCreate(LocationClass model)
        {
            return View("LocationIndex");
        }
        [HttpPost]
        public ActionResult LocationEdit(LocationClass model)
        {
            return View("LocationIndex");
        }
        public ActionResult LocationIndex()
        {
            return View();
        }

        public ActionResult CategoryCreate()
        {
            return View();
        }
        public ActionResult CategoryEdit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CategoryCreate(CategoriesClass model)
        {
            return View("CategoryIndex");
        }
        [HttpPost]
        public ActionResult CategoryEdit(CategoriesClass model)
        {
            return View("CategoryIndex");
        }
        public ActionResult CategoryIndex()
        {
            return View();
        }

        public ActionResult BusinessDirectoryCreate()
        {
            return View();
        }
        public ActionResult BusinessDirectoryEdit()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BusinessDirectoryCreate(BusinessDirectoryClass model)
        {
            return View("BusinessDirectoryIndex");
        }
        [HttpPost]
        public ActionResult BusinessDirectoryEdit(BusinessDirectoryClass model)
        {
            return View("BusinessDirectoryIndex");
        }
        public ActionResult BusinessDirectoryIndex()
        {
            return View();
        }
        public ActionResult DashBoard()
        {
            return View();
        }
    }
}
