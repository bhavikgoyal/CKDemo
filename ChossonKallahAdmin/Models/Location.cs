using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using DB_con;
using ChossonKallahAdmin.GlobalUtilities;
using System.Web.Mvc;

namespace ChossonKallah.Models
{
    public class LocationClass
    {
        #region "properties"
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Locationid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Locationname { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Locationurl { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isactive { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isdeleted { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> Createdon { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> Deletedon { get; set; }



        public LocationClass()
        {
        }
        #endregion
    }

    public class LocationCtl : IDisposable
    {
        #region "constructors"

        ConnectionCls obj_con = null;
        //Default Constructor
        public LocationCtl()
        {
            obj_con = new ConnectionCls();
        }

        //Select Constructor
        public LocationCtl(Int32? id)
        {
            obj_con = new ConnectionCls();
            LocationClass obj_Loc = new LocationClass();
            using (DataTable dt = selectdatatable(id))
            {
                if (dt.Rows.Count > 0)
                {

                    obj_Loc.Locationid = Convert.ToInt32(dt.Rows[0]["Locationid"]);
                    obj_Loc.Locationname = Convert.ToString(dt.Rows[0]["Locationname"]);
                    obj_Loc.Locationurl = Convert.ToString(dt.Rows[0]["Locationurl"]);
                    obj_Loc.Isactive = Convert.ToBoolean(dt.Rows[0]["Isactive"]);
                    obj_Loc.Isdeleted = Convert.ToBoolean(dt.Rows[0]["Isdeleted"]);
                    obj_Loc.Createdon = Convert.ToDateTime(dt.Rows[0]["Createdon"]);
                    obj_Loc.Deletedon = Convert.ToDateTime(dt.Rows[0]["Deletedon"]);

                }
            }
        }


        #endregion

        #region "methods"

        //insert data into database 
        public Int32? insert(LocationClass obj)
        {
            try
            {
                obj.Locationid = 0;
                obj_con.clearParameter();
                createParameter(obj, DBTrans.Insert);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_Location_insert", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Locationid = Convert.ToInt32(obj_con.getValue("@Locationid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_Location_insert:" + ex.Message);
            }
        }

        //update data into database 
        public Int32? update(LocationClass obj)
        {
            try
            {
                obj_con.clearParameter();
                obj = updateObject(obj);
                createParameter(obj, DBTrans.Update);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_Location_update", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Locationid = Convert.ToInt32(obj_con.getValue("@Locationid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_Location_update:" + ex.Message);
            }
        }

        //delete data from database 
        public void delete(Int32? Locationid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.BeginTransaction();
                obj_con.addParameter("@Locationid", Locationid);
                obj_con.ExecuteNoneQuery("sp_Location_delete", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_Location_delete:" + ex.Message);
            }
        }

        //select all data from database 
        public List<LocationClass> getAll()
        {
            try
            {
                obj_con.clearParameter();
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Location_selectall", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Location_selectall:" + ex.Message);
            }
        }

        //select data from database as Paging
        public List<LocationClass> selectPaging(Int64 firstPageIndex, Int64 pageSize)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@pageFirstIndex", firstPageIndex);
                obj_con.addParameter("@pageLastIndex", pageSize);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Location_selectPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Location_selectPaging:" + ex.Message);
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
                return SessionUtilities.ConvertDataTableTojSonString(ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Location_selectIndexPaging", CommandType.StoredProcedure)));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Location_selectIndexPaging:" + ex.Message);
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
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Location_selectIndexPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Location_selectIndexPaging:" + ex.Message);
            }
        }
        public List<LocationClass> selectIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
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
        public List<LocationClass> selectlist(Int32? Locationid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Locationid", Locationid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Location_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Location_select:" + ex.Message);
            }
        }

        //select data from database as Objject
        public LocationClass selectById(Int32? Locationid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Locationid", Locationid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Location_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToOjbect(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Location_select:" + ex.Message);
            }
        }

        public List<SelectListItem> getLIstOfState(string locationname)
        {
            try
            {
                List<SelectListItem> lstObj = new List<SelectListItem>();
                obj_con.clearParameter();
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_StateData_SelectAllForSelectInAddEditLocation", CommandType.StoredProcedure));
                SelectListItem sl = new SelectListItem();
                sl.Text = "--Please Select--";
                sl.Value = "0";
                sl.Selected = (string.IsNullOrEmpty(locationname) ? true : false);
                lstObj.Add(sl);
                foreach (DataRow dr in dt.Rows) {
                    sl = new SelectListItem();
                    sl.Text = Convert.ToString(dr["FullName"]);
                    sl.Value = Convert.ToString(dr["FullName"]);
                    sl.Selected = (string.IsNullOrEmpty(locationname) ? false : ((locationname == Convert.ToString(dr["FullName"]))? true : false) );
                    lstObj.Add(sl);
                }
                return lstObj;
            }
            catch (Exception ex)
            {
                throw new Exception("sp_StateData_SelectAllForSelectInAddEditLocation:" + ex.Message);
            }
        }

        //select data from database as datatable
        public DataTable selectdatatable(Int32? Locationid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Locationid", Locationid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Location_select", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Location_select:" + ex.Message);
            }
        }

        //create parameter 
        public void createParameter(LocationClass obj, DB_con.DBTrans trans)
        {
            try
            {
                obj_con.clearParameter();

                if (Convert.ToString(obj.Locationname) != "")
                    obj_con.addParameter("@Locationname", string.IsNullOrEmpty(Convert.ToString(obj.Locationname)) ? "" : obj.Locationname);
                else
                    obj_con.addParameter("@Locationname", DBNull.Value);

                if (Convert.ToString(obj.Locationurl) != "")
                    obj_con.addParameter("@Locationurl", string.IsNullOrEmpty(Convert.ToString(obj.Locationurl)) ? "" : obj.Locationurl);
                else
                    obj_con.addParameter("@Locationurl", DBNull.Value);

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

                if (Convert.ToString(obj.Locationid) != "")
                    obj_con.addParameter("@Locationid", Convert.ToInt32(obj.Locationid), trans);
                else
                    obj_con.addParameter("@Locationid", DBNull.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //update edited object 
        public LocationClass updateObject(LocationClass obj)
        {
            try
            {

                LocationClass oldObj = selectById(obj.Locationid);
                if (obj.Locationname == null)
                    obj.Locationname = oldObj.Locationname;

                if (obj.Locationurl == null)
                    obj.Locationurl = oldObj.Locationurl;

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
        public List<LocationClass> ConvertToList(DataTable dt)
        {
            List<LocationClass> Locationlist = new List<LocationClass>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LocationClass obj_Location = new LocationClass();

                if (Convert.ToString(dt.Rows[i]["Locationid"]) != "")
                    obj_Location.Locationid = Convert.ToInt32(dt.Rows[i]["Locationid"]);

                if (Convert.ToString(dt.Rows[i]["Locationname"]) != "")
                    obj_Location.Locationname = Convert.ToString(dt.Rows[i]["Locationname"]);

                if (Convert.ToString(dt.Rows[i]["Locationurl"]) != "")
                    obj_Location.Locationurl = Convert.ToString(dt.Rows[i]["Locationurl"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_Location.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
                    obj_Location.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

                if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
                    obj_Location.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

                if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
                    obj_Location.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);


                Locationlist.Add(obj_Location);
            }
            return Locationlist;
        }

        //Convert DataTable To object method
        public LocationClass ConvertToOjbect(DataTable dt)
        {
            LocationClass obj_Location = new LocationClass();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (Convert.ToString(dt.Rows[i]["Locationid"]) != "")
                    obj_Location.Locationid = Convert.ToInt32(dt.Rows[i]["Locationid"]);

                if (Convert.ToString(dt.Rows[i]["Locationname"]) != "")
                    obj_Location.Locationname = Convert.ToString(dt.Rows[i]["Locationname"]);

                if (Convert.ToString(dt.Rows[i]["Locationurl"]) != "")
                    obj_Location.Locationurl = Convert.ToString(dt.Rows[i]["Locationurl"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_Location.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
                    obj_Location.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

                if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
                    obj_Location.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

                if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
                    obj_Location.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);
            }
            return obj_Location;
        }


        //disposble method
        void IDisposable.Dispose()
        {
            System.GC.SuppressFinalize(this);
            obj_con.closeConnection();
        }

        public DataTable CheckLocationExists(string Locationname)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Locationname", Locationname);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Location_CheckLocationExists", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Location_CheckLocationExists:" + ex.Message);
            }
        }
        public DataTable CheckLocationExistsUpdate(string Locationname,Int32? Locationid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Locationname", Locationname);
                obj_con.addParameter("@Locationid", Locationid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Location_CheckLocationExists_Update", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Location_CheckLocationExists_Update:" + ex.Message);
            }
        }
        #endregion
    }

}
