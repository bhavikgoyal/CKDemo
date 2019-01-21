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
    public class BusinessReviewClass
    {
        #region "properties"
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Businessreviewid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Businessid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Review { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Rating { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isactive { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> Addedon { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Addedbyip { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isdeleted { get; set; }



        public BusinessReviewClass()
        {
        }
        #endregion
    }

    public class BusinessReviewCtl : IDisposable
    {
        #region "constructors"

        ConnectionCls obj_con = null;
        //Default Constructor
        public BusinessReviewCtl()
        {
            obj_con = new ConnectionCls();
        }

        //Select Constructor
        public BusinessReviewCtl(Int32? id)
        {
            obj_con = new ConnectionCls();
            BusinessReviewClass obj_Bus = new BusinessReviewClass();
            using (DataTable dt = selectdatatable(id))
            {
                if (dt.Rows.Count > 0)
                {

                    obj_Bus.Businessreviewid = Convert.ToInt32(dt.Rows[0]["Businessreviewid"]);
                    obj_Bus.Businessid = Convert.ToInt32(dt.Rows[0]["Businessid"]);
                    obj_Bus.Name = Convert.ToString(dt.Rows[0]["Name"]);
                    obj_Bus.Email = Convert.ToString(dt.Rows[0]["Email"]);
                    obj_Bus.Review = Convert.ToString(dt.Rows[0]["Review"]);
                    obj_Bus.Rating = Convert.ToString(dt.Rows[0]["Rating"]);
                    obj_Bus.Isactive = Convert.ToBoolean(dt.Rows[0]["Isactive"]);
                    obj_Bus.Addedon = Convert.ToDateTime(dt.Rows[0]["Addedon"]);
                    obj_Bus.Addedbyip = Convert.ToString(dt.Rows[0]["Addedbyip"]);
                    obj_Bus.Isdeleted = Convert.ToBoolean(dt.Rows[0]["Isdeleted"]);

                }
            }
        }


        #endregion

        #region "methods"

        //insert data into database 
        public Int32? insert(BusinessReviewClass obj)
        {
            try
            {
                obj.Businessreviewid = 0;
                obj_con.clearParameter();
                createParameter(obj, DBTrans.Insert);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_BusinessReview_insert", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Businessreviewid = Convert.ToInt32(obj_con.getValue("@Businessreviewid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_BusinessReview_insert:" + ex.Message);
            }
        }

        //update data into database 
        public Int32? update(BusinessReviewClass obj)
        {
            try
            {
                obj_con.clearParameter();
                obj = updateObject(obj);
                createParameter(obj, DBTrans.Update);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_BusinessReview_update", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Businessreviewid = Convert.ToInt32(obj_con.getValue("@Businessreviewid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_BusinessReview_update:" + ex.Message);
            }
        }

        //delete data from database 
        public void delete(Int32? Businessreviewid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.BeginTransaction();
                obj_con.addParameter("@Businessreviewid", Businessreviewid);
                obj_con.ExecuteNoneQuery("sp_BusinessReview_delete", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_BusinessReview_delete:" + ex.Message);
            }
        }

        //select all data from database 
        public List<BusinessReviewClass> getAll()
        {
            try
            {
                obj_con.clearParameter();
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessReview_selectall", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessReview_selectall:" + ex.Message);
            }
        }

        //select data from database as Paging
        public List<BusinessReviewClass> selectPaging(Int64 firstPageIndex, Int64 pageSize)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@pageFirstIndex", firstPageIndex);
                obj_con.addParameter("@pageLastIndex", pageSize);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessReview_selectPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessReview_selectPaging:" + ex.Message);
            }
        }

        public string selectIndexPaging(Int64 PageSize, Int64 PageIndex, string Search, string Businessid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@PageSize", PageSize);
                obj_con.addParameter("@PageIndex", PageIndex);
                obj_con.addParameter("@Search", Search);
                obj_con.addParameter("@Businessid", Businessid);
                return ChossonKallahAdmin.GlobalUtilities.SessionUtilities.ConvertDataTableTojSonString(ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessReview_selectIndexPaging", CommandType.StoredProcedure)));

            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessReview_selectIndexPaging:" + ex.Message);
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
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessReview_selectIndexPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessReview_selectIndexPaging:" + ex.Message);
            }
        }
        public List<BusinessReviewClass> selectIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
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
        public List<BusinessReviewClass> selectlist(Int32? Businessreviewid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businessreviewid", Businessreviewid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessReview_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessReview_select:" + ex.Message);
            }
        }

        //select data from database as Objject
        public BusinessReviewClass selectById(Int32? Businessreviewid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businessreviewid", Businessreviewid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessReview_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToOjbect(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessReview_select:" + ex.Message);
            }
        }

        //select data from database as datatable
        public DataTable selectdatatable(Int32? Businessreviewid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businessreviewid", Businessreviewid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessReview_select", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessReview_select:" + ex.Message);
            }
        }

        //create parameter 
        public void createParameter(BusinessReviewClass obj, DB_con.DBTrans trans)
        {
            try
            {
                obj_con.clearParameter();

                if (Convert.ToString(obj.Businessid) != "")
                    obj_con.addParameter("@Businessid", string.IsNullOrEmpty(Convert.ToString(obj.Businessid)) ? 0 : obj.Businessid);
                else
                    obj_con.addParameter("@Businessid", DBNull.Value);

                if (Convert.ToString(obj.Name) != "")
                    obj_con.addParameter("@Name", string.IsNullOrEmpty(Convert.ToString(obj.Name)) ? "" : obj.Name);
                else
                    obj_con.addParameter("@Name", DBNull.Value);

                if (Convert.ToString(obj.Email) != "")
                    obj_con.addParameter("@Email", string.IsNullOrEmpty(Convert.ToString(obj.Email)) ? "" : obj.Email);
                else
                    obj_con.addParameter("@Email", DBNull.Value);

                if (Convert.ToString(obj.Review) != "")
                    obj_con.addParameter("@Review", string.IsNullOrEmpty(Convert.ToString(obj.Review)) ? "" : obj.Review);
                else
                    obj_con.addParameter("@Review", DBNull.Value);

                if (Convert.ToString(obj.Rating) != "")
                    obj_con.addParameter("@Rating", string.IsNullOrEmpty(Convert.ToString(obj.Rating)) ? "" : obj.Rating);
                else
                    obj_con.addParameter("@Rating", DBNull.Value);

                if (Convert.ToString(obj.Isactive) != "")
                    obj_con.addParameter("@Isactive", string.IsNullOrEmpty(Convert.ToString(obj.Isactive)) ? false : obj.Isactive);
                else
                    obj_con.addParameter("@Isactive", DBNull.Value);

                if (Convert.ToString(obj.Addedon) != "")
                    obj_con.addParameter("@Addedon", string.IsNullOrEmpty(Convert.ToString(obj.Addedon)) ? Convert.ToDateTime("1900-01-01") : obj.Addedon);
                else
                    obj_con.addParameter("@Addedon", DBNull.Value);

                if (Convert.ToString(obj.Addedbyip) != "")
                    obj_con.addParameter("@Addedbyip", string.IsNullOrEmpty(Convert.ToString(obj.Addedbyip)) ? "" : obj.Addedbyip);
                else
                    obj_con.addParameter("@Addedbyip", DBNull.Value);

                if (Convert.ToString(obj.Isdeleted) != "")
                    obj_con.addParameter("@Isdeleted", string.IsNullOrEmpty(Convert.ToString(obj.Isdeleted)) ? false : obj.Isdeleted);
                else
                    obj_con.addParameter("@Isdeleted", DBNull.Value);

                if (Convert.ToString(obj.Businessreviewid) != "")
                    obj_con.addParameter("@Businessreviewid", Convert.ToInt32(obj.Businessreviewid), trans);
                else
                    obj_con.addParameter("@Businessreviewid", DBNull.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //update edited object 
        public BusinessReviewClass updateObject(BusinessReviewClass obj)
        {
            try
            {

                BusinessReviewClass oldObj = selectById(obj.Businessreviewid);
                if (obj.Businessid == null)
                    obj.Businessid = oldObj.Businessid;

                if (obj.Name == null)
                    obj.Name = oldObj.Name;

                if (obj.Email == null)
                    obj.Email = oldObj.Email;

                if (obj.Review == null)
                    obj.Review = oldObj.Review;

                if (obj.Rating == null)
                    obj.Rating = oldObj.Rating;

                if (obj.Isactive == null)
                    obj.Isactive = oldObj.Isactive;

                if (obj.Addedon == null)
                    obj.Addedon = oldObj.Addedon;

                if (obj.Addedbyip == null)
                    obj.Addedbyip = oldObj.Addedbyip;

                if (obj.Isdeleted == null)
                    obj.Isdeleted = oldObj.Isdeleted;

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
        public List<BusinessReviewClass> ConvertToList(DataTable dt)
        {
            List<BusinessReviewClass> BusinessReviewlist = new List<BusinessReviewClass>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BusinessReviewClass obj_BusinessReview = new BusinessReviewClass();

                if (Convert.ToString(dt.Rows[i]["Businessreviewid"]) != "")
                    obj_BusinessReview.Businessreviewid = Convert.ToInt32(dt.Rows[i]["Businessreviewid"]);

                if (Convert.ToString(dt.Rows[i]["Businessid"]) != "")
                    obj_BusinessReview.Businessid = Convert.ToInt32(dt.Rows[i]["Businessid"]);

                if (Convert.ToString(dt.Rows[i]["Name"]) != "")
                    obj_BusinessReview.Name = Convert.ToString(dt.Rows[i]["Name"]);

                if (Convert.ToString(dt.Rows[i]["Email"]) != "")
                    obj_BusinessReview.Email = Convert.ToString(dt.Rows[i]["Email"]);

                if (Convert.ToString(dt.Rows[i]["Review"]) != "")
                    obj_BusinessReview.Review = Convert.ToString(dt.Rows[i]["Review"]);

                if (Convert.ToString(dt.Rows[i]["Rating"]) != "")
                    obj_BusinessReview.Rating = Convert.ToString(dt.Rows[i]["Rating"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_BusinessReview.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Addedon"]) != "")
                    obj_BusinessReview.Addedon = Convert.ToDateTime(dt.Rows[i]["Addedon"]);

                if (Convert.ToString(dt.Rows[i]["Addedbyip"]) != "")
                    obj_BusinessReview.Addedbyip = Convert.ToString(dt.Rows[i]["Addedbyip"]);

                if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
                    obj_BusinessReview.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);


                BusinessReviewlist.Add(obj_BusinessReview);
            }
            return BusinessReviewlist;
        }

        //Convert DataTable To object method
        public BusinessReviewClass ConvertToOjbect(DataTable dt)
        {
            BusinessReviewClass obj_BusinessReview = new BusinessReviewClass();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (Convert.ToString(dt.Rows[i]["Businessreviewid"]) != "")
                    obj_BusinessReview.Businessreviewid = Convert.ToInt32(dt.Rows[i]["Businessreviewid"]);

                if (Convert.ToString(dt.Rows[i]["Businessid"]) != "")
                    obj_BusinessReview.Businessid = Convert.ToInt32(dt.Rows[i]["Businessid"]);

                if (Convert.ToString(dt.Rows[i]["Name"]) != "")
                    obj_BusinessReview.Name = Convert.ToString(dt.Rows[i]["Name"]);

                if (Convert.ToString(dt.Rows[i]["Email"]) != "")
                    obj_BusinessReview.Email = Convert.ToString(dt.Rows[i]["Email"]);

                if (Convert.ToString(dt.Rows[i]["Review"]) != "")
                    obj_BusinessReview.Review = Convert.ToString(dt.Rows[i]["Review"]);

                if (Convert.ToString(dt.Rows[i]["Rating"]) != "")
                    obj_BusinessReview.Rating = Convert.ToString(dt.Rows[i]["Rating"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_BusinessReview.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Addedon"]) != "")
                    obj_BusinessReview.Addedon = Convert.ToDateTime(dt.Rows[i]["Addedon"]);

                if (Convert.ToString(dt.Rows[i]["Addedbyip"]) != "")
                    obj_BusinessReview.Addedbyip = Convert.ToString(dt.Rows[i]["Addedbyip"]);

                if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
                    obj_BusinessReview.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);
            }
            return obj_BusinessReview;
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
