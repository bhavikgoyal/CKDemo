using System;
using System.Web.Mvc;
using ChossonKallah.Models;

namespace ChossonKallah.Controllers
{
    public class CountriesController : Controller
    	{
        	//private CountriesCtl db = new CountriesCtl();
            	//{privateVariables}

		

		public ActionResult Create()
		{
			 using(CountriesCtl db = new CountriesCtl()){
			 Session["CreatePreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
				 return View();
			}

		}
	

		 [HttpPost]
		[ValidateAntiForgeryToken]
		 public ActionResult Create(CountriesClass Obj_Countries, string command)
		{
			
			 using(CountriesCtl db = new CountriesCtl()){
			 if (ModelState.IsValid)
			{
					 db.insert(Obj_Countries);
					 if (command.ToLower().Trim() == "save"){
						 string sesionval = Convert.ToString(Session["CreatePreviousURL"]);
						 if (!string.IsNullOrEmpty(sesionval)){
							 Session.Remove("CreatePreviousURL");
							 return Redirect(sesionval); 
						 } else
							 return RedirectToAction("Index"); 
					 }else {
						 ModelState.Clear();
						 return View(); 
					 }
				 }
			
			 return View( Obj_Countries);
		}
	 }

		

		 public ActionResult Edit(Int32? Countryid)
		{
			
			 using(CountriesCtl db = new CountriesCtl()){
				 CountriesClass obj_Countries = db.selectById(Countryid);
				Session["EditPreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
					 return View(obj_Countries);
		}
		}

		

		 [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(CountriesClass Obj_Countries)
		{
			 using(CountriesCtl db = new CountriesCtl()){
			 if (ModelState.IsValid){
				 db.update(Obj_Countries);
				 string sesionval = Convert.ToString(Session["EditPreviousURL"]);
				 if (!string.IsNullOrEmpty(sesionval)){
					 Session.Remove("EditPreviousURL");
					 return Redirect(sesionval);
				 }else 
					 return RedirectToAction("Index"); 
			 }
		 return View( Obj_Countries);
		}
		}

		

		 public ActionResult Details(Int32? Countryid)
		{
			
			 using(CountriesCtl db = new CountriesCtl()){ CountriesClass obj_Countries = db.selectById(Countryid);
				 return View(obj_Countries);
		}
		}

		

		 public ActionResult Delete(Int32? Countryid) {
			 using(CountriesCtl db = new CountriesCtl()){
			 db.delete(Countryid);
			 return RedirectToAction("Index");
		 }
	}
		 

		public ActionResult Index()
		{
			 
			 return View();
		}

		

		 public ActionResult Indexpaging(Int64 PageSize, Int64 PageIndex, string Search){
			 
			 using(CountriesCtl db = new CountriesCtl()){return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search)); 
		}
		}
		public Int32 IndexpagingCount(Int64 PageSize, Int64 PageIndex, string Search){
			
			 using(CountriesCtl db = new CountriesCtl()){return db.selectIndexPagingCount(PageSize, PageIndex, Search); 
		}
		}
		 
	 public ActionResult IndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search) {
			 using(CountriesCtl db = new CountriesCtl()){
		 return PartialView( db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
	 }
		} 
		

		public ActionResult VIndex()
		{
			 
			 return View();
		}

		

		 public ActionResult VIndexpaging(Int64 PageSize, Int64 PageIndex, string Search){
			 
			 using(CountriesCtl db = new CountriesCtl()){return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search)); 
		}
		}
		public Int32 VIndexpagingCount(Int64 PageSize, Int64 PageIndex, string Search){
			
			 using(CountriesCtl db = new CountriesCtl()){return db.selectIndexPagingCount(PageSize, PageIndex, Search); 
		}
		}
		 
	 public ActionResult VIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search) {
			 using(CountriesCtl db = new CountriesCtl()){
		 return PartialView( db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
	 }
		} 
		

		 public ActionResult EditTableRowDelete(Int32? Countryid) {
			 using(CountriesCtl db = new CountriesCtl()){
			 db.delete(Countryid);
			 return RedirectToAction("EditTable");
		 }
			}
		 

		public ActionResult EditTable()
		{
			 
			 return View();
		}

		

		 public ActionResult EditTablePaging(Int64 PageSize, Int64 PageIndex, string Search){
			 
			 using(CountriesCtl db = new CountriesCtl()){return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search)); 
		}
		}
		public Int32 EditTablePagingCount(Int64 PageSize, Int64 PageIndex, string Search){
			
			 using(CountriesCtl db = new CountriesCtl()){return db.selectIndexPagingCount(PageSize, PageIndex, Search); 
		}
		}
		 
	 public ActionResult EditTableLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search) {
			 using(CountriesCtl db = new CountriesCtl()){
		 return PartialView( db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
	 }
		} 
		
	 [HttpPost]
	 public ActionResult SaveRecords(FormCollection model) {
		 if (ModelState.IsValid) {
			 using(CountriesCtl db = new CountriesCtl()){
			 var CountryidArray = model.GetValues("item.Countryid");
			 var CountrynameArray = model.GetValues("item.Countryname");
			 var IsactiveArray = model.GetValues("item.Isactive");
			 var IsdeletedArray = model.GetValues("item.Isdeleted");
			 var CreatedonArray = model.GetValues("item.Createdon");
			 var DeletedonArray = model.GetValues("item.Deletedon");
			 for (Int32 i = 0; i < CountryidArray.Length; i++ ) {
				 CountriesClass obj_update = db.selectById(Convert.ToInt32(CountryidArray[i]));
				 if (!string.IsNullOrEmpty(Convert.ToString(CountryidArray)))
					 obj_update.Countryid = Convert.ToInt32(CountryidArray[i]);
				 if (!string.IsNullOrEmpty(Convert.ToString(CountrynameArray)))
					 obj_update.Countryname = Convert.ToString(CountrynameArray[i]);
				 if (!string.IsNullOrEmpty(Convert.ToString(IsactiveArray)))
					 obj_update.Isactive = Convert.ToBoolean(IsactiveArray[i]);
				 if (!string.IsNullOrEmpty(Convert.ToString(IsdeletedArray)))
					 obj_update.Isdeleted = Convert.ToBoolean(IsdeletedArray[i]);
				 if (!string.IsNullOrEmpty(Convert.ToString(CreatedonArray)))
					 obj_update.Createdon = Convert.ToDateTime(CreatedonArray[i]);
				 if (!string.IsNullOrEmpty(Convert.ToString(DeletedonArray)))
					 obj_update.Deletedon = Convert.ToDateTime(DeletedonArray[i]);
				 db.update(obj_update);
			 } 
		 }
		}
		 return RedirectToAction("EditTable");
	 }
	 
	 public ActionResult EditTableRowsDelete(string records) {
			 using(CountriesCtl db = new CountriesCtl()){
		 foreach(string id in records.Trim(',').Split(',')  ){
			 if(!string.IsNullOrEmpty(id.Trim())){
				 db.delete(Convert.ToInt32(id));
			 }
		 }
		 return View();
		}
	 } 
		//{ActionResultMethod}
		

	         

	}

}
