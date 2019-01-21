using System;
using System.Web.Mvc;
using ChossonKallah.Models;

namespace ChossonKallah.Controllers
{
    public class StatesController : Controller
    	{
        	//private StatesCtl db = new StatesCtl();
            	//{privateVariables}

		

		public ActionResult Create()
		{
			 using(StatesCtl db = new StatesCtl()){
			 Session["CreatePreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
				 return View();
			}

		}
	

		 [HttpPost]
		[ValidateAntiForgeryToken]
		 public ActionResult Create(StatesClass Obj_States, string command)
		{
			
			 using(StatesCtl db = new StatesCtl()){
			 if (ModelState.IsValid)
			{
					 db.insert(Obj_States);
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
			
			 return View( Obj_States);
		}
	 }

		

		 public ActionResult Edit(Int32? Stateid)
		{
			
			 using(StatesCtl db = new StatesCtl()){
				 StatesClass obj_States = db.selectById(Stateid);
				Session["EditPreviousURL"] = Convert.ToString(ControllerContext.HttpContext.Request.UrlReferrer);
					 return View(obj_States);
		}
		}

		

		 [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(StatesClass Obj_States)
		{
			 using(StatesCtl db = new StatesCtl()){
			 if (ModelState.IsValid){
				 db.update(Obj_States);
				 string sesionval = Convert.ToString(Session["EditPreviousURL"]);
				 if (!string.IsNullOrEmpty(sesionval)){
					 Session.Remove("EditPreviousURL");
					 return Redirect(sesionval);
				 }else 
					 return RedirectToAction("Index"); 
			 }
		 return View( Obj_States);
		}
		}

		

		 public ActionResult Details(Int32? Stateid)
		{
			
			 using(StatesCtl db = new StatesCtl()){ StatesClass obj_States = db.selectById(Stateid);
				 return View(obj_States);
		}
		}

		

		 public ActionResult Delete(Int32? Stateid) {
			 using(StatesCtl db = new StatesCtl()){
			 db.delete(Stateid);
			 return RedirectToAction("Index");
		 }
	}
		 

		public ActionResult Index()
		{
			 
			 return View();
		}

		

		 public ActionResult Indexpaging(Int64 PageSize, Int64 PageIndex, string Search){
			 
			 using(StatesCtl db = new StatesCtl()){return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search)); 
		}
		}
		public Int32 IndexpagingCount(Int64 PageSize, Int64 PageIndex, string Search){
			
			 using(StatesCtl db = new StatesCtl()){return db.selectIndexPagingCount(PageSize, PageIndex, Search); 
		}
		}
		 
	 public ActionResult IndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search) {
			 using(StatesCtl db = new StatesCtl()){
		 return PartialView( db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
	 }
		} 
		

		public ActionResult VIndex()
		{
			 
			 return View();
		}

		

		 public ActionResult VIndexpaging(Int64 PageSize, Int64 PageIndex, string Search){
			 
			 using(StatesCtl db = new StatesCtl()){return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search)); 
		}
		}
		public Int32 VIndexpagingCount(Int64 PageSize, Int64 PageIndex, string Search){
			
			 using(StatesCtl db = new StatesCtl()){return db.selectIndexPagingCount(PageSize, PageIndex, Search); 
		}
		}
		 
	 public ActionResult VIndexLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search) {
			 using(StatesCtl db = new StatesCtl()){
		 return PartialView( db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
	 }
		} 
		

		 public ActionResult EditTableRowDelete(Int32? Stateid) {
			 using(StatesCtl db = new StatesCtl()){
			 db.delete(Stateid);
			 return RedirectToAction("EditTable");
		 }
			}
		 

		public ActionResult EditTable()
		{
			 
			 return View();
		}

		

		 public ActionResult EditTablePaging(Int64 PageSize, Int64 PageIndex, string Search){
			 
			 using(StatesCtl db = new StatesCtl()){return PartialView(db.selectIndexPaging(PageSize, PageIndex, Search)); 
		}
		}
		public Int32 EditTablePagingCount(Int64 PageSize, Int64 PageIndex, string Search){
			
			 using(StatesCtl db = new StatesCtl()){return db.selectIndexPagingCount(PageSize, PageIndex, Search); 
		}
		}
		 
	 public ActionResult EditTableLazyLoading(Int64 StartIndex, Int64 EndIndex, string Search) {
			 using(StatesCtl db = new StatesCtl()){
		 return PartialView( db.selectIndexLazyLoading(StartIndex, EndIndex, Search));
	 }
		} 
		
	 [HttpPost]
	 public ActionResult SaveRecords(FormCollection model) {
		 if (ModelState.IsValid) {
			 using(StatesCtl db = new StatesCtl()){
			 var StateidArray = model.GetValues("item.Stateid");
			 var StatenameArray = model.GetValues("item.Statename");
			 var CountryidArray = model.GetValues("item.Countryid");
			 var IsactiveArray = model.GetValues("item.Isactive");
			 var IsdeletedArray = model.GetValues("item.Isdeleted");
			 var CreatedonArray = model.GetValues("item.Createdon");
			 var DeletedonArray = model.GetValues("item.Deletedon");
			 for (Int32 i = 0; i < StateidArray.Length; i++ ) {
				 StatesClass obj_update = db.selectById(Convert.ToInt32(StateidArray[i]));
				 if (!string.IsNullOrEmpty(Convert.ToString(StateidArray)))
					 obj_update.Stateid = Convert.ToInt32(StateidArray[i]);
				 if (!string.IsNullOrEmpty(Convert.ToString(StatenameArray)))
					 obj_update.Statename = Convert.ToString(StatenameArray[i]);
				 if (!string.IsNullOrEmpty(Convert.ToString(CountryidArray)))
					 obj_update.Countryid = Convert.ToInt32(CountryidArray[i]);
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
			 using(StatesCtl db = new StatesCtl()){
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
