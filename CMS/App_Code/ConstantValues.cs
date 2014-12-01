
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.App_Code
{
    public class ConstantValues
    {
        #region Status

        //subscribe unsubscribe new user status
        public static string STATUS_SUBSCRIBE_NAME = "Subscribe";
        public static int STATUS_SUBSCRIBE_ID = 1;

        public static string STATUS_UN_SUBSCRIBE_NAME = "UnSubscribe";
        public static int STATUS_UN_SUBSCRIBE_ID = 2;

        public static string STATUS_SUBSCRIBE_INIT_NAME = "Attempt to Subscribe";
        public static int STATUS_SUBSCRIBE_INIT_ID = 7;

      
        //common status
        public static string STATUS_NOT_ROBI_MSG = "You are not using ROBI network";
        public static int STATUS_NOT_ROBI_ID = 3;

        public static string STATUS_NEW_USER_MSG = "New User";
        public static int STATUS_NEW_USER_ID = 4;

        public static string STATUS_EXCEPTION_MSG = "An Exception occured";
        public static int STATUS_EXCEPTION_ID = 5;

        //rip apk allowled versionCode status
       //  public ActionResult DeviceLogin(double? versionName) in Account Controller
        public static string RIP_APK_VERSION_Name = "2.1.1";
            //2.1;
        //1.3

        public static int STATUS_APK_VERSION_NOT_ALLOWLED = 6;
        public static string STATUS_APK_VERSION_NOT_ALLOWLED_MSG = "This Version is not allowled to use rip";




        public static string STATUS_DeductionBy_Fee_Name = "Deduction by fee"; 

        public static string STATUS_RefundByFee_Name = "Refund by fee";

        public static string STATUS_Renew_Name = "Renew";

        public static string STATUS_Resume_Name = "Resume";


        //login status
        #region Login Status
       
        public static string LOGIN_STATUS_SUCCESS_MSG = "Login Suucces";
        public static int LOGIN_STATUS_SUCCESS_ID = 1;

        public static string LOGIN_STATUS_FAILED_MSG = "Login Failed";
        public static int LOGIN_STATUS_FAILED_ID = 0;
        #endregion


        //Log off status
        #region Log Off Status
        public static string LOGOFF_STATUS_SUCCESS_MSG = "Log Off Succes";
        public static int LOGOFF_STATUS_SUCCESS_ID = 1;

        public static string LOGOFF_STATUS_FAILED_MSG = "Log Off Failed";
        public static int LOGOFF_STATUS_FAILED_ID = 0;

        #endregion

        //SOAP UnSubscribe request status result

        public static string STATUS_UNSUBSCRIBE_SUCCESS_MSG = "UnSubscribe Success";
        public static int STATUS_UNSUBSCRIBE_SUCCESS_ID = 1;

        public static string STATUS_UNSUBSCRIBE_FAILED_MSG = "UnSubscribe Failed";
        public static int STATUS_UNSUBSCRIBE_FAILED_ID = 2;


        public static int InitValue = 1;
        //SOAP status
        //request genrate status
        public static string SOAP_RESPONSE_XML_RESULT_TAG = "result";
        public static string SOAP_RESPONSE_XML_resultDescription_TAG="resultDescription";

        public static string SOAP_STATUS_WHILE_REQUEST = "Init";
        public static string SOAP_STATUS_AFTER_RESPONSE_SUCCESS = "Success";
        public static string SOAP_STATUS_AFTER_RESPONSE_FAILED = "Failed";       

        //api call Subscribe/UnSubscribe RequestType Message
        #region Subsribe_Unsubscribe_api_requestModel_value

          public static string Subscribe_ProductRequest="subscribeProductRequest";
          public static string Subscribe_ProductReq = "subscribeProductReq";

          public static string UnSubscribe_ProductRequest="unSubscribeProductRequest";
          public static string UnSubscribe_ProductReq = "unSubscribeProductReq";
          
           //common value
           public static string userType = "0";
           public static string productID = "0300396932";
           public static string isAutoExtend="0";

        #endregion

           #region SoapManagerReturnStatus

           public static string SOAP_RESPONSE_RESULT_UNSUCCESS_MSG = "Response result operation Unsuccess";
           public static int SOAP_RESPONSE_RESULT_UNSUCCESS_ID = 0;

           public static string SOAP_RESPONSE_RESULT_SUCCESS_MSG = "Response result operation Success";
           public static int SOAP_RESPONSE_RESULT_SUCCESS_ID = 1;


           public static string SOAP_RESPONSE_NOT_FOUND_MSG = "Response is empty ";
           public static int SOAP_RESPONSE_NOT_FOUND_ID = 2;

           #endregion

        #endregion


        public static string cause1 = "Not Useful";
        public static string cause2 = "Charge Is High";
        public static string cause3 = "Will Subscribe later";
        public static string cause4 = "Others";      


        public static string Device = "android";
        
        public static string ResultSuccess = "Success";
        public static string soap_description = "successful";
       
        public static int ResultID = 0;

        public static string ErrorFailed = "Failed";

        public static string ResponseError = "ResponseFailed";
        public static string HeaderError = "HeaderNotFound";
        public static string SubscribeException = "Exception";

        public static int SUB_DATE_RANGE = 30;


        #region Authorization_MSISDN_Log_File

        public static string workDir = "c:\\Log";
        public static string logFile = workDir + "\\log.txt";

        public static string AuthorizeLogFile = workDir + "\\StatusChangedException.txt";
        public static string ChangeStatusToUnSubscribe = "Change Status from Subscribe 1 to  UnSubscribe 2.";

        #endregion
    }
}