using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Mvc;
using DB_con;

namespace ChossonKallah.Models
{
    public class BusinessDirectoryClass
    {
        #region "properties"
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Businessid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Businessname { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Website { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Businessurl { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Phonenumber { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Phonenumber2 { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Businessimage { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? Locationid { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public Int32? CategoryId { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Categories { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Address { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Addressline2 { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string City { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string State { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Zipcode { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BusinessVideoURL { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BusinessLogo { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Locationlatitude { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Locationlongitude { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CategoryIdWithComma { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isactive { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool IsFeatured { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool HasBrochure { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public bool Isdeleted { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> Createdon { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<DateTime> Deletedon { get; set; }



        public BusinessDirectoryClass()
        {
        }
        #endregion
    }

    public class BusinessDirectoryCtl : IDisposable
    {
        #region "constructors"

        ConnectionCls obj_con = null;
        //Default Constructor
        public BusinessDirectoryCtl()
        {
            obj_con = new ConnectionCls();
        }

        //Select Constructor
        public BusinessDirectoryCtl(Int32? id)
        {
            obj_con = new ConnectionCls();
            BusinessDirectoryClass obj_Bus = new BusinessDirectoryClass();
            using (DataTable dt = selectdatatable(id))
            {
                if (dt.Rows.Count > 0)
                {

                    obj_Bus.Businessid = Convert.ToInt32(dt.Rows[0]["Businessid"]);
                    obj_Bus.Businessname = Convert.ToString(dt.Rows[0]["Businessname"]);
                    obj_Bus.Website = Convert.ToString(dt.Rows[0]["Website"]);
                    obj_Bus.Businessurl = Convert.ToString(dt.Rows[0]["Businessurl"]);
                    obj_Bus.Phonenumber = Convert.ToString(dt.Rows[0]["Phonenumber"]);
                    obj_Bus.Businessimage = Convert.ToString(dt.Rows[0]["Businessimage"]);
                    obj_Bus.Locationid = Convert.ToInt32(dt.Rows[0]["Locationid"]);
                    obj_Bus.Address = Convert.ToString(dt.Rows[0]["Address"]);
                    obj_Bus.Addressline2 = Convert.ToString(dt.Rows[0]["Addressline2"]);
                    obj_Bus.City = Convert.ToString(dt.Rows[0]["City"]);
                    obj_Bus.State = Convert.ToString(dt.Rows[0]["State"]);
                    obj_Bus.Zipcode = Convert.ToString(dt.Rows[0]["Zipcode"]);
                    obj_Bus.Locationlatitude = Convert.ToString(dt.Rows[0]["Locationlatitude"]);
                    obj_Bus.Locationlongitude = Convert.ToString(dt.Rows[0]["Locationlongitude"]);
                    obj_Bus.Isactive = Convert.ToBoolean(dt.Rows[0]["Isactive"]);
                    obj_Bus.Isdeleted = Convert.ToBoolean(dt.Rows[0]["Isdeleted"]);
                    obj_Bus.Createdon = Convert.ToDateTime(dt.Rows[0]["Createdon"]);
                    obj_Bus.Deletedon = Convert.ToDateTime(dt.Rows[0]["Deletedon"]);
                    obj_Bus.Phonenumber2 = Convert.ToString(dt.Rows[0]["Phonenumber2"]);
                    obj_Bus.Email = Convert.ToString(dt.Rows[0]["Email"]);
                    obj_Bus.HasBrochure = Convert.ToBoolean(dt.Rows[0]["HasBrochure"]);
                    obj_Bus.IsFeatured = Convert.ToBoolean(dt.Rows[0]["IsFeatured"]);
                    obj_Bus.BusinessVideoURL = Convert.ToString(dt.Rows[0]["BusinessVideoURL"]);
                    obj_Bus.BusinessLogo = Convert.ToString(dt.Rows[0]["BusinessLogo"]);
                }
            }
        }


        #endregion

        #region "methods"

        //insert data into database 
        public Int32? insert(BusinessDirectoryClass obj)
        {
            try
            {
                obj.Businessid = 0;
                obj_con.clearParameter();
                createParameter(obj, DBTrans.Insert);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_BusinessDirectory_insert", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Businessid = Convert.ToInt32(obj_con.getValue("@Businessid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_BusinessDirectory_insert:" + ex.Message);
            }
        }

        //update data into database 
        public Int32? update(BusinessDirectoryClass obj)
        {
            try
            {
                obj_con.clearParameter();
                obj = updateObject(obj);
                createParameter(obj, DBTrans.Update);
                obj_con.BeginTransaction();
                obj_con.ExecuteNoneQuery("sp_BusinessDirectory_update", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
                return obj.Businessid = Convert.ToInt32(obj_con.getValue("@Businessid"));
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_BusinessDirectory_update:" + ex.Message);
            }
        }

        //delete data from database 
        public void delete(Int32? Businessid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.BeginTransaction();
                obj_con.addParameter("@Businessid", Businessid);
                obj_con.ExecuteNoneQuery("sp_BusinessDirectory_delete", CommandType.StoredProcedure);
                obj_con.CommitTransaction();
            }
            catch (Exception ex)
            {
                obj_con.RollbackTransaction();
                throw new Exception("sp_BusinessDirectory_delete:" + ex.Message);
            }
        }

        //select all data from database 
        public List<BusinessDirectoryClass> getAll()
        {
            try
            {
                obj_con.clearParameter();
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessDirectory_selectall", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessDirectory_selectall:" + ex.Message);
            }
        }

        //select data from database as Paging
        public List<BusinessDirectoryClass> selectPaging(Int64 firstPageIndex, Int64 pageSize)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@pageFirstIndex", firstPageIndex);
                obj_con.addParameter("@pageLastIndex", pageSize);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessDirectory_selectPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessDirectory_selectPaging:" + ex.Message);
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
                return ChossonKallahAdmin.GlobalUtilities.SessionUtilities.ConvertDataTableTojSonString(ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessDirectory_selectIndexPaging", CommandType.StoredProcedure)));

            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessDirectory_selectIndexPaging:" + ex.Message);
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
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessDirectory_selectIndexPaging", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessDirectory_selectIndexPaging:" + ex.Message);
            }
        }
        public List<BusinessDirectoryClass> selectIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search)
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
        public List<BusinessDirectoryClass> selectlist(Int32? Businessid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businessid", Businessid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessDirectory_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToList(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessDirectory_select:" + ex.Message);
            }
        }

        //select data from database as Objject
        public BusinessDirectoryClass selectById(Int32? Businessid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businessid", Businessid);
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessDirectory_select", CommandType.StoredProcedure));
                obj_con.CommitTransaction();
                obj_con.closeConnection();
                return ConvertToOjbect(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessDirectory_select:" + ex.Message);
            }
        }

        

        //select data from database as datatable
        public DataTable selectdatatable(Int32? Businessid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Businessid", Businessid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_BusinessDirectory_select", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_BusinessDirectory_select:" + ex.Message);
            }
        }

        //create parameter 
        public void createParameter(BusinessDirectoryClass obj, DB_con.DBTrans trans)
        {
            try
            {
                obj_con.clearParameter();

                if (Convert.ToString(obj.Businessname) != "")
                    obj_con.addParameter("@Businessname", string.IsNullOrEmpty(Convert.ToString(obj.Businessname)) ? "" : obj.Businessname);
                else
                    obj_con.addParameter("@Businessname", DBNull.Value);

                if (Convert.ToString(obj.Website) != "")
                    obj_con.addParameter("@Website", string.IsNullOrEmpty(Convert.ToString(obj.Website)) ? "" : obj.Website);
                else
                    obj_con.addParameter("@Website", DBNull.Value);

                if (Convert.ToString(obj.Businessurl) != "")
                    obj_con.addParameter("@Businessurl", string.IsNullOrEmpty(Convert.ToString(obj.Businessurl)) ? "" : obj.Businessurl);
                else
                    obj_con.addParameter("@Businessurl", DBNull.Value);

                if (Convert.ToString(obj.Categories) != "")
                    obj_con.addParameter("@Categories", string.IsNullOrEmpty(Convert.ToString(obj.Categories)) ? "" : obj.Categories);
                else
                    obj_con.addParameter("@Categories", DBNull.Value);

                if (Convert.ToString(obj.Phonenumber) != "")
                    obj_con.addParameter("@Phonenumber", string.IsNullOrEmpty(Convert.ToString(obj.Phonenumber)) ? "" : obj.Phonenumber);
                else
                    obj_con.addParameter("@Phonenumber", DBNull.Value);

                if (Convert.ToString(obj.Phonenumber2) != "")
                    obj_con.addParameter("@Phonenumber2", string.IsNullOrEmpty(Convert.ToString(obj.Phonenumber2)) ? "" : obj.Phonenumber2);
                else
                    obj_con.addParameter("@Phonenumber2", DBNull.Value);

                if (Convert.ToString(obj.Email) != "")
                    obj_con.addParameter("@Email", string.IsNullOrEmpty(Convert.ToString(obj.Email)) ? "" : obj.Email);
                else
                    obj_con.addParameter("@Email", DBNull.Value);

                if (Convert.ToString(obj.BusinessVideoURL) != "")
                    obj_con.addParameter("@BusinessVideoURL", string.IsNullOrEmpty(Convert.ToString(obj.BusinessVideoURL)) ? "" : obj.BusinessVideoURL);
                else
                    obj_con.addParameter("@BusinessVideoURL", DBNull.Value);

                if (Convert.ToString(obj.BusinessLogo) != "")
                    obj_con.addParameter("@BusinessLogo", string.IsNullOrEmpty(Convert.ToString(obj.BusinessLogo)) ? "" : obj.BusinessLogo);
                else
                    obj_con.addParameter("@BusinessLogo", DBNull.Value);

                if (Convert.ToString(obj.Businessimage) != "")
                    obj_con.addParameter("@Businessimage", string.IsNullOrEmpty(Convert.ToString(obj.Businessimage)) ? "" : obj.Businessimage);
                else
                    obj_con.addParameter("@Businessimage", DBNull.Value);

                if (Convert.ToString(obj.Locationid) != "")
                    obj_con.addParameter("@Locationid", string.IsNullOrEmpty(Convert.ToString(obj.Locationid)) ? 0 : obj.Locationid);
                else
                    obj_con.addParameter("@Locationid", DBNull.Value);

                if (Convert.ToString(obj.Address) != "")
                    obj_con.addParameter("@Address", string.IsNullOrEmpty(Convert.ToString(obj.Address)) ? "" : obj.Address);
                else
                    obj_con.addParameter("@Address", DBNull.Value);

                if (Convert.ToString(obj.Addressline2) != "")
                    obj_con.addParameter("@Addressline2", string.IsNullOrEmpty(Convert.ToString(obj.Addressline2)) ? "" : obj.Addressline2);
                else
                    obj_con.addParameter("@Addressline2", DBNull.Value);

                if (Convert.ToString(obj.City) != "")
                    obj_con.addParameter("@City", string.IsNullOrEmpty(Convert.ToString(obj.City)) ? "" : obj.City);
                else
                    obj_con.addParameter("@City", DBNull.Value);

                if (Convert.ToString(obj.State) != "")
                    obj_con.addParameter("@State", string.IsNullOrEmpty(Convert.ToString(obj.State)) ? "" : obj.State);
                else
                    obj_con.addParameter("@State", DBNull.Value);

                if (Convert.ToString(obj.Zipcode) != "")
                    obj_con.addParameter("@Zipcode", string.IsNullOrEmpty(Convert.ToString(obj.Zipcode)) ? "" : obj.Zipcode);
                else
                    obj_con.addParameter("@Zipcode", DBNull.Value);

                if (Convert.ToString(obj.Locationlatitude) != "")
                    obj_con.addParameter("@Locationlatitude", string.IsNullOrEmpty(Convert.ToString(obj.Locationlatitude)) ? "" : obj.Locationlatitude);
                else
                    obj_con.addParameter("@Locationlatitude", DBNull.Value);

                if (Convert.ToString(obj.Locationlongitude) != "")
                    obj_con.addParameter("@Locationlongitude", string.IsNullOrEmpty(Convert.ToString(obj.Locationlongitude)) ? "" : obj.Locationlongitude);
                else
                    obj_con.addParameter("@Locationlongitude", DBNull.Value);

                if (Convert.ToString(obj.Isactive) != "")
                    obj_con.addParameter("@Isactive", string.IsNullOrEmpty(Convert.ToString(obj.Isactive)) ? false : obj.Isactive);
                else
                    obj_con.addParameter("@Isactive", DBNull.Value);

                if (Convert.ToString(obj.Isdeleted) != "")
                    obj_con.addParameter("@Isdeleted", string.IsNullOrEmpty(Convert.ToString(obj.Isdeleted)) ? false : obj.Isdeleted);
                else
                    obj_con.addParameter("@Isdeleted", DBNull.Value);

                if (Convert.ToString(obj.IsFeatured) != "")
                    obj_con.addParameter("@IsFeatured", string.IsNullOrEmpty(Convert.ToString(obj.IsFeatured)) ? false : obj.IsFeatured);
                else
                    obj_con.addParameter("@IsFeatured", DBNull.Value);

                if (Convert.ToString(obj.HasBrochure) != "")
                    obj_con.addParameter("@HasBrochure", string.IsNullOrEmpty(Convert.ToString(obj.HasBrochure)) ? false : obj.HasBrochure);
                else
                    obj_con.addParameter("@HasBrochure", DBNull.Value);

                if (Convert.ToString(obj.Createdon) != "")
                    obj_con.addParameter("@Createdon", string.IsNullOrEmpty(Convert.ToString(obj.Createdon)) ? Convert.ToDateTime("1900-01-01") : obj.Createdon);
                else
                    obj_con.addParameter("@Createdon", DBNull.Value);

                if (Convert.ToString(obj.Deletedon) != "")
                    obj_con.addParameter("@Deletedon", string.IsNullOrEmpty(Convert.ToString(obj.Deletedon)) ? Convert.ToDateTime("1900-01-01") : obj.Deletedon);
                else
                    obj_con.addParameter("@Deletedon", DBNull.Value);

                if (Convert.ToString(obj.Businessid) != "")
                    obj_con.addParameter("@Businessid", Convert.ToInt32(obj.Businessid), trans);
                else
                    obj_con.addParameter("@Businessid", DBNull.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //update edited object 
        public BusinessDirectoryClass updateObject(BusinessDirectoryClass obj)
        {
            try
            {

                BusinessDirectoryClass oldObj = selectById(obj.Businessid);
                if (obj.Businessname == null)
                    obj.Businessname = oldObj.Businessname;

                if (obj.Website == null)
                    obj.Website = oldObj.Website;

                if (obj.Businessurl == null)
                    obj.Businessurl = oldObj.Businessurl;

                if (obj.Phonenumber == null)
                    obj.Phonenumber = oldObj.Phonenumber;

                if (obj.Businessimage == null)
                    obj.Businessimage = oldObj.Businessimage;
                if (obj.BusinessLogo == null)
                    obj.BusinessLogo = oldObj.BusinessLogo;

                if (obj.Locationid == null)
                    obj.Locationid = oldObj.Locationid;

                if (obj.Address == null)
                    obj.Address = oldObj.Address;

                if (obj.Addressline2 == null)
                    obj.Addressline2 = oldObj.Addressline2;

                if (obj.City == null)
                    obj.City = oldObj.City;

                if (obj.State == null)
                    obj.State = oldObj.State;

                if (obj.Zipcode == null)
                    obj.Zipcode = oldObj.Zipcode;

                if (obj.Locationlatitude == null)
                    obj.Locationlatitude = oldObj.Locationlatitude;

                if (obj.Locationlongitude == null)
                    obj.Locationlongitude = oldObj.Locationlongitude;

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
        public List<BusinessDirectoryClass> ConvertToList(DataTable dt)
        {
            List<BusinessDirectoryClass> BusinessDirectorylist = new List<BusinessDirectoryClass>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                BusinessDirectoryClass obj_BusinessDirectory = new BusinessDirectoryClass();

                if (Convert.ToString(dt.Rows[i]["Businessid"]) != "")
                    obj_BusinessDirectory.Businessid = Convert.ToInt32(dt.Rows[i]["Businessid"]);

                if (Convert.ToString(dt.Rows[i]["Businessname"]) != "")
                    obj_BusinessDirectory.Businessname = Convert.ToString(dt.Rows[i]["Businessname"]);

                if (Convert.ToString(dt.Rows[i]["Website"]) != "")
                    obj_BusinessDirectory.Website = Convert.ToString(dt.Rows[i]["Website"]);

                if (Convert.ToString(dt.Rows[i]["Businessurl"]) != "")
                    obj_BusinessDirectory.Businessurl = Convert.ToString(dt.Rows[i]["Businessurl"]);

                if (Convert.ToString(dt.Rows[i]["Phonenumber"]) != "")
                    obj_BusinessDirectory.Phonenumber = Convert.ToString(dt.Rows[i]["Phonenumber"]);

                if (Convert.ToString(dt.Rows[i]["Businessimage"]) != "")
                    obj_BusinessDirectory.Businessimage = Convert.ToString(dt.Rows[i]["Businessimage"]);

                if (Convert.ToString(dt.Rows[i]["Locationid"]) != "")
                    obj_BusinessDirectory.Locationid = Convert.ToInt32(dt.Rows[i]["Locationid"]);

                if (Convert.ToString(dt.Rows[i]["Address"]) != "")
                    obj_BusinessDirectory.Address = Convert.ToString(dt.Rows[i]["Address"]);

                if (Convert.ToString(dt.Rows[i]["Addressline2"]) != "")
                    obj_BusinessDirectory.Addressline2 = Convert.ToString(dt.Rows[i]["Addressline2"]);

                if (Convert.ToString(dt.Rows[i]["City"]) != "")
                    obj_BusinessDirectory.City = Convert.ToString(dt.Rows[i]["City"]);

                if (Convert.ToString(dt.Rows[i]["State"]) != "")
                    obj_BusinessDirectory.State = Convert.ToString(dt.Rows[i]["State"]);

                if (Convert.ToString(dt.Rows[i]["Zipcode"]) != "")
                    obj_BusinessDirectory.Zipcode = Convert.ToString(dt.Rows[i]["Zipcode"]);

                if (Convert.ToString(dt.Rows[i]["Locationlatitude"]) != "")
                    obj_BusinessDirectory.Locationlatitude = Convert.ToString(dt.Rows[i]["Locationlatitude"]);

                if (Convert.ToString(dt.Rows[i]["Locationlongitude"]) != "")
                    obj_BusinessDirectory.Locationlongitude = Convert.ToString(dt.Rows[i]["Locationlongitude"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_BusinessDirectory.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
                    obj_BusinessDirectory.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

                if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
                    obj_BusinessDirectory.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

                if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
                    obj_BusinessDirectory.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);


                BusinessDirectorylist.Add(obj_BusinessDirectory);
            }
            return BusinessDirectorylist;
        }

        //Convert DataTable To object method
        public BusinessDirectoryClass ConvertToOjbect(DataTable dt)
        {
            BusinessDirectoryClass obj_BusinessDirectory = new BusinessDirectoryClass();
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (Convert.ToString(dt.Rows[i]["Businessid"]) != "")
                    obj_BusinessDirectory.Businessid = Convert.ToInt32(dt.Rows[i]["Businessid"]);

                if (Convert.ToString(dt.Rows[i]["Businessname"]) != "")
                    obj_BusinessDirectory.Businessname = Convert.ToString(dt.Rows[i]["Businessname"]);

                if (Convert.ToString(dt.Rows[i]["Website"]) != "")
                    obj_BusinessDirectory.Website = Convert.ToString(dt.Rows[i]["Website"]);

                if (Convert.ToString(dt.Rows[i]["Businessurl"]) != "")
                    obj_BusinessDirectory.Businessurl = Convert.ToString(dt.Rows[i]["Businessurl"]);

                if (Convert.ToString(dt.Rows[i]["Phonenumber"]) != "")
                    obj_BusinessDirectory.Phonenumber = Convert.ToString(dt.Rows[i]["Phonenumber"]);

                if (Convert.ToString(dt.Rows[i]["Businessimage"]) != "")
                    obj_BusinessDirectory.Businessimage = Convert.ToString(dt.Rows[i]["Businessimage"]);

                if (Convert.ToString(dt.Rows[i]["Locationid"]) != "")
                    obj_BusinessDirectory.Locationid = Convert.ToInt32(dt.Rows[i]["Locationid"]);

                if (Convert.ToString(dt.Rows[i]["Address"]) != "")
                    obj_BusinessDirectory.Address = Convert.ToString(dt.Rows[i]["Address"]);

                if (Convert.ToString(dt.Rows[i]["Addressline2"]) != "")
                    obj_BusinessDirectory.Addressline2 = Convert.ToString(dt.Rows[i]["Addressline2"]);

                if (Convert.ToString(dt.Rows[i]["City"]) != "")
                    obj_BusinessDirectory.City = Convert.ToString(dt.Rows[i]["City"]);

                if (Convert.ToString(dt.Rows[i]["State"]) != "")
                    obj_BusinessDirectory.State = Convert.ToString(dt.Rows[i]["State"]);

                if (Convert.ToString(dt.Rows[i]["Zipcode"]) != "")
                    obj_BusinessDirectory.Zipcode = Convert.ToString(dt.Rows[i]["Zipcode"]);

                if (Convert.ToString(dt.Rows[i]["Locationlatitude"]) != "")
                    obj_BusinessDirectory.Locationlatitude = Convert.ToString(dt.Rows[i]["Locationlatitude"]);

                if (Convert.ToString(dt.Rows[i]["Locationlongitude"]) != "")
                    obj_BusinessDirectory.Locationlongitude = Convert.ToString(dt.Rows[i]["Locationlongitude"]);

                if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
                    obj_BusinessDirectory.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

                if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
                    obj_BusinessDirectory.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

                if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
                    obj_BusinessDirectory.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

                if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
                    obj_BusinessDirectory.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);

                if (Convert.ToString(dt.Rows[i]["PhoneNumber2"]) != "")
                    obj_BusinessDirectory.Phonenumber2 = Convert.ToString(dt.Rows[i]["PhoneNumber2"]);

                if (Convert.ToString(dt.Rows[i]["Email"]) != "")
                    obj_BusinessDirectory.Email = Convert.ToString(dt.Rows[i]["Email"]);

                if (Convert.ToString(dt.Rows[i]["HasBrochure"]) != "")
                    obj_BusinessDirectory.HasBrochure = Convert.ToBoolean(dt.Rows[i]["HasBrochure"]);

                if (Convert.ToString(dt.Rows[i]["IsFeatured"]) != "")
                    obj_BusinessDirectory.IsFeatured = Convert.ToBoolean(dt.Rows[i]["IsFeatured"]);

                if (Convert.ToString(dt.Rows[i]["BusinessVideoURL"]) != "")
                    obj_BusinessDirectory.BusinessVideoURL = Convert.ToString(dt.Rows[i]["BusinessVideoURL"]);

                if (Convert.ToString(dt.Rows[i]["CategoryId"]) != "")
                    obj_BusinessDirectory.CategoryIdWithComma = Convert.ToString(dt.Rows[i]["CategoryId"]).Replace(" ", "").Trim();

                if (Convert.ToString(dt.Rows[i]["BusinessLogo"]) != "")
                    obj_BusinessDirectory.BusinessLogo = Convert.ToString(dt.Rows[i]["BusinessLogo"]);
            }
            return obj_BusinessDirectory;
        }


        //disposble method
        void IDisposable.Dispose()
        {
            System.GC.SuppressFinalize(this);
            obj_con.closeConnection();
        }

        public List<SelectListItem> LocationDropdown()
        {
            try
            {
                obj_con.clearParameter();
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Location_Id_Name_selectall", CommandType.StoredProcedure));
                List<SelectListItem> IdName = new List<SelectListItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SelectListItem obj = new SelectListItem();
                    obj.Text = Convert.ToString(dt.Rows[i]["LocationName"]);
                    obj.Value = Convert.ToString(dt.Rows[i]["LocationId"]);
                    IdName.Add(obj);
                }
                return IdName;
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Location_Id_Name_selectall:" + ex.Message);
            }
        }

        public List<SelectListItem> CategoryDropdown()
        {
            try
            {
                obj_con.clearParameter();
                DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Category_Id_Name_selectall", CommandType.StoredProcedure));
                List<SelectListItem> IdName = new List<SelectListItem>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SelectListItem obj = new SelectListItem();
                    obj.Text = Convert.ToString(dt.Rows[i]["CategoryName"]);
                    obj.Value = Convert.ToString(dt.Rows[i]["CategoryId"]);
                    IdName.Add(obj);
                }
                return IdName;
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Category_Id_Name_selectall:" + ex.Message);
            }
        }

        public DataTable CheckAdminExists(string Email)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Email", Email);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Admin_CheckBusinessDirectoryExists", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Admin_CheckBusinessDirectoryExists:" + ex.Message);
            }
        }

        public DataTable CheckAdminExistsUpdate(string Email, Int32? businessdirectoryid)
        {
            try
            {
                obj_con.clearParameter();
                obj_con.addParameter("@Email", Email);
                obj_con.addParameter("@businessdirectoryid", businessdirectoryid);
                return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Admin_CheckBusinessdirectoryExistsUpdate", CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                throw new Exception("sp_Admin_CheckBusinessdirectoryExistsUpdate:" + ex.Message);
            }
        }
        #endregion
    }

}
