using CoreLayout.Models.Common;
using CoreLayout.Models.WRN;
using CoreLayout.Services.Common.OTPVerification;
using CoreLayout.Services.Common.SequenceGenerate;
using CoreLayout.Services.Masters.Category;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.District;
using CoreLayout.Services.Masters.Institute;
using CoreLayout.Services.Masters.Religion;
using CoreLayout.Services.Masters.State;
using CoreLayout.Services.WRN;
using CoreLayout.Services.WRN.WRNCourseDetails;
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
    public class WRNCourseDetailsController : Controller
    {
        private readonly ILogger<WRNCourseDetailsController> _logger;
        private readonly IDataProtector _protector;
        public readonly IConfiguration _configuration;
        //public readonly IWRNRegistrationService _wRNRegistrationService;
        //public readonly IWRNQualificationService _wRNQualificationService;
        private IHttpContextAccessor _httpContextAccessor;
        public readonly IWRNCourseDetailsService _wRNCourseDetailsService;
        public readonly IDistrictService _districtService;
        public readonly IInstituteService _instituteService;
        public readonly ICourseService _courseService;
        public WRNCourseDetailsController(ILogger<WRNCourseDetailsController> logger,
            IDataProtectionProvider provider, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor, IWRNCourseDetailsService wRNCourseDetailsService,
            IDistrictService districtService, IInstituteService instituteService,
            ICourseService courseService)
        {
            _logger = logger;
            _protector = provider.CreateProtector("WRNCourseDetails.WRNCourseDetailsController");
            _configuration = configuration;
            //_wRNRegistrationService = wRNRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            //_wRNQualificationService = wRNQualificationService;
            _wRNCourseDetailsService = wRNCourseDetailsService;
            _districtService = districtService;
            _instituteService = instituteService;
            _courseService = courseService;
        }

        #region Add Course
        [HttpGet]
        public async Task<IActionResult> WRNCourse()
        {
            WRNCourseDetailsModel wRNCourseDetailsModel = new WRNCourseDetailsModel();
            try
            {

                wRNCourseDetailsModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
                wRNCourseDetailsModel.CreatedBy = HttpContext.Session.GetInt32("SessionId");
                wRNCourseDetailsModel.DistrictList = await _districtService.Get7DistrictAsync();
                wRNCourseDetailsModel.CourseList = await _courseService.GetAllCourse();

                List<WRNCourseDetailsModel> dataLst = new List<WRNCourseDetailsModel>();
                var data = await _wRNCourseDetailsService.GetAllWRNCourseDetailsAsync();
                wRNCourseDetailsModel.WRNCourseDataList = data;
                //encryption
                foreach (var _data in data)
                {
                    var stringId = _data.Id.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();
            }
            return View("~/Views/WRN/WRNCourseDetails/WRNCourse.cshtml", wRNCourseDetailsModel);
        }

        [HttpPost]
        public async Task<IActionResult> WRNCourse(WRNCourseDetailsModel wRNCourseDetailsModel)
        {
            try
            {
                string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                wRNCourseDetailsModel.CreatedBy = HttpContext.Session.GetInt32("SessionCreatedBy");
                wRNCourseDetailsModel.IPAddress = ipAddress;
                wRNCourseDetailsModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
                wRNCourseDetailsModel.WRNCourseDataList = await _wRNCourseDetailsService.GetAllWRNCourseDetailsAsync();
                wRNCourseDetailsModel.DistrictList = await _districtService.Get7DistrictAsync();
                wRNCourseDetailsModel.CourseList = await _courseService.GetAllCourse();
                string result = CheckValidation(wRNCourseDetailsModel);
                if (result == "")
                {
                    //check already exit
                    var IsAlreadyExit = (from s in await _wRNCourseDetailsService.GetAllWRNCourseDetailsAsync()
                                         where s.RegistrationNo == wRNCourseDetailsModel.RegistrationNo
                                         && s.DistrictId == wRNCourseDetailsModel.DistrictId
                                         && s.CourseId == wRNCourseDetailsModel.CourseId
                                         select s).Distinct().ToList();
                    if (IsAlreadyExit.Count == 0)
                    {
                        var data = await _wRNCourseDetailsService.CreateWRNCourseDetailsAsync(wRNCourseDetailsModel);
                        if (data.Equals(1))
                        {
                            TempData["success"] = "Course has add successfully !";
                        }
                        else
                        {
                            TempData["error"] = "Course has not add successfully !";
                        }
                    }
                    else
                    {
                        TempData["warning"] = "Course has already added !";
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
            List<WRNCourseDetailsModel> dataLst = new List<WRNCourseDetailsModel>();
            var binddata = await _wRNCourseDetailsService.GetAllWRNCourseDetailsAsync();
            wRNCourseDetailsModel.WRNCourseDataList = binddata;
            return View("~/Views/WRN/WRNCourseDetails/WRNCourse.cshtml", wRNCourseDetailsModel);
        }
        #endregion

        public string CheckValidation(WRNCourseDetailsModel wRNCourseDetailsModel)
        {
            string msg = "";
            if (wRNCourseDetailsModel.DistrictId == 0)
            {
                msg = "Please select district";
            }
            else if (wRNCourseDetailsModel.InstituteId == 0)
            {
                msg = "Please select institute";
            }
            else if (wRNCourseDetailsModel.CourseId == 0)
            {
                msg = "Please select course";
            }
            return msg;
        }
        public async Task<JsonResult> GetInstitute(string districtid)
        {
            var InstituteList = (from s in await _instituteService.AffiliationInstituteIntakeData()
                                       select new SelectListItem()
                                       {
                                           Text = s.InstituteName,
                                           Value = s.InstituteID.ToString(),
                                       }).Distinct().ToList();
            InstituteList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(InstituteList);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            WRNCourseDetailsModel wRNCourseDetailsModel = new WRNCourseDetailsModel();
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _wRNCourseDetailsService.GetWRNCourseDetailsByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _wRNCourseDetailsService.DeleteWRNCourseDetailsAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Course has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Course has not been deleted";
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
            return RedirectToAction(nameof(WRNCourse));
           
        }

        public async Task<ActionResult> qualificationList(string id)
        {
            //ViewBag.type = 1;
            var guid_id = _protector.Unprotect(id);
            var data = await _wRNCourseDetailsService.GetWRNCourseDetailsByIdAsync(Convert.ToInt32(guid_id));
            if (data == null)
            {
                return NotFound();
            }
            return PartialView("~/Views/WRN/WRNCourseDetails/_WRNCourseList.cshtml", data);
        }
    }

}
