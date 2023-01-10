using CoreLayout.Models.WRN;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.District;
using CoreLayout.Services.Masters.Institute;
using CoreLayout.Services.WRN.WRNCourseDetails;
using CoreLayout.Services.WRN.WRNRegistration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public readonly IWRNRegistrationService _wRNRegistrationService;
        public WRNCourseDetailsController(ILogger<WRNCourseDetailsController> logger,
            IDataProtectionProvider provider, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor, IWRNCourseDetailsService wRNCourseDetailsService,
            IDistrictService districtService, IInstituteService instituteService,
            ICourseService courseService, IWRNRegistrationService wRNRegistrationService)
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
            _wRNRegistrationService = wRNRegistrationService;
        }

        #region Add Course
        [HttpGet]
        public async Task<IActionResult> WRNCourse()
        {
            if (HttpContext.Session.GetString("SessionRegistrationNo") != null)
            {
                WRNCourseDetailsModel wRNCourseDetailsModel = new WRNCourseDetailsModel();
                try
                {
                    string dob = HttpContext.Session.GetString("SessionDOB");
                    string mobile = HttpContext.Session.GetString("SessionMobileNo");
                    wRNCourseDetailsModel.FinalSubmit = Convert.ToBoolean(HttpContext.Session.GetInt32("SessionFinalSubmit"));
                    wRNCourseDetailsModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
                    wRNCourseDetailsModel.CreatedBy = HttpContext.Session.GetInt32("SessionId");
                    wRNCourseDetailsModel.DistrictList = await _districtService.Get7DistrictAsync();
                    wRNCourseDetailsModel.CourseList = await _courseService.GetAllCourse();

                    #region check step-1 is completed or not
                    var registrationdata = (from s in await _wRNRegistrationService.GetAllWRNRegistrationAsync()
                                            where s.RegistrationNo == wRNCourseDetailsModel.RegistrationNo
                                            && s.DOB == dob && s.MobileNo == mobile
                                            && s.ApplicationNo != null && s.RegistrationNo != null
                                            select s).Distinct().ToList();
                    #endregion
                    if (registrationdata.Count == 0)
                    {
                        TempData["warning"] = "First Complete previous steps !";  
                       return RedirectToAction("DashBoard", "WRNDashBoard");
                    }
                    else
                    {
                        var data = await _wRNCourseDetailsService.GetAllWRNCourseByRegistrationsAsync(wRNCourseDetailsModel.RegistrationNo);
                        wRNCourseDetailsModel.WRNCourseDataList = data;

                        //encryption
                        foreach (var _data in data)
                        {
                            var stringId = _data.Id.ToString();
                            _data.EncryptedId = _protector.Protect(stringId);
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.ToString();
                  
                }
                return View("~/Views/WRN/WRNCourseDetails/WRNCourse.cshtml", wRNCourseDetailsModel);
            }
            else
            {
                return RedirectToAction("Logout", "WRNRegistration");
            }
        }

        [HttpPost]
        public async Task<IActionResult> WRNCourse(WRNCourseDetailsModel wRNCourseDetailsModel)
        {
            try
            {
                string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                wRNCourseDetailsModel.FinalSubmit = Convert.ToBoolean(HttpContext.Session.GetInt32("SessionFinalSubmit"));
                wRNCourseDetailsModel.CreatedBy = HttpContext.Session.GetInt32("SessionCreatedBy");
                wRNCourseDetailsModel.IPAddress = ipAddress;
                wRNCourseDetailsModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
                wRNCourseDetailsModel.WRNCourseDataList = await _wRNCourseDetailsService.GetAllWRNCourseDetailsAsync();
                string DOB = HttpContext.Session.GetString("SessionDOB");
                string MobileNo = HttpContext.Session.GetString("SessionMobileNo");
                //wRNCourseDetailsModel.DistrictList = await _districtService.Get7DistrictAsync();
                //wRNCourseDetailsModel.CourseList = await _courseService.GetAllCourse();
                //var checkFinalSubmit = await _wRNRegistrationService.GetWRNRegistrationByLoginAsync(wRNCourseDetailsModel.RegistrationNo, MobileNo, DOB);
                if (wRNCourseDetailsModel.FinalSubmit == false)
                {
                    string result = CheckValidation(wRNCourseDetailsModel);
                    if (result == "")
                    {
                        //check total 10 record check
                        var check10record = await _wRNCourseDetailsService.GetAllWRNCourseDetailsAsync();
                        if (check10record.Count <= 10)
                        {
                            //check add only 3 course same type
                            var Check3CourseCount = await _wRNCourseDetailsService.Check3CourseCountAsync(wRNCourseDetailsModel.RegistrationNo);
                            //where s.RegistrationNo == wRNCourseDetailsModel.RegistrationNo
                            //select s.CourseId).Distinct().ToList();
                            if (Check3CourseCount.CourseCount <= 2)
                            {
                                var Check3CourseList = await _wRNCourseDetailsService.Check3CourseListAsync(wRNCourseDetailsModel.RegistrationNo);
                                int i = 0;
                                foreach (var _data in Check3CourseList)
                                {
                                    if (wRNCourseDetailsModel.CourseId == _data.CourseId)
                                    {
                                        i = 1;
                                    }
                                }
                                if (i == 0)
                                {
                                    //check already exit
                                    var IsAlreadyExit = (from s in await _wRNCourseDetailsService.GetAllWRNCourseDetailsAsync()
                                                         where s.RegistrationNo == wRNCourseDetailsModel.RegistrationNo
                                                         && s.DistrictId == wRNCourseDetailsModel.DistrictId
                                                         && s.CourseId == wRNCourseDetailsModel.CourseId
                                                         && s.InstituteId == wRNCourseDetailsModel.InstituteId
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
                                    TempData["warning"] = "You can add only 3 course !";
                                }
                            }
                            else
                            {
                                TempData["warning"] = "You can add only 3 course same type !";
                            }
                        }
                        else
                        {
                            TempData["warning"] = "You can add only 10 courses !";
                        }
                    }
                    else
                    {
                        TempData["warning"] = result;
                    }
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
            //bind data
            // List<WRNCourseDetailsModel> dataLst = new List<WRNCourseDetailsModel>();
            //var binddata = await _wRNCourseDetailsService.GetAllWRNCourseByRegistrationsAsync(wRNCourseDetailsModel.RegistrationNo);
            //wRNCourseDetailsModel.WRNCourseDataList = binddata;
            return RedirectToAction("WRNCourse");
            //return View("~/Views/WRN/WRNCourseDetails/WRNCourse.cshtml", wRNCourseDetailsModel);
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
        public async Task<JsonResult> GetInstitute(int districtid)
        {
            var InstituteList = (from s in await _instituteService.DistinctAffiliationInstituteIntakeData()
                                 where s.DistrictID == districtid
                                 select new SelectListItem()
                                 {
                                     Text = s.InstituteCodeWithName,
                                     Value = s.InstituteID.ToString(),
                                 }).Distinct().ToList();
            InstituteList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(InstituteList);
        }

        public async Task<JsonResult> GetCourse(int InstituteId)
        {
            var CourseList = (from s in await _courseService.GetAllCourseByInstitute(InstituteId)
                              select new SelectListItem()
                              {
                                  Text = s.CourseCodeWithName,
                                  Value = s.CourseID.ToString(),
                              }).Distinct().ToList();
            CourseList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(CourseList);
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
