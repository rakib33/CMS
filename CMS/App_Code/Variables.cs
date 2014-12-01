using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class Variables
    {
        #region subscription_Variable
        public string RequestLog { get; set; }

        public string response { get; set; }

        public string result { get; set; }

        public string resultDescription { get; set; }

        public string DateTime { get; set; }
        #endregion   

       
        public string UserPhoneNo { set; get; }

      //  public int IsSubscribed { set; get; }       
      
        public string UserId { get; set; }
        public int _loginResult { get; set; }

        public int CheckResult { get; set; }

        public int UnSubscribedResult { get; set; }

        public int InitResult { get; set; }
        public string HeaderAll { get; set; }

        #region DateTime

        public DateTime ExpiredDateTime { get; set; }
        public DateTime CurrentDateTime { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int days { get; set; }
        public int hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }

        //temporaray variable
       
        #endregion

        #region ControllerInfo

        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Parameter { get; set; }

        #endregion


    }

    public struct IsSubscribedWithRemainingDate
    {
        public int StatusID { get; set; }
        public int ExpiredRemainingDate { get; set; }

        public string ExpiredDate { get; set; }
       // public DateTime ExpiredDate { get; set; }
        public bool Renew { get; set; }
        public string UserPhoneNo { get; set; }
        public int LoginResult { get; set; }

    };

   
}