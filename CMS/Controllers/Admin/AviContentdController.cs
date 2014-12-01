using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;

using CMS.Models;

using CMS.App_Code;
using System.IO;



namespace CMS.Controllers.Admin
{
    public class AviContentdController : Controller
    {
        private DBHandler db = new DBHandler();

        private Variables variable = new Variables();   

        
        // GET: /AviContentd/
        public ActionResult Index()
        {
            var query = (from e in db.Avicontetds
                         orderby e.MID
                         select new AviContentView
                         {
                             ID = e.ID,
                             MID = e.MID,
                             ContentType = e.ContentType
                         }).ToList();

            return View(query);
            
            //return View(await db.Avicontetds.ToListAsync());
                                        
        }

        //public void Audio(int id=0)
        //{
        //    AviContentd content = db.Avicontetds.Find(id);
        //    byte[] bt = content.AVIContent;
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.ContentType = "audio/" + content.ContentType;
        //    Response.BinaryWrite(bt);
        //    Response.End();
          
            
        //}

       /// <summary>
            /// return audio file to play
            /// </summary>
       
        public ActionResult PlayAudio(int id = 0)
        {
            AviContentd content = db.Avicontetds.Find(id);
            if (content != null)
            {
                 byte[] bt = content.AVIContent;
                 return File(bt, "audio/mpeg", "callrecording.mp3");

               // return File(content.AVIContent, "audio/" + content.ContentType);
            }
            else
                return null;
            
        }

   

        // GET: /AviContentd/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AviContentd avicontentd = await db.Avicontetds.FindAsync(id);
            if (avicontentd == null)
            {
                return HttpNotFound();
            }
            return View(avicontentd);
        }

        // GET: /AviContentd/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /AviContentd/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Create(AviContentd avicontentd, HttpPostedFileBase file)
        {
            if (avicontentd.MID!=0 && file != null)
            {
                 
               // var filename = file.FileName;                    //Path.GetFileName(name);
                var ext = Path.GetExtension(file.FileName);           

                Stream fs = file.InputStream;
                BinaryReader br = new BinaryReader(fs);
                byte[] filedata = br.ReadBytes((Int32)fs.Length);

               // avicontentd.ContentName = file.FileName;
                avicontentd.AVIContent = filedata;
                avicontentd.ContentType = ext.ToString();
                db.Avicontetds.Add(avicontentd);
                try
                {
                   await db.SaveChangesAsync();                  
                  variable.InitResult=avicontentd.ID;
                  
                    //now update Contentd.AviContent=avicontentd.ID in RowID=2
                 // UpdateAviContentd(avicontentd.MID, avicontentd.ID);

                  return RedirectToAction("Index");
                }

                catch (Exception mx)
                {
                    return Content(mx.Message);
                }         
             
                             
            }

            return View(avicontentd);

        }


        public void UpdateAviContentd(int mid=0,int aviContent=0)
        {
            Contentd contentd = new Contentd();
            if (mid != 0 && aviContent != 0)
            {
               

                var AviContentdList = from e in db.objContentd where e.MID == mid && e.RowID == 2 select e; 
                    //db.objContentd.Where(p => p.MID == mid && p.RowID == 2).Select(p => p).ToList();

                foreach (var e in AviContentdList)
                {
                    contentd.ID = e.ID;

                    contentd.MID = e.MID;
                    contentd.RowID = e.RowID;
                    contentd.Content = e.Content;

                    //add aviContent for change
                    contentd.AVIContent = aviContent;

                    contentd.GroupID = e.GroupID;
                    contentd.CssID = e.CssID;
                  
                    db.Entry(contentd).State = EntityState.Modified;
                    db.SaveChanges();

                    break;
                }

             
            }
        }

        // GET: /AviContentd/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AviContentd avicontentd = await db.Avicontetds.FindAsync(id);
            if (avicontentd == null)
            {
                return HttpNotFound();
            }
            return View(avicontentd);
        }

        // POST: /AviContentd/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Edit(AviContentd avicontentd, HttpPostedFileBase file)
        {
            if (avicontentd.MID != 0 && file != null)
            {

                // var filename = file.FileName;                    //Path.GetFileName(name);
                var ext = Path.GetExtension(file.FileName);

                Stream fs = file.InputStream;
                BinaryReader br = new BinaryReader(fs);
                byte[] filedata = br.ReadBytes((Int32)fs.Length);


                avicontentd.AVIContent = filedata;
                avicontentd.ContentType = ext.ToString();
              
                try
                {
                    db.Entry(avicontentd).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (Exception mx)
                {
                    return Content(mx.Message);
                }


            }

            return View(avicontentd);

        }


        

        // GET: /AviContentd/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AviContentd avicontentd = await db.Avicontetds.FindAsync(id);
            if (avicontentd == null)
            {
                return HttpNotFound();
            }
            return View(avicontentd);
        }

        // POST: /AviContentd/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AviContentd avicontentd = await db.Avicontetds.FindAsync(id);
            db.Avicontetds.Remove(avicontentd);
            await db.SaveChangesAsync();
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
    }
}
