using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMS.Models;
using CMS.App_Code;
using System.Reflection;

namespace CMS.Controllers.Admin
{
    public class LinksController : Controller
    {
        private DBHandler db = new DBHandler();

        [CustomAuthorize]
        // GET: /Links/
        public ActionResult Index()
        {
            return View(db.objLinks.ToList());
        }

        // GET: /Links/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Links links = db.objLinks.Find(id);
            if (links == null)
            {
                return HttpNotFound();
            }
            return View(links);
        }

        // GET: /Links/Create
        public ActionResult Create()
        {
            List<SelectListItem> links = new SelectList((from e in db.objLinks where e.IsParent == true orderby e.Name select e), "ID", "Name").ToList();
            links.Insert(0, (new SelectListItem { Text = "Top Parent", Value = "0" }));
            ViewData["Links"] = links;
            ViewBag.AID = new SelectList(db.objApplication, "ID", "Name");
            return View();
        }

        // POST: /Links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,AID,Name,Parent,URL,IMG_Path,IsParent,Frame,SQID,IsActive")] Links links)
        {
            if (ModelState.IsValid)
            {
                db.objLinks.Add(links);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(links);
        }

        // GET: /Links/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Links links = db.objLinks.Find(id);
            if (links == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> editList = new SelectList((from e in db.objLinks where e.IsParent == true orderby e.Name select e), "ID", "Name").ToList();
            editList.Insert(0, (new SelectListItem { Text = "Top Parent", Value = "0" }));
            ViewData["parent"] = editList;

            ViewBag.AID = new SelectList(db.objApplication, "ID", "Name");
           

            
            return View(links);
        }

        // POST: /Links/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,AID,Name,Parent,URL,IMG_Path,IsParent,Frame,SQID,IsActive")] Links links)
        {
            if (ModelState.IsValid)
            {
                db.Entry(links).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(links);
        }

        // GET: /Links/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Links links = db.objLinks.Find(id);
            if (links == null)
            {
                return HttpNotFound();
            }
            return View(links);
        }

        // POST: /Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Links links = db.objLinks.Find(id);
            db.objLinks.Remove(links);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //public ActionResult GetControllerNames()
        //{
        //    List<string> controllerNames = new List<string>();
        //    GetSubClasses<Controller>().ForEach(
        //         type => controllerNames.Add(type.Name));


        //    int count = controllerNames.Count;

        //    for (int i = 0; i < count; i++)
        //    {
        //        string abc = controllerNames[i].ToString();


        //        var thisType = GetType();
        //        Type t = thisType.Assembly.GetType(
        //            thisType.Namespace + "." + abc);
        //        var conName = t.FullName.ToString();

        //        get_all_action(conName);
        //    }
        //    return null;
        //}
        //public static List<Type> GetSubClasses<T>()
        //{

        //    return Assembly.GetCallingAssembly().GetTypes().Where(
        //        type => type.IsSubclassOf(typeof(T))).ToList();

        //}
        //public ActionResult get_all_action(string controllername)
        //{
        //    Type t = Type.GetType(controllername);
        //    MethodInfo[] mi = t.GetMethods(BindingFlags.Public);

        //    List<SelectListItem> action = new List<SelectListItem>();

        //    foreach (MethodInfo m in mi)
        //    {
        //        if (m.IsPublic)
        //            if (typeof(ActionResult).IsAssignableFrom(m.ReturnParameter.ParameterType))
        //            {
        //                action.Add(new SelectListItem() { Value = m.Name, Text = m.Name });
        //            }
        //    }

        //    var List = new SelectList(action, "Value", "Text");
        //    ViewBag.Action = List;

        //    return Json(List, JsonRequestBehavior.AllowGet);
            
        //    //return View();
        //}

    }
}
