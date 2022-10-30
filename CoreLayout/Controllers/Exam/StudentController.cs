using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Exam;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Exam.ExamMaster;
using CoreLayout.Services.Exam.Student;
using CoreLayout.Services.Masters.Branch;
using CoreLayout.Services.Masters.CourseDetails;
using CoreLayout.Services.Masters.District;
using CoreLayout.Services.Masters.Religion;
using CoreLayout.Services.Masters.State;
using CoreLayout.Services.Masters.Tehsil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CoreLayout.Controllers.Exam
{
    [Authorize(Roles = "Administrator")]
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IDataProtector _protector;
        private readonly IExamMasterService _examMasterService;
        private readonly ICourseDetailsService _courseDetailsService;
        private readonly IStudentService _studentService;

        private readonly IStateService _stateService;
        private readonly IDistrictService _districtService;
        private readonly ITehsilService _tehsilService;
        private readonly IReligionService _religionService;
        public StudentController(ILogger<StudentController> logger, IDataProtectionProvider provider, 
            IExamMasterService examMasterService, ICourseDetailsService courseDetailsService,
            IStudentService studentService, IStateService stateService, IDistrictService districtService,
            ITehsilService tehsilService, IReligionService religionService)
        {
            _logger = logger;
            _examMasterService = examMasterService;
            _protector = provider.CreateProtector("Student.StudentController");
            _courseDetailsService = courseDetailsService;
            _studentService = studentService;

            _stateService = stateService;
            _districtService = districtService;
            _tehsilService = tehsilService;
            _religionService = religionService;
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _studentService.GetAllStudentAsync();
                foreach (var _data in data)
                {
                    var stringId = _data.StudentID.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.StudentID;
                }
                id = id + 1;
                ViewBag.MaxStudentId = _protector.Protect(id.ToString());
                //end
                return View("~/Views/Exam/Student/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/Student/Index.cshtml");
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _studentService.GetStudentByIdAsync(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/Exam/Student/Details.cshtml", data);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> CreateAsync(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                StudentModel studentModel = new StudentModel();
                studentModel.PStateList = await _stateService.GetAllState();
                studentModel.PDistrictList = await _districtService.GetAllDistrict();
                //studentModel.PTehsilList = await _tehsilService.GetAllTehsil();
                studentModel.CStateList = await _stateService.GetAllState();
                studentModel.CDistrictList = await _districtService.GetAllDistrict();
                //studentModel.CTehsilList = await _tehsilService.GetAllTehsil();
                studentModel.ReligionList = await _religionService.GetAllReligion();
                return View("~/Views/Exam/Student/Create.cshtml", studentModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(StudentModel studentModel)
        {
            studentModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            studentModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            studentModel.PStateList = await _stateService.GetAllState();
            studentModel.PDistrictList = await _districtService.GetAllDistrict();
            //studentModel.PTehsilList = await _tehsilService.GetAllTehsil();
            studentModel.CStateList = await _stateService.GetAllState();
            studentModel.CDistrictList = await _districtService.GetAllDistrict();
            //studentModel.CTehsilList = await _tehsilService.GetAllTehsil();
            studentModel.ReligionList = await _religionService.GetAllReligion();
            if (ModelState.IsValid)
            {

                var res = await _studentService.CreateStudentAsync(studentModel);
                if (res.Equals(1))
                {
                    TempData["success"] = "Student has been saved";
                }
                else
                {
                    TempData["error"] = "Student has not been saved";
                }
                return RedirectToAction(nameof(Index));

            }
            return View("~/Views/Exam/Student/Create.cshtml", studentModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _studentService.GetStudentByIdAsync(Convert.ToInt32(guid_id));
                data.PStateList = await _stateService.GetAllState();
                //data.PDistrictList = await _districtService.GetAllDistrict();
                //data.PTehsilList = await _tehsilService.GetAllTehsil();
                data.CStateList = await _stateService.GetAllState();
                //data.CDistrictList = await _districtService.GetAllDistrict();
                //data.CTehsilList = await _tehsilService.GetAllTehsil();
                data.ReligionList = await _religionService.GetAllReligion();
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/Exam/Student/Edit.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/Student/Edit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int StudentID, StudentModel studentModel)
        {
            try
            {
                studentModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                studentModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                studentModel.PStateList = await _stateService.GetAllState();
                //studentModel.PDistrictList = await _districtService.GetAllDistrict();
                //studentModel.PTehsilList = await _tehsilService.GetAllTehsil();
                studentModel.CStateList = await _stateService.GetAllState();
                //studentModel.CDistrictList = await _districtService.GetAllDistrict();
                //studentModel.CTehsilList = await _tehsilService.GetAllTehsil();
                studentModel.ReligionList = await _religionService.GetAllReligion();
                if (ModelState.IsValid)
                {
                    var value = await _studentService.GetStudentByIdAsync(StudentID);
                    if (await TryUpdateModelAsync<StudentModel>(value))
                    {
                        var res = await _studentService.UpdateStudentAsync(studentModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Student has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Student has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/Student/Edit.cshtml", studentModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _studentService.GetStudentByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _studentService.DeleteStudentAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Student has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Student has not been deleted";
                    }
                }
                else
                {
                    TempData["error"] = "Some thing went wrong!";
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }

            return RedirectToAction(nameof(Index));
        }
        public JsonResult GetDistrict(int StateId)
        {
            var GetDistrictList = (from district in _districtService.GetAllDistrict().Result
                               .Where(x => x.StateId == StateId)
                                   select new SelectListItem()
                                   {
                                       Text = district.DistrictName,
                                       Value = district.DistrictId.ToString(),
                                   }).ToList();
            GetDistrictList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(GetDistrictList);
        }

        public JsonResult GetTehsil(int DistrictId)
        {
            var GetTehsilList = (from tehsil in _tehsilService.GetAllTehsil().Result
                               .Where(x => x.DistrictId == DistrictId)
                                 select new SelectListItem()
                                 {
                                     Text = tehsil.TehsilName,
                                     Value = tehsil.TehsilId.ToString(),
                                 }).ToList();
            GetTehsilList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(GetTehsilList);
        }

        //[AcceptVerbs("GET", "POST")]
        //public IActionResult VerifyName(string examName)
        //{

        //    var already = (from data in _examMasterService.GetAllExamMasterAsync().Result
        //                   where data.ExamName == examName.Trim()
        //                   select data).ToList();

        //    if (already.Count > 0)
        //    {
        //        return Json($"{examName} is already in use.");
        //    }

        //    return Json(true);


        //}
    }
}
