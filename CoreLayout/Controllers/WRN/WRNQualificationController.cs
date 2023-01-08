using CoreLayout.Models.Common;
using CoreLayout.Models.WRN;
using CoreLayout.Services.Common.OTPVerification;
using CoreLayout.Services.Common.SequenceGenerate;
using CoreLayout.Services.Masters.Category;
using CoreLayout.Services.Masters.District;
using CoreLayout.Services.Masters.Religion;
using CoreLayout.Services.Masters.State;
using CoreLayout.Services.WRN;
using CoreLayout.Services.WRN.WRNQualification;
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
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.WRN
{
    public class WRNQualificationController : Controller
    {
        private readonly ILogger<WRNQualificationController> _logger;
        private readonly IDataProtector _protector;
        public readonly IConfiguration _configuration;
        public readonly IWRNRegistrationService _wRNRegistrationService;
        public readonly IWRNQualificationService _wRNQualificationService;
        private IHttpContextAccessor _httpContextAccessor;

        public WRNQualificationController(ILogger<WRNQualificationController> logger,
            IDataProtectionProvider provider, IConfiguration configuration,
            IWRNRegistrationService wRNRegistrationService, IHttpContextAccessor httpContextAccessor,
           IWRNQualificationService wRNQualificationService)
        {
            _logger = logger;
            _protector = provider.CreateProtector("WRNQualification.WRNQualificationController");
            _configuration = configuration;
            _wRNRegistrationService = wRNRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _wRNQualificationService = wRNQualificationService;
        }

        #region Add Qualification
        [HttpGet]
        public async Task<IActionResult> QualificationAsync()
        {
            WRNQualificationModel wRNQualificationModel = new WRNQualificationModel();
            try
            {

                wRNQualificationModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
                wRNQualificationModel.CreatedBy = HttpContext.Session.GetInt32("SessionId");
                wRNQualificationModel.EducationalQualificationList = await _wRNQualificationService.GetAllEducationalQualification();
                //wRNQualificationModel.BoardUniversityList = await _wRNQualificationService.GetAllBoardUniversity();
                wRNQualificationModel.BoardUniversityTypeList = await _wRNQualificationService.GetAllBoardUniversityType();
                List<WRNQualificationModel> dataLst = new List<WRNQualificationModel>();
                var data = await _wRNQualificationService.GetAllWRNQualificationAsync();
                wRNQualificationModel.DataList = data;
                //encryption
                foreach (var _data in data)
                {
                    var stringId = _data.Id.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }

                //bind year
                int currentYear = Convert.ToInt32(DateTime.Now.Year);
                List<int> yrs = new List<int>();

                for (int i = 1980; i <= currentYear; i++)
                {
                    yrs.Add(i);
                }
                wRNQualificationModel.PassingYearList = yrs;

                //bind month
                List<int> mon = new List<int>();
                //var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                for (int i = 1; i < 13; i++)
                {
                    mon.Add(i);
                }
                wRNQualificationModel.PassingMonthList = mon;
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();
            }
            return View("~/Views/WRN/WRNQualification/Qualification.cshtml", wRNQualificationModel);
        }

        [HttpPost]
        public async Task<IActionResult> Qualification(WRNQualificationModel wRNQualificationModel)
        {
            try
            {
                string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                wRNQualificationModel.CreatedBy = HttpContext.Session.GetInt32("SessionCreatedBy");
                wRNQualificationModel.IPAddress = ipAddress;
                wRNQualificationModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
                wRNQualificationModel.EducationalQualificationList = await _wRNQualificationService.GetAllEducationalQualification();
                //wRNQualificationModel.BoardUniversityList = await _wRNQualificationService.GetAllBoardUniversity();
                wRNQualificationModel.BoardUniversityTypeList = await _wRNQualificationService.GetAllBoardUniversityType();

                //bind year
                int currentYear = Convert.ToInt32(DateTime.Now.Year);
                List<int> yrs = new List<int>();

                for (int i = 1980; i <= currentYear; i++)
                {
                    yrs.Add(i);
                }
                wRNQualificationModel.PassingYearList = yrs;

                //bind month
                List<int> mon = new List<int>();
                //var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                for (int i = 1; i < 13; i++)
                {
                    mon.Add(i);
                }
                wRNQualificationModel.PassingMonthList = mon;
                //validation
                string result = CheckValidation(wRNQualificationModel);
                if (result == "")
                {
                    //check already exit
                    var IsAlreadyExit = (from s in await _wRNQualificationService.GetAllWRNQualificationAsync()
                                         where s.RegistrationNo == wRNQualificationModel.RegistrationNo && s.QualificationId == wRNQualificationModel.QualificationId
                                         select s).Distinct().ToList();
                    if (IsAlreadyExit.Count == 0)
                    {
                        var data = await _wRNQualificationService.CreateWRNQualificationAsync(wRNQualificationModel);
                        if (data.Equals(1))
                        {
                            TempData["success"] = "Qualification has add successfully !";
                        }
                        else
                        {
                            TempData["error"] = "Qualification has not add successfully !";
                        }
                    }
                    else
                    {
                        TempData["warning"] = "Qualification has already added !";
                    }
                }
                else
                {
                    TempData["warning"] = result;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();
            }
            //bind data
            List<WRNQualificationModel> dataLst = new List<WRNQualificationModel>();
            var binddata = await _wRNQualificationService.GetAllWRNQualificationAsync();
            wRNQualificationModel.DataList = binddata;
            return View("~/Views/WRN/WRNQualification/Qualification.cshtml", wRNQualificationModel);
        }
        #endregion

        public string CheckValidation(WRNQualificationModel wRNQualificationModel)
        {
            string msg = "";
            if (wRNQualificationModel.BoardUniversityId == 236 || wRNQualificationModel.BoardUniversityId == 1225)
            {
                if (wRNQualificationModel.OtherUniversity == null)
                {
                    msg = "Please enter other university";
                }
            }
            else if (wRNQualificationModel.MarksCriteria == "PERCENTAGE" || wRNQualificationModel.MarksCriteria == "GRADE")
            {
                if (wRNQualificationModel.PercentagOfMarksObtained == null)
                {
                    msg = "Please enter percentage/grade";
                }
            }
            else if (wRNQualificationModel.ResultStatus == "PASSED")
            {
                if (wRNQualificationModel.PassingYear.ToString() == "")
                {
                    msg = "Please select passing year";
                }
                else if (wRNQualificationModel.PassingMonth.ToString() == "")
                {
                    msg = "Please select passing month";
                }
                else if (wRNQualificationModel.MarksCriteria == null)
                {
                    msg = "Please enter marks criteria";
                }
                else if (wRNQualificationModel.PercentagOfMarksObtained == null)
                {
                    msg = "Please enter percentage/grade";
                }
                else if (wRNQualificationModel.DivisionClassGrade == null)
                {
                    msg = "Please enter division";
                }
                else if (wRNQualificationModel.Subjects == null)
                {
                    msg = "Please enter subjects";
                }
            }
            return msg;
        }
        public async Task<JsonResult> GetUniversity(string QualificationType)
        {
            var BoardUniversityList = (from university in await _wRNQualificationService.GetAllBoardUniversityByType(QualificationType)
                                       select new SelectListItem()
                                       {
                                           Text = university.University,
                                           Value = university.Id.ToString(),
                                       }).Distinct().ToList();
            BoardUniversityList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(BoardUniversityList);
        }

        //[HttpGet]
        //public async Task<IActionResult> Details(string id)
        //{
        //    var guid_id = _protector.Unprotect(id);
        //    var data = await _wRNQualificationService.GetWRNQualificationByIdAsync(Convert.ToInt32(guid_id));
        //    try
        //    {
        //        data.EncryptedId = id;
        //        if (data == null)
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["error"] = ex.ToString();
        //    }
        //    //return View("~/Views/WRN/WRNQualification/Qualification.cshtml", data);
        //    return RedirectToAction(nameof(Qualification));
        //}

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            WRNQualificationModel wRNQualificationModel = new WRNQualificationModel();
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _wRNQualificationService.GetWRNQualificationByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _wRNQualificationService.DeleteWRNQualificationAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Data has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Data has not been deleted";
                    }
                }
                else
                {
                    TempData["error"] = "Some thing went wrong!";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();
            }
            //var binddata = await _wRNQualificationService.GetAllWRNQualificationAsync();
            //wRNQualificationModel.DataList = binddata;
            ////encryption
            //foreach (var _data in binddata)
            //{
            //    var stringId = _data.Id.ToString();
            //    _data.EncryptedId = _protector.Protect(stringId);
            //}
            return RedirectToAction(nameof(Qualification));
            //return View("~/Views/WRN/WRNQualification/Qualification.cshtml");
        }

        public async Task<ActionResult> qualificationList(string id)
        {
            //ViewBag.type = 1;
            var guid_id = _protector.Unprotect(id);
            var data = await _wRNQualificationService.GetWRNQualificationByIdAsync(Convert.ToInt32(guid_id));
            //var data = await _wRNQualificationService.GetAllByIdForDetailsAsync(Convert.ToInt32(guid_id));
            if (data == null)
            {
                return NotFound();
            }
            return PartialView("~/Views/WRN/WRNQualification/_QualificationList.cshtml", data);

        }
    }

}
