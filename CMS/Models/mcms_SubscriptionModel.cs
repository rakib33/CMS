using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CMS.Models
{
    

    //public class mcms_soap_request
    //{
    //    [HiddenInput(DisplayValue = false)]
    //    [Key]
    //    public int Id { get; set; }
    //    public string requestType { get; set; }
    //    public string spId    {get;set;}
    //    public string spPassword {get;set;}
    //    public string timeStamp {get;set;}
    //    public string userID {get;set;}
    //    public string Type {get;set;}
    //    public string operCode{get;set;}
    //    public string productID{get;set;}
     
    //    public string isAutoExtend {get;set;}
    //    public string channelID {get;set;}
    //    public string extensionInfo {get;set;}
    //    public string requestLog {get;set;}
    //    public string responseLog {get;set;}
    //    public string result {get;set;}
    //    public string resultDescription {get;set;}
    //    public string msisdn { get; set; }
    
    //}

    //public class mcms_soap_response
    //{
    //      [Key,Column(Order=1)]
    //      [ForeignKey("mcms_soap_request")] 
    //      public int ResponseId { get; set; }
          
    //      //Navigation property of FK
    //      public virtual mcms_soap_request mcms_soap_request { get; set; }


    //      public string responseType {get;set;}
    //      public string result {get;set;}
    //      public string resultDescription {get;set;}
    //      public string Log { get; set; }
    //}
    public class mcmsSubscribeUser
    {
       // public int Id { get; set; }
        [Required]
        [StringLength(6)]
        public string PhoneCode { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "You must enter 11 digit")]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "Enter 11 digit only.")]
        public string UserPhoneNo { get; set; }
    }

    //public class RequestModel
    //{
    //    //public string  spId { get; set; }

    //    //public string spPassword { get; set; }

    //    public string serviceId { get; set; }

    //    public string timeStamp { get; set; }

    //    public string OA { get; set; }

    //    public string FA { get; set; }

    //    public string endUserIdentifier { get; set; }

    //    public string description { get; set; }

    //    public string currency { get; set; }

    //    public string amount { get; set; }

    //    public string code { get; set; }


    //}

    public class RequestModel
    {
        public string RequestType { get; set; }
        public string ProductRequest { get; set; }
        public string ProductReq { get; set; }
        public string userID { get; set; }
        public string userType { get; set; }
        public string productID { get; set; }
        public string isAutoExtend { get; set; }
    }
    //for store request response
    public class SOAP_Log
    {
        [Key]
        public int idn { get; set; }

        public string request_type { get; set; }

        //idn+request_type
        public string referance_code { get; set; }

        public string msisdn { get; set; }

        public string spid { get; set; }

        public string productid { get; set; }

        public string opercode { get; set; }

        public string isautoextend { get; set; }

        public string channelid { get; set; }

        public string timestamp { get; set; }

        public string request_xml { get; set; }

        public string response_xml { get; set; }

        public string result_code { get; set; }

        public string resulttime { get; set; }

        public string status { get; set; }

        public string ContentID { get; set; }


    }
}