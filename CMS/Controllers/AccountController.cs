using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using CMS.App_Code;
using CMS.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.IO;

namespace CMS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private CheckMsisdnUserStatus objCheckMsisdnUserStatus = new CheckMsisdnUserStatus();
        private IsSubscribedWithRemainingDate IsSubscribedWithRemainingDate;
        private Variables variable = new Variables();

        public AccountController()
            : this(new UserManager<IdentityUser>(new UserStore<IdentityUser>(new MCMS_DBContext())))
        {
        }
         DBHandler db = new DBHandler();
                
        public AccountController(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<IdentityUser> UserManager { get; private set; }

        public RoleManager<IdentityRole> RoleManager { get; private set; }


        public AccountController(string a)
            : this(new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new MCMS_DBContext())))
        {
        }

        public AccountController(RoleManager<IdentityRole> roleManager)
        {
            RoleManager = roleManager;
        }


        protected override void HandleUnknownAction(string actionName)
        {
            ViewData["name"] = actionName;
            View("Error").ExecuteResult(this.ControllerContext);
        } 

        [NonAction]
        //this method is used from others controllers method when attempt to access login
        public string LoginFromInternal(LoginViewModel model,string IsDevice)
        {
            //For internal Controller login access first assign Failed status
            variable._loginResult = ConstantValues.LOGIN_STATUS_FAILED_ID;  

            if (ModelState.IsValid)
            {
                try
                {
                  Login(model, IsDevice);
                }
                
                catch(Exception)
                {
                   return ConstantValues.LOGIN_STATUS_FAILED_MSG;
                }
            }
            
            // login suucees then this value will ve changed in Login method see Login(LoginViewModel model, string IsDevice = null)
            //how the value variable._loginResult changed or not
            if (variable._loginResult == ConstantValues.LOGIN_STATUS_SUCCESS_ID)

                return ConstantValues.LOGIN_STATUS_SUCCESS_MSG;
            else
                return ConstantValues.LOGIN_STATUS_FAILED_MSG;
        }
     
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)  
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string IsDevice = null)
        {
            if (ModelState.IsValid)
            {

                //devicelogin
                var user = await UserManager.FindAsync(model.UserName, model.Password);

                if (user != null)
                {
                     
                    //internal Controller login access
                    variable._loginResult = ConstantValues.LOGIN_STATUS_SUCCESS_ID;                   
                                                     
                    await SignInAsync(user, model.RememberMe);

                    //if (IsDevice == ConstantValues.Device)
                    //    return Json(new {ConstantValues.LOGIN_STATUS_SUCCESS_MSG },JsonRequestBehavior.AllowGet);
                    //else
                        return RedirectToAction("Index", "Home");
                }

            }
            else
            {              
               
               ModelState.AddModelError("", "Invalid User Name or Password");
                
            }

            //if (IsDevice == ConstantValues.Device)
            //    return Json(new { ConstantValues.LOGIN_STATUS_FAILED_MSG}, JsonRequestBehavior.AllowGet);
            //else
            return View(model);
        }
               
       

       
        //this is login option from mobile device
        //this is Task<String> for return string if we return page from login method Task<ActionResult> used and return abc

        [AllowAnonymous]
        [HttpGet]
        public ActionResult DeviceLogin(string versionName) //Task<string> string username=null, string password=null double? versionName
        {
            //version check
            if (versionName != ConstantValues.RIP_APK_VERSION_Name)
            {
                IsSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_APK_VERSION_NOT_ALLOWLED;
            }
            else
            {
                //Header Check
              
                variable.UserPhoneNo = objCheckMsisdnUserStatus.GetHeaderWhileActionRequested(Request);
                  
               

                if (variable.UserPhoneNo != null && !variable.UserPhoneNo.Equals(""))
                {

                    //check is user subscribed or unsubscribed or new user              

                    IsSubscribedWithRemainingDate = objCheckMsisdnUserStatus.checkUserStatus(variable.UserPhoneNo);
                        //obj.IsSubscribedWithRemainingDate(variable.UserPhoneNo);

                    //then add UserPhoneNO to IsSubscribedWithRemainingDate object 
                    IsSubscribedWithRemainingDate.UserPhoneNo = variable.UserPhoneNo;

                    //assign login Status 0 means Failed
                    variable._loginResult = IsSubscribedWithRemainingDate.LoginResult = ConstantValues.LOGIN_STATUS_FAILED_ID;


                    //if user is a subescribed (1) user then check identityUser table

                    if (IsSubscribedWithRemainingDate.StatusID == ConstantValues.STATUS_SUBSCRIBE_ID)
                    {                                            
                                               

                        //add userPhone and Login Status ID "0" or "1"
                        //if already Authenticated
                        if (User.Identity.IsAuthenticated)
                        {
                            IsSubscribedWithRemainingDate.LoginResult = ConstantValues.LOGIN_STATUS_SUCCESS_ID;
                        }
                        else
                        {
                            IsSubscribedWithRemainingDate.LoginResult = DeviceAuthorizationLogin(variable.UserPhoneNo);
                        }
                        //return With Login Success 1 or Failed 0
                        return Json(new { IsSubscribedWithRemainingDate }, JsonRequestBehavior.AllowGet);
                    }

                    //if user is UnSubscribed 
                    else if (IsSubscribedWithRemainingDate.StatusID == ConstantValues.STATUS_UN_SUBSCRIBE_ID)
                    {
                        //add userPhone and  LOGIN_STATUS_FAILED_ID

                        return Json(new { IsSubscribedWithRemainingDate }, JsonRequestBehavior.AllowGet);
                    }

                    else
                    {
                        //if it new user add userPhone and  LOGIN_STATUS_FAILED_ID
                        return Json(new { IsSubscribedWithRemainingDate }, JsonRequestBehavior.AllowGet);
                    }
                }


                //if header is not found LOGIN_STATUS_NOT_ROBI_ID 2
                IsSubscribedWithRemainingDate.StatusID = ConstantValues.STATUS_NOT_ROBI_ID;
            }
             return Json(new { IsSubscribedWithRemainingDate}, JsonRequestBehavior.AllowGet);
        
            //
            
        }

        
        public int DeviceAuthorizationLogin(string UserNo)
        {
            variable._loginResult = 0;
            LoginViewModel modellogin = new LoginViewModel();
            modellogin.UserName = UserNo;
            modellogin.Password = UserNo;
            modellogin.RememberMe = false;

            //check user is it IdentityUser table
             Login(modellogin, ConstantValues.Device);

            return variable._loginResult;
        }
       
        // GET: /Account/Register
        [NonAction]
      //  [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [NonAction]
        //called from Subscrib/SubscribeUser
        public string RegisterCalled(RegisterViewModel model)
        {
            ViewBag.UserID = "";
            try
            {
                var a = Register(model, ConstantValues.Device);   //ConstantValues.Device=android
            }
            catch(Exception)
            {
                return ViewBag.UserID;
            }
            //return user Id to add in mcms_subcriptioncheck table
            return ViewBag.UserID;
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [NonAction]
       // [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string IsDevice)  //,string IsDevice
        {
          
            if (ModelState.IsValid)
            {
               
                var user = new IdentityUser() { UserName = model.UserName };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                     //if success add UserID                      
                     ViewBag.UserID = user.Id;
                  
                    //add a role for this user
                    var success = await UserManager.AddToRolesAsync(user.Id, "User");                        
                   
                         
                    //logined
                    await SignInAsync(user, isPersistent: false);                                                       
                  
                    return RedirectToAction("Index", "Home");
                }
            }

         
                return View(model);
        }
                  
       // [HttpPost]
        

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult LogOff(string IsDevice = null)
        {
            AuthenticationManager.SignOut();

            if (IsDevice == ConstantValues.Device)
            {
                ViewBag.DeviceLogOff = ConstantValues.LOGOFF_STATUS_SUCCESS_MSG;
            }

            return RedirectToAction("home", "Home");
        }

        [AllowAnonymous]
        public string DeviceLogOff()
        {
            ViewBag.DeviceLogOff = ConstantValues.LOGOFF_STATUS_FAILED_MSG;
            LogOff(ConstantValues.Device);

            return ViewBag.DeviceLogOff;
        }
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        //private async Task SignInAsync(IdentityUser user, bool isPersistent)
        private async Task SignInAsync(IdentityUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        
        //public async Task<ActionResult> SubscribeLogin(LoginViewModel model, string returnUrl, string Device = null)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //UserManager.IsLockedOut
        //        //devicelogin
        //        if (Device.Equals(ConstantValues.LoginInFromDevice))
        //        {
        //            var user = await UserManager.FindAsync(model.UserName, model.Password);
        //            if (user != null)
        //            {
        //                mcms_SubcriptionCheck csCheck = db.objCheckSubcription.First(t => t.UserName == model.UserName);
        //                if (csCheck.Status == ConstantValues.STATUS_SUBSCRIBE_ID && csCheck.ExpireDate > DateTime.Now)
        //                {
        //                    ViewBag.DeviceMessage = "Success";
        //                    await SignInAsync(user, model.RememberMe);
        //                    return "Su";
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", "Subcription Expired.");
        //                }
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Invalid username or password.");
        //            }
        //        }
        //        else
        //        {
        //            var user = await UserManager.FindAsync(model.UserName, model.Password);
        //            if (user != null)
        //            {
        //                ViewBag.DeviceMessage = "Success";
        //                await SignInAsync(user, model.RememberMe);
        //               // return RedirectToLocal(returnUrl);
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Invalid username or password.");
        //            }
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    //return View(model);
        //}

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {

            return View();
        }

        //[AllowAnonymous]
        //
        // POST: /Account/ForgotPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
       // public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(model.Email);
        //        if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return View("ForgotPasswordConfirmation");
        //        }

        //        var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //        await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
        //        ViewBag.Link = callbackUrl;
        //        return View("ForgotPasswordConfirmation");
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // GET: /Account/ForgotPasswordConfirmation
        //[AllowAnonymous]
        //public ActionResult ForgotPasswordConfirmation()
        //{
        //    return View();
        //}

        //
        // GET: /Account/ResetPassword
       // [AllowAnonymous]
        //public ActionResult ResetPassword(string code)
        //{
        //    return code == null ? View("Error") : View();
        //}

        //
        // POST: /Account/ResetPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await UserManager.FindByNameAsync(model.Email);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }
        //    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }
        //    AddErrors(result);
        //    return View();
        //}

        //
        // GET: /Account/ResetPasswordConfirmation
       // [AllowAnonymous]
        //public ActionResult ResetPasswordConfirmation()
        //{
        //    return View();
        //}
        // POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Request a redirect to the external login provider
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}

        //
        // GET: /Account/SendCode
       // [AllowAnonymous]
        //public async Task<ActionResult> SendCode(string returnUrl)
        //{
        //    var userId = await SignInManager.GetVerifiedUserIdAsync();
        //    if (userId == null)
        //    {
        //        return View("Error");
        //    }
        //    var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
        //    var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
        //    return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
        //}

        //
        // POST: /Account/SendCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SendCode(SendCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    // Generate the token and send it
        //    if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
        //    {
        //        return View("Error");
        //    }
        //    return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl });
        //}

        //
        // GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
        //        case SignInStatus.Failure:
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

        //
        // POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        //
        // POST: /Account/LogOff

     
        
      
    }
}
