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
	public class ContactUsDataClass
    {
		#region "properties"
			[DisplayFormat(ConvertEmptyStringToNull = false)]
public Int32?  Contactusdataid {get;set;}
[DisplayFormat(ConvertEmptyStringToNull = false)]
public string  Name {get;set;}
[DisplayFormat(ConvertEmptyStringToNull = false)]
public string  Email {get;set;}
[DisplayFormat(ConvertEmptyStringToNull = false)]
public string  Messagesubject {get;set;}
[DisplayFormat(ConvertEmptyStringToNull = false)]
public string  Detailedmessage {get;set;}
 [DisplayFormat(ConvertEmptyStringToNull = false, DataFormatString = "{0:MM/dd/yyyy}")] 
			 public Nullable<DateTime>  Addeddatetime {get;set;}
[DisplayFormat(ConvertEmptyStringToNull = false)]
public string  Addedip {get;set;}


	 public ContactUsDataClass(){
	 }
		#endregion
    }

	public class ContactUsDataCtl :  IDisposable
	{
		#region "constructors"
		
			ConnectionCls obj_con = null;
//Default Constructor
public ContactUsDataCtl()
{
	 obj_con = new ConnectionCls();
}

//Select Constructor
public ContactUsDataCtl(Int32? id)
{
obj_con = new ConnectionCls();
ContactUsDataClass  obj_Con= new ContactUsDataClass();
using (DataTable dt = selectdatatable(id))
{
if (dt.Rows.Count > 0)
{

obj_Con.Contactusdataid = Convert.ToInt32(dt.Rows[0]["Contactusdataid"]);
obj_Con.Name = Convert.ToString(dt.Rows[0]["Name"]);
obj_Con.Email = Convert.ToString(dt.Rows[0]["Email"]);
obj_Con.Messagesubject = Convert.ToString(dt.Rows[0]["Messagesubject"]);
obj_Con.Detailedmessage = Convert.ToString(dt.Rows[0]["Detailedmessage"]);
obj_Con.Addeddatetime = Convert.ToDateTime(dt.Rows[0]["Addeddatetime"]);
obj_Con.Addedip = Convert.ToString(dt.Rows[0]["Addedip"]);

}
}
}

			
		#endregion
		
		#region "methods"
		
			//insert data into database 
public Int32? insert(ContactUsDataClass obj)
{
try 
{
obj_con.clearParameter();
createParameter(obj, DBTrans.Insert);
obj_con.BeginTransaction();
obj_con.ExecuteNoneQuery("sp_ContactUsData_insert", CommandType.StoredProcedure);
obj_con.CommitTransaction();
return obj.Contactusdataid = Convert.ToInt32(obj_con.getValue("@Contactusdataid"));
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_ContactUsData_insert:"+ex.Message);
}
}

//update data into database 
public Int32? update(ContactUsDataClass obj)
{
try 
{
obj_con.clearParameter();
obj = updateObject(obj);
createParameter(obj, DBTrans.Update);
obj_con.BeginTransaction();
obj_con.ExecuteNoneQuery("sp_ContactUsData_update", CommandType.StoredProcedure);
obj_con.CommitTransaction();
return obj.Contactusdataid = Convert.ToInt32(obj_con.getValue("@Contactusdataid"));
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_ContactUsData_update:"+ex.Message);
}
}

//delete data from database 
public void delete(Int32? Contactusdataid)
{
try 
{
obj_con.clearParameter();
obj_con.BeginTransaction();
obj_con.addParameter("@Contactusdataid", Contactusdataid );
obj_con.ExecuteNoneQuery("sp_ContactUsData_delete", CommandType.StoredProcedure);
obj_con.CommitTransaction();
}
catch (Exception ex)
{
obj_con.RollbackTransaction();
throw new Exception("sp_ContactUsData_delete:"+ex.Message);
}
}

//select all data from database 
public List<ContactUsDataClass> getAll()
{
try 
{
obj_con.clearParameter();
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_ContactUsData_selectall", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToList(dt);
}
catch (Exception ex)
{
throw new Exception("sp_ContactUsData_selectall:"+ex.Message);
}
}

//select data from database as Paging
public List<ContactUsDataClass> selectPaging(Int64 firstPageIndex, Int64 pageSize)
{
	 try 
	 {
		 obj_con.clearParameter();
		 obj_con.addParameter("@pageFirstIndex", firstPageIndex );
		 obj_con.addParameter("@pageLastIndex", pageSize );
		 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_ContactUsData_selectPaging", CommandType.StoredProcedure));
		 obj_con.CommitTransaction();
		 obj_con.closeConnection();
		 return ConvertToList(dt);
	  }
	 catch (Exception ex)
	 {
		 throw new Exception("sp_ContactUsData_selectPaging:"+ex.Message);
	 }
}

	 public List<ContactUsDataClass> selectIndexPaging(Int64 PageSize, Int64 PageIndex, string Search){
		 try{
			 obj_con.clearParameter();
			 obj_con.addParameter("@PageSize", PageSize);
			 obj_con.addParameter("@PageIndex", PageIndex);
			 obj_con.addParameter("@Search", Search);
			 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_ContactUsData_selectIndexPaging", CommandType.StoredProcedure));
			 obj_con.CommitTransaction();
			 obj_con.closeConnection();
			 return ConvertToList(dt);
		 } catch (Exception ex){
			 throw new Exception("sp_ContactUsData_selectIndexPaging:"+ex.Message);
		 }
	 }
	 public Int32 selectIndexPagingCount(Int64 PageSize, Int64 PageIndex, string Search){
		 try{
			 obj_con.clearParameter();
			 obj_con.addParameter("@PageSize", PageSize);
			 obj_con.addParameter("@PageIndex", PageIndex);
			 obj_con.addParameter("@Search", Search);
			 DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_ContactUsData_selectIndexPaging", CommandType.StoredProcedure));
			 obj_con.CommitTransaction();
			 obj_con.closeConnection();
			 return Convert.ToInt32(dt.Rows[0][0]);
		 } catch (Exception ex){
			 throw new Exception("sp_ContactUsData_selectIndexPaging:"+ex.Message);
		 }
	 }
	 public List<ContactUsDataClass> selectIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search){
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
public List<ContactUsDataClass> selectlist(Int32? Contactusdataid)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Contactusdataid", Contactusdataid );
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_ContactUsData_select", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToList(dt);
}
catch (Exception ex)
{
throw new Exception("sp_ContactUsData_select:"+ex.Message);
}
}

//select data from database as Objject
public ContactUsDataClass selectById(Int32? Contactusdataid)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Contactusdataid", Contactusdataid );
DataTable dt = ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_ContactUsData_select", CommandType.StoredProcedure));
obj_con.CommitTransaction();
obj_con.closeConnection();
return ConvertToOjbect(dt);
}
catch (Exception ex)
{
throw new Exception("sp_ContactUsData_select:"+ex.Message);
}
}

//select data from database as datatable
public DataTable selectdatatable(Int32? Contactusdataid)
{
try 
{
obj_con.clearParameter();
obj_con.addParameter("@Contactusdataid", Contactusdataid );
return ConvertDatareadertoDataTable(obj_con.ExecuteReader("sp_ContactUsData_select", CommandType.StoredProcedure));
}
catch (Exception ex)
{
throw new Exception("sp_ContactUsData_select:"+ex.Message);
}
}

//create parameter 
public void createParameter(ContactUsDataClass  obj, DB_con.DBTrans trans)
{
try
{
	 obj_con.clearParameter();

		 if(Convert.ToString(obj.Name) != "")
			obj_con.addParameter("@Name", string.IsNullOrEmpty(Convert.ToString(obj.Name)) ? "" : obj.Name);
	 else 
obj_con.addParameter("@Name", DBNull.Value );

		 if(Convert.ToString(obj.Email) != "")
			obj_con.addParameter("@Email", string.IsNullOrEmpty(Convert.ToString(obj.Email)) ? "" : obj.Email);
	 else 
obj_con.addParameter("@Email", DBNull.Value );

		 if(Convert.ToString(obj.Messagesubject) != "")
			obj_con.addParameter("@Messagesubject", string.IsNullOrEmpty(Convert.ToString(obj.Messagesubject)) ? "" : obj.Messagesubject);
	 else 
obj_con.addParameter("@Messagesubject", DBNull.Value );

		 if(Convert.ToString(obj.Detailedmessage) != "")
			obj_con.addParameter("@Detailedmessage", string.IsNullOrEmpty(Convert.ToString(obj.Detailedmessage)) ? "" : obj.Detailedmessage);
	 else 
obj_con.addParameter("@Detailedmessage", DBNull.Value );

		 if(Convert.ToString(obj.Addeddatetime) != "")
			obj_con.addParameter("@Addeddatetime", string.IsNullOrEmpty(Convert.ToString(obj.Addeddatetime)) ? Convert.ToDateTime("1900-01-01") : obj.Addeddatetime);
	 else 
obj_con.addParameter("@Addeddatetime", DBNull.Value );

		 if(Convert.ToString(obj.Addedip) != "")
			obj_con.addParameter("@Addedip", string.IsNullOrEmpty(Convert.ToString(obj.Addedip)) ? "" : obj.Addedip);
	 else 
obj_con.addParameter("@Addedip", DBNull.Value );

		 if(Convert.ToString(obj.Contactusdataid) != "")
			obj_con.addParameter("@Contactusdataid", Convert.ToInt32(obj.Contactusdataid), trans);
	 else 
obj_con.addParameter("@Contactusdataid", DBNull.Value );
}
catch (Exception ex)
{
throw ex;
}
}

//update edited object 
public ContactUsDataClass updateObject(ContactUsDataClass  obj)
{
try
{

	 ContactUsDataClass oldObj = selectById(obj.Contactusdataid);
 if (obj.Name == null)
	 obj.Name = oldObj.Name; 

 if (obj.Email == null)
	 obj.Email = oldObj.Email; 

 if (obj.Messagesubject == null)
	 obj.Messagesubject = oldObj.Messagesubject; 

 if (obj.Detailedmessage == null)
	 obj.Detailedmessage = oldObj.Detailedmessage; 

 if (obj.Addeddatetime == null)
	 obj.Addeddatetime = oldObj.Addeddatetime; 

 if (obj.Addedip == null)
	 obj.Addedip = oldObj.Addedip; 

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
 public List<ContactUsDataClass> ConvertToList(DataTable dt)
{
 List<ContactUsDataClass> ContactUsDatalist = new List<ContactUsDataClass>();
for(int i = 0; i<dt.Rows.Count; i++)
{
ContactUsDataClass obj_ContactUsData = new ContactUsDataClass();

		 if (Convert.ToString(dt.Rows[i]["Contactusdataid"]) != "")
			obj_ContactUsData.Contactusdataid = Convert.ToInt32(dt.Rows[i]["Contactusdataid"]);

		 if (Convert.ToString(dt.Rows[i]["Name"]) != "")
			obj_ContactUsData.Name = Convert.ToString(dt.Rows[i]["Name"]);

		 if (Convert.ToString(dt.Rows[i]["Email"]) != "")
			obj_ContactUsData.Email = Convert.ToString(dt.Rows[i]["Email"]);

		 if (Convert.ToString(dt.Rows[i]["Messagesubject"]) != "")
			obj_ContactUsData.Messagesubject = Convert.ToString(dt.Rows[i]["Messagesubject"]);

		 if (Convert.ToString(dt.Rows[i]["Detailedmessage"]) != "")
			obj_ContactUsData.Detailedmessage = Convert.ToString(dt.Rows[i]["Detailedmessage"]);

		 if (Convert.ToString(dt.Rows[i]["Addeddatetime"]) != "")
			obj_ContactUsData.Addeddatetime = Convert.ToDateTime(dt.Rows[i]["Addeddatetime"]);

		 if (Convert.ToString(dt.Rows[i]["Addedip"]) != "")
			obj_ContactUsData.Addedip = Convert.ToString(dt.Rows[i]["Addedip"]);


ContactUsDatalist.Add(obj_ContactUsData);
}
return ContactUsDatalist;
}

//Convert DataTable To object method
 public ContactUsDataClass ConvertToOjbect(DataTable dt)
{
 ContactUsDataClass obj_ContactUsData = new ContactUsDataClass();
for(int i = 0; i<dt.Rows.Count; i++)
{

		 if (Convert.ToString(dt.Rows[i]["Contactusdataid"]) != "")
			obj_ContactUsData.Contactusdataid = Convert.ToInt32(dt.Rows[i]["Contactusdataid"]);

		 if (Convert.ToString(dt.Rows[i]["Name"]) != "")
			obj_ContactUsData.Name = Convert.ToString(dt.Rows[i]["Name"]);

		 if (Convert.ToString(dt.Rows[i]["Email"]) != "")
			obj_ContactUsData.Email = Convert.ToString(dt.Rows[i]["Email"]);

		 if (Convert.ToString(dt.Rows[i]["Messagesubject"]) != "")
			obj_ContactUsData.Messagesubject = Convert.ToString(dt.Rows[i]["Messagesubject"]);

		 if (Convert.ToString(dt.Rows[i]["Detailedmessage"]) != "")
			obj_ContactUsData.Detailedmessage = Convert.ToString(dt.Rows[i]["Detailedmessage"]);

		 if (Convert.ToString(dt.Rows[i]["Addeddatetime"]) != "")
			obj_ContactUsData.Addeddatetime = Convert.ToDateTime(dt.Rows[i]["Addeddatetime"]);

		 if (Convert.ToString(dt.Rows[i]["Addedip"]) != "")
			obj_ContactUsData.Addedip = Convert.ToString(dt.Rows[i]["Addedip"]);
}
return obj_ContactUsData;
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
