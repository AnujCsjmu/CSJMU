using CoreLayout.Models.Exam;
using CoreLayout.Services.Exam.ExamMaster;
using CoreLayout.Services.Exam.SubjectProfile;
using CoreLayout.Services.Masters.CourseDetails;
using CoreLayout.Services.Masters.Institute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace CoreLayout.Controllers.Exam
{
    [Authorize(Roles = "Administrator,Institute,Sub-Institute")]
    public class SubjectProfileController : Controller
    {
        private readonly ILogger<SubjectProfileController> _logger;
        private readonly IDataProtector _protector;
        private readonly IExamMasterService _examMasterService;
        private readonly ICourseDetailsService _courseDetailsService;
        private readonly ISubjectProfileService _subjectProfileService;
        private readonly IInstituteService _instituteService;
        public SubjectProfileController(ILogger<SubjectProfileController> logger,
            IDataProtectionProvider provider, IExamMasterService examMasterService,
            ICourseDetailsService courseDetailsService, ISubjectProfileService subjectProfileService,
            IInstituteService instituteService)
        {
            _logger = logger;
            _examMasterService = examMasterService;
            _protector = provider.CreateProtector("ExamForm.ExamFormController");
            _courseDetailsService = courseDetailsService;
            _subjectProfileService = subjectProfileService;
            _instituteService = instituteService;
        }

        [HttpGet]
        //[AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _subjectProfileService.GetAllSubjectProfileAsync();
                ViewBag.Subjects = await _subjectProfileService.GetSubjectFromSubjectProfileMapping();
                foreach (var _data in data)
                {
                    var stringId = _data.SubjectProfileId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.SubjectProfileId;
                }
                id = id + 1;
                ViewBag.MaxSubjectProfileId = _protector.Protect(id.ToString());
                //end
                return View("~/Views/Exam/SubjectProfile/Index.cshtml", data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/Exam/SubjectProfile/Index.cshtml");
        }

        [HttpGet]
        //[AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _subjectProfileService.GetSubjectProfileByIdAsync(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View("~/Views/Exam/SubjectProfile/Details.cshtml", data);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        //[AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> CreateAsync(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                SubjectProfileModel examFormModel = new SubjectProfileModel();
                examFormModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
                //examFormModel.CourseList =(from s in await _instituteService.AffiliationInstituteIntakeData()
                //                           where s.sy
                return View("~/Views/Exam/SubjectProfile/Create.cshtml", examFormModel);
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
        //[AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(SubjectProfileModel subjectProfileModel)
        {
            subjectProfileModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            subjectProfileModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            subjectProfileModel.ExamList = await _examMasterService.GetAllExamMasterAsync();
            if (ModelState.IsValid)
            {
                var res = await _subjectProfileService.CreateSubjectProfileAsync(subjectProfileModel);
                if (res.Equals(1))
                {
                    TempData["success"] = "Subject Combination has been saved";
                }
                else
                {
                    TempData["error"] = "Subject Combination has not been saved";
                }
                return RedirectToAction(nameof(Index));

            }
            return View("~/Views/Exam/SubjectProfile/Create.cshtml", subjectProfileModel);

        }

        public async Task<JsonResult> GetCourse(int Examid)
        {
            int SessionInstituteId = (int)HttpContext.Session.GetInt32("SessionInstituteId");
            var sessionid = await _examMasterService.GetExamMasterByIdAsync(Examid);
            var CourseList = (from s in await _subjectProfileService.GetCourseFromAff_SubjectProfile(SessionInstituteId, sessionid.SessionId)
                              select new SelectListItem()
                              {
                                  Text = s.CourseName,
                                  Value = s.CourseId.ToString(),
                              }).Distinct().ToList();

            CourseList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(CourseList);
        }
        public async Task<JsonResult> GetFaculty(int Examid,int CourseId)
        {
            int SessionInstituteId = (int)HttpContext.Session.GetInt32("SessionInstituteId");
            var sessionid = await _examMasterService.GetExamMasterByIdAsync(Examid);
            var FacultyList = (from s in await _subjectProfileService.GetFacultyFromAff_SubjectProfile(SessionInstituteId, sessionid.SessionId, CourseId)
                              select new SelectListItem()
                              {
                                  Text = s.FacultyName,
                                  Value = s.FacultyID.ToString(),
                              }).Distinct().ToList();

            FacultyList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(FacultyList);
        }

        public async Task<JsonResult> GetOtherFaculty(int Examid)
        {
            int SessionInstituteId = (int)HttpContext.Session.GetInt32("SessionInstituteId");
            var sessionid = await _examMasterService.GetExamMasterByIdAsync(Examid);
            var OtherFacultyList = (from s in await _subjectProfileService.GetOtherFacultyFromAff_SubjectProfile(SessionInstituteId, sessionid.SessionId)
                               select new SelectListItem()
                               {
                                   Text = s.OtherFacultyName,
                                   Value = s.OtherFacultyId.ToString(),
                               }).Distinct().ToList();

            OtherFacultyList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(OtherFacultyList);
        }

        public async Task<JsonResult> GetMinorFaculty(int Examid)
        {
            int SessionInstituteId = (int)HttpContext.Session.GetInt32("SessionInstituteId");
            var sessionid = await _examMasterService.GetExamMasterByIdAsync(Examid);
            var MinorFacultyList = (from s in await _subjectProfileService.GetMinorFacultyFromAff_SubjectProfile(SessionInstituteId, sessionid.SessionId)
                                    select new SelectListItem()
                                    {
                                        Text = s.MinorFacultyName,
                                        Value = s.MinorFacultyId.ToString(),
                                    }).Distinct().ToList();

            MinorFacultyList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(MinorFacultyList);
        }

        public async Task<JsonResult> GetVocationalSubject(int Examid, int CourseId)
        {
            int SessionInstituteId = (int)HttpContext.Session.GetInt32("SessionInstituteId");
            var sessionid = await _examMasterService.GetExamMasterByIdAsync(Examid);
            var VocationalSubjectList = (from s in await _subjectProfileService.GetSubjectFromAff_SubjectProfile(SessionInstituteId, sessionid.SessionId, CourseId)
                                    where s.SubjectType== "Vocational"
                                    select new SelectListItem()
                                    {
                                        Text = s.VocationalSubjectName,
                                        Value = s.VocationalSubjectId.ToString(),
                                    }).Distinct().ToList();

            VocationalSubjectList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(VocationalSubjectList);
        }

        public async Task<JsonResult> GetCurricularSubject(int Examid, int CourseId)
        {
            int SessionInstituteId = (int)HttpContext.Session.GetInt32("SessionInstituteId");
            var sessionid = await _examMasterService.GetExamMasterByIdAsync(Examid);
            var CurricularSubjectList = (from s in await _subjectProfileService.GetSubjectFromAff_SubjectProfile(SessionInstituteId, sessionid.SessionId, CourseId)
                                         where s.SubjectType == "Vocational"//change it Curricular 
                                         select new SelectListItem()
                                         {
                                             Text = s.CoCurricularSubjectName,
                                             Value = s.CoCurricularSubjectId.ToString(),
                                         }).Distinct().ToList();

            CurricularSubjectList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(CurricularSubjectList);
        }

        public async Task<JsonResult> GetMinorSubject(int Examid, int CourseId)
        {
            int SessionInstituteId = (int)HttpContext.Session.GetInt32("SessionInstituteId");
            var sessionid = await _examMasterService.GetExamMasterByIdAsync(Examid);
            var MinorSubjectList = (from s in await _subjectProfileService.GetSubjectFromAff_SubjectProfile(SessionInstituteId, sessionid.SessionId, CourseId)
                                         where s.SubjectType == "Vocational"//change it minor 
                                    select new SelectListItem()
                                         {
                                             Text = s.MinorSubjectName,
                                             Value = s.MinorSubjectId.ToString(),
                                         }).Distinct().ToList();

            MinorSubjectList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(MinorSubjectList);
        }
        public async Task<JsonResult> GetMajorSubject(int Examid, int CourseId)
        {
            int SessionInstituteId = (int)HttpContext.Session.GetInt32("SessionInstituteId");
            var sessionid = await _examMasterService.GetExamMasterByIdAsync(Examid);
            var MajorSubjectList = (from s in await _subjectProfileService.GetSubjectFromAff_SubjectProfile(SessionInstituteId, sessionid.SessionId, CourseId)
                                    where s.SubjectType == "Vocational"//change it major 
                                    select new SelectListItem()
                                    {
                                        Text = s.MajorSubjectName,
                                        Value = s.MajorSubjectId.ToString(),
                                    }).Distinct().ToList();

            //MajorSubjectList.Insert(0, new SelectListItem()
            //{
            //    Text = "----Select----",
            //    Value = string.Empty
            //});
            return Json(MajorSubjectList);
        }
    }
}
