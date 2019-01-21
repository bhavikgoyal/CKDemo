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
	public class GeneralSettingsClass
    {
		#region "properties"
			[DisplayFormat(ConvertEmptyStringToNull = false)]
public Int32?  Generalsettings {get;set;}
[DisplayFormat(ConvertEmptyStringToNull = false)]
public string  Key {get;set;}
[DisplayFormat(ConvertEmptyStringToNull = false)]
public string  Descrpition {get;set;}


	 public GeneralSettingsClass(){
	 }
		#endregion
    }

	public class GeneralSettingsCtl :  IDisposable
	{
		#region "constructors"
		
			ConnectionCls obj_con = null;
//Default Constructor
public GeneralSettingsCtl()
{
	 obj_con = new ConnectionCls();
}

//Select Constructor
public GeneralSettingsCtl(Int32? id)
{
obj_con = new ConnectionCls();
GeneralSettingsClass  obj_Gen= new GeneralSettingsClass();
using (DataTable dt = selectdatatable(id))
{
if (dt.Rows.Count > 0)
{

obj_Gen.Generalsettings = Convert.ToInt32(dt.Rows[0]["Generalsettings"]);
obj_Gen.Key = Convert.ToString(dt.Rows[0]["Key"]);
obj_Gen.Descrpition = Convert.ToString(dt.Rows[0]["Descrpition"]);

}
}
}

			
		#endregion
		
		#region "methods"
		
			//insert data into database 
public Int32? insert(GeneralSettingsClass obj)
{
try 
{
obj_con.clearParameter();
createParameter(obj, DBTrans.Insert);
obj_con.BeginTransaction();
obj_con.ExecuteNoneQuery("sp_GeneralSettings_insert", CommandType.StoredProcedure);
obj_con.CommitTransaction();
return obj.Generalsettings = Convert.ToInt32(obj_con.getValue("@Generalsettings"));
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_GeneralSettings_insert:"+ex.Message);
}
}

//update data into database 
public Int32? update(GeneralSettingsClass obj)
{
try 
{
obj_con.clearParameter();
obj = updateObject(obj);
createParameter(obj, DBTrans.Update);
obj_con.BeginTransaction();
obj_con.ExecuteNoneQuery("sp_GeneralSettings_update", CommandType.StoredProcedure);
obj_con.CommitTransaction();
return obj.Generalsettings = Convert.ToInt32(obj_con.getValue("@Generalsettings"));
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_GeneralSettings_update:"+ex.Message);
}
}

//delete data from database 
public void delete(Int32? Generalsettings)
{
try 
{
obj_con.clearParameter();
obj_con.BeginTransaction();
obj_con.addParameter("@Generalsettings", Generalsettings );
obj_con.ExecuteNoneQuery("sp_GeneralSettings_delete", CommandType.StoredProcedure);
obj_con.CommitTransaction();
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_GeneralSettings_delete:"+ex.Message);
}
}

//select all data from database 
public List<GeneralSettingsClass> getAll()
{
try 
{
obj_con.clearParameter();
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_GeneralSettings_selectall", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToList(dt);
}
catch (Exception ex)
{
throw new Exception("sp_GeneralSettings_selectall:"+ex.Message);
}
}

//select data from database as Paging
public List<GeneralSettingsClass> selectPaging(Int64 firstPageIndex, Int64 pageSize)
{
	 try 
	 {
		 obj_con.clearParameter();
		 obj_con.addParameter("@pageFirstIndex", firstPageIndex );
		 obj_con.addParameter("@pageLastIndex", pageSize );
		 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_GeneralSettings_selectPaging", CommandType.StoredProcedure));
		 obj_con.CommitTransaction();
		 obj_con.closeConnection();
		 return ConvertToList(dt);
	  }
	 catch (Exception ex)
	 {
		 throw new Exception("sp_GeneralSettings_selectPaging:"+ex.Message);
	 }
}

	 public List<GeneralSettingsClass> selectIndexPaging(Int64 PageSize, Int64 PageIndex, string Search){
		 try{
			 obj_con.clearParameter();
			 obj_con.addParameter("@PageSize", PageSize);
			 obj_con.addParameter("@PageIndex", PageIndex);
			 obj_con.addParameter("@Search", Search);
			 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_GeneralSettings_selectIndexPaging", CommandType.StoredProcedure));
			 obj_con.CommitTransaction();
			 obj_con.closeConnection();
			 return ConvertToList(dt);
		 } catch (Exception ex){
			 throw new Exception("sp_GeneralSettings_selectIndexPaging:"+ex.Message);
		 }
	 }
	 public Int32 selectIndexPagingCount(Int64 PageSize, Int64 PageIndex, string Search){
		 try{
			 obj_con.clearParameter();
			 obj_con.addParameter("@PageSize", PageSize);
			 obj_con.addParameter("@PageIndex", PageIndex);
			 obj_con.addParameter("@Search", Search);
			 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_GeneralSettings_selectIndexPaging", CommandType.StoredProcedure));
			 obj_con.CommitTransaction();
			 obj_con.closeConnection();
			 return Convert.ToInt32(dt.Rows[0][0]);
		 } catch (Exception ex){
			 throw new Exception("sp_GeneralSettings_selectIndexPaging:"+ex.Message);
		 }
	 }
	 public List<GeneralSettingsClass> selectIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search){
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
public List<GeneralSettingsClass> selectlist(Int32? Generalsettings)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Generalsettings", Generalsettings );
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_GeneralSettings_select", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToList(dt);
}
catch (Exception ex)
{
throw new Exception("sp_GeneralSettings_select:"+ex.Message);
}
}

//select data from database as Objject
public GeneralSettingsClass selectById(Int32? Generalsettings)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Generalsettings", Generalsettings );
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_GeneralSettings_select", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToOjbect(dt);
}
catch (Exception ex)
{
throw new Exception("sp_GeneralSettings_select:"+ex.Message);
}
}

//select data from database as datatable
public DataTable selectdatatable(Int32? Generalsettings)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Generalsettings", Generalsettings );
return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_GeneralSettings_select", CommandType.StoredProcedure));
}
catch (Exception ex)
{
throw new Exception("sp_GeneralSettings_select:"+ex.Message);
}
}

//create parameter 
public void createParameter(GeneralSettingsClass  obj, DB_con.DBTrans trans)
{
try
{
	 obj_con.clearParameter();

		 if(Convert.ToString(obj.Key) != "")
			obj_con.addParameter("@Key", string.IsNullOrEmpty(Convert.ToString(obj.Key)) ? "" : obj.Key);
	 else 
obj_con.addParameter("@Key", DBNull.Value );

		 if(Convert.ToString(obj.Descrpition) != "")
			obj_con.addParameter("@Descrpition", string.IsNullOrEmpty(Convert.ToString(obj.Descrpition)) ? "" : obj.Descrpition);
	 else 
obj_con.addParameter("@Descrpition", DBNull.Value );

		 if(Convert.ToString(obj.Generalsettings) != "")
			obj_con.addParameter("@Generalsettings", Convert.ToInt32(obj.Generalsettings), trans);
	 else 
obj_con.addParameter("@Generalsettings", DBNull.Value );
}
catch (Exception ex)
{
throw ex;
}
}

//update edited object 
public GeneralSettingsClass updateObject(GeneralSettingsClass  obj)
{
try
{

	 GeneralSettingsClass oldObj = selectById(obj.Generalsettings);
 if (obj.Key == null)
	 obj.Key = oldObj.Key; 

 if (obj.Descrpition == null)
	 obj.Descrpition = oldObj.Descrpition; 

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
 public List<GeneralSettingsClass> ConvertToList(DataTable dt)
{
 List<GeneralSettingsClass> GeneralSettingslist = new List<GeneralSettingsClass>();
for(int i = 0; i<dt.Rows.Count; i++)
{
GeneralSettingsClass obj_GeneralSettings = new GeneralSettingsClass();

		 if (Convert.ToString(dt.Rows[i]["Generalsettings"]) != "")
			obj_GeneralSettings.Generalsettings = Convert.ToInt32(dt.Rows[i]["Generalsettings"]);

		 if (Convert.ToString(dt.Rows[i]["Key"]) != "")
			obj_GeneralSettings.Key = Convert.ToString(dt.Rows[i]["Key"]);

		 if (Convert.ToString(dt.Rows[i]["Descrpition"]) != "")
			obj_GeneralSettings.Descrpition = Convert.ToString(dt.Rows[i]["Descrpition"]);


GeneralSettingslist.Add(obj_GeneralSettings);
}
return GeneralSettingslist;
}

//Convert DataTable To object method
 public GeneralSettingsClass ConvertToOjbect(DataTable dt)
{
 GeneralSettingsClass obj_GeneralSettings = new GeneralSettingsClass();
for(int i = 0; i<dt.Rows.Count; i++)
{

		 if (Convert.ToString(dt.Rows[i]["Generalsettings"]) != "")
			obj_GeneralSettings.Generalsettings = Convert.ToInt32(dt.Rows[i]["Generalsettings"]);

		 if (Convert.ToString(dt.Rows[i]["Key"]) != "")
			obj_GeneralSettings.Key = Convert.ToString(dt.Rows[i]["Key"]);

		 if (Convert.ToString(dt.Rows[i]["Descrpition"]) != "")
			obj_GeneralSettings.Descrpition = Convert.ToString(dt.Rows[i]["Descrpition"]);
}
return obj_GeneralSettings;
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
