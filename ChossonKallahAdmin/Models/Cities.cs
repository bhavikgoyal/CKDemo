using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using DB_con;

namespace ChossonKallah.Models
{
    public class CitiesClass
    {
		#region "properties"
			[DisplayFormat(ConvertEmptyStringToNull = false)]
public Int32?  Cityid {get;set;}

[DisplayFormat(ConvertEmptyStringToNull = false)]
public string  Cityname {get;set;}

[DisplayFormat(ConvertEmptyStringToNull = false)]
public Int32?  Stateid {get;set;}

[DisplayFormat(ConvertEmptyStringToNull = false)]
public bool  Isactive {get;set;}

[DisplayFormat(ConvertEmptyStringToNull = false)]
public bool  Isdeleted {get;set;}

 [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")] 
			 public Nullable<DateTime>  Createdon {get;set;}

 [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")] 
			 public Nullable<DateTime>  Deletedon {get;set;}



	 public CitiesClass(){
	 }
		#endregion
    }

	public class CitiesCtl :  IDisposable
	{
		#region "constructors"
		
			ConnectionCls obj_con = null;
//Default Constructor
public CitiesCtl()
{
	 obj_con = new ConnectionCls();
}

//Select Constructor
public CitiesCtl(Int32? id)
{
obj_con = new ConnectionCls();
CitiesClass  obj_Cit= new CitiesClass();
using (DataTable dt = selectdatatable(id))
{
if (dt.Rows.Count > 0)
{

obj_Cit.Cityid = Convert.ToInt32(dt.Rows[0]["Cityid"]);
obj_Cit.Cityname = Convert.ToString(dt.Rows[0]["Cityname"]);
obj_Cit.Stateid = Convert.ToInt32(dt.Rows[0]["Stateid"]);
obj_Cit.Isactive = Convert.ToBoolean(dt.Rows[0]["Isactive"]);
obj_Cit.Isdeleted = Convert.ToBoolean(dt.Rows[0]["Isdeleted"]);
obj_Cit.Createdon = Convert.ToDateTime(dt.Rows[0]["Createdon"]);
obj_Cit.Deletedon = Convert.ToDateTime(dt.Rows[0]["Deletedon"]);

}
}
}

			
		#endregion
		
		#region "methods"
		
			//insert data into database 
public Int32? insert(CitiesClass obj)
{
try 
{
obj_con.clearParameter();
createParameter(obj, DBTrans.Insert);
obj_con.BeginTransaction();
obj_con.ExecuteNoneQuery("sp_Cities_insert", CommandType.StoredProcedure);
obj_con.CommitTransaction();
return obj.Cityid = Convert.ToInt32(obj_con.getValue("@Cityid"));
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_Cities_insert:"+ex.Message);
}
}

//update data into database 
public Int32? update(CitiesClass obj)
{
try 
{
obj_con.clearParameter();
obj = updateObject(obj);
createParameter(obj, DBTrans.Update);
obj_con.BeginTransaction();
obj_con.ExecuteNoneQuery("sp_Cities_update", CommandType.StoredProcedure);
obj_con.CommitTransaction();
return obj.Cityid = Convert.ToInt32(obj_con.getValue("@Cityid"));
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_Cities_update:"+ex.Message);
}
}

//delete data from database 
public void delete(Int32? Cityid)
{
try 
{
obj_con.clearParameter();
obj_con.BeginTransaction();
obj_con.addParameter("@Cityid", Cityid );
obj_con.ExecuteNoneQuery("sp_Cities_delete", CommandType.StoredProcedure);
obj_con.CommitTransaction();
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_Cities_delete:"+ex.Message);
}
}

//select all data from database 
public List<CitiesClass> getAll()
{
try 
{
obj_con.clearParameter();
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Cities_selectall", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToList(dt);
}
catch (Exception ex)
{
throw new Exception("sp_Cities_selectall:"+ex.Message);
}
}

//select data from database as Paging
public List<CitiesClass> selectPaging(Int64 firstPageIndex, Int64 pageSize)
{
	 try 
	 {
		 obj_con.clearParameter();
		 obj_con.addParameter("@pageFirstIndex", firstPageIndex );
		 obj_con.addParameter("@pageLastIndex", pageSize );
		 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Cities_selectPaging", CommandType.StoredProcedure));
		 obj_con.CommitTransaction();
		 obj_con.closeConnection();
		 return ConvertToList(dt);
	  }
	 catch (Exception ex)
	 {
		 throw new Exception("sp_Cities_selectPaging:"+ex.Message);
	 }
}

	 public List<CitiesClass> selectIndexPaging(Int64 PageSize, Int64 PageIndex, string Search){
		 try{
			 obj_con.clearParameter();
			 obj_con.addParameter("@PageSize", PageSize);
			 obj_con.addParameter("@PageIndex", PageIndex);
			 obj_con.addParameter("@Search", Search);
			 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Cities_selectIndexPaging", CommandType.StoredProcedure));
			 obj_con.CommitTransaction();
			 obj_con.closeConnection();
			 return ConvertToList(dt);
		 } catch (Exception ex){
			 throw new Exception("sp_Cities_selectIndexPaging:"+ex.Message);
		 }
	 }
	 public Int32 selectIndexPagingCount(Int64 PageSize, Int64 PageIndex, string Search){
		 try{
			 obj_con.clearParameter();
			 obj_con.addParameter("@PageSize", PageSize);
			 obj_con.addParameter("@PageIndex", PageIndex);
			 obj_con.addParameter("@Search", Search);
			 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Cities_selectIndexPaging", CommandType.StoredProcedure));
			 obj_con.CommitTransaction();
			 obj_con.closeConnection();
			 return Convert.ToInt32(dt.Rows[0][0]);
		 } catch (Exception ex){
			 throw new Exception("sp_Cities_selectIndexPaging:"+ex.Message);
		 }
	 }
	 public List<CitiesClass> selectIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search){
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
public List<CitiesClass> selectlist(Int32? Cityid)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Cityid", Cityid );
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Cities_select", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToList(dt);
}
catch (Exception ex)
{
throw new Exception("sp_Cities_select:"+ex.Message);
}
}

//select data from database as Objject
public CitiesClass selectById(Int32? Cityid)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Cityid", Cityid );
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Cities_select", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToOjbect(dt);
}
catch (Exception ex)
{
throw new Exception("sp_Cities_select:"+ex.Message);
}
}

//select data from database as datatable
public DataTable selectdatatable(Int32? Cityid)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Cityid", Cityid );
return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_Cities_select", CommandType.StoredProcedure));
}
catch (Exception ex)
{
throw new Exception("sp_Cities_select:"+ex.Message);
}
}

//create parameter 
public void createParameter(CitiesClass  obj, DB_con.DBTrans trans)
{
try
{
	 obj_con.clearParameter();

		 if(Convert.ToString(obj.Cityname) != "")
			obj_con.addParameter("@Cityname", string.IsNullOrEmpty(Convert.ToString(obj.Cityname)) ? "" : obj.Cityname);
	 else 
obj_con.addParameter("@Cityname", DBNull.Value );

		 if(Convert.ToString(obj.Stateid) != "")
			obj_con.addParameter("@Stateid", string.IsNullOrEmpty(Convert.ToString(obj.Stateid)) ? 0 : obj.Stateid);
	 else 
obj_con.addParameter("@Stateid", DBNull.Value );

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

		 if(Convert.ToString(obj.Cityid) != "")
			obj_con.addParameter("@Cityid", Convert.ToInt32(obj.Cityid), trans);
	 else 
obj_con.addParameter("@Cityid", DBNull.Value );
}
catch (Exception ex)
{
throw ex;
}
}

//update edited object 
public CitiesClass updateObject(CitiesClass  obj)
{
try
{

	 CitiesClass oldObj = selectById(obj.Cityid);
 if (obj.Cityname == null)
	 obj.Cityname = oldObj.Cityname; 

 if (obj.Stateid == null)
	 obj.Stateid = oldObj.Stateid; 

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
 public List<CitiesClass> ConvertToList(DataTable dt)
{
 List<CitiesClass> Citieslist = new List<CitiesClass>();
for(int i = 0; i<dt.Rows.Count; i++)
{
CitiesClass obj_Cities = new CitiesClass();

		 if (Convert.ToString(dt.Rows[i]["Cityid"]) != "")
			obj_Cities.Cityid = Convert.ToInt32(dt.Rows[i]["Cityid"]);

		 if (Convert.ToString(dt.Rows[i]["Cityname"]) != "")
			obj_Cities.Cityname = Convert.ToString(dt.Rows[i]["Cityname"]);

		 if (Convert.ToString(dt.Rows[i]["Stateid"]) != "")
			obj_Cities.Stateid = Convert.ToInt32(dt.Rows[i]["Stateid"]);

		 if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
			obj_Cities.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

		 if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
			obj_Cities.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

		 if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
			obj_Cities.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

		 if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
			obj_Cities.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);


Citieslist.Add(obj_Cities);
}
return Citieslist;
}

//Convert DataTable To object method
 public CitiesClass ConvertToOjbect(DataTable dt)
{
 CitiesClass obj_Cities = new CitiesClass();
for(int i = 0; i<dt.Rows.Count; i++)
{

		 if (Convert.ToString(dt.Rows[i]["Cityid"]) != "")
			obj_Cities.Cityid = Convert.ToInt32(dt.Rows[i]["Cityid"]);

		 if (Convert.ToString(dt.Rows[i]["Cityname"]) != "")
			obj_Cities.Cityname = Convert.ToString(dt.Rows[i]["Cityname"]);

		 if (Convert.ToString(dt.Rows[i]["Stateid"]) != "")
			obj_Cities.Stateid = Convert.ToInt32(dt.Rows[i]["Stateid"]);

		 if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
			obj_Cities.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

		 if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
			obj_Cities.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

		 if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
			obj_Cities.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

		 if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
			obj_Cities.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);
}
return obj_Cities;
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
