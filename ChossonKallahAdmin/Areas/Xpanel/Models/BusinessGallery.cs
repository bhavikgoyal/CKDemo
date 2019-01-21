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
    public class BusinessGalleryClass
    {
        #region "properties"
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Businessgalleryid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Businessid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Imagename { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isactive { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Sequence { get; set; }



        public BusinessGalleryClass()
        {
        }
        #endregion
    }

    public class BusinessGalleryCtl : IDisposable
    {
        #region "constructors"

        ConnectionCls obj_con = null;
        //Default Constructor
        public BusinessGalleryCtl()
        {
            obj_con = new ConnectionCls();
        }

        //Select Constructor
        public BusinessGalleryCtl(Int32? id)
        {
            obj_con = new ConnectionCls();
            BusinessGalleryClass obj_Bus = new BusinessGalleryClass();
            using (DataTable dt = selectdatatable(id))
            {
                if (dt.Rows.Count > 0)
                {

                    obj_Bus.Businessgalleryid = Convert.ToInt32(dt.Rows[0]["Businessgalleryid"]);
                    obj_Bus.Businessid = Convert.ToInt32(dt.Rows[0]["Businessid"]);
                    obj_Bus.Imagename = Convert.ToString(dt.Rows[0]["Imagename"]);
                    obj_Bus.Isactive = Convert.ToBoolean(dt.Rows[0]["Isactive"]);
                    obj_Bus.Sequence = Convert.ToInt32(dt.Rows[0]["Sequence"]);

                }
            }
        }


        #endregion

        #region "methods"

        //insert data into database 
        public Int32? insert(BusinessGalleryClass obj)
        {
            try
            {
                obj.Businessgalleryid = 0;
                obj_con.clearParameter();
                createParameter(obj, DBTrans.Insert);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_BusinessGallery_insert", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Businessgalleryid = Convert.ToInt32(obj_con.getValue("@Businessgalleryid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_BusinessGallery_insert:" + ex.Message);
            }
        }

        //update data into database 
        public Int32? update(BusinessGalleryClass obj)
        {
            try
            {
                obj_con.clearParameter();
                obj = updateObject(obj);
                createParameter(obj, DBTrans.Update);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_BusinessGallery_update", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Businessgalleryid = Convert.ToInt32(obj_con.getValue("@Businessgalleryid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_BusinessGallery_update:" + ex.Message);
            }
        }

        //delete data from database 
        public void delete(Int32? Businessgalleryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.BeginTransaction();
                obj_con.addParameter("@Businessgalleryid", Businessgalleryid);
                obj_con.ExecuteNoneQuery("sp_BusinessGallery_delete", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_BusinessGallery_delete:" + ex.Message);
            }
        }

        //select all data from database 
        public List<BusinessGalleryClass> getAll()
        {
            try
            {
                obj_con.clearParameter();
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessGallery_selectall", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessGallery_selectall:" + ex.Message);
            }
        }

        //select data from database as Paging
        public List<BusinessGalleryClass> selectPaging(Int64 firstPageIndex, Int64 pageSize)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@pageFirstIndex", firstPageIndex);
                obj_con.addParameter("@pageLastIndex", pageSize);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessGallery_selectPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessGallery_selectPaging:" + ex.Message);
            }
        }

        public List<BusinessGalleryClass> selectIndexPaging(Int64 PageSize, Int64 PageIndex, string Search)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@PageSize", PageSize);
                obj_con.addParameter("@PageIndex", PageIndex);
                obj_con.addParameter("@Search", Search);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessGallery_selectIndexPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessGallery_selectIndexPaging:" + ex.Message);
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
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessGallery_selectIndexPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessGallery_selectIndexPaging:" + ex.Message);
            }
        }
        public List<BusinessGalleryClass> selectIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
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
        public List<BusinessGalleryClass> selectlist(Int32? Businessgalleryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businessgalleryid", Businessgalleryid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessGallery_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessGallery_select:" + ex.Message);
            }
        }

        //select data from database as Objject
        public BusinessGalleryClass selectById(Int32? Businessgalleryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businessgalleryid", Businessgalleryid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessGallery_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToOjbect(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessGallery_select:" + ex.Message);
            }
        }

        //select data from database as datatable
        public DataTable selectdatatable(Int32? Businessgalleryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businessgalleryid", Businessgalleryid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessGallery_select", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessGallery_select:" + ex.Message);
            }
        }

        //create parameter 
        public void createParameter(BusinessGalleryClass obj, DB_con.DBTrans trans)
        {
            try
            {
                obj_con.clearParameter();

                if (Convert.ToString(obj.Businessid) != "")
                    obj_con.addParameter("@Businessid", string.IsNullOrEmpty(Convert.ToString(obj.Businessid)) ? 0 : obj.Businessid);
                else
                    obj_con.addParameter("@Businessid", DBNull.Value);

                if (Convert.ToString(obj.Imagename) != "")
                    obj_con.addParameter("@Imagename", string.IsNullOrEmpty(Convert.ToString(obj.Imagename)) ? "" : obj.Imagename);
                else
                    obj_con.addParameter("@Imagename", DBNull.Value);

                if (Convert.ToString(obj.Isactive) != "")
                    obj_con.addParameter("@Isactive", string.IsNullOrEmpty(Convert.ToString(obj.Isactive)) ? false : obj.Isactive);
                else
                    obj_con.addParameter("@Isactive", DBNull.Value);

                if (Convert.ToString(obj.Sequence) != "")
                    obj_con.addParameter("@Sequence", string.IsNullOrEmpty(Convert.ToString(obj.Sequence)) ? 0 : obj.Sequence);
                else
                    obj_con.addParameter("@Sequence", DBNull.Value);

                if (Convert.ToString(obj.Businessgalleryid) != "")
                    obj_con.addParameter("@Businessgalleryid", Convert.ToInt32(obj.Businessgalleryid), trans);
                else
                    obj_con.addParameter("@Businessgalleryid", DBNull.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //update edited object 
        public BusinessGalleryClass updateObject(BusinessGalleryClass obj)
        {
            try
            {

                BusinessGalleryClass oldObj = selectById(obj.Businessgalleryid);
                if (obj.Businessid == null)
                    obj.Businessid = oldObj.Businessid;

                if (obj.Imagename == null)
                    obj.Imagename = oldObj.Imagename;

                if (obj.Isactive == null)
                    obj.Isactive = oldObj.Isactive;

                if (obj.Sequence == null)
                    obj.Sequence = oldObj.Sequence;

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
        public List<BusinessGalleryClass> ConvertToList(DataTable dt)
        {
            List<BusinessGalleryClass> BusinessGallerylist = new List<BusinessGalleryClass>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BusinessGalleryClass obj_BusinessGallery = new BusinessGalleryClass();

                if (Convert.ToString(dt.Rows[i]["Businessgalleryid"]) != "")
                    obj_BusinessGallery.Businessgalleryid = Convert.ToInt32(dt.Rows[i]["Businessgalleryid"]);

                if (Convert.ToString(dt.Rows[i]["Businessid"]) != "")
                    obj_BusinessGallery.Businessid = Convert.ToInt32(dt.Rows[i]["Businessid"]);

                if (Convert.ToString(dt.Rows[i]["Imagename"]) != "")
                    obj_BusinessGallery.Imagename = Convert.ToString(dt.Rows[i]["Imagename"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_BusinessGallery.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Sequence"]) != "")
                    obj_BusinessGallery.Sequence = Convert.ToInt32(dt.Rows[i]["Sequence"]);


                BusinessGallerylist.Add(obj_BusinessGallery);
            }
            return BusinessGallerylist;
        }

        //Convert DataTable To object method
        public BusinessGalleryClass ConvertToOjbect(DataTable dt)
        {
            BusinessGalleryClass obj_BusinessGallery = new BusinessGalleryClass();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (Convert.ToString(dt.Rows[i]["Businessgalleryid"]) != "")
                    obj_BusinessGallery.Businessgalleryid = Convert.ToInt32(dt.Rows[i]["Businessgalleryid"]);

                if (Convert.ToString(dt.Rows[i]["Businessid"]) != "")
                    obj_BusinessGallery.Businessid = Convert.ToInt32(dt.Rows[i]["Businessid"]);

                if (Convert.ToString(dt.Rows[i]["Imagename"]) != "")
                    obj_BusinessGallery.Imagename = Convert.ToString(dt.Rows[i]["Imagename"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_BusinessGallery.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Sequence"]) != "")
                    obj_BusinessGallery.Sequence = Convert.ToInt32(dt.Rows[i]["Sequence"]);
            }
            return obj_BusinessGallery;
        }

        public string SelectGalleryImages(Int32? Businessid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businessid", Businessid);
                return ChossonKallahAdmin.GlobalUtilities.SessionUtilities.ConvertDataTableTojSonString(ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessGallery_SelectImagesByBusiness", CommandType.StoredProcedure)));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessGallery_SelectImagesByBusiness:" + ex.Message);
            }
        }

        //disposble method
        void IDisposable.Dispose()
        {
            System.GC.SuppressFinalize(this);
            obj_con.closeConnection();
        }

        public DataTable selectByBusinessId(Int32? Businessid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@businessgalleryid", Businessid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessGallery_select", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessGallery_select:" + ex.Message);
            }
        }
        #endregion
    }

}
