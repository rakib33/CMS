using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.App_Code;

namespace CMS.Controllers
{
    public class OptionController : Controller
    {
        //
        // GET: /Option/
        public ActionResult About(string IsDevice)
        {
            if (IsDevice == ConstantValues.Device)
            {
                ViewBag.Device = IsDevice;
                return View("About");
            }
            return View();
        }

        public ActionResult Terms(string IsDevice)
        {
            if (IsDevice == ConstantValues.Device)
            {
                ViewBag.Device = IsDevice;
                return View("Terms");
            }
            return View();
        }

        public ActionResult Policy(string IsDevice)
        {
            if (IsDevice == ConstantValues.Device)
            {
                ViewBag.Device = IsDevice;
                return View("Policy");
            }
            return View();
        }

        public ActionResult Disclaimer(string IsDevice)
        {
            if (IsDevice == ConstantValues.Device)
            {
                ViewBag.Device = IsDevice;
                return View("Disclaimer");
            }
            return View();
        }

        public ActionResult FAQ(string IsDevice)
        {
            if (IsDevice == ConstantValues.Device)
            {
                ViewBag.Device = IsDevice;
                return View("FAQ");
            }
            return View();
        }

        public ActionResult Contact(string IsDevice)
        {
            if (IsDevice == ConstantValues.Device)
            {
                ViewBag.Device = IsDevice;
                return View("Contact");
            }
              return View();
        }
	}
}