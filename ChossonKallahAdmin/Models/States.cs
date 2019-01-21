using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using DB_con;

namespace ChossonKallah.Models
{
    public class StatesClass
    {
		#region "properties"
			[DisplayFormat(ConvertEmptyStringToNull = false)]
public Int32?  Stateid {get;set;}

[DisplayFormat(ConvertEmptyStringToNull = false)]
public string  Statename {get;set;}

[DisplayFormat(ConvertEmptyStringToNull = false)]
public Int32?  Countryid {get;set;}

[DisplayFormat(ConvertEmptyStringToNull = false)]
public bool  Isactive {get;set;}

[DisplayFormat(ConvertEmptyStringToNull = false)]
public bool  Isdeleted {get;set;}

 [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")] 
			 public Nullable<DateTime>  Createdon {get;set;}

 [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")] 
			 public Nullable<DateTime>  Deletedon {get;set;}



	 public StatesClass(){
	 }
		#endregion
    }

	public class StatesCtl :  IDisposable
	{
		#region "constructors"
		
			ConnectionCls obj_con = null;
//Default Constructor
public StatesCtl()
{
	 obj_con = new ConnectionCls();
}

//Select Constructor
public StatesCtl(Int32? id)
{
obj_con = new ConnectionCls();
StatesClass  obj_Sta= new StatesClass();
using (DataTable dt = selectdatatable(id))
{
if (dt.Rows.Count > 0)
{

obj_Sta.Stateid = Convert.ToInt32(dt.Rows[0]["Stateid"]);
obj_Sta.Statename = Convert.ToString(dt.Rows[0]["Statename"]);
obj_Sta.Countryid = Convert.ToInt32(dt.Rows[0]["Countryid"]);
obj_Sta.Isactive = Convert.ToBoolean(dt.Rows[0]["Isactive"]);
obj_Sta.Isdeleted = Convert.ToBoolean(dt.Rows[0]["Isdeleted"]);
obj_Sta.Createdon = Convert.ToDateTime(dt.Rows[0]["Createdon"]);
obj_Sta.Deletedon = Convert.ToDateTime(dt.Rows[0]["Deletedon"]);

}
}
}

			
		#endregion
		
		#region "methods"
		
			//insert data into database 
public Int32? insert(StatesClass obj)
{
try 
{
obj_con.clearParameter();
createParameter(obj, DBTrans.Insert);
obj_con.BeginTransaction();
obj_con.ExecuteNoneQuery("sp_States_insert", CommandType.StoredProcedure);
obj_con.CommitTransaction();
return obj.Stateid = Convert.ToInt32(obj_con.getValue("@Stateid"));
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_States_insert:"+ex.Message);
}
}

//update data into database 
public Int32? update(StatesClass obj)
{
try 
{
obj_con.clearParameter();
obj = updateObject(obj);
createParameter(obj, DBTrans.Update);
obj_con.BeginTransaction();
obj_con.ExecuteNoneQuery("sp_States_update", CommandType.StoredProcedure);
obj_con.CommitTransaction();
return obj.Stateid = Convert.ToInt32(obj_con.getValue("@Stateid"));
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_States_update:"+ex.Message);
}
}

//delete data from database 
public void delete(Int32? Stateid)
{
try 
{
obj_con.clearParameter();
obj_con.BeginTransaction();
obj_con.addParameter("@Stateid", Stateid );
obj_con.ExecuteNoneQuery("sp_States_delete", CommandType.StoredProcedure);
obj_con.CommitTransaction();
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_States_delete:"+ex.Message);
}
}

//select all data from database 
public List<StatesClass> getAll()
{
try 
{
obj_con.clearParameter();
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_States_selectall", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToList(dt);
}
catch (Exception ex)
{
throw new Exception("sp_States_selectall:"+ex.Message);
}
}

//select data from database as Paging
public List<StatesClass> selectPaging(Int64 firstPageIndex, Int64 pageSize)
{
	 try 
	 {
		 obj_con.clearParameter();
		 obj_con.addParameter("@pageFirstIndex", firstPageIndex );
		 obj_con.addParameter("@pageLastIndex", pageSize );
		 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_States_selectPaging", CommandType.StoredProcedure));
		 obj_con.CommitTransaction();
		 obj_con.closeConnection();
		 return ConvertToList(dt);
	  }
	 catch (Exception ex)
	 {
		 throw new Exception("sp_States_selectPaging:"+ex.Message);
	 }
}

	 public List<StatesClass> selectIndexPaging(Int64 PageSize, Int64 PageIndex, string Search){
		 try{
			 obj_con.clearParameter();
			 obj_con.addParameter("@PageSize", PageSize);
			 obj_con.addParameter("@PageIndex", PageIndex);
			 obj_con.addParameter("@Search", Search);
			 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_States_selectIndexPaging", CommandType.StoredProcedure));
			 obj_con.CommitTransaction();
			 obj_con.closeConnection();
			 return ConvertToList(dt);
		 } catch (Exception ex){
			 throw new Exception("sp_States_selectIndexPaging:"+ex.Message);
		 }
	 }
	 public Int32 selectIndexPagingCount(Int64 PageSize, Int64 PageIndex, string Search){
		 try{
			 obj_con.clearParameter();
			 obj_con.addParameter("@PageSize", PageSize);
			 obj_con.addParameter("@PageIndex", PageIndex);
			 obj_con.addParameter("@Search", Search);
			 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_States_selectIndexPaging", CommandType.StoredProcedure));
			 obj_con.CommitTransaction();
			 obj_con.closeConnection();
			 return Convert.ToInt32(dt.Rows[0][0]);
		 } catch (Exception ex){
			 throw new Exception("sp_States_selectIndexPaging:"+ex.Message);
		 }
	 }
	 public List<StatesClass> selectIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search){
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
public List<StatesClass> selectlist(Int32? Stateid)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Stateid", Stateid );
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_States_select", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToList(dt);
}
catch (Exception ex)
{
throw new Exception("sp_States_select:"+ex.Message);
}
}

//select data from database as Objject
public StatesClass selectById(Int32? Stateid)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Stateid", Stateid );
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_States_select", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToOjbect(dt);
}
catch (Exception ex)
{
throw new Exception("sp_States_select:"+ex.Message);
}
}

//select data from database as datatable
public DataTable selectdatatable(Int32? Stateid)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Stateid", Stateid );
return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_States_select", CommandType.StoredProcedure));
}
catch (Exception ex)
{
throw new Exception("sp_States_select:"+ex.Message);
}
}

//create parameter 
public void createParameter(StatesClass  obj, DB_con.DBTrans trans)
{
try
{
	 obj_con.clearParameter();

		 if(Convert.ToString(obj.Statename) != "")
			obj_con.addParameter("@Statename", string.IsNullOrEmpty(Convert.ToString(obj.Statename)) ? "" : obj.Statename);
	 else 
obj_con.addParameter("@Statename", DBNull.Value );

		 if(Convert.ToString(obj.Countryid) != "")
			obj_con.addParameter("@Countryid", string.IsNullOrEmpty(Convert.ToString(obj.Countryid)) ? 0 : obj.Countryid);
	 else 
obj_con.addParameter("@Countryid", DBNull.Value );

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

		 if(Convert.ToString(obj.Stateid) != "")
			obj_con.addParameter("@Stateid", Convert.ToInt32(obj.Stateid), trans);
	 else 
obj_con.addParameter("@Stateid", DBNull.Value );
}
catch (Exception ex)
{
throw ex;
}
}

//update edited object 
public StatesClass updateObject(StatesClass  obj)
{
try
{

	 StatesClass oldObj = selectById(obj.Stateid);
 if (obj.Statename == null)
	 obj.Statename = oldObj.Statename; 

 if (obj.Countryid == null)
	 obj.Countryid = oldObj.Countryid; 

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
 public List<StatesClass> ConvertToList(DataTable dt)
{
 List<StatesClass> Stateslist = new List<StatesClass>();
for(int i = 0; i<dt.Rows.Count; i++)
{
StatesClass obj_States = new StatesClass();

		 if (Convert.ToString(dt.Rows[i]["Stateid"]) != "")
			obj_States.Stateid = Convert.ToInt32(dt.Rows[i]["Stateid"]);

		 if (Convert.ToString(dt.Rows[i]["Statename"]) != "")
			obj_States.Statename = Convert.ToString(dt.Rows[i]["Statename"]);

		 if (Convert.ToString(dt.Rows[i]["Countryid"]) != "")
			obj_States.Countryid = Convert.ToInt32(dt.Rows[i]["Countryid"]);

		 if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
			obj_States.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

		 if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
			obj_States.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

		 if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
			obj_States.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

		 if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
			obj_States.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);


Stateslist.Add(obj_States);
}
return Stateslist;
}

//Convert DataTable To object method
 public StatesClass ConvertToOjbect(DataTable dt)
{
 StatesClass obj_States = new StatesClass();
for(int i = 0; i<dt.Rows.Count; i++)
{

		 if (Convert.ToString(dt.Rows[i]["Stateid"]) != "")
			obj_States.Stateid = Convert.ToInt32(dt.Rows[i]["Stateid"]);

		 if (Convert.ToString(dt.Rows[i]["Statename"]) != "")
			obj_States.Statename = Convert.ToString(dt.Rows[i]["Statename"]);

		 if (Convert.ToString(dt.Rows[i]["Countryid"]) != "")
			obj_States.Countryid = Convert.ToInt32(dt.Rows[i]["Countryid"]);

		 if (Convert.ToString(dt.Rows[i]["Isactive"]) != "")
			obj_States.Isactive = Convert.ToBoolean(dt.Rows[i]["Isactive"]);

		 if (Convert.ToString(dt.Rows[i]["Isdeleted"]) != "")
			obj_States.Isdeleted = Convert.ToBoolean(dt.Rows[i]["Isdeleted"]);

		 if (Convert.ToString(dt.Rows[i]["Createdon"]) != "")
			obj_States.Createdon = Convert.ToDateTime(dt.Rows[i]["Createdon"]);

		 if (Convert.ToString(dt.Rows[i]["Deletedon"]) != "")
			obj_States.Deletedon = Convert.ToDateTime(dt.Rows[i]["Deletedon"]);
}
return obj_States;
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
