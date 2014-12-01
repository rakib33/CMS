using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.Web.Mvc;

using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

using CMS.App_Code;
using CMS.Models;

namespace CMS.App_Code
{
    public class CheckMsisdnUserStatus
    {
        private DBHandler db = new DBHandler();
        private Variables variable = new Variables();

        //Get header from CustomAuthorizeAttribute
        public string GetMSISDNHeader(AuthorizationContext filterContext) //AuthorizationContext
        {
            string[] Header = filterContext.HttpContext.Request.Headers.AllKeys;

            for (int i = 0; i < Header.Length; i++)
            {
                if (Header[i].Contains("msisdn") ||
               Header[i].ToLower().StartsWith("msisdn", StringComparison.CurrentCultureIgnoreCase)
               || Header[i].ToLower().EndsWith("msisdn", StringComparison.CurrentCultureIgnoreCase)
                 || Header[i].ToLower().StartsWith("mdn", StringComparison.CurrentCultureIgnoreCase)
                 || Header[i].ToLower().EndsWith("mdn", StringComparison.CurrentCultureIgnoreCase))
                {

                    return filterContext.HttpContext.Request.Headers[Header[i]].ToString();

                }
            }

            return null;
        
        }

        //Get msisdn header while any Controller Action is  requested
        public string GetHeaderWhileActionRequested(HttpRequestBase Request) //AuthorizationContext
        {

            string[] Header = Request.Headers.AllKeys;
                //filterContext.HttpContext.Request.Headers.AllKeys;

            for (int i = 0; i < Header.Length; i++)
            {
                if (Header[i].Contains("msisdn") ||
               Header[i].ToLower().StartsWith("msisdn", StringComparison.CurrentCultureIgnoreCase)
               || Header[i].ToLower().EndsWith("msisdn", StringComparison.CurrentCultureIgnoreCase)
                 || Header[i].ToLower().StartsWith("mdn", StringComparison.CurrentCultureIgnoreCase)
                 || Header[i].ToLower().EndsWith("mdn", StringComparison.CurrentCultureIgnoreCase))
                {

                    return Request.Headers[Header[i]].ToString();

                }
            }

            return null;

        }

        public IsSubscribedWithRemainingDate checkUserStatus(string userNo)
        {
            //Check weather user is subcribed or new user.... 
            var resultmodel = db.objCheckSubcription.Where(t => t.UserName == userNo).Select(t => t).ToList();

            IsSubscribedWithRemainingDate isSubscribedWithRemainingDate = new IsSubscribedWithRemainingDate();

            //for new user or unsubscribed user assign false and o 
            isSubscribedWithRemainingDate.Renew = false;
            isSubscribedWithRemainingDate.ExpiredRemainingDate = 0;


            //if New user 
            if (resultmodel.Count == 0)
            {
                isSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_NEW_USER_ID; //4

            }

                 //if user exists
            else
            {

                // 1 mean subscribed 
                if (resultmodel[0].Status == ConstantValues.STATUS_SUBSCRIBE_ID)
                {
                    isSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_SUBSCRIBE_ID;//1

                    isSubscribedWithRemainingDate.ExpiredDate = Convert.ToString(resultmodel[0].ExpireDate.ToString("MM/dd/yyyy HH:mm:ss tt"));
                    // isSubscribedWithRemainingDate.ExpiredDate = resultmodel[0].ExpireDate;
                    //.ToString("MM/dd/yyyy HH:mm:ss tt")

                    //Compare Expiread DateTime with Current DateTime

                    variable.CheckResult = DateTime.Compare(DateTime.Now, resultmodel[0].ExpireDate);
                  
                    //variable.CheckResult<0 means DateTime.Now<resultmodel[0].ExpireDate
                    //variable.CheckResult==0 means DateTime.Now==resultmodel[0].ExpireDate
                    //variable.CheckResult>0 means DateTime.Now>resultmodel[0].ExpireDate

                    if (variable.CheckResult > 0)
                    {
                        //expiread date over so status is Unsubscribed
                        ChangeCurrentStatus(userNo);
                        isSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_UN_SUBSCRIBE_ID; //1
                    }
                    else
                    {
                        isSubscribedWithRemainingDate.ExpiredRemainingDate = Convert.ToInt32((resultmodel[0].ExpireDate - DateTime.Now).TotalDays);

                        //has some time but days 0 so we give day =1
                        if (isSubscribedWithRemainingDate.ExpiredRemainingDate == 0)
                            isSubscribedWithRemainingDate.ExpiredRemainingDate = 1;

                        //check if ExpiredRemainingDate is less 30 renew true else renew false
                        if (isSubscribedWithRemainingDate.ExpiredRemainingDate < ConstantValues.SUB_DATE_RANGE)
                            isSubscribedWithRemainingDate.Renew = true;
                    }

                }
                //unsubscribed
                else if (resultmodel[0].Status == ConstantValues.STATUS_UN_SUBSCRIBE_ID)
                {
                    isSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_UN_SUBSCRIBE_ID;  //2
                }

                //Exception
                else
                {
                    isSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_EXCEPTION_ID; //5

                }

            }


            return isSubscribedWithRemainingDate;

        }

        //change current status Subscribe to Unsubscribe
        public void ChangeCurrentStatus(string userNo)
        {
            try
            {
                var result = (from e in db.objCheckSubcription where e.UserName == userNo select e).ToList();

                if (result.Count == 1)
                {
                    result[0].Status = ConstantValues.STATUS_UN_SUBSCRIBE_ID;
                    db.SaveChanges();
                }

            }
            catch (Exception)
            {
                DirectoryManager dir = new DirectoryManager();
                dir.Directory_Manager(ConstantValues.workDir, ConstantValues.AuthorizeLogFile, ConstantValues.ChangeStatusToUnSubscribe, userNo);

            }
        }
           
    }
}