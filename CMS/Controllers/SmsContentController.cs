using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Controllers
{
    public class SmsContentController : Controller
    {
        //
        // GET: /SmsContent/
        public ActionResult SMSSystem()
        {
            List<SMS> sms = new List<SMS>
            {
                new SMS{id=1,header="I wish you the best of everything",body="My Blessing"},
                new SMS{id=2,header="you the best of everything",body="Blessing"},
            };
            
            return Json(new { sms},JsonRequestBehavior.AllowGet);
        }

       
	}
    public class SMS
    {
        public int id { get; set; }
        public string header { get; set; }

        public string body { get; set; }
    }
}