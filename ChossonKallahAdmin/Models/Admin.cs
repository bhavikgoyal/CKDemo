using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using DB_con;
using ChossonKallahAdmin.GlobalUtilities;

namespace ChossonKallah.Models
{
    public class AdminClass
    {
        #region "properties"
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Adminid { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Username { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email { get; set; }


        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Password { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isactive { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isdeleted { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> Createdon { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> Deletedon { get; set; }

        public AdminClass()
        {
        }
        #endregion
    }

    public class AdminCtl : IDisposable
    {
        #region "constructors"

        ConnectionCls obj_con = null;
        public AdminCtl()
        {
            obj_con = new ConnectionCls();
        }

        //Select Constructor
        public AdminCtl(Int32? id)
        {
            obj_con = new ConnectionCls();
            AdminClass obj_Adm = new AdminClass();
            using (DataTable dt = selectdatatable(id))
            {
                if (dt.Rows.Count > 0)
                {
                    obj_Adm.Adminid = Convert.ToInt32(dt.Rows[0]["Adminid"]);
                    obj_Adm.Name = Convert.ToString(dt.Rows[0]["Name"]);
                    obj_Adm.Username = Convert.ToString(dt.Rows[0]["Username"]);
                    obj_Adm.Email = Convert.ToString(dt.Rows[0]["Email"]);
                    obj_Adm.Password = Convert.ToString(dt.Rows[0]["Password"]);
                    obj_Adm.Isactive = Convert.ToBoolean(dt.Rows[0]["Isactive"]);
                    obj_Adm.Isdeleted = Convert.ToBoolean(dt.Rows[0]["Isdeleted"]);
                    obj_Adm.Createdon = Convert.ToDateTime(dt.Rows[0]["Createdon"]);
                    obj_Adm.Deletedon = Convert.ToDateTime(dt.Rows[0]["Deletedon"]);
                }
            }
        }

        #endregion

        #region "methods"

        //insert data into database 
        public Int32? insert(AdminClass obj)
        {
            try
            {
                obj.Adminid = 0;
                obj_con.clearParameter();
                createParameter(obj, DBTrans.Insert);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_Admin_insert", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Adminid = Convert.ToInt32(obj_con.getValue("@Adminid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_Admin_insert:" + ex.Message);
            }
        }

        //update data into database 
        public Int32? update(AdminClass obj)
        {
            try
            {
                obj_con.clearParameter();
                obj = updateObject(obj);
                createParameter(obj, DBTrans.Update);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_Admin_update", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Adminid = Convert.ToInt32(obj_con.getValue("@Adminid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_Admin_update:" + ex.Message);
            }
        }

        //delete data from database 
        public void delete(Int32? Adminid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.BeginTransaction();
                obj_con.addParameter("@Adminid", Adminid);
                obj_con.ExecuteNoneQuery("sp_Admin_delete", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_Admin_delete:" + ex.Message);
            }
        }

        //select all data from database 
        public string getAll()
        {
            try
            {
                obj_con.clearParameter();
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Admin_selectall", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return SessionUtilities.ConvertDataTableTojSonString(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Admin_selectall:" + ex.Message);
            }
        }
        public DataTable CheckAdminExists(string Email)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Email", Email);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Admin_CheckAdminExists", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Admin_CheckAdminExists:" + ex.Message);
            }
        }
        public DataTable CheckAdminExistsUpdate(string Email,Int32? adminid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Email", Email);
                obj_con.addParameter("@adminid", adminid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Admin_CheckAdminExistsUpdate", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Admin_CheckAdminExistsUpdate:" + ex.Message);
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
                return SessionUtilities.ConvertDataTableTojSonString(ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Admin_selectIndexPaging", CommandType.StoredProcedure)));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Admin_selectIndexPaging:" + ex.Message);
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
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Admin_selectIndexPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Admin_selectIndexPaging:" + ex.Message);
            }
        }

        //select data from database as list
        public string selectlist(Int32? Adminid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Adminid", Adminid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Admin_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return SessionUtilities.ConvertDataTableTojSonString(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Admin_select:" + ex.Message);
            }
        }

        //select data from database as Objject
        public AdminClass selectById(Int32? Adminid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Adminid", Adminid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Admin_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToOjbect(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Admin_select:" + ex.Message);
            }
        }

        //select data from database as datatable
        public DataTable selectdatatable(Int32? Adminid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Adminid", Adminid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Admin_select", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Admin_select:" + ex.Message);
            }
        }

        //create parameter 
        public void createParameter(AdminClass obj, DB_con.DBTrans trans)
        {
            try
            {
                obj_con.clearParameter();

                if (Convert.ToString(obj.Name) != "")
                    obj_con.addParameter("@Name", string.IsNullOrEmpty(Convert.ToString(obj.Name)) ? "" : obj.Name);
                else
                    obj_con.addParameter("@Name", DBNull.Value);

                if (Convert.ToString(obj.Username) != "")
                    obj_con.addParameter("@Username", string.IsNullOrEmpty(Convert.ToString(obj.Username)) ? "" : obj.Username);
                else
                    obj_con.addParameter("@Username", DBNull.Value);

                if (Convert.ToString(obj.Email) != "")
                    obj_con.addParameter("@Email", string.IsNullOrEmpty(Convert.ToString(obj.Email)) ? "" : obj.Email);
                else
                    obj_con.addParameter("@Email", DBNull.Value);

                if (Convert.ToString(obj.Password) != "")
                    obj_con.addParameter("@Password", string.IsNullOrEmpty(Convert.ToString(obj.Password)) ? "" : obj.Password);
                else
                    obj_con.addParameter("@Password", DBNull.Value);

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

                if (Convert.ToString(obj.Adminid) != "")
                    obj_con.addParameter("@Adminid", Convert.ToInt32(obj.Adminid), trans);
                else
                    obj_con.addParameter("@Adminid", DBNull.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //update edited object 
        public AdminClass updateObject(AdminClass obj)
        {
            try
            {

                AdminClass oldObj = selectById(obj.Adminid);
                if (obj.Name == null)
                    obj.Name = oldObj.Name;

                if (obj.Username == null)
                    obj.Username = oldObj.Username;

                if (obj.Email == null)
                    obj.Email = oldObj.Email;

                if (obj.Password == null)
                    obj.Password = oldObj.Password;

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

        //Convert DataTable To object method
        public AdminClass ConvertToOjbect(DataTable dt)
        {
            AdminClass obj_Admin = new AdminClass();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (Convert.ToString(dt.Rows[i]["Adminid"]) != "")
                    obj_Admin.Adminid = Convert.ToInt32(dt.Rows[i]["Adminid"]);

                if (Convert.ToString(dt.Rows[i]["Name"]) != "")
                    obj_Admin.Name = Convert.ToString(dt.Rows[i]["Name"]);

                if (Convert.ToString(dt.Rows[i]["Username"]) != "")
                    obj_Admin.Username = Convert.ToString(dt.Rows[i]["Username"]);

                if (Convert.ToString(dt.Rows[i]["Email"]) != "")
                    obj_Admin.Email = Convert.ToString(dt.Rows[i]["Email"]);

                if (Convert.ToString(dt.Rows[i]["Password"]) != "")
                    obj_Admin.Password = Convert.ToString(dt.Rows[i]["Password"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_Admin.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
                    obj_Admin.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

                if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
                    obj_Admin.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

                if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
                    obj_Admin.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);
            }
            return obj_Admin;
        }


        //disposble method
        void IDisposable.Dispose()
        {
            System.GC.SuppressFinalize(this);
            obj_con.closeConnection();
        }
        // Login
        public DataTable CheckUserNamePass(AdminClass obj)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.BeginTransaction();
                obj_con.addParameter("@Email", obj.Email);
                obj_con.addParameter("@Password", obj.Password);
                obj_con.ExecuteNoneQuery("sp_admin_CheckUserNamePassLogin", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_admin_CheckUserNamePassLogin", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_admin_CheckUserNamePassLogin");
            }
        }
        #endregion
    }

}
