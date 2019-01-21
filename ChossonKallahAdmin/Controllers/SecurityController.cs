using ChossonKallah.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChossonKallahAdmin.GlobalUtilities;
using ChossonKallahAdmin.EF6;
using System.Data.SqlClient;

namespace ChossonKallahAdmin.Controllers
{
    public class SecurityController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult CheckUserNamePass(string email, string password)
        {
            try
            {
                using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
                {
                    var EmailPara = new SqlParameter("@Email", email);
                    var PasswordPara = new SqlParameter("@Password", password);
                    var result = context.Database.SqlQuery<Admin>("sp_admin_CheckUserNamePassLogin @Email,@Password", EmailPara, PasswordPara).FirstOrDefault();
                    if (!string.IsNullOrEmpty(Convert.ToString(result)))
                    {
                        SessionUtilities.AdminId = result.AdminId.ToString();
                        SessionUtilities.Username = result.Username.ToString();
                        return Json(string.Format("Success, {0} ", result.AdminId.ToString()));
                    }
                    else
                    {
                        return Json(string.Format("Error"));
                    }
                }
            }
            catch (Exception)
            {
                return Json(string.Format("Error"));
            }
            //AdminClass obj1 = new AdminClass();
            //AdminCtl obj = new AdminCtl();
            //obj1.Email = email;
            //obj1.Password = password;
            //DataTable dt = new DataTable();
            //List<AdminCtl> viewModelList = new List<AdminCtl>();
            //dt = obj.CheckUserNamePass(obj1);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    SessionUtilities.AdminId = dt.Rows[0]["AdminId"].ToString();
            //    SessionUtilities.Username = dt.Rows[0]["Username"].ToString();
            //    return Json(string.Format("Success, {0} ", dt.Rows[0]["AdminId"].ToString()));
            //}
            //else
            //{
            //    return Json(viewModelList, JsonRequestBehavior.AllowGet);
            //}
        }
    }
}