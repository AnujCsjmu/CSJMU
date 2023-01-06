﻿using CoreLayout.Models.Common;
using CoreLayout.Models.WRN;
using CoreLayout.Services.Common.OTPVerification;
using CoreLayout.Services.Common.SequenceGenerate;
using CoreLayout.Services.Masters.Category;
using CoreLayout.Services.Masters.District;
using CoreLayout.Services.Masters.Religion;
using CoreLayout.Services.Masters.State;
using CoreLayout.Services.WRN;
using CoreLayout.Services.WRN.WRNRegistration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ICategoryService _categoryService;
        private readonly IReligionService _religionService;
        private readonly IStateService _stateService;
        private readonly IDistrictService _districtService;
        public WRNRegistrationController(ILogger<WRNRegistrationController> logger,
            IDataProtectionProvider provider, IConfiguration configuration,
            IWRNRegistrationService wRNRegistrationService, IHttpContextAccessor httpContextAccessor,
            CommonController commonController, ISequenceGenerateService sequenceGenerateService,
            IOTPVerificationService oTPVerificationService, ICategoryService categoryService,
            IReligionService religionService, IStateService stateService, IDistrictService districtService)
        {
            _logger = logger;
            _protector = provider.CreateProtector("WRNRegistration.WRNRegistrationController");
            _configuration = configuration;
            _wRNRegistrationService = wRNRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _commonController = commonController;
            _sequenceGenerateService = sequenceGenerateService;
            _oTPVerificationService = oTPVerificationService;
            _categoryService = categoryService;
            _religionService = religionService;
            _stateService = stateService;
            _districtService = districtService;
        }

        #region Login
        public IActionResult Login()
        {
            return View("~/Views/WRN/WRNRegistration/Login.cshtml");
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
                        ModelState.AddModelError("", "Invalid details!");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("SessionId", res.Id);
                        HttpContext.Session.SetInt32("SessionCreatedBy", (int)res.CreatedBy);
                        HttpContext.Session.SetString("SessionFirstName", res.FirstName);
                        if (res.MiddleName != null)
                        {
                            HttpContext.Session.SetString("SessionMiddleName", res.MiddleName);
                        }
                        if (res.LastName != null)
                        {
                            HttpContext.Session.SetString("SessionLastName", res.LastName);
                        }
                        HttpContext.Session.SetString("SessionFatherName", res.FatherName);
                        HttpContext.Session.SetString("SessionMotherName", res.MotherName);
                        HttpContext.Session.SetString("SessionDOB", res.DOB);
                        HttpContext.Session.SetString("SessionMobileNo", res.MobileNo);
                        HttpContext.Session.SetString("SessionEmailId", res.EmailId);
                        HttpContext.Session.SetString("SessionRegistrationNo", res.RegistrationNo);
                        return View("~/Views/WRN/WRNDashboard/Dashboard.cshtml", wRNRegistration);
                    }
                }
                else
                {
                    TempData["error"] = "Enter all fields!";
                    ModelState.AddModelError("", "Enter all fields!");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/WRN/WRNRegistration/Login.cshtml");
        }
        #endregion

        #region Registration 
        [HttpGet]
        public IActionResult Registration()
        {
            return View("~/Views/WRN/WRNRegistration/Registration.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationAsync(WRNRegistrationModel wRNRegistration)
        {
            try
            {
                var data = await _oTPVerificationService.GetOTPVerificationByMobileAsync(wRNRegistration.MobileNo);
                if (data.OTP == wRNRegistration.EmailVerificationCode && data.IsVerified == true)
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
        #endregion


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
                            sbSMS.Append("Dear " + wRNRegistrationModel.FirstName + wRNRegistrationModel.MiddleName + wRNRegistrationModel.LastName + ",");//+ wRNRegistration.FirstName
                            sbSMS.Append("<br>");
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
                                msg = "Email not sent." + false;
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
            return Json(new { res, msg });
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
        public bool SendEmail(WRNRegistrationModel wRNRegistrationModel)
        {
            #region email code
            bool result = false;
            StringBuilder sbSMS = new StringBuilder();
            sbSMS.Append("Dear " + wRNRegistrationModel.FirstName + wRNRegistrationModel.MiddleName + wRNRegistrationModel.LastName + ",");//+ wRNRegistration.FirstName
            sbSMS.Append("<br>");
            sbSMS.Append("<br>");
            sbSMS.Append(wRNRegistrationModel.RegistrationNo);
            sbSMS.Append(" is the WRN Registration Number. Please do not share with anyone.");
            sbSMS.Append("<br>");
            sbSMS.Append("<br>");
            sbSMS.Append("CHHATRAPATI SHAHU JI MAHARAJ UNIVERSITY.");
            result = _commonController.SendMail_Fromcsjmusms(wRNRegistrationModel.EmailId, "WRN Registraion", sbSMS.ToString());
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

        #region Complete Registration
        [HttpGet]
        public async Task<IActionResult> CompleteRegistrationAsync()
        {
            if (HttpContext.Session.GetString("SessionRegistrationNo") != null)
            {
                #region bind data after update
                WRNRegistrationModel wRNRegistrationModel = new WRNRegistrationModel();
                wRNRegistrationModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
                wRNRegistrationModel.DOB = HttpContext.Session.GetString("SessionDOB");
                wRNRegistrationModel.MobileNo = HttpContext.Session.GetString("SessionMobileNo");
                var data = await _wRNRegistrationService.GetWRNRegistrationByLoginAsync(wRNRegistrationModel.RegistrationNo, wRNRegistrationModel.MobileNo, wRNRegistrationModel.DOB);
                if (data == null)
                {
                    TempData["success"] = "Data not found !";
                }
                else
                {
                    wRNRegistrationModel.FirstName = HttpContext.Session.GetString("SessionFirstName");
                    wRNRegistrationModel.MiddleName = HttpContext.Session.GetString("SessionMiddleName");
                    wRNRegistrationModel.LastName = HttpContext.Session.GetString("SessionLastName");
                    wRNRegistrationModel.FatherName = HttpContext.Session.GetString("SessionFatherName");
                    wRNRegistrationModel.MotherName = HttpContext.Session.GetString("SessionMotherName");
                    wRNRegistrationModel.EmailId = HttpContext.Session.GetString("SessionEmailId");
                    wRNRegistrationModel.CategoryList = await _categoryService.GetAllCategory();
                    wRNRegistrationModel.ReligionList = await _religionService.GetAllReligion();
                    wRNRegistrationModel.StateList = await _stateService.GetAllState();
                    wRNRegistrationModel.DistrictList = await _districtService.GetAllDistrict();
                    wRNRegistrationModel.AcademicSession = "2022-23";//change

                    wRNRegistrationModel.ApplicationNo = data.ApplicationNo;
                    wRNRegistrationModel.ModeOfAdmission = data.ModeOfAdmission;
                    wRNRegistrationModel.HindiName = data.HindiName;
                    wRNRegistrationModel.Gender = data.Gender;
                    wRNRegistrationModel.AadharNumber = data.AadharNumber;
                    wRNRegistrationModel.CategoryId = data.CategoryId;
                    wRNRegistrationModel.Nationality = data.Nationality;
                    wRNRegistrationModel.ReligionId = data.ReligionId;
                    wRNRegistrationModel.PhysicalDisabled = data.PhysicalDisabled;

                    wRNRegistrationModel.PermanentAddress = data.PermanentAddress;
                    wRNRegistrationModel.PermanentStateId = data.PermanentStateId;
                    wRNRegistrationModel.PermanentDistrictId = data.PermanentDistrictId;
                    wRNRegistrationModel.PermanentPincode = data.PermanentPincode;

                    wRNRegistrationModel.CommunicationAddress = data.CommunicationAddress;
                    wRNRegistrationModel.CommunicationStateId = data.CommunicationStateId;
                    wRNRegistrationModel.CommunicationDistrictId = data.CommunicationDistrictId;
                    wRNRegistrationModel.CommunicationPincode = data.CommunicationPincode;
                    wRNRegistrationModel.FinalSubmit = data.FinalSubmit;
                }

                #endregion
                //return View(wRNRegistrationModel);
                return View("~/Views/WRN/WRNRegistration/CompleteRegistration.cshtml", wRNRegistrationModel);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompleteRegistrationAsync(WRNRegistrationModel wRNRegistrationModel)
        {
            try
            {
                //wRNRegistrationModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
                var checkFinalSubmit = await _wRNRegistrationService.GetWRNRegistrationByLoginAsync(wRNRegistrationModel.RegistrationNo, wRNRegistrationModel.MobileNo, wRNRegistrationModel.DOB);
                if (checkFinalSubmit.FinalSubmit != 1)
                {
                    string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                    wRNRegistrationModel.ModifiedBy = (int)HttpContext.Session.GetInt32("SessionCreatedBy");
                    wRNRegistrationModel.IPAddress = ipAddress;
                    wRNRegistrationModel.Id = (int)HttpContext.Session.GetInt32("SessionId");
                    wRNRegistrationModel.ApplicationNo = "01/2022/2022-23/" + wRNRegistrationModel.Id;
                    wRNRegistrationModel.FinalSubmit = 0;
                    //if (ModelState.IsValid)
                    //{
                    var data = await _wRNRegistrationService.UpdateWRNRegistrationAsync(wRNRegistrationModel);
                    if (data.Equals(1))
                    {
                        TempData["success"] = "Data has been updated !";
                    }
                    else
                    {
                        TempData["warning"] = "Data has not been updated !";
                    }
                    //}
                    //else
                    //{
                    //    TempData["warning"] = "Some thing went wrong !";
                    //}
                }
                else
                {
                    TempData["warning"] = "Data has been final submitted, you can't any changes !";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();
            }
            return RedirectToAction("CompleteRegistration", "WRNRegistration");
            //return View("~/Views/WRN/WRNRegistration/CompleteRegistration.cshtml", wRNRegistrationModel);
        }
        #endregion

        #region LogOut
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _httpContextAccessor.HttpContext.Session.Clear();
            //Clear cookies
            var cookies = _httpContextAccessor.HttpContext.Request.Cookies;
            foreach (var cookie in cookies)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookie.Key);
            }
            var myCookies = Request.Cookies.Keys;
            foreach (string cookie in myCookies)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookie, new CookieOptions()
                {
                    Domain = "www.google.com"
                });
            }
            return RedirectToAction("Login");
        }
        #endregion

        public async Task<JsonResult> GetDistrict(int StateId)
        {
            var DistrictList = (from district in await _districtService.GetAllDistrict()
                                where district.StateId == StateId
                                select new SelectListItem()
                                {
                                    Text = district.DistrictName,
                                    Value = district.DistrictId.ToString(),
                                }).ToList();

            DistrictList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(DistrictList);
        }


        #region final submit
        [HttpPost]
        public async Task<IActionResult> FinalSubmit(WRNRegistrationModel wRNRegistrationModel)
        {
            try
            {
                var checkFinalSubmit = await _wRNRegistrationService.GetWRNRegistrationByLoginAsync(wRNRegistrationModel.RegistrationNo, wRNRegistrationModel.MobileNo, wRNRegistrationModel.DOB);
                if (checkFinalSubmit.FinalSubmit != 1)
                {
                    wRNRegistrationModel.FinalSubmit = 1;
                    wRNRegistrationModel.Id = checkFinalSubmit.Id;
                    var data = await _wRNRegistrationService.UpdateFinalSubmitAsync(wRNRegistrationModel);
                    if (data.Equals(1))
                    {
                        TempData["success"] = "Data has been final submitted !";
                    }
                    else
                    {
                        TempData["warning"] = "Data has not been final submitted !";
                    }
                }
                else
                {
                    TempData["warning"] = "Data already final submitted !";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return RedirectToAction("CompleteRegistration", "WRNRegistration");
        }
        #endregion

        
    }

}
