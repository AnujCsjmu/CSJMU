using CoreLayout.Models.Common;
using CoreLayout.Models.WRN;
using CoreLayout.Services.Common.OTPVerification;
using CoreLayout.Services.Common.SequenceGenerate;
using CoreLayout.Services.WRN;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.WRN
{
    public class WRNRegistrationController : Controller
    {
        private readonly ILogger<WRNRegistrationController> _logger;
        private readonly IDataProtector _protector;
        public readonly IConfiguration _configuration;
        public readonly IWRNRegistrationService _wRNRegistrationService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly CommonController _commonController;
        private readonly ISequenceGenerateService _sequenceGenerateService;
        private readonly IOTPVerificationService _oTPVerificationService;
        public WRNRegistrationController(ILogger<WRNRegistrationController> logger, IDataProtectionProvider provider, IConfiguration configuration, IWRNRegistrationService wRNRegistrationService, IHttpContextAccessor httpContextAccessor, CommonController commonController, ISequenceGenerateService sequenceGenerateService, IOTPVerificationService oTPVerificationService)
        {
            _logger = logger;
            _protector = provider.CreateProtector("Circular.CircularController");
            _configuration = configuration;
            _wRNRegistrationService = wRNRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _commonController = commonController;
            _sequenceGenerateService = sequenceGenerateService;
            _oTPVerificationService = oTPVerificationService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(WRNRegistrationModel wRNRegistration)
        {
            try
            {
                if (wRNRegistration.RegistrationNo != null && wRNRegistration.MobileNo != null && wRNRegistration.DOB != null)
                {
                    var res = await _wRNRegistrationService.GetWRNRegistrationByLoginAsync(wRNRegistration.RegistrationNo, wRNRegistration.MobileNo, wRNRegistration.DOB);
                    if (res == null)
                    {
                        TempData["error"] = "Invalid details!";
                    }
                    else
                    {
                        HttpContext.Session.SetString("SessionFirstName", res.FirstName);
                        //HttpContext.Session.SetString("SessionMiddleName", res.MiddleName);
                        HttpContext.Session.SetString("SessionLastName", res.LastName);
                        HttpContext.Session.SetString("SessionMobileNo", res.MobileNo);
                        HttpContext.Session.SetString("SessionEmailId", res.EmailId);
                        HttpContext.Session.SetString("SessionRegistrationNo", res.RegistrationNo);
                        return RedirectToAction("Dashboard", "WRNDashboard");
                    }
                }
                else
                {
                    TempData["error"] = "Enter all fields!";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();
            }
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationAsync(WRNRegistrationModel wRNRegistration)
        {
            try
            {
                var data = await _oTPVerificationService.GetOTPVerificationByMobileAsync(wRNRegistration.MobileNo);
                if (data.OTP == wRNRegistration.EmailVerificationCode && data.IsVerified==true)
                {
                    if (wRNRegistration.FirstName != null && wRNRegistration.EmailId != null && wRNRegistration.MobileNo != null && wRNRegistration.DOB != null && wRNRegistration.FatherName != null && wRNRegistration.MotherName != null)
                    {
                        string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                        wRNRegistration.CreatedBy = 1;
                        wRNRegistration.IPAddress = ipAddress;
                        wRNRegistration.RegistrationNo = RegistrationNo();
                        if (wRNRegistration.RegistrationNo != "")
                        {
                            var res = await _wRNRegistrationService.CreateWRNRegistrationAsync(wRNRegistration);
                            if (res.Equals(1))
                            {
                                //send email code
                                bool sendemail = SendEmail(wRNRegistration);
                                TempData["success"] = "Data has been saved";
                                return RedirectToAction(nameof(Login));
                            }
                            else
                            {
                                TempData["error"] = "Data has not been saved";
                            }
                        }
                        else
                        {
                            TempData["error"] = "Registration No is not generate!";
                        }
                    }
                    else
                    {
                        TempData["error"] = "Some value is blank!";
                    }
                }
                else
                {
                    TempData["error"] = "Email OTP is not matched/verified!";
                }
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError("", ex.ToString());
                TempData["error"] = ex.ToString();
            }
            return View();
        }
        private bool IsEmailValid(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
        public bool IsMobileValid(string input)
        {
            bool result = false;
            try
            {
               
                if (Regex.IsMatch(input, "\\A[0-9]{10}\\z"))
                {
                    result = true;
                }
                else
                {
                    // Nope, no match, do your worst
                }
            }
            catch (ArgumentException ex)
            {
                // Syntax error in the regular expression
            }
            return result;
        }
        //[Route("api/AjaxAPI/SendOTPEmail")]
        [HttpPost]
        public async Task<IActionResult> SendOTPEmailAsync(WRNRegistrationModel wRNRegistrationModel)
        {

            bool result = false;
            string msg = string.Empty;
            bool res = false;
            string otp = string.Empty;
            int data = 0;
            var IsCompleteRegistration = await _wRNRegistrationService.GetWRNRegistrationByMobileAsync(wRNRegistrationModel.MobileNo);
            if (IsCompleteRegistration == null)
            {
                string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                if (wRNRegistrationModel.EmailId != null && wRNRegistrationModel.FirstName != null
                    && wRNRegistrationModel.MobileNo != null && wRNRegistrationModel.FatherName != null
                    && wRNRegistrationModel.MotherName != null)
                {
                    bool isEmailValid = IsEmailValid(wRNRegistrationModel.EmailId);
                    bool isMobileValid = IsMobileValid(wRNRegistrationModel.MobileNo);
                    if (isMobileValid == true && isEmailValid == true)
                    {
                        #region random 5 digit number
                        int _min = 000001;
                        int _max = 999999;
                        Random _rdm = new Random();
                        otp = _rdm.Next(_min, _max).ToString();
                        #endregion

                        #region insert otp into database
                        OTPVerificationModel oTPVerificationModel = new OTPVerificationModel();
                        oTPVerificationModel.EmailId = wRNRegistrationModel.EmailId;
                        oTPVerificationModel.MobileNo = wRNRegistrationModel.MobileNo;
                        oTPVerificationModel.OTP = otp;
                        oTPVerificationModel.IsVerified = false;
                        oTPVerificationModel.IPAddress = ipAddress;

                        //get data from mobile no
                        var isRecordExit = await _oTPVerificationService.GetOTPVerificationByMobileAsync(wRNRegistrationModel.MobileNo);
                        if (isRecordExit == null)
                        {
                            data = await _oTPVerificationService.CreateOTPVerificationAsync(oTPVerificationModel);
                        }
                        else
                        {
                            data = await _oTPVerificationService.UpdateOTPVerificationAsync(oTPVerificationModel);
                        }
                        #endregion
                        if (data.Equals(1))
                        {
                            #region email code
                            StringBuilder sbSMS = new StringBuilder();
                            sbSMS.Append("Dear, " + wRNRegistrationModel.FirstName);//+ wRNRegistration.FirstName
                            sbSMS.Append("<br>");
                            sbSMS.Append(otp);
                            sbSMS.Append(" is the OTP for verification. Please do not share with anyone.");
                            sbSMS.Append("<br>");
                            sbSMS.Append("<br>");
                            sbSMS.Append("CHHATRAPATI SHAHU JI MAHARAJ UNIVERSITY.");
                            result = _commonController.SendMail_Fromcsjmusms(wRNRegistrationModel.EmailId, "WRN Registraion", sbSMS.ToString());
                            #endregion
                            if (result == true)
                            {
                                msg = "Email sent sucessfully.";
                                res = true;
                            }
                            else
                            {
                                msg = "Email not sent."+false;
                            }
                        }
                        else
                        {
                            msg = "Some thing went wrong.";
                        }
                    }
                    else
                    {
                        msg = "Invalid mobile number/email id";
                    }
                }
                else
                {
                    msg = "Some fiels are blanks in current/previous tab";
                }
            }
            else
            {
                msg = "You are already register from this mobile no";
                //TempData["error"] = "You are already register from this mobile no";
            }
            // return msg;
            //return new JsonResult(msg, res);
            return Json( new { res,msg });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOTPEmail(WRNRegistrationModel wRNRegistrationModel)
        {
            string msg = string.Empty;
            string otp = string.Empty;
            bool res = false;
            string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await _oTPVerificationService.GetOTPVerificationByMobileAndOTPAsync(wRNRegistrationModel.MobileNo, wRNRegistrationModel.EmailVerificationCode);
            if (result != null)
            {
                if (wRNRegistrationModel.EmailVerificationCode != null && wRNRegistrationModel.MobileVerificationCode != null)
                {
                    #region insert otp into database
                    OTPVerificationModel oTPVerificationModel = new OTPVerificationModel();
                    oTPVerificationModel.EmailId = wRNRegistrationModel.EmailId;
                    oTPVerificationModel.MobileNo = wRNRegistrationModel.MobileNo;
                    oTPVerificationModel.OTP = otp;
                    oTPVerificationModel.IsVerified = true;
                    oTPVerificationModel.IPAddress = ipAddress;
                    int data = await _oTPVerificationService.UpdateOTPVerificationByMobileAsync(oTPVerificationModel);
                    #endregion
                    if (data.Equals(1))
                    {
                        msg = "Email is verified.";
                        res = true;
                    }
                    else
                    {
                        msg = "Email is not verified.";
                    }
                }
                else
                {
                    msg = "OTP is not found.";
                }
            }
            else
            {
                msg = "Invalid OTP.";
            }
            // return msg;
            //return new JsonResult(msg);
            return Json(new { res, msg });
        }
        public bool SendEmail(WRNRegistrationModel wRNRegistration)
        {
            #region email code
            bool result = false;
            StringBuilder sbSMS = new StringBuilder();
            sbSMS.Append("Dear, " + wRNRegistration.FirstName);
            sbSMS.Append("<br>");
            sbSMS.Append(wRNRegistration.RegistrationNo);
            sbSMS.Append(" is the WRN Registration Number. Please do not share with anyone.");
            sbSMS.Append("CHHATRAPATI SHAHU JI MAHARAJ UNIVERSITY.");
            result = _commonController.SendMail_Fromcsjmusms(wRNRegistration.EmailId, "WRN Registraion", sbSMS.ToString());
            return result;
            #endregion
        }
        private string RegistrationNo()
        {
            string Serialno = string.Empty;
            var data = _sequenceGenerateService.GetSequenceGenerateByIdAsync(8).Result;
            if (data != null)
            {
                int currentCountLength = data.CurrentCount.ToString().Length;
                int startIndexToReplace = data.SeqLength - currentCountLength - 1;
                string stringreplace = data.Sample.Remove(startIndexToReplace, currentCountLength);
                Serialno = (data.Sample.Remove(startIndexToReplace, currentCountLength)) + data.CurrentCount.ToString();
            }
            return Serialno;

        }

    }
}
