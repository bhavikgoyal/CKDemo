using ChossonKallahAdmin.EF6;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ChossonKallahAdmin.GlobalUtilities
{
    public static class SessionUtilities
    {
        public static string AdminId
        {
            get { return Convert.ToString(HttpContext.Current.Session["AdminId"]); }
            set { HttpContext.Current.Session["AdminId"] = value; }
        }
        public static string LocationName
        {
            get { return Convert.ToString(HttpContext.Current.Session["LocationName"]); }
            set { HttpContext.Current.Session["LocationName"] = value; }
        }

        public static string Username
        {
            get
            { return Convert.ToString(HttpContext.Current.Session["Username"]); }
            set { HttpContext.Current.Session["Username"] = value; }
        }


        public static string ConvertDataTableTojSonString(DataTable dataTable)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer =
            new System.Web.Script.Serialization.JavaScriptSerializer();

            List<Dictionary<String, Object>> tableRows = new List<Dictionary<String, Object>>();

            Dictionary<String, Object> row;

            foreach (DataRow dr in dataTable.Rows)
            {
                row = new Dictionary<String, Object>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                tableRows.Add(row);
            }
            return serializer.Serialize(tableRows);
        }

        public static bool SaveImage(HttpPostedFileBase Bannername, string For)
        {
            if (Bannername != null)
            {
                string _filename = string.Empty;
                string actFolder = string.Empty;
                _filename = Path.GetFileName(Bannername.FileName);
                if (!Directory.Exists(HostingEnvironment.MapPath("~/WebsiteImages/" + For + "/")))
                {
                    Directory.CreateDirectory(HostingEnvironment.MapPath("~/WebsiteImages/" + For + "/"));
                }
                actFolder = Path.Combine(HostingEnvironment.MapPath("~/WebsiteImages/" + For + "/"), _filename);
                Bannername.SaveAs(actFolder);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool SaveImage(HttpPostedFileBase Bannername, string For, string BusinessName)
        {
            if (Bannername != null)
            {
                string _filename = string.Empty;
                string actFolder = string.Empty;
                _filename = Path.GetFileName(Bannername.FileName);
                if (!Directory.Exists(HostingEnvironment.MapPath("~/WebsiteImages/" + For + "/" + BusinessName + "/")))
                {
                    Directory.CreateDirectory(HostingEnvironment.MapPath("~/WebsiteImages/" + For + "/" + BusinessName + "/"));
                }
                actFolder = Path.Combine(HostingEnvironment.MapPath("~/WebsiteImages/" + For + "/" + BusinessName + "/"), _filename);
                Bannername.SaveAs(actFolder);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] columns = null;
            if (Linqlist == null) return dt;
            foreach (T Record in Linqlist)
            {
                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public enum Position : short
        {
            Top,
            Right,
            Bottom,
            Left
        }
        public static List<SelectListItem> GetState()
        {
            using (var context = new ChossonKallahAdmin.EF6.ChossonKallah())
            {
                DataTable dt = new DataTable();
                var Result = context.Database.SqlQuery<StateData>("sp_StateData_SelectAllForSelectInAddEditLocation").ToList();
                dt = SessionUtilities.LINQResultToDataTable(Result);
                List<SelectListItem> lstObj = new List<SelectListItem>();
                SelectListItem sl = new SelectListItem();
                sl.Text = "--Please Select--";
                sl.Value = "0";
                sl.Selected = true;
                lstObj.Add(sl);
                foreach (DataRow dr in dt.Rows)
                {
                    sl = new SelectListItem();
                    sl.Text = Convert.ToString(dr["FullName"]);
                    sl.Value = Convert.ToString(dr["FullName"]);
                    sl.Selected = false;
                    lstObj.Add(sl);
                }
                return lstObj;
            }
        }
        public static List<SelectListItem> GetCategories()
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
                return IdName;
            }
        }
    }
}