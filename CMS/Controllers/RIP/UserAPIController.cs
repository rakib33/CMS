using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using RazorEngine;

using CMS.Models;
using CMS.App_Code;
using System.Text;
using System.Xml;

namespace CMS.Controllers.RIP
{
    [Authorize]
    public class UserAPIController : ApiController
    {
        private DBHandler db = new DBHandler();
        //GET api/userapi

        //  [Route("api/UserAPI/RobiIslamicPath/{id}")]

       
        [Route("Get/ID/{id}/Page")]    ///api/UserAPI/Get/ID/1/Page”
        public HttpResponseMessage Get(int id = 0)
        {
            Links link = db.objLinks.Find(id);
            if (link == null)
            {
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            string viewpath = HttpContext.Current.Server.MapPath(link.URL);
            var templete = File.ReadAllText(viewpath);
            string parsedview = RazorEngine.Razor.Parse(templete);
            response.Content = new StringContent(parsedview);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
            HttpContext.Current.Session["api"] = "Called";
            return response;
        }

        
      
       [HttpGet]
       public HttpResponseMessage GetAllLatestLinks(int id = 0, int aid = 1)
        {
            IEnumerable<Links> menus = (from e in db.objLinks where (e.Parent == id && e.AID == aid) orderby e.SQID select e);

            return Request.CreateResponse(HttpStatusCode.OK, new { menus });
        }

        [HttpGet]
        public XmlDocument fs_curl()
        {
            // return new string[] { "value1", "value2" };
            StringBuilder sb = new StringBuilder();

            sb.Append("<include>");
            sb.Append("<user id=\"5555\">");

            sb.Append("<params>");
            sb.Append("<param name=\"password\" value=\"$${default_password}\"/>");
            sb.Append("<param name=\"vm-password\" value=\"5555\"/>");
            sb.Append("</params>");

            sb.Append("<variables>");
            sb.Append("<variable name=\"toll_allow\" value=\"domestic,international,local\"/>");
            sb.Append("<variable name=\"accountcode\" value=\"5555\"/>");
            sb.Append("<variable name=\"user_context\" value=\"default\"/>");
            sb.Append("<variable name=\"effective_caller_id_name\" value=\"Extension 5555\"/>");
            sb.Append("<variable name=\"effective_caller_id_number\" value=\"5555\"/>");
            sb.Append("<variable name=\"outbound_caller_id_name\" value=\"$${outbound_caller_name}\"/>");
            sb.Append("<variable name=\"outbound_caller_id_number\" value=\"$${outbound_caller_id}\"/>");
            sb.Append("<variable name=\"callgroup\" value=\"techsupport\"/>");
            sb.Append("</variables>");

            sb.Append("</user>");
            sb.Append("</include>");

            string response = sb.ToString();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(response);

            return xml;
        }
    }
}
