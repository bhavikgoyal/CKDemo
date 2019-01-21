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
    public class BusinessCategoryClass
    {
        #region "properties"
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Businesscategoryid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Businessid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Categoryid { get; set; }



        public BusinessCategoryClass()
        {
        }
        #endregion
    }

    public class BusinessCategoryCtl : IDisposable
    {
        #region "constructors"

        ConnectionCls obj_con = null;
        //Default Constructor
        public BusinessCategoryCtl()
        {
            obj_con = new ConnectionCls();
        }

        //Select Constructor
        public BusinessCategoryCtl(Int32? id)
        {
            obj_con = new ConnectionCls();
            BusinessCategoryClass obj_Bus = new BusinessCategoryClass();
            using (DataTable dt = selectdatatable(id))
            {
                if (dt.Rows.Count > 0)
                {

                    obj_Bus.Businesscategoryid = Convert.ToInt32(dt.Rows[0]["Businesscategoryid"]);
                    obj_Bus.Businessid = Convert.ToInt32(dt.Rows[0]["Businessid"]);
                    obj_Bus.Categoryid = Convert.ToInt32(dt.Rows[0]["Categoryid"]);

                }
            }
        }


        #endregion

        #region "methods"

        //insert data into database 
        public Int32? insert(BusinessCategoryClass obj)
        {
            try
            {
                obj_con.clearParameter();
                createParameter(obj, DBTrans.Insert);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_BusinessCategory_insert", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Businesscategoryid = Convert.ToInt32(obj_con.getValue("@Businesscategoryid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_BusinessCategory_insert:" + ex.Message);
            }
        }

        //update data into database 
        public Int32? update(BusinessCategoryClass obj)
        {
            try
            {
                obj_con.clearParameter();
                obj = updateObject(obj);
                createParameter(obj, DBTrans.Update);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_BusinessCategory_update", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Businesscategoryid = Convert.ToInt32(obj_con.getValue("@Businesscategoryid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_BusinessCategory_update:" + ex.Message);
            }
        }

        //delete data from database 
        public void delete(Int32? Businesscategoryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.BeginTransaction();
                obj_con.addParameter("@Businesscategoryid", Businesscategoryid);
                obj_con.ExecuteNoneQuery("sp_BusinessCategory_delete", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_BusinessCategory_delete:" + ex.Message);
            }
        }

        //select all data from database 
        public List<BusinessCategoryClass> getAll()
        {
            try
            {
                obj_con.clearParameter();
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessCategory_selectall", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessCategory_selectall:" + ex.Message);
            }
        }

        //select data from database as Paging
        public List<BusinessCategoryClass> selectPaging(Int64 firstPageIndex, Int64 pageSize)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@pageFirstIndex", firstPageIndex);
                obj_con.addParameter("@pageLastIndex", pageSize);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessCategory_selectPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessCategory_selectPaging:" + ex.Message);
            }
        }

        public List<BusinessCategoryClass> selectIndexPaging(Int64 PageSize, Int64 PageIndex, string Search)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@PageSize", PageSize);
                obj_con.addParameter("@PageIndex", PageIndex);
                obj_con.addParameter("@Search", Search);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessCategory_selectIndexPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessCategory_selectIndexPaging:" + ex.Message);
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
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessCategory_selectIndexPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessCategory_selectIndexPaging:" + ex.Message);
            }
        }
        public List<BusinessCategoryClass> selectIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
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
        public List<BusinessCategoryClass> selectlist(Int32? Businesscategoryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businesscategoryid", Businesscategoryid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessCategory_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessCategory_select:" + ex.Message);
            }
        }

        //select data from database as Objject
        public BusinessCategoryClass selectById(Int32? Businesscategoryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businesscategoryid", Businesscategoryid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessCategory_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToOjbect(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessCategory_select:" + ex.Message);
            }
        }

        //select data from database as datatable
        public DataTable selectdatatable(Int32? Businesscategoryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businesscategoryid", Businesscategoryid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessCategory_select", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessCategory_select:" + ex.Message);
            }
        }

        //create parameter 
        public void createParameter(BusinessCategoryClass obj, DB_con.DBTrans trans)
        {
            try
            {
                obj_con.clearParameter();

                if (Convert.ToString(obj.Businessid) != "")
                    obj_con.addParameter("@Businessid", string.IsNullOrEmpty(Convert.ToString(obj.Businessid)) ? 0 : obj.Businessid);
                else
                    obj_con.addParameter("@Businessid", DBNull.Value);

                if (Convert.ToString(obj.Categoryid) != "")
                    obj_con.addParameter("@Categoryid", string.IsNullOrEmpty(Convert.ToString(obj.Categoryid)) ? 0 : obj.Categoryid);
                else
                    obj_con.addParameter("@Categoryid", DBNull.Value);

                if (Convert.ToString(obj.Businesscategoryid) != "")
                    obj_con.addParameter("@Businesscategoryid", Convert.ToInt32(obj.Businesscategoryid), trans);
                else
                    obj_con.addParameter("@Businesscategoryid", DBNull.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //update edited object 
        public BusinessCategoryClass updateObject(BusinessCategoryClass obj)
        {
            try
            {

                BusinessCategoryClass oldObj = selectById(obj.Businesscategoryid);
                if (obj.Businessid == null)
                    obj.Businessid = oldObj.Businessid;

                if (obj.Categoryid == null)
                    obj.Categoryid = oldObj.Categoryid;

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
        public List<BusinessCategoryClass> ConvertToList(DataTable dt)
        {
            List<BusinessCategoryClass> BusinessCategorylist = new List<BusinessCategoryClass>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BusinessCategoryClass obj_BusinessCategory = new BusinessCategoryClass();

                if (Convert.ToString(dt.Rows[i]["Businesscategoryid"]) != "")
                    obj_BusinessCategory.Businesscategoryid = Convert.ToInt32(dt.Rows[i]["Businesscategoryid"]);

                if (Convert.ToString(dt.Rows[i]["Businessid"]) != "")
                    obj_BusinessCategory.Businessid = Convert.ToInt32(dt.Rows[i]["Businessid"]);

                if (Convert.ToString(dt.Rows[i]["Categoryid"]) != "")
                    obj_BusinessCategory.Categoryid = Convert.ToInt32(dt.Rows[i]["Categoryid"]);


                BusinessCategorylist.Add(obj_BusinessCategory);
            }
            return BusinessCategorylist;
        }

        //Convert DataTable To object method
        public BusinessCategoryClass ConvertToOjbect(DataTable dt)
        {
            BusinessCategoryClass obj_BusinessCategory = new BusinessCategoryClass();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (Convert.ToString(dt.Rows[i]["Businesscategoryid"]) != "")
                    obj_BusinessCategory.Businesscategoryid = Convert.ToInt32(dt.Rows[i]["Businesscategoryid"]);

                if (Convert.ToString(dt.Rows[i]["Businessid"]) != "")
                    obj_BusinessCategory.Businessid = Convert.ToInt32(dt.Rows[i]["Businessid"]);

                if (Convert.ToString(dt.Rows[i]["Categoryid"]) != "")
                    obj_BusinessCategory.Categoryid = Convert.ToInt32(dt.Rows[i]["Categoryid"]);
            }
            return obj_BusinessCategory;
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
