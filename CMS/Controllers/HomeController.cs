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
using System.Web.Routing;
using CMS.Controllers;
using RazorEngine.Templating;



namespace CMS.Controllers
{


   // [CustomAuthorize]
    public class HomeController : Controller
    {
        private DBHandler db = new DBHandler();
               
                     
        public ActionResult Settings()
        {

            return View();
        }

       // [CustomAuthorize]  
        public ActionResult Index(string IsDevice)       
        {

          // IEnumerable<Links> ist = (from e in db.objLinks where e.Parent == 0 orderby e.SQID select e);
            try
            {
                IEnumerable<Links> ist = db.objLinks.Where(x => x.Parent == 0).OrderBy(e => e.SQID).Select(e => e);
                
                if(IsDevice==ConstantValues.Device)
                    ViewBag.Header = ConstantValues.Device;
                
                return View(ist);
            }
            catch (Exception)
            {
                return Content("Loading Failed");
            }
            
        }

       // [CustomAuthorize]  
        public ActionResult MenuList(int? id, string name = null,string IsDevice=null)       
         {

             char[] delimiterChars = { '/', '?','='};

             string text = "/Home/Hajj?page_id=";
            
             string[] words = text.Split(delimiterChars);
             //words[0] = "";
             //words[1] = "Home";
             //words[2] = "Hajj";
             //words[3] = "page_id";

            
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                if (IsDevice == ConstantValues.Device)
                    ViewBag.Header = ConstantValues.Device;

                IEnumerable<Links> ist = (from e in db.objLinks where e.Parent == id orderby e.SQID select e);

                ViewBag.Name = name;

               
                return View(ist);
            }
            catch (Exception)
            {
                return Content("Loading Failed");
            }

        }


        // [CustomAuthorize]  
        public ActionResult Suras(int? page_id, string IsDevice = null)
        {
            if (page_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var sura = (from links in db.objLinks
                            where links.ID == page_id

                            join master in db.objContentm on links.ID equals master.LinksID
                            join details in db.objContentd on master.ID equals details.MID

                            join css in db.objCSS on details.CssID equals css.ID
                            orderby details.RowID

                            select new Content_m_d
                            {
                                ID = details.RowID,
                                Content = details.Content,
                                ClassName = css.ClassName,
                                Group_Id = details.GroupID,
                                aud_ved_img = details.AVIContent
                            }).ToList();

             
                ViewBag.Device = IsDevice;

              

                return View(sura);
            }
            catch (Exception mx)
            {
                var a = mx.Message;
                return Content("Loading Failed");
            }

        }


       // [CustomAuthorize]  
        public ActionResult PlayAudio(int id = 0)
        {
            if (id > 0)
            {
               
                var Avicontent = (from e in db.Avicontetds where e.ID == id select e).ToList();  
                if (Avicontent.Count != 0)
                {
                    byte[] bt = Avicontent[0].AVIContent;
                    return File(bt, "audio/mpeg", "callrecording.mp3");

                   
                }               
                    
            }

            return null;
        }

        
       // [CustomAuthorize]  
        public ActionResult Dua(int? page_id, string IsDevice = null)
        {
            if (page_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var dua = (from links in db.objLinks
                           where links.ID == page_id
                           join master in db.objContentm on links.ID equals master.LinksID
                           join details in db.objContentd on master.ID equals details.MID
                           join css in db.objCSS on details.CssID equals css.ID
                           orderby details.RowID
                           select new Content_m_d
                           {
                               ID = details.RowID,
                               Content = details.Content,
                               ClassName = css.ClassName,
                               Group_Id = details.GroupID,
                               SuraName = links.Name,                            // master.SecondName
                               aud_ved_img = details.AVIContent
                           }).ToList();

                //ViewBag.Header =dua[0].SuraName;
                ViewBag.Device = IsDevice;
                
                //if row selected
                  //if (dua.Count > 0)
                  //    ViewBag.aud_ved_img = dua[1].aud_ved_img;

                return View(dua);
            }
            catch (Exception)
            {
                return Content("Loading Failed");
            }
        }

       // [CustomAuthorize]  
        public ActionResult EBook(int? page_id, string IsDevice = null)
        {

            if (page_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var pdf = (from links in db.objLinks
                            where links.ID == page_id
                            join master in db.objContentm on links.ID equals master.LinksID
                            join avicontentd in db.Avicontetds on master.ID equals avicontentd.MID
                            select avicontentd).ToList();

                

                if(pdf.Count>0)
                   ViewBag.ID = pdf[0].ID;

                if (IsDevice == ConstantValues.Device)
                {
                    ViewBag.Device = IsDevice;
                    return View("B_EBook");
                }
                else
                return View();
            }
            catch (Exception)
            {
                return Content("Loading Failed");
            }

        }


       //  [CustomAuthorize]  
        public ActionResult howto(int? page_id, string IsDevice = null)
        {
            if (page_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var howto = (from links in db.objLinks
                            where links.ID == page_id
                            join master in db.objContentm on links.ID equals master.LinksID
                            join details in db.objContentd on master.ID equals details.MID
                            join css in db.objCSS on details.CssID equals css.ID
                            orderby details.RowID
                            select new Content_m_d
                            {
                                ID = details.RowID,
                                Content = details.Content,
                                ClassName = css.ClassName,
                                Group_Id = details.GroupID,
                                // SuraName = links.Name,                            // master.SecondName
                                aud_ved_img = details.AVIContent
                            }).ToList();

             
                ViewBag.Device = IsDevice;

                //if row selected
                //if (howto.Count > 0)
                //    ViewBag.aud_ved_img = howto[1].aud_ved_img;

                return View(howto);
            }

            catch (Exception)
            {
                return Content("Loading Failed");
            }

        }

        // [CustomAuthorize]  
        public ActionResult Islam_O_Susastho(int? page_id, string IsDevice = null)
        {
            if (page_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var IslamOSusatho = (from links in db.objLinks
                            where links.ID == page_id
                            join master in db.objContentm on links.ID equals master.LinksID
                            join details in db.objContentd on master.ID equals details.MID
                            join css in db.objCSS on details.CssID equals css.ID
                            orderby details.RowID
                            select new Content_m_d
                            {
                                ID = details.RowID,
                                Content = details.Content,
                                ClassName = css.ClassName,
                                Group_Id = details.GroupID,
                                // SuraName = links.Name,                            // master.SecondName
                                aud_ved_img = details.AVIContent
                            }).ToList();

               
                ViewBag.Device = IsDevice;

                //if row selected
                //if (IslamOSusatho.Count > 0)
                //ViewBag.aud_ved_img = IslamOSusatho[1].aud_ved_img;

                return View(IslamOSusatho);
            }

            catch (Exception)
            {
                return Content("Loading Failed");
            }

            
        }


        // [CustomAuthorize]  
        public ActionResult Namaj(int? page_id, string IsDevice = null)
        {
            if (page_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var Namaj = (from links in db.objLinks
                                     where links.ID == page_id
                                     join master in db.objContentm on links.ID equals master.LinksID
                                     join details in db.objContentd on master.ID equals details.MID
                                     join css in db.objCSS on details.CssID equals css.ID
                                     orderby details.RowID
                                     select new Content_m_d
                                     {
                                         ID = details.RowID,
                                         Content = details.Content,
                                         ClassName = css.ClassName,
                                         Group_Id = details.GroupID,
                                         // SuraName = links.Name,                            // master.SecondName
                                         aud_ved_img = details.AVIContent
                                     }).ToList();

                ViewBag.Device = IsDevice;                           

                return View(Namaj);
            }

            catch (Exception)
            {
                return Content("Loading Failed");
            }
        }

        // [CustomAuthorize]  
        public ActionResult Romjan(int? page_id, string IsDevice = null)
        {
            if (page_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var Romjan = (from links in db.objLinks
                             where links.ID == page_id
                             join master in db.objContentm on links.ID equals master.LinksID
                             join details in db.objContentd on master.ID equals details.MID
                             join css in db.objCSS on details.CssID equals css.ID
                             orderby details.RowID
                             select new Content_m_d
                             {
                                 ID = details.RowID,
                                 Content = details.Content,
                                 ClassName = css.ClassName,
                                 Group_Id = details.GroupID,
                                 // SuraName = links.Name,                            // master.SecondName
                                 aud_ved_img = details.AVIContent
                             }).ToList();

                // ViewBag.Header =sura[0].SuraName;
                ViewBag.Device = IsDevice;
                
                //if row selected
                if(Romjan.Count>0)
                ViewBag.aud_ved_img = Romjan[1].aud_ved_img;

                return View(Romjan);
            }

            catch (Exception)
            {
                return Content("Loading Failed");
            }


        }


       //  [CustomAuthorize]  
        public ActionResult Hajj(int? page_id, string IsDevice = null)
        {
            if (page_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                var Hajj = (from links in db.objLinks
                              where links.ID == page_id
                              join master in db.objContentm on links.ID equals master.LinksID
                              join details in db.objContentd on master.ID equals details.MID
                              join css in db.objCSS on details.CssID equals css.ID
                              orderby details.RowID
                              select new Content_m_d
                              {
                                  ID = details.RowID,
                                  Content = details.Content,
                                  ClassName = css.ClassName,
                                  Group_Id = details.GroupID,
                                  // SuraName = links.Name,                            // master.SecondName
                                  aud_ved_img = details.AVIContent
                              }).ToList();

                // ViewBag.Header =sura[0].SuraName;
                ViewBag.Device = IsDevice;

                //if row selected
                if (Hajj.Count > 0)
                    ViewBag.aud_ved_img = Hajj[1].aud_ved_img;

                return View(Hajj);
            }

            catch (Exception)
            {
                return Content("Loading Failed");
            }


        }

      protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


      // [CustomAuthorize]  
       public ActionResult MyLibrary(string IsDevice)
        {
            List<AllBook> books = new List<AllBook>
            {

                new AllBook{Id=1, SQID=1, BookName="A", charge=5M, IsActive=true},
                new AllBook{Id=2, SQID=2, BookName="A", charge=5M, IsActive=true},
                new AllBook{Id=3, SQID=3, BookName="A", charge=5M, IsActive=true},
                new AllBook{Id=4, SQID=4, BookName="A", charge=5M, IsActive=false},

            };

            List<MyBook> MyBook = new List<MyBook>
            {
               new MyBook{Id=1, UserNo="01875469321", AllBookID=1,url="/Home/EBook/?page_id=", IsActive=true},
               new MyBook{Id=2, UserNo="01875469321", AllBookID=3,url="/Home/EBook/?page_id=", IsActive=true},
            
            };

            var UserNo = "01875469321";

            var AllBook = (from e in MyBook
                           where e.UserNo == "01875469321" && e.IsActive == true
                           join b in books on e.AllBookID equals b.Id
                           orderby b.SQID
                           select b).ToList();
                
                
                //(from e in books where e.IsActive == true
                //           join m in MyBook on e.Id equals m.AllBookID 
                //           orderby e.SQID select e).ToList();

            var myBook = (from m in MyBook
                         where m.UserNo == "01875469321" && m.IsActive == true
                         join b in AllBook on m.AllBookID equals b.Id
                         orderby b.SQID
                         select new BookViewModel
                         {
                              Id=b.SQID,
                              BookName=b.BookName,
                              Url=m.url
                         }).ToList();

            //var intersect = (from e in books where e.IsActive orderby e.SQID select e);

            MyLibraryList List = new MyLibraryList();
            
            List.AllBook = AllBook;
            List.MyBook = myBook;
            List.UserNo = UserNo;
        
            if (IsDevice == ConstantValues.Device)
            {
                return Json(List,JsonRequestBehavior.AllowGet);
            }

            return View();
        }

        
	}

    

 public class AllBook
    {
        public int Id { get; set; }
        public int SQID { get; set; }
        public string BookName { get; set; }
        public decimal charge { get; set; }

        public bool IsActive { get; set; }

    }

    public class MyBook
    {
        public int Id { get; set; }
        public string UserNo { get; set; }
        public int AllBookID { get; set; }

        public string url { get; set; }
        public bool IsActive { get; set; }

    }

    public class BookViewModel
    {
        public int Id { get; set; }
        public string BookName { get; set; }

        public string Url { get; set; }
    }

    public struct MyLibraryList 
    {
        public List<AllBook> AllBook { get; set; }
        public List<BookViewModel> MyBook { get; set; }

        public string UserNo { get; set; }
    }

}
