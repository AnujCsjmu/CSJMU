using CoreLayout.Models.Common;
using CoreLayout.Models.WRN;
using CoreLayout.Services.Common.OTPVerification;
using CoreLayout.Services.Common.SequenceGenerate;
using CoreLayout.Services.Masters.Category;
using CoreLayout.Services.Masters.District;
using CoreLayout.Services.Masters.Religion;
using CoreLayout.Services.Masters.State;
using CoreLayout.Services.WRN;
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
                        //TempData["error"] = "Invalid details!";
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
                        return View("~/Views/WRNDashboard/Dashboard.cshtml");
                    }
                }
                else
                {
                    //TempData["error"] = "Enter all fields!";
                    ModelState.AddModelError("", "Enter all fields!");
                }
            }
            catch (Exception ex)
            {
                //TempData["error"] = ex.ToString();
                ModelState.AddModelError("", ex.ToString());
            }
            return View();
        }
        #endregion

        #region Registration 
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

        #region Complete Registration
        public async Task<IActionResult> CompleteRegistrationAsync()
        {
            if (HttpContext.Session.GetString("SessionRegistrationNo") != null)
            {
                WRNRegistrationModel wRNRegistrationModel = new WRNRegistrationModel();
                wRNRegistrationModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
                wRNRegistrationModel.FirstName = HttpContext.Session.GetString("SessionFirstName");
                wRNRegistrationModel.MiddleName = HttpContext.Session.GetString("SessionMiddleName");
                wRNRegistrationModel.LastName = HttpContext.Session.GetString("SessionLastName");
                wRNRegistrationModel.FatherName = HttpContext.Session.GetString("SessionFatherName");
                wRNRegistrationModel.MotherName = HttpContext.Session.GetString("SessionMotherName");
                wRNRegistrationModel.DOB = HttpContext.Session.GetString("SessionDOB");
                wRNRegistrationModel.MobileNo = HttpContext.Session.GetString("SessionMobileNo");
                wRNRegistrationModel.EmailId = HttpContext.Session.GetString("SessionEmailId");
                wRNRegistrationModel.CategoryList = await _categoryService.GetAllCategory();
                wRNRegistrationModel.ReligionList = await _religionService.GetAllReligion();
                wRNRegistrationModel.StateList = await _stateService.GetAllState();
                wRNRegistrationModel.DistrictList = await _districtService.GetAllDistrict();

                #region bind data after update
                var data = await _wRNRegistrationService.GetWRNRegistrationByLoginAsync(wRNRegistrationModel.RegistrationNo, wRNRegistrationModel.MobileNo, wRNRegistrationModel.DOB);
                if (data == null)
                {
                }
                else
                {
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
                }

                #endregion
                return View(wRNRegistrationModel);
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
                string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                wRNRegistrationModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
                wRNRegistrationModel.ModifiedBy = (int)HttpContext.Session.GetInt32("SessionCreatedBy");
                wRNRegistrationModel.IPAddress = ipAddress;
                wRNRegistrationModel.Id = (int)HttpContext.Session.GetInt32("SessionId");
                wRNRegistrationModel.ApplicationNo = "01/2022/2022-23/" + wRNRegistrationModel.Id;
                var data = await _wRNRegistrationService.UpdateWRNRegistrationAsync(wRNRegistrationModel);
                if (data.Equals(1))
                {
                    TempData["success"] = "Data has been updated !";
                }
                else
                {
                    TempData["warning"] = "Data has not been updated !";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();
            }
            return RedirectToAction("CompleteRegistration", "WRNRegistration");
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
        //public async Task<IActionResult> Report()
        //{
        //    var data = await _wRNRegistrationService.GetWRNRegistrationByLoginAsync(wRNRegistrationModel.RegistrationNo, wRNRegistrationModel.MobileNo, wRNRegistrationModel.DOB);
        //    if (data == null)
        //    {
        //    }
        //    else
        //    {
        //        Document document = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
        //        Font NormalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, Color.BLACK);
        //        using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
        //        {
        //            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
        //            Phrase phrase = null;
        //            PdfPCell cell = null;
        //            PdfPTable table = null;
        //            Color color = null;

        //            document.Open();

        //            //Header Table
        //            table = new PdfPTable(2);
        //            table.TotalWidth = 500f;
        //            table.LockedWidth = true;
        //            table.SetWidths(new float[] { 0.3f, 0.7f });

        //            //Company Logo
        //            cell = ImageCell("~/images/northwindlogo.gif", 30f, PdfPCell.ALIGN_CENTER);
        //            table.AddCell(cell);

        //            //Company Name and Address
        //            phrase = new Phrase();
        //            phrase.Add(new Chunk("Microsoft Northwind Traders Company\n\n", FontFactory.GetFont("Arial", 16, Font.BOLD, Color.RED)));
        //            phrase.Add(new Chunk("107, Park site,\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, Color.BLACK)));
        //            phrase.Add(new Chunk("Salt Lake Road,\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, Color.BLACK)));
        //            phrase.Add(new Chunk("Seattle, USA", FontFactory.GetFont("Arial", 8, Font.NORMAL, Color.BLACK)));
        //            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
        //            cell.VerticalAlignment = PdfCell.ALIGN_TOP;
        //            table.AddCell(cell);

        //            //Separater Line
        //            color = new Color(System.Drawing.ColorTranslator.FromHtml("#A9A9A9"));
        //            DrawLine(writer, 25f, document.Top - 79f, document.PageSize.Width - 25f, document.Top - 79f, color);
        //            DrawLine(writer, 25f, document.Top - 80f, document.PageSize.Width - 25f, document.Top - 80f, color);
        //            document.Add(table);

        //            table = new PdfPTable(2);
        //            table.HorizontalAlignment = Element.ALIGN_LEFT;
        //            table.SetWidths(new float[] { 0.3f, 1f });
        //            table.SpacingBefore = 20f;

        //            //Employee Details
        //            cell = PhraseCell(new Phrase("Employee Record", FontFactory.GetFont("Arial", 12, Font.UNDERLINE, Color.BLACK)), PdfPCell.ALIGN_CENTER);
        //            cell.Colspan = 2;
        //            table.AddCell(cell);
        //            cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
        //            cell.Colspan = 2;
        //            cell.PaddingBottom = 30f;
        //            table.AddCell(cell);

        //            //Photo
        //            cell = ImageCell(string.Format("~/photos/{0}.jpg", dr["EmployeeId"]), 25f, PdfPCell.ALIGN_CENTER);
        //            table.AddCell(cell);

        //            //Name
        //            phrase = new Phrase();
        //            phrase.Add(new Chunk(dr["TitleOfCourtesy"] + " " + dr["FirstName"] + " " + dr["LastName"] + "\n", FontFactory.GetFont("Arial", 10, Font.BOLD, Color.BLACK)));
        //            phrase.Add(new Chunk("(" + dr["Title"].ToString() + ")", FontFactory.GetFont("Arial", 8, Font.BOLD, Color.BLACK)));
        //            cell = PhraseCell(phrase, PdfPCell.ALIGN_LEFT);
        //            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
        //            table.AddCell(cell);
        //            document.Add(table);

        //            DrawLine(writer, 160f, 80f, 160f, 690f, Color.BLACK);
        //            DrawLine(writer, 115f, document.Top - 200f, document.PageSize.Width - 100f, document.Top - 200f, Color.BLACK);

        //            table = new PdfPTable(2);
        //            table.SetWidths(new float[] { 0.5f, 2f });
        //            table.TotalWidth = 340f;
        //            table.LockedWidth = true;
        //            table.SpacingBefore = 20f;
        //            table.HorizontalAlignment = Element.ALIGN_RIGHT;

        //            //Employee Id
        //            table.AddCell(PhraseCell(new Phrase("Employee code:", FontFactory.GetFont("Arial", 8, Font.BOLD, Color.BLACK)), PdfPCell.ALIGN_LEFT));
        //            table.AddCell(PhraseCell(new Phrase("000" + dr["EmployeeId"], FontFactory.GetFont("Arial", 8, Font.NORMAL, Color.BLACK)), PdfPCell.ALIGN_LEFT));
        //            cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
        //            cell.Colspan = 2;
        //            cell.PaddingBottom = 10f;
        //            table.AddCell(cell);


        //            //Address
        //            table.AddCell(PhraseCell(new Phrase("Address:", FontFactory.GetFont("Arial", 8, Font.BOLD, Color.BLACK)), PdfPCell.ALIGN_LEFT));
        //            phrase = new Phrase(new Chunk(dr["Address"] + "\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, Color.BLACK)));
        //            phrase.Add(new Chunk(dr["City"] + "\n", FontFactory.GetFont("Arial", 8, Font.NORMAL, Color.BLACK)));
        //            phrase.Add(new Chunk(dr["Region"] + " " + dr["Country"] + " " + dr["PostalCode"], FontFactory.GetFont("Arial", 8, Font.NORMAL, Color.BLACK)));
        //            table.AddCell(PhraseCell(phrase, PdfPCell.ALIGN_LEFT));
        //            cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
        //            cell.Colspan = 2;
        //            cell.PaddingBottom = 10f;
        //            table.AddCell(cell);

        //            //Date of Birth
        //            table.AddCell(PhraseCell(new Phrase("Date of Birth:", FontFactory.GetFont("Arial", 8, Font.BOLD, Color.BLACK)), PdfPCell.ALIGN_LEFT));
        //            table.AddCell(PhraseCell(new Phrase(Convert.ToDateTime(dr["BirthDate"]).ToString("dd MMMM, yyyy"), FontFactory.GetFont("Arial", 8, Font.NORMAL, Color.BLACK)), PdfPCell.ALIGN_LEFT));
        //            cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
        //            cell.Colspan = 2;
        //            cell.PaddingBottom = 10f;
        //            table.AddCell(cell);

        //            //Phone
        //            table.AddCell(PhraseCell(new Phrase("Phone Number:", FontFactory.GetFont("Arial", 8, Font.BOLD, Color.BLACK)), PdfPCell.ALIGN_LEFT));
        //            table.AddCell(PhraseCell(new Phrase(dr["HomePhone"] + " Ext: " + dr["Extension"], FontFactory.GetFont("Arial", 8, Font.NORMAL, Color.BLACK)), PdfPCell.ALIGN_LEFT));
        //            cell = PhraseCell(new Phrase(), PdfPCell.ALIGN_CENTER);
        //            cell.Colspan = 2;
        //            cell.PaddingBottom = 10f;
        //            table.AddCell(cell);

        //            //Addtional Information
        //            table.AddCell(PhraseCell(new Phrase("Addtional Information:", FontFactory.GetFont("Arial", 8, Font.BOLD, Color.BLACK)), PdfPCell.ALIGN_LEFT));
        //            table.AddCell(PhraseCell(new Phrase(dr["Notes"].ToString(), FontFactory.GetFont("Arial", 8, Font.NORMAL, Color.BLACK)), PdfPCell.ALIGN_JUSTIFIED));
        //            document.Add(table);
        //            document.Close();
        //            byte[] bytes = memoryStream.ToArray();
        //            memoryStream.Close();
        //            Response.Clear();
        //            Response.ContentType = "application/pdf";
        //            Response.AddHeader("Content-Disposition", "attachment; filename=Employee.pdf");
        //            Response.ContentType = "application/pdf";
        //            Response.Buffer = true;
        //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //            Response.BinaryWrite(bytes);
        //            Response.End();
        //            Response.Close();
        //        }



        //    }
        //    return RedirectToAction("CompleteRegistration", "WRNRegistration");
        //}
        //private static void DrawLine(PdfWriter writer, float x1, float y1, float x2, float y2, Color color)
        //{
        //    PdfContentByte contentByte = writer.DirectContent;
        //    contentByte.SetColorStroke(color);
        //    contentByte.MoveTo(x1, y1);
        //    contentByte.LineTo(x2, y2);
        //    contentByte.Stroke();
        //}
        //private static PdfPCell PhraseCell(Phrase phrase, int align)
        //{
        //    PdfPCell cell = new PdfPCell(phrase);
        //    cell.BorderColor = Color.WHITE;
        //    cell.VerticalAlignment = PdfCell.ALIGN_TOP;
        //    cell.HorizontalAlignment = align;
        //    cell.PaddingBottom = 2f;
        //    cell.PaddingTop = 0f;
        //    return cell;
        //}
        //private static PdfPCell ImageCell(string path, float scale, int align)
        //{
        //    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(HttpContext.Current.Server.MapPath(path));
        //    image.ScalePercent(scale);
        //    PdfPCell cell = new PdfPCell(image);
        //    cell.BorderColor = Color.WHITE;
        //    cell.VerticalAlignment = PdfCell.ALIGN_TOP;
        //    cell.HorizontalAlignment = align;
        //    cell.PaddingBottom = 0f;
        //    cell.PaddingTop = 0f;
        //    return cell;
        //}
    }

}
