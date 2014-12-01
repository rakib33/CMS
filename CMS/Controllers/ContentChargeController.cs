using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CMS.App_Code;
using CMS.Models;




namespace CMS.Controllers
{
    public class ContentChargeController : Controller
    {
       
        private DBHandler db = new DBHandler();
        private Variables variable = new Variables();

        [HttpPost]
        public ActionResult pdfCharge(int id = 0, string IsDevice = null)
        {
            
            variable.InitResult = id;
            return Json(new { variable.InitResult }, JsonRequestBehavior.AllowGet);
        }
            
        
        
    
    }
}
