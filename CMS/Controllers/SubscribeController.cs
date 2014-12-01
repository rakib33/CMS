using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CMS.App_Code;
using CMS.Models;
using System.Text;
using System.Xml;

using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Xml.Linq;


namespace CMS.Controllers
{
    public class SubscribeController : Controller
    {

        //
        // GET: /Subscribe/


        // Global variable description
        private DBHandler db = new DBHandler();
        private Variables variable = new Variables();
        private SOAP_Log Item = new SOAP_Log();
        private RequestModel RequestModel = new RequestModel();
 
        private IsSubscribedWithRemainingDate _IsSubscribedWithRemainingDate = new IsSubscribedWithRemainingDate();

        private CheckMsisdnUserStatus objCheckMsisdnUserStatus = new CheckMsisdnUserStatus();

        // GET: /Subscribe/
        public ActionResult HeaderError(string IsDevice=null)
        {
            ViewBag.From = IsDevice;
            return View();

        }

        [HttpGet]
        public ActionResult Subscribe(int? Option)
        {
            ViewBag.Subscribe = Option;// 4= New User throw SubscribeUser Action, 2=RenewResume
            return View();           
        }

        public ActionResult DeviceSubscribe(int? Option)
        {
            if (Option == ConstantValues.STATUS_NEW_USER_ID)
            { 
               ViewBag.Message="You are not a Subscribed User.Please restart!!";
            }
            else if(Option==ConstantValues.STATUS_UN_SUBSCRIBE_ID)
            {
             ViewBag.Message="You do not have necessary rights to access this page.Please restart";
            }
            else
            {
                //exception
                ViewBag.Message = "Sorry!!Access Denied!!";
            
            }
           
            return View();        
        }

        //used from CustomAuth
        [AllowAnonymous]
        public ActionResult LoginFailed(string IsDevice=null)
        {
            ViewBag.From = IsDevice;
            return View();
        }

        //To create a new user 
        [HttpPost]
        public async Task<ActionResult> SubscribeUser(mcmsSubscribeUser mcmssubscribeuser, string IsDevice)
        {

            try
            {

                //Header Check              

              

                variable.UserPhoneNo = objCheckMsisdnUserStatus.GetHeaderWhileActionRequested(Request);
                 
                

                RegisterViewModel RModel = new Models.RegisterViewModel();


                //assign login status failed
                _IsSubscribedWithRemainingDate.LoginResult = ConstantValues.LOGIN_STATUS_FAILED_ID;

                //if header exists
                if (variable.UserPhoneNo != null && !variable.UserPhoneNo.Equals(""))
                {
                    //assign UserName/PhoneNo 
                    _IsSubscribedWithRemainingDate.UserPhoneNo = variable.UserPhoneNo;

                    //Check weather user is subcribed or new user.... 

                    var resultmodel = db.objCheckSubcription.Where(t => t.UserName == variable.UserPhoneNo).Select(t => t).ToList();

                    //User is not exists add User Info
                    if (resultmodel.Count == 0)
                    {

                        //for register a new user

                        RModel.UserName = variable.UserPhoneNo;
                        RModel.Password = variable.UserPhoneNo;
                        RModel.ConfirmPassword = variable.UserPhoneNo;


                        //get user id of new user
                        AccountController ObjAccount = new AccountController();
                        variable.UserId = ObjAccount.RegisterCalled(RModel);

                        // if User Create then add user info in mcms_SubcriptionCheck table
                        if (variable.UserId != null && !variable.UserId.Equals(""))
                        {

                            // SubscribeStatus = true;
                            mcms_SubcriptionCheck aCheck = new mcms_SubcriptionCheck();

                            aCheck.Id = variable.UserId;
                            aCheck.UserName = RModel.UserName;

                            aCheck.SubscribeDate = DateTime.Now;
                            aCheck.ExpireDate = DateTime.Now.AddDays(30);

                            variable.ExpiredDateTime = aCheck.ExpireDate;

                            aCheck.Status = ConstantValues.STATUS_SUBSCRIBE_INIT_ID;//attempt to Subscribe id= 7
                            db.objCheckSubcription.Add(aCheck);
                            db.SaveChanges();


                            //now call api fore genrate soap request                           

                            variable.CheckResult = await SoapManager(variable.UserPhoneNo, ConstantValues.STATUS_SUBSCRIBE_NAME);

                            //if soap response success 1

                            if (variable.CheckResult == ConstantValues.SOAP_RESPONSE_RESULT_SUCCESS_ID) //1
                            {
                                //now update  aCheck.Status from INIT_ID to ConstantValues.STATUS_SUBSCRIBE_ID

                                UpdateSubcriptionCheckStatus(ConstantValues.STATUS_SUBSCRIBE_ID, variable.UserPhoneNo);

                                //login status success
                                _IsSubscribedWithRemainingDate.LoginResult = ConstantValues.LOGIN_STATUS_SUCCESS_ID;
                                _IsSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_SUBSCRIBE_ID;         //To do update aCheck.Status=ConstantValues.STATUS_SUBSCRIBE_ID
                                _IsSubscribedWithRemainingDate.ExpiredRemainingDate = Convert.ToInt32((aCheck.ExpireDate - aCheck.SubscribeDate).TotalDays);


                                //check renew true or false
                                //check if ExpiredRemainingDate is less 30 renew true else renew false
                                if (_IsSubscribedWithRemainingDate.ExpiredRemainingDate < ConstantValues.SUB_DATE_RANGE)
                                    _IsSubscribedWithRemainingDate.Renew = true;
                                else
                                    _IsSubscribedWithRemainingDate.Renew = false;

                            }
                            else
                            {
                                _IsSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_UN_SUBSCRIBE_ID;
                                //  _IsSubscribedWithRemainingDate.ExpiredRemainingDate = Convert.ToInt32((aCheck.ExpireDate - aCheck.SubscribeDate).TotalDays - 1);

                            }


                        }
                        //new user but user is not create
                        else
                        {
                            //any reson user is not created
                            _IsSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_UN_SUBSCRIBE_ID;

                        }
                    }

                }
                //if user does not found
                else
                {
                    _IsSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_NOT_ROBI_ID; ;
                }
            }
            catch (Exception mx)
            {
                var a = mx.Message;
                _IsSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_EXCEPTION_ID;
            }

            // if request come from Device=android return json
            if (IsDevice == ConstantValues.Device)
                return Json(new { _IsSubscribedWithRemainingDate }, JsonRequestBehavior.AllowGet);
            else
            {
                if (_IsSubscribedWithRemainingDate.StatusID == ConstantValues.STATUS_SUBSCRIBE_ID)
                {
                    ViewBag.Date = variable.ExpiredDateTime;

                    if (_IsSubscribedWithRemainingDate.LoginResult == ConstantValues.LOGIN_STATUS_SUCCESS_ID)
                    {                        
                        return View("SubscribeSucess");
                    }
                    else
                    return View("LoginError");
                }
                else if (_IsSubscribedWithRemainingDate.StatusID == ConstantValues.STATUS_UN_SUBSCRIBE_ID)
                    return View("SubscribeUnSucess");
                else
                    return View("SubscribeUnSucess");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ReNewResume(String IsDevice)
        {
            try
            {
                

                //get MSISDN from header 
               
                variable.UserPhoneNo = objCheckMsisdnUserStatus.GetHeaderWhileActionRequested(Request);
                    
                
                //if header Exists

                if (variable.UserPhoneNo != null && !variable.UserPhoneNo.Equals(""))
                {
                    //check status from CheckSubcription table
                    var resultmodel = db.objCheckSubcription.Where(t => t.UserName == variable.UserPhoneNo).Select(t => t).ToList();

                    _IsSubscribedWithRemainingDate.UserPhoneNo = variable.UserPhoneNo;

                    //assign login failed
                    _IsSubscribedWithRemainingDate.LoginResult = ConstantValues.LOGIN_STATUS_FAILED_ID;

                    //if exists
                    if (resultmodel.Count == 1)
                    {
                   
                                       
                        //now call api fore genrate soap request and check if user is subscribed 
                        //this is renew, a user who is already logged in can renew so this user already logged in

                        if (resultmodel[0].Status == ConstantValues.STATUS_SUBSCRIBE_ID)
                            //already subscribed so Request Type=renew
                            variable.CheckResult = await SoapManager(variable.UserPhoneNo, ConstantValues.STATUS_Renew_Name);
                        else
                            //Unsubscribe status means user re start service again RequestType=Resume  and does not logged in
                            variable.CheckResult = await SoapManager(variable.UserPhoneNo, ConstantValues.STATUS_Resume_Name);

                        //end call api

                        //if Soap Response Success 1
                        if (variable.CheckResult == ConstantValues.SOAP_RESPONSE_RESULT_SUCCESS_ID)
                        {
                            //for renew user who alredy logged in Satus=1 means Subscribed then add one months with existings  
                            if (resultmodel[0].Status == ConstantValues.STATUS_SUBSCRIBE_ID)
                            {
                                                                
                                resultmodel[0].ExpireDate = resultmodel[0].ExpireDate.AddDays(30);

                                variable.ExpiredDateTime = resultmodel[0].ExpireDate;

                                _IsSubscribedWithRemainingDate.ExpiredRemainingDate = Convert.ToInt32((resultmodel[0].ExpireDate - DateTime.Now).TotalDays);  //+1


                                //check is user already Logged in or not
                                if (Request.IsAuthenticated)
                                {
                                    _IsSubscribedWithRemainingDate.LoginResult = ConstantValues.LOGIN_STATUS_SUCCESS_ID;
                                }
                                else
                                {
                                    //login again
                                    //assign login status failed
                                    variable.InitResult = ConstantValues.InitValue; //1
                                }
                            }

                            //For unsubscribed user who is not logged in, status=2 add one month from current date
                            else if (resultmodel[0].Status == ConstantValues.STATUS_UN_SUBSCRIBE_ID)
                            {
                                resultmodel[0].SubscribeDate = DateTime.Now;
                                resultmodel[0].ExpireDate = DateTime.Now.AddDays(30);

                                variable.ExpiredDateTime = resultmodel[0].ExpireDate;

                                //change subscribe status from unsubscibed=2 to Subscribed=1
                                resultmodel[0].Status = ConstantValues.STATUS_SUBSCRIBE_ID;

                                //check Expired date and reduced 1 day because DateTime.Now.AddMonths(1) add current day + 30=31

                                _IsSubscribedWithRemainingDate.ExpiredRemainingDate = Convert.ToInt32((resultmodel[0].ExpireDate - DateTime.Now).TotalDays);  //

                                //assign login status failed
                                variable.InitResult = ConstantValues.InitValue; //1
                            }

                            db.SaveChanges();

                            _IsSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_SUBSCRIBE_ID;


                            

                            //check if remaining days less 30 it access renew if greater renew false
                            if (_IsSubscribedWithRemainingDate.ExpiredRemainingDate < ConstantValues.SUB_DATE_RANGE)

                                _IsSubscribedWithRemainingDate.Renew = true;
                            else
                                _IsSubscribedWithRemainingDate.Renew = false;

                            //for above case Status is Subscribed and login status failed so we need to log in
                            //now login this user

                            if (variable.InitResult == ConstantValues.InitValue)
                            {
                                //assign login status failed then hit login method

                                LoginViewModel LogInModel = new LoginViewModel();
                                AccountController ObjAccount = new AccountController();

                               
                                LogInModel.UserName = resultmodel[0].UserName;  //user phone
                                LogInModel.Password = resultmodel[0].UserName;
                                LogInModel.RememberMe = false;

                                //login for this user and get jason data as string 
                                variable.result = ObjAccount.LoginFromInternal(LogInModel, ConstantValues.Device);
                                

                                if (variable.result == ConstantValues.LOGIN_STATUS_SUCCESS_MSG)
                                {
                                    _IsSubscribedWithRemainingDate.LoginResult = ConstantValues.LOGIN_STATUS_SUCCESS_ID;

                                }
                                //no need to assign failed status because it assign before


                            }

                            //end log in

                        }
                    }

                }
                else
                    //header not found 
                    _IsSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_NOT_ROBI_ID;
            }
            catch (Exception mx)
            {
                string a = mx.Message;
            }

            if (IsDevice == ConstantValues.Device)
            {
                return Json(new { _IsSubscribedWithRemainingDate }, JsonRequestBehavior.AllowGet);
            }
                          
           else
            {
                if (_IsSubscribedWithRemainingDate.StatusID == ConstantValues.STATUS_SUBSCRIBE_ID)
                {
                    ViewBag.Date = variable.ExpiredDateTime;

                    if (_IsSubscribedWithRemainingDate.LoginResult == ConstantValues.LOGIN_STATUS_SUCCESS_ID)
                    {
                        return View("SubscribeSucess");
                    }
                    else
                        return View("LoginError");
                }
                else if (_IsSubscribedWithRemainingDate.StatusID == ConstantValues.STATUS_UN_SUBSCRIBE_ID)
                    return View("SubscribeUnSucess");
                else
                    return View("SubscribeUnSucess");
            }
            
        }




        //[HttpGet]
        //public ActionResult UnSubsCribe()
        //{
        //    return View();
        //}


        [HttpGet]  // in global it is post [HttpPost]
            
        public async Task<ActionResult> UnSubscribe(mcmsSubscribeUser mcmssubscribeuser,string msg, string IsDevice) 
        {           

            //get MSISDN from header 
           
            variable.UserPhoneNo = objCheckMsisdnUserStatus.GetHeaderWhileActionRequested(Request);
            
            //if header Exists

            if (variable.UserPhoneNo != null && !variable.UserPhoneNo.Equals(""))        
               {            

                variable.CheckResult = await SoapManager(variable.UserPhoneNo,ConstantValues.STATUS_UN_SUBSCRIBE_NAME);

                //work only has a response success- 1 or failed-0  
                if (variable.CheckResult != ConstantValues.SOAP_RESPONSE_NOT_FOUND_ID)
                {

                    var resultmodel = db.objCheckSubcription.Where(t => t.UserName == variable.UserPhoneNo).ToList();

                    if (resultmodel.Count == 1)
                    {

                        if (msg == "1")
                        {
                            resultmodel[0].SelectUnsbStatus = ConstantValues.cause1;
                        }
                        else if (msg == "2")
                        {
                            resultmodel[0].SelectUnsbStatus = ConstantValues.cause2;
                        }
                        else if (msg == "3")
                        {
                            resultmodel[0].SelectUnsbStatus = ConstantValues.cause3;
                        }
                        else
                        {
                            resultmodel[0].SelectUnsbStatus = ConstantValues.cause4;
                        }

                        //check if response result is success then status will unsubscribe else status does not change

                        if (variable.CheckResult == ConstantValues.SOAP_RESPONSE_RESULT_SUCCESS_ID)
                        {
                            resultmodel[0].Status = ConstantValues.STATUS_UN_SUBSCRIBE_ID;

                            //set UnSubscribe Success Id 1 as return
                            variable.UnSubscribedResult = ConstantValues.STATUS_UNSUBSCRIBE_SUCCESS_ID; //1

                        }
                        else
                        {
                            //set UnSubscribe Success Id 1 as return
                            variable.UnSubscribedResult = ConstantValues.STATUS_UNSUBSCRIBE_FAILED_ID;  //2
                        }

                        db.SaveChanges();


                        //so check if user is Authenticate then Logged of
                        if (Request.IsAuthenticated)
                        {
                            AccountController objAccount = new AccountController();
                            variable.result = objAccount.DeviceLogOff();

                        }

                    }
                    //if User Does not in DB return UnSubscribe Failed
                    else
                    {
                        variable.UnSubscribedResult = ConstantValues.STATUS_UNSUBSCRIBE_FAILED_ID;  //2
                        //return RedirectToAction("UnSubscribeFailed");
                    }
                }
                    //Response not Found 2 Unsubscribe
                else
                {
                    variable.UnSubscribedResult = ConstantValues.STATUS_UNSUBSCRIBE_FAILED_ID;
                }
            }
                //if header does not Found return Unsubscribe-2 
            else
            {
                variable.UnSubscribedResult = ConstantValues.STATUS_UNSUBSCRIBE_FAILED_ID;

            }


            return Json(new { variable.UnSubscribedResult }, JsonRequestBehavior.AllowGet);
        }







        public ActionResult SubscribeSucess(string name)
        {
            var datePick = db.objCheckSubcription.First(t => t.UserName == name);
            ViewBag.Date = datePick.ExpireDate.ToString("dd/MM/yyyy"); ;
            return View();
        }
        public ActionResult SubscribeUnSucess()
        {
            return View();
        }
        public ActionResult UnSubscribeSucess()
        {
            return View();
        }
        public ActionResult UnSubscribeFailed()
        {
            return View();
        }


        #region Healper

        public void UpdateSubcriptionCheckStatus(int StatusID=0,string userID=null)
        {
            
            if (StatusID != 0 && userID != null)
            {
                try
                {
                    var result = db.objCheckSubcription.Where(p => p.UserName == userID).Select(p => p).ToList();

                    if (result.Count == 1)  //update the field
                    {

                        result[0].Status = StatusID;
                        db.SaveChanges();
                    }
                }

                catch(Exception)
                {
                
                }
            }
           
        }
            
              

        //check user status for DeviceLogin 
        //[NonAction]
        //public int IsSubscribed(string userNo)
        //{
        //    //Check weather user is subcribed or new user.... 
        //    mcms_SubcriptionCheck resultmodel = db.objCheckSubcription.First(t => t.UserName == userNo);

        //    if (resultmodel != null)
        //    {
        //        // 1 mean subscribed 
        //        if (resultmodel.Status == ConstantValues.STATUS_SUBSCRIBE_ID)
        //           return ConstantValues.STATUS_SUBSCRIBE_ID ;

        //        else
        //            //unsubscribed
        //           return ConstantValues.STATUS_UN_SUBSCRIBE_ID;

        //    }
        //    else
        //        //new user unsubscribe
        //        return ConstantValues.STATUS_UN_SUBSCRIBE_ID;

        //}


        
        public async Task<int> SoapManager(string UserNo,string RequestType)
        {
           

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://rahbar.arrivoltd.com/sm/");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //api/RequestResponse/SOAP_Request model genarate

                RequestModel.RequestType = RequestType;
               
                //UnSubscribe
                if (RequestType == ConstantValues.STATUS_UN_SUBSCRIBE_NAME)
                {
                    RequestModel.ProductRequest = ConstantValues.UnSubscribe_ProductRequest;
                    RequestModel.ProductReq = ConstantValues.UnSubscribe_ProductReq;
                }
                else
                {
                    RequestModel.ProductRequest = ConstantValues.Subscribe_ProductRequest;
                    RequestModel.ProductReq = ConstantValues.Subscribe_ProductReq;
                }

                RequestModel.userID = variable.UserPhoneNo;
                RequestModel.userType = ConstantValues.userType;
                RequestModel.productID = ConstantValues.productID;
                RequestModel.isAutoExtend = ConstantValues.isAutoExtend;                            
                
                HttpResponseMessage response = await client.PostAsJsonAsync("api/RequestResponse/SOAPRequest",RequestModel);
                             
                

                if (response.IsSuccessStatusCode)
                {
                   variable.response =await response.Content.ReadAsStringAsync();             
                    

                }
              

            }//end using 

            ////  save the RequstSOAP with MSISDN/UserPhoneNo and Status=init while requsting to server
            SOAP_Log SOAPItem = new SOAP_Log();

            //create new field only for new user Subscrbetion 
            if (RequestType == ConstantValues.STATUS_SUBSCRIBE_NAME)
            {
                SOAPItem.request_type = RequestType;
                SOAPItem.request_xml = "RequestXML";
                SOAPItem.msisdn = UserNo;
                SOAPItem.status = ConstantValues.SOAP_STATUS_WHILE_REQUEST;
                SOAPItem.response_xml = variable.response;

                db.SOAP_Log.Add(SOAPItem);
                db.SaveChanges();
            }

           

            //Check response
            if (variable.response != null && !variable.response.Equals(""))  //variable.response.Length!=0
            {
                               
                XmlDocument xml = new XmlDocument();
               
                //try{
                    //get response as XML after removing unexpected "\".....\"" from response string
                    xml = ResponseXML(variable.response);

                    
                        XmlNodeList element = xml.GetElementsByTagName(ConstantValues.SOAP_RESPONSE_XML_RESULT_TAG);

                       // XmlNodeList resultDescription = xml.GetElementsByTagName(ConstantValues.SOAP_RESPONSE_XML_resultDescription_TAG);

                        if (element.Count == 1)
                        {
                            variable.result = element[0].InnerText; //element[0].InnerXml;
                           // variable.resultDescription = resultDescription[0].InnerText;

                            //element[0].NextSibling.InnerText;

                            //select row for update 

                            var response = db.SOAP_Log.Where(x => x.msisdn == UserNo).Select(x => x).ToList();

                            ////save request type,Request, response
                              response[0].request_type = RequestType;
                              SOAPItem.request_xml = "RequestXML";
                              response[0].response_xml = variable.response;

                            //if response result success 00000=0

                              if (int.Parse(variable.result) == ConstantValues.ResultID)
                              {
                                  response[0].status = ConstantValues.SOAP_STATUS_AFTER_RESPONSE_SUCCESS;

                                  variable.result = ConstantValues.SOAP_STATUS_AFTER_RESPONSE_SUCCESS;
                              }

                              else
                                  response[0].status = variable.result;
                                    //ConstantValues.SOAP_STATUS_AFTER_RESPONSE_FAILED;

                            //for both case save result code
                            response[0].result_code = variable.result;

                            db.SaveChanges();

                            //if response success
                            if (variable.result == ConstantValues.SOAP_STATUS_AFTER_RESPONSE_SUCCESS)
                            {
                                return ConstantValues.SOAP_RESPONSE_RESULT_SUCCESS_ID;  //1
                            }
                            else
                                //operation UnSuccess
                                return ConstantValues.SOAP_RESPONSE_RESULT_UNSUCCESS_ID;//0
                        }
                   
                //}

                //catch (Exception mx)
                //{
                //    string a = mx.Message;
                //    return false;
                //}
            }

           //does not get a response
            return ConstantValues.SOAP_RESPONSE_NOT_FOUND_ID;//2
            
        }

        public XmlDocument ResponseXML(string response)
        {
            //variable.response = "\"<?xml version=\\\"1.0\\\" encoding=\\\"utf-8\\\" ?><soapenv:Envelope xmlns:soapenv=\\\"http://schemas.xmlsoap.org/soap/envelope/\\\" xmlns:xsi=\\\"http://www.w3.org/2001/XMLSchema-instance\\\"><soapenv:Body><ns1:subscribeProductResponse xmlns:ns1=\\\"http://www.csapi.org/schema/parlayx/subscribe/manage/v1_0/local\\\"><ns1:subscribeProductRsp><result>10001211</result><resultDescription>MSISDN is null.</resultDescription></ns1:subscribeProductRsp></ns1:subscribeProductResponse></soapenv:Body></soapenv:Envelope>\"";

            variable.response = response.Replace(@"\", @"");

            variable.response = variable.response.Remove(0, 1);

            variable.response = variable.response.Remove(variable.response.Length - 1);

           // return variable.response;
            XmlDocument doc = new XmlDocument();

            //try
            //{
                doc.LoadXml(variable.response);
            //}
            //catch (Exception)
            //{
            //    doc = null;
            //}

            return doc;
        }
        #endregion

       


        public ActionResult DateRetrive()
        {
            DateTime d = new DateTime();
            DateTime E = new DateTime();

           

            d = DateTime.Now;

            string sahd = d.ToString("MM/dd/yyyy HH:mm:ss tt");
            E = d.AddDays(30);
            

            var h = E.Year;
            var m = E.Month;
            var day = E.Day;

            
            var Edate = E - d;

            var dateAndTime = Edate.TotalDays;

         
            E = new DateTime(h,m,day,23,59,59);
            d = new DateTime(h,m,day,23,59,30);

            int f1 = DateTime.Compare(E, d);

            Edate = E - d;

            var hr = Edate.Hours;
            var mm = Edate.Minutes;
            var ss = Edate.Seconds;

            if (hr > 0||mm>0)
            { 
            
            }

            dateAndTime = Edate.TotalDays;

            return View();
        }


        //
    }


}