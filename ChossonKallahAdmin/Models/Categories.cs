using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Mvc;
using DB_con;

namespace ChossonKallah.Models
{
    public class CategoriesClass
    {
        #region "properties"
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Categoryid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Categoryname { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Parentcategoryid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Categoryurl { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isfeaturedcategory { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isactive { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isdeleted { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> Createdon { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> Deletedon { get; set; }



        public CategoriesClass()
        {
        }
        #endregion
    }

    public class CategoriesCtl : IDisposable
    {
        #region "constructors"

        ConnectionCls obj_con = null;
        //Default Constructor
        public CategoriesCtl()
        {
            obj_con = new ConnectionCls();
        }

        //Select Constructor
        public CategoriesCtl(Int32? id)
        {
            obj_con = new ConnectionCls();
            CategoriesClass obj_Cat = new CategoriesClass();
            using (DataTable dt = selectdatatable(id))
            {
                if (dt.Rows.Count > 0)
                {

                    obj_Cat.Categoryid = Convert.ToInt32(dt.Rows[0]["Categoryid"]);
                    obj_Cat.Categoryname = Convert.ToString(dt.Rows[0]["Categoryname"]);
                    obj_Cat.Parentcategoryid = Convert.ToInt32(dt.Rows[0]["Parentcategoryid"]);
                    obj_Cat.Categoryurl = Convert.ToString(dt.Rows[0]["Categoryurl"]);
                    obj_Cat.Isfeaturedcategory = Convert.ToBoolean(dt.Rows[0]["Isfeaturedcategory"]);
                    obj_Cat.Isactive = Convert.ToBoolean(dt.Rows[0]["Isactive"]);
                    obj_Cat.Isdeleted = Convert.ToBoolean(dt.Rows[0]["Isdeleted"]);
                    obj_Cat.Createdon = Convert.ToDateTime(dt.Rows[0]["Createdon"]);
                    obj_Cat.Deletedon = Convert.ToDateTime(dt.Rows[0]["Deletedon"]);

                }
            }
        }


        #endregion

        #region "methods"

        //insert data into database 
        public Int32? insert(CategoriesClass obj)
        {
            try
            {
                obj.Categoryid = 0;
                obj_con.clearParameter();
                createParameter(obj, DBTrans.Insert);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_Categories_insert", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Categoryid = Convert.ToInt32(obj_con.getValue("@Categoryid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_Categories_insert:" + ex.Message);
            }
        }

        //update data into database 
        public Int32? update(CategoriesClass obj)
        {
            try
            {
                obj_con.clearParameter();
                createParameter(obj, DBTrans.Update);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_Categories_update", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Categoryid = Convert.ToInt32(obj_con.getValue("@Categoryid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_Categories_update:" + ex.Message);
            }
        }

        //delete data from database 
        public void delete(Int32? Categoryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.BeginTransaction();
                obj_con.addParameter("@Categoryid", Categoryid);
                obj_con.ExecuteNoneQuery("sp_Categories_delete", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_Categories_delete:" + ex.Message);
            }
        }

        //select all data from database 
        public List<CategoriesClass> getAll()
        {
            try
            {
                obj_con.clearParameter();
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Categories_selectall", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Categories_selectall:" + ex.Message);
            }
        }
        public List<SelectListItem> CategoryDropdown(Int32? Categoryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Categoryid", Categoryid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Categories_Id_Name_selectall", CommandType.StoredProcedure));
                List<SelectListItem> IdName = new List<SelectListItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SelectListItem obj = new SelectListItem();
                    obj.Text = Convert.ToString(dt.Rows[i]["Categoryname"]);
                    obj.Value = Convert.ToString(dt.Rows[i]["Categoryid"]);
                    IdName.Add(obj);
                }
                return IdName;
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Categories_selectall:" + ex.Message);
            }
        }

        //select data from database as Paging
        public List<CategoriesClass> selectPaging(Int64 firstPageIndex, Int64 pageSize)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@pageFirstIndex", firstPageIndex);
                obj_con.addParameter("@pageLastIndex", pageSize);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Categories_selectPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Categories_selectPaging:" + ex.Message);
            }
        }

        public string selectIndexPaging(Int64 PageSize, Int64 PageIndex, string Search)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@PageSize", PageSize);
                obj_con.addParameter("@PageIndex", PageIndex);
                obj_con.addParameter("@Search", Search);
                return ChossonKallahAdmin.GlobalUtilities.SessionUtilities.ConvertDataTableTojSonString(ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Categories_selectIndexPaging", CommandType.StoredProcedure)));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Categories_selectIndexPaging:" + ex.Message);
            }
        }
        public Int32 selectIndexPagingCount(Int64 PageSize, Int64 PageIndex, string Search)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@PageSize", PageSize);
                obj_con.addParameter("@PageIndex", PageIndex);
                obj_con.addParameter("@Search", Search);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Categories_selectIndexPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Categories_selectIndexPaging:" + ex.Message);
            }
        }
        public List<CategoriesClass> selectIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@StartIndex", StartIndex);
                obj_con.addParameter("@EndIndex", EndIndex);
                obj_con.addParameter("@Search", Search);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_AddressBook_selectLazyLoading", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_AddressBook_selectLazyLoading:" + ex.Message);
            }
        }
        //select data from database as list
        public List<CategoriesClass> selectlist(Int32? Categoryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Categoryid", Categoryid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Categories_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Categories_select:" + ex.Message);
            }
        }

        //select data from database as Objject
        public CategoriesClass selectById(Int32? Categoryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Categoryid", Categoryid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Categories_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToOjbect(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Categories_select:" + ex.Message);
            }
        }

        //select data from database as datatable
        public DataTable selectdatatable(Int32? Categoryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Categoryid", Categoryid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Categories_select", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Categories_select:" + ex.Message);
            }
        }

        //create parameter 
        public void createParameter(CategoriesClass obj, DB_con.DBTrans trans)
        {
            try
            {
                obj_con.clearParameter();

                if (Convert.ToString(obj.Categoryname) != "")
                    obj_con.addParameter("@Categoryname", string.IsNullOrEmpty(Convert.ToString(obj.Categoryname)) ? "" : obj.Categoryname);
                else
                    obj_con.addParameter("@Categoryname", DBNull.Value);

                if (Convert.ToString(obj.Parentcategoryid) != "")
                    obj_con.addParameter("@Parentcategoryid", string.IsNullOrEmpty(Convert.ToString(obj.Parentcategoryid)) ? 0 : obj.Parentcategoryid);
                else
                    obj_con.addParameter("@Parentcategoryid", DBNull.Value);

                if (Convert.ToString(obj.Categoryurl) != "")
                    obj_con.addParameter("@Categoryurl", string.IsNullOrEmpty(Convert.ToString(obj.Categoryurl)) ? "" : obj.Categoryurl);
                else
                    obj_con.addParameter("@Categoryurl", DBNull.Value);

                if (Convert.ToString(obj.Isfeaturedcategory) != "")
                    obj_con.addParameter("@Isfeaturedcategory", string.IsNullOrEmpty(Convert.ToString(obj.Isfeaturedcategory)) ? false : obj.Isfeaturedcategory);
                else
                    obj_con.addParameter("@Isfeaturedcategory", DBNull.Value);

                if (Convert.ToString(obj.Isactive) != "")
                    obj_con.addParameter("@Isactive", string.IsNullOrEmpty(Convert.ToString(obj.Isactive)) ? false : obj.Isactive);
                else
                    obj_con.addParameter("@Isactive", DBNull.Value);

                if (Convert.ToString(obj.Isdeleted) != "")
                    obj_con.addParameter("@Isdeleted", string.IsNullOrEmpty(Convert.ToString(obj.Isdeleted)) ? false : obj.Isdeleted);
                else
                    obj_con.addParameter("@Isdeleted", DBNull.Value);

                if (Convert.ToString(obj.Createdon) != "")
                    obj_con.addParameter("@Createdon", string.IsNullOrEmpty(Convert.ToString(obj.Createdon)) ? Convert.ToDateTime("1900-01-01") : obj.Createdon);
                else
                    obj_con.addParameter("@Createdon", DBNull.Value);

                if (Convert.ToString(obj.Deletedon) != "")
                    obj_con.addParameter("@Deletedon", string.IsNullOrEmpty(Convert.ToString(obj.Deletedon)) ? Convert.ToDateTime("1900-01-01") : obj.Deletedon);
                else
                    obj_con.addParameter("@Deletedon", DBNull.Value);

                if (Convert.ToString(obj.Categoryid) != "")
                    obj_con.addParameter("@Categoryid", Convert.ToInt32(obj.Categoryid), trans);
                else
                    obj_con.addParameter("@Categoryid", DBNull.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //update edited object 
        public CategoriesClass updateObject(CategoriesClass obj)
        {
            try
            {

                CategoriesClass oldObj = selectById(obj.Categoryid);
                if (obj.Categoryname == null)
                    obj.Categoryname = oldObj.Categoryname;

                if (obj.Parentcategoryid == null)
                    obj.Parentcategoryid = oldObj.Parentcategoryid;

                if (obj.Categoryurl == null)
                    obj.Categoryurl = oldObj.Categoryurl;

                if (obj.Isfeaturedcategory == null)
                    obj.Isfeaturedcategory = oldObj.Isfeaturedcategory;

                if (obj.Isactive == null)
                    obj.Isactive = oldObj.Isactive;

                if (obj.Isdeleted == null)
                    obj.Isdeleted = oldObj.Isdeleted;

                if (obj.Createdon == null)
                    obj.Createdon = oldObj.Createdon;

                if (obj.Deletedon == null)
                    obj.Deletedon = oldObj.Deletedon;

                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Convert IDataReader To DataTable method
        public DataTable ConvertDatareadertoDataTable(IDataReader dr)
        {
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }

        //Convert DataTable To List method
        public List<CategoriesClass> ConvertToList(DataTable dt)
        {
            List<CategoriesClass> Categorieslist = new List<CategoriesClass>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CategoriesClass obj_Categories = new CategoriesClass();

                if (Convert.ToString(dt.Rows[i]["Categoryid"]) != "")
                    obj_Categories.Categoryid = Convert.ToInt32(dt.Rows[i]["Categoryid"]);

                if (Convert.ToString(dt.Rows[i]["Categoryname"]) != "")
                    obj_Categories.Categoryname = Convert.ToString(dt.Rows[i]["Categoryname"]);

                if (Convert.ToString(dt.Rows[i]["Parentcategoryid"]) != "")
                    obj_Categories.Parentcategoryid = Convert.ToInt32(dt.Rows[i]["Parentcategoryid"]);

                if (Convert.ToString(dt.Rows[i]["Categoryurl"]) != "")
                    obj_Categories.Categoryurl = Convert.ToString(dt.Rows[i]["Categoryurl"]);

                if (Convert.ToString(dt.Rows[i]["Isfeaturedcategory"]) != "")
                    obj_Categories.Isfeaturedcategory = Convert.ToBoolean(dt.Rows[i]["Isfeaturedcategory"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_Categories.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
                    obj_Categories.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

                if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
                    obj_Categories.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

                if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
                    obj_Categories.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);


                Categorieslist.Add(obj_Categories);
            }
            return Categorieslist;
        }

        //Convert DataTable To object method
        public CategoriesClass ConvertToOjbect(DataTable dt)
        {
            CategoriesClass obj_Categories = new CategoriesClass();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (Convert.ToString(dt.Rows[i]["Categoryid"]) != "")
                    obj_Categories.Categoryid = Convert.ToInt32(dt.Rows[i]["Categoryid"]);

                if (Convert.ToString(dt.Rows[i]["Categoryname"]) != "")
                    obj_Categories.Categoryname = Convert.ToString(dt.Rows[i]["Categoryname"]);

                if (Convert.ToString(dt.Rows[i]["Parentcategoryid"]) != "")
                    obj_Categories.Parentcategoryid = Convert.ToInt32(dt.Rows[i]["Parentcategoryid"]);

                if (Convert.ToString(dt.Rows[i]["Categoryurl"]) != "")
                    obj_Categories.Categoryurl = Convert.ToString(dt.Rows[i]["Categoryurl"]);

                if (Convert.ToString(dt.Rows[i]["Isfeaturedcategory"]) != "")
                    obj_Categories.Isfeaturedcategory = Convert.ToBoolean(dt.Rows[i]["Isfeaturedcategory"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_Categories.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
                    obj_Categories.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

                if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
                    obj_Categories.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

                if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
                    obj_Categories.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);
            }
            return obj_Categories;
        }


        //disposble method
        void IDisposable.Dispose()
        {
            System.GC.SuppressFinalize(this);
            obj_con.closeConnection();
        }

        public DataTable CheckCategoryExists(string Categoryname)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Categoryname", Categoryname);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Category_CheckCategoryExists", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Category_CheckCategoryExists:" + ex.Message);
            }
        }
        public DataTable CheckCategoryExistsUpdate(string Categoryname,Int32? Categoryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Categoryname", Categoryname);
                obj_con.addParameter("@Categoryid", Categoryid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Category_CheckCategoryExistsUpdate", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Category_CheckCategoryExists:" + ex.Message);
            }
        }
        #endregion
    }
}
