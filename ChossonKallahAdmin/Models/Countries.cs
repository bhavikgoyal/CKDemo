using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using DB_con;

namespace ChossonKallah.Models
{
    public class CountriesClass
    {
		#region "properties"
			[DisplayFormat(ConvertEmptyStringToNull = false)]
public Int32?  Countryid {get;set;}

[DisplayFormat(ConvertEmptyStringToNull = false)]
public string  Countryname {get;set;}

[DisplayFormat(ConvertEmptyStringToNull = false)]
public bool  Isactive {get;set;}

[DisplayFormat(ConvertEmptyStringToNull = false)]
public bool  Isdeleted {get;set;}

 [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")] 
			 public Nullable<DateTime>  Createdon {get;set;}

 [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")] 
			 public Nullable<DateTime>  Deletedon {get;set;}



	 public CountriesClass(){
	 }
		#endregion
    }

	public class CountriesCtl :  IDisposable
	{
		#region "constructors"
		
			ConnectionCls obj_con = null;
//Default Constructor
public CountriesCtl()
{
	 obj_con = new ConnectionCls();
}

//Select Constructor
public CountriesCtl(Int32? id)
{
obj_con = new ConnectionCls();
CountriesClass  obj_Cou= new CountriesClass();
using (DataTable dt = selectdatatable(id))
{
if (dt.Rows.Count > 0)
{

obj_Cou.Countryid = Convert.ToInt32(dt.Rows[0]["Countryid"]);
obj_Cou.Countryname = Convert.ToString(dt.Rows[0]["Countryname"]);
obj_Cou.Isactive = Convert.ToBoolean(dt.Rows[0]["Isactive"]);
obj_Cou.Isdeleted = Convert.ToBoolean(dt.Rows[0]["Isdeleted"]);
obj_Cou.Createdon = Convert.ToDateTime(dt.Rows[0]["Createdon"]);
obj_Cou.Deletedon = Convert.ToDateTime(dt.Rows[0]["Deletedon"]);

}
}
}

			
		#endregion
		
		#region "methods"
		
			//insert data into database 
public Int32? insert(CountriesClass obj)
{
try 
{
obj_con.clearParameter();
createParameter(obj, DBTrans.Insert);
obj_con.BeginTransaction();
obj_con.ExecuteNoneQuery("sp_Countries_insert", CommandType.StoredProcedure);
obj_con.CommitTransaction();
return obj.Countryid = Convert.ToInt32(obj_con.getValue("@Countryid"));
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_Countries_insert:"+ex.Message);
}
}

//update data into database 
public Int32? update(CountriesClass obj)
{
try 
{
obj_con.clearParameter();
obj = updateObject(obj);
createParameter(obj, DBTrans.Update);
obj_con.BeginTransaction();
obj_con.ExecuteNoneQuery("sp_Countries_update", CommandType.StoredProcedure);
obj_con.CommitTransaction();
return obj.Countryid = Convert.ToInt32(obj_con.getValue("@Countryid"));
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_Countries_update:"+ex.Message);
}
}

//delete data from database 
public void delete(Int32? Countryid)
{
try 
{
obj_con.clearParameter();
obj_con.BeginTransaction();
obj_con.addParameter("@Countryid", Countryid );
obj_con.ExecuteNoneQuery("sp_Countries_delete", CommandType.StoredProcedure);
obj_con.CommitTransaction();
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_Countries_delete:"+ex.Message);
}
}

//select all data from database 
public List<CountriesClass> getAll()
{
try 
{
obj_con.clearParameter();
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Countries_selectall", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToList(dt);
}
catch (Exception ex)
{
throw new Exception("sp_Countries_selectall:"+ex.Message);
}
}

//select data from database as Paging
public List<CountriesClass> selectPaging(Int64 firstPageIndex, Int64 pageSize)
{
	 try 
	 {
		 obj_con.clearParameter();
		 obj_con.addParameter("@pageFirstIndex", firstPageIndex );
		 obj_con.addParameter("@pageLastIndex", pageSize );
		 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Countries_selectPaging", CommandType.StoredProcedure));
		 obj_con.CommitTransaction();
		 obj_con.closeConnection();
		 return ConvertToList(dt);
	  }
	 catch (Exception ex)
	 {
		 throw new Exception("sp_Countries_selectPaging:"+ex.Message);
	 }
}

	 public List<CountriesClass> selectIndexPaging(Int64 PageSize, Int64 PageIndex, string Search){
		 try{
			 obj_con.clearParameter();
			 obj_con.addParameter("@PageSize", PageSize);
			 obj_con.addParameter("@PageIndex", PageIndex);
			 obj_con.addParameter("@Search", Search);
			 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Countries_selectIndexPaging", CommandType.StoredProcedure));
			 obj_con.CommitTransaction();
			 obj_con.closeConnection();
			 return ConvertToList(dt);
		 } catch (Exception ex){
			 throw new Exception("sp_Countries_selectIndexPaging:"+ex.Message);
		 }
	 }
	 public Int32 selectIndexPagingCount(Int64 PageSize, Int64 PageIndex, string Search){
		 try{
			 obj_con.clearParameter();
			 obj_con.addParameter("@PageSize", PageSize);
			 obj_con.addParameter("@PageIndex", PageIndex);
			 obj_con.addParameter("@Search", Search);
			 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Countries_selectIndexPaging", CommandType.StoredProcedure));
			 obj_con.CommitTransaction();
			 obj_con.closeConnection();
			 return Convert.ToInt32(dt.Rows[0][0]);
		 } catch (Exception ex){
			 throw new Exception("sp_Countries_selectIndexPaging:"+ex.Message);
		 }
	 }
	 public List<CountriesClass> selectIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search){
		 try{
			 obj_con.clearParameter();
			 obj_con.addParameter("@StartIndex", StartIndex);
			 obj_con.addParameter("@EndIndex", EndIndex);
			 obj_con.addParameter("@Search", Search);
			 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_AddressBook_selectLazyLoading", CommandType.StoredProcedure));
			 obj_con.CommitTransaction();
			 obj_con.closeConnection();
			 return ConvertToList(dt);
	 }catch (Exception ex){
		 throw new Exception("sp_AddressBook_selectLazyLoading:"+ex.Message);
	 }
	 }
//select data from database as list
public List<CountriesClass> selectlist(Int32? Countryid)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Countryid", Countryid );
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Countries_select", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToList(dt);
}
catch (Exception ex)
{
throw new Exception("sp_Countries_select:"+ex.Message);
}
}

//select data from database as Objject
public CountriesClass selectById(Int32? Countryid)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Countryid", Countryid );
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Countries_select", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToOjbect(dt);
}
catch (Exception ex)
{
throw new Exception("sp_Countries_select:"+ex.Message);
}
}

//select data from database as datatable
public DataTable selectdatatable(Int32? Countryid)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Countryid", Countryid );
return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Countries_select", CommandType.StoredProcedure));
}
catch (Exception ex)
{
throw new Exception("sp_Countries_select:"+ex.Message);
}
}

//create parameter 
public void createParameter(CountriesClass  obj, DB_con.DBTrans trans)
{
try
{
	 obj_con.clearParameter();

		 if(Convert.ToString(obj.Countryname) != "")
			obj_con.addParameter("@Countryname", string.IsNullOrEmpty(Convert.ToString(obj.Countryname)) ? "" : obj.Countryname);
	 else 
obj_con.addParameter("@Countryname", DBNull.Value );

		 if(Convert.ToString(obj.Isactive) != "")
			obj_con.addParameter("@Isactive", string.IsNullOrEmpty(Convert.ToString(obj.Isactive)) ? false : obj.Isactive);
	 else 
obj_con.addParameter("@Isactive", DBNull.Value );

		 if(Convert.ToString(obj.Isdeleted) != "")
			obj_con.addParameter("@Isdeleted", string.IsNullOrEmpty(Convert.ToString(obj.Isdeleted)) ? false : obj.Isdeleted);
	 else 
obj_con.addParameter("@Isdeleted", DBNull.Value );

		 if(Convert.ToString(obj.Createdon) != "")
			obj_con.addParameter("@Createdon", string.IsNullOrEmpty(Convert.ToString(obj.Createdon)) ? Convert.ToDateTime("1900-01-01") : obj.Createdon);
	 else 
obj_con.addParameter("@Createdon", DBNull.Value );

		 if(Convert.ToString(obj.Deletedon) != "")
			obj_con.addParameter("@Deletedon", string.IsNullOrEmpty(Convert.ToString(obj.Deletedon)) ? Convert.ToDateTime("1900-01-01") : obj.Deletedon);
	 else 
obj_con.addParameter("@Deletedon", DBNull.Value );

		 if(Convert.ToString(obj.Countryid) != "")
			obj_con.addParameter("@Countryid", Convert.ToInt32(obj.Countryid), trans);
	 else 
obj_con.addParameter("@Countryid", DBNull.Value );
}
catch (Exception ex)
{
throw ex;
}
}

//update edited object 
public CountriesClass updateObject(CountriesClass  obj)
{
try
{

	 CountriesClass oldObj = selectById(obj.Countryid);
 if (obj.Countryname == null)
	 obj.Countryname = oldObj.Countryname; 

 if (obj.Isactive == null)
	 obj.Isactive = oldObj.Isactive; 

 if (obj.Isdeleted == null)
	 obj.Isdeleted = oldObj.Isdeleted; 

 if (obj.Createdon == null)
	 obj.Createdon = oldObj.Createdon; 

 if (obj.Deletedon == null)
	 obj.Deletedon = oldObj.Deletedon; 

	 return obj;}
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
 public List<CountriesClass> ConvertToList(DataTable dt)
{
 List<CountriesClass> Countrieslist = new List<CountriesClass>();
for(int i = 0; i<dt.Rows.Count; i++)
{
CountriesClass obj_Countries = new CountriesClass();

		 if (Convert.ToString(dt.Rows[i]["Countryid"]) != "")
			obj_Countries.Countryid = Convert.ToInt32(dt.Rows[i]["Countryid"]);

		 if (Convert.ToString(dt.Rows[i]["Countryname"]) != "")
			obj_Countries.Countryname = Convert.ToString(dt.Rows[i]["Countryname"]);

		 if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
			obj_Countries.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

		 if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
			obj_Countries.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

		 if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
			obj_Countries.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

		 if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
			obj_Countries.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);


Countrieslist.Add(obj_Countries);
}
return Countrieslist;
}

//Convert DataTable To object method
 public CountriesClass ConvertToOjbect(DataTable dt)
{
 CountriesClass obj_Countries = new CountriesClass();
for(int i = 0; i<dt.Rows.Count; i++)
{

		 if (Convert.ToString(dt.Rows[i]["Countryid"]) != "")
			obj_Countries.Countryid = Convert.ToInt32(dt.Rows[i]["Countryid"]);

		 if (Convert.ToString(dt.Rows[i]["Countryname"]) != "")
			obj_Countries.Countryname = Convert.ToString(dt.Rows[i]["Countryname"]);

		 if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
			obj_Countries.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

		 if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
			obj_Countries.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

		 if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
			obj_Countries.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

		 if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
			obj_Countries.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);
}
return obj_Countries;
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
