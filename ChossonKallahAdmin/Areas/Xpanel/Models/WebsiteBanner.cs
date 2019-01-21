using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using DB_con;


namespace ChossonKallah.Models
{
    public class WebsiteBannerClass
    {
        #region "properties"
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Websitebannerid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Bannername { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BannerImage { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isactive { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Sequence { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Bannertextline1 { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Bannertextline2 { get; set; }





        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> Createdon { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> Deletedon { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Deletedby { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isdeleted { get; set; }




        public WebsiteBannerClass()
        {
        }
        #endregion
    }

    public class WebsiteBannerCtl : IDisposable
    {
        #region "constructors"

        ConnectionCls obj_con = null;
        //Default Constructor
        public WebsiteBannerCtl()
        {
            obj_con = new ConnectionCls();
        }

        //Select Constructor
        public WebsiteBannerCtl(Int32? id)
        {
            obj_con = new ConnectionCls();
            WebsiteBannerClass obj_Web = new WebsiteBannerClass();
            using (DataTable dt = selectdatatable(id))
            {
                if (dt.Rows.Count > 0)
                {

                    obj_Web.Websitebannerid = Convert.ToInt32(dt.Rows[0]["Websitebannerid"]);
                    obj_Web.Bannername = Convert.ToString(dt.Rows[0]["Bannername"]);
                    obj_Web.Isactive = Convert.ToBoolean(dt.Rows[0]["Isactive"]);
                    obj_Web.Sequence = Convert.ToInt32(dt.Rows[0]["Sequence"]);
                    obj_Web.Bannertextline1 = Convert.ToString(dt.Rows[0]["Bannertextline1"]);
                    obj_Web.Bannertextline2 = Convert.ToString(dt.Rows[0]["Bannertextline2"]);

                }
            }
        }


        #endregion

        #region "methods"

        //insert data into database 
        public Int32? insert(WebsiteBannerClass obj)
        {
            try
            {
                obj.Websitebannerid = 0;
                obj_con.clearParameter();
                createParameter(obj, DBTrans.Insert);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_WebsiteBanner_insert", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Websitebannerid = Convert.ToInt32(obj_con.getValue("@Websitebannerid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_WebsiteBanner_insert:" + ex.Message);
            }
        }

        //update data into database 
        public Int32? update(WebsiteBannerClass obj)
        {
            try
            {
                obj_con.clearParameter();
                obj = updateObject(obj);
                createParameter(obj, DBTrans.Update);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_WebsiteBanner_update", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Websitebannerid = Convert.ToInt32(obj_con.getValue("@Websitebannerid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_WebsiteBanner_update:" + ex.Message);
            }
        }

        public Int32? GetMaxSequenceNumber()
        {
            try
            {
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_WebsiteBanner_GetMaxNumber", CommandType.StoredProcedure));
                return Convert.ToInt32(dt.Rows[0]["LastSequenceNumber"]);
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_WebsiteBanner_GetMaxNumber:" + ex.Message);
            }
        }

        //delete data from database 
        public void delete(Int32? Websitebannerid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.BeginTransaction();
                obj_con.addParameter("@Websitebannerid", Websitebannerid);
                obj_con.ExecuteNoneQuery("sp_WebsiteBanner_delete", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_WebsiteBanner_delete:" + ex.Message);
            }
        }

        //select all data from database 
        public List<WebsiteBannerClass> getAll()
        {
            try
            {
                obj_con.clearParameter();
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_WebsiteBanner_selectall", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_WebsiteBanner_selectall:" + ex.Message);
            }
        }

        //select data from database as Paging
        public List<WebsiteBannerClass> selectPaging(Int64 firstPageIndex, Int64 pageSize)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@pageFirstIndex", firstPageIndex);
                obj_con.addParameter("@pageLastIndex", pageSize);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_WebsiteBanner_selectPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_WebsiteBanner_selectPaging:" + ex.Message);
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
                return ChossonKallahAdmin.GlobalUtilities.SessionUtilities.ConvertDataTableTojSonString(ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_WebsiteBanner_selectIndexPaging", CommandType.StoredProcedure)));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_WebsiteBanner_selectIndexPaging:" + ex.Message);
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
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_WebsiteBanner_selectIndexPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_WebsiteBanner_selectIndexPaging:" + ex.Message);
            }
        }
        public List<WebsiteBannerClass> selectIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
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
        public List<WebsiteBannerClass> selectlist(Int32? Websitebannerid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Websitebannerid", Websitebannerid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_WebsiteBanner_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_WebsiteBanner_select:" + ex.Message);
            }
        }

        //select data from database as Objject
        public WebsiteBannerClass selectById(Int32? Websitebannerid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Websitebannerid", Websitebannerid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_WebsiteBanner_select", CommandType.StoredProcedure));
                return ConvertToOjbect(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_WebsiteBanner_select:" + ex.Message);
            }
        }

        //select data from database as datatable
        public DataTable selectdatatable(Int32? Websitebannerid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Websitebannerid", Websitebannerid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_WebsiteBanner_select", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_WebsiteBanner_select:" + ex.Message);
            }
        }

        //create parameter 
        public void createParameter(WebsiteBannerClass obj, DB_con.DBTrans trans)
        {
            try
            {
                obj_con.clearParameter();
                if (Convert.ToString(obj.Createdon) != "")
                    obj_con.addParameter("@createdon", string.IsNullOrEmpty(Convert.ToString(obj.Createdon)) ? Convert.ToDateTime("1900-01-01") : obj.Createdon);
                else
                    obj_con.addParameter("@createdon", DBNull.Value);

                if (Convert.ToString(obj.Deletedby) != "")
                    obj_con.addParameter("@deletedby", string.IsNullOrEmpty(Convert.ToString(obj.Deletedby)) ? 0 : obj.Deletedby);
                else
                    obj_con.addParameter("@deletedby", DBNull.Value);

                if (Convert.ToString(obj.Isdeleted) != "")
                    obj_con.addParameter("@isdeleted", string.IsNullOrEmpty(Convert.ToString(obj.Isdeleted)) ? false : obj.Isdeleted);
                else
                    obj_con.addParameter("@isdeleted", DBNull.Value);
                if (Convert.ToString(obj.Deletedon) != "")
                    obj_con.addParameter("@deletedon", string.IsNullOrEmpty(Convert.ToString(obj.Deletedon)) ? Convert.ToDateTime("1900-01-01") : obj.Deletedon);
                else
                    obj_con.addParameter("@deletedon", DBNull.Value);

                if (Convert.ToString(obj.Bannername) != "")
                    obj_con.addParameter("@Bannername", string.IsNullOrEmpty(Convert.ToString(obj.Bannername)) ? "" : obj.Bannername);
                else
                    obj_con.addParameter("@Bannername", DBNull.Value);

                if (Convert.ToString(obj.BannerImage) != "")
                    obj_con.addParameter("@BannerImage", string.IsNullOrEmpty(Convert.ToString(obj.BannerImage)) ? "" : obj.BannerImage);
                else
                    obj_con.addParameter("@BannerImage", DBNull.Value);

                if (Convert.ToString(obj.Isactive) != "")
                    obj_con.addParameter("@Isactive", string.IsNullOrEmpty(Convert.ToString(obj.Isactive)) ? false : obj.Isactive);
                else
                    obj_con.addParameter("@Isactive", DBNull.Value);


                if (Convert.ToString(obj.Bannertextline1) != "")
                    obj_con.addParameter("@Bannertextline1", string.IsNullOrEmpty(Convert.ToString(obj.Bannertextline1)) ? "" : obj.Bannertextline1);
                else
                    obj_con.addParameter("@Bannertextline1", DBNull.Value);

                if (Convert.ToString(obj.Bannertextline2) != "")
                    obj_con.addParameter("@Bannertextline2", string.IsNullOrEmpty(Convert.ToString(obj.Bannertextline2)) ? "" : obj.Bannertextline2);
                else
                    obj_con.addParameter("@Bannertextline2", DBNull.Value);

                if (Convert.ToString(obj.Sequence) != "")
                    obj_con.addParameter("@Sequence", Convert.ToInt32(obj.Sequence));
                else
                    obj_con.addParameter("@Sequence", DBNull.Value);
                if (Convert.ToString(obj.Websitebannerid) != "")
                    obj_con.addParameter("@Websitebannerid", Convert.ToInt32(obj.Websitebannerid), trans);
                else
                    obj_con.addParameter("@Websitebannerid", DBNull.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //update edited object 
        public WebsiteBannerClass updateObject(WebsiteBannerClass obj)
        {
            try
            {

                WebsiteBannerClass oldObj = selectById(obj.Websitebannerid);
                if (obj.Bannername == null)
                    obj.Bannername = oldObj.Bannername;

                if (obj.BannerImage == null)
                    obj.BannerImage = oldObj.BannerImage;

                if (obj.Isactive == null)
                    obj.Isactive = oldObj.Isactive;

                if (obj.Sequence == null)
                    obj.Sequence = oldObj.Sequence;

                if (obj.Bannertextline1 == null)
                    obj.Bannertextline1 = oldObj.Bannertextline1;

                if (obj.Bannertextline2 == null)
                    obj.Bannertextline2 = oldObj.Bannertextline2;

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
        public List<WebsiteBannerClass> ConvertToList(DataTable dt)
        {
            List<WebsiteBannerClass> WebsiteBannerlist = new List<WebsiteBannerClass>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                WebsiteBannerClass obj_WebsiteBanner = new WebsiteBannerClass();

                if (Convert.ToString(dt.Rows[i]["Websitebannerid"]) != "")
                    obj_WebsiteBanner.Websitebannerid = Convert.ToInt32(dt.Rows[i]["Websitebannerid"]);

                if (Convert.ToString(dt.Rows[i]["Bannername"]) != "")
                    obj_WebsiteBanner.Bannername = Convert.ToString(dt.Rows[i]["Bannername"]);

                if (Convert.ToString(dt.Rows[i]["BannerImage"]) != "")
                    obj_WebsiteBanner.BannerImage = Convert.ToString(dt.Rows[i]["BannerImage"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_WebsiteBanner.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Sequence"]) != "")
                    obj_WebsiteBanner.Sequence = Convert.ToInt32(dt.Rows[i]["Sequence"]);

                if (Convert.ToString(dt.Rows[i]["Bannertextline1"]) != "")
                    obj_WebsiteBanner.Bannertextline1 = Convert.ToString(dt.Rows[i]["Bannertextline1"]);

                if (Convert.ToString(dt.Rows[i]["Bannertextline2"]) != "")
                    obj_WebsiteBanner.Bannertextline2 = Convert.ToString(dt.Rows[i]["Bannertextline2"]);


                WebsiteBannerlist.Add(obj_WebsiteBanner);
            }
            return WebsiteBannerlist;
        }

        //Convert DataTable To object method
        public WebsiteBannerClass ConvertToOjbect(DataTable dt)
        {
            WebsiteBannerClass obj_WebsiteBanner = new WebsiteBannerClass();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (Convert.ToString(dt.Rows[i]["Websitebannerid"]) != "")
                    obj_WebsiteBanner.Websitebannerid = Convert.ToInt32(dt.Rows[i]["Websitebannerid"]);

                if (Convert.ToString(dt.Rows[i]["Bannername"]) != "")
                    obj_WebsiteBanner.Bannername = Convert.ToString(dt.Rows[i]["Bannername"]);

                if (Convert.ToString(dt.Rows[i]["BannerImage"]) != "")
                    obj_WebsiteBanner.BannerImage = Convert.ToString(dt.Rows[i]["BannerImage"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_WebsiteBanner.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Sequence"]) != "")
                    obj_WebsiteBanner.Sequence = Convert.ToInt32(dt.Rows[i]["Sequence"]);

                if (Convert.ToString(dt.Rows[i]["Bannertextline1"]) != "")
                    obj_WebsiteBanner.Bannertextline1 = Convert.ToString(dt.Rows[i]["Bannertextline1"]);

                if (Convert.ToString(dt.Rows[i]["Bannertextline2"]) != "")
                    obj_WebsiteBanner.Bannertextline2 = Convert.ToString(dt.Rows[i]["Bannertextline2"]);
            }
            return obj_WebsiteBanner;
        }


        //disposble method
        void IDisposable.Dispose()
        {
            System.GC.SuppressFinalize(this);
            obj_con.closeConnection();
        }

        #endregion
    }

}
