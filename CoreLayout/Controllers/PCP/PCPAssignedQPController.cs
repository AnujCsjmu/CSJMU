using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Models.PCP;
using CoreLayout.Models.QPDetails;
using CoreLayout.Services.Exam.ExamCourseMapping;
using CoreLayout.Services.Exam.ExamMaster;
using CoreLayout.Services.Masters.Branch;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.CourseBranchMapping;
using CoreLayout.Services.Masters.CourseDetails;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.PCP.PCPAssignedQP;
using CoreLayout.Services.PCP.PCPRegistration;
using CoreLayout.Services.QPDetails.QPMaster;
using CoreLayout.Services.QPDetails.QPType;
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

namespace CoreLayout.Controllers.PCP
{
    [Authorize(Roles = "QPAssign")]
    public class PCPAssignedQPController : Controller
    {
        private readonly ILogger<PCPAssignedQPController> _logger;
        private readonly IPCPAssignedQPService _pCPAssignedQPService;
        private readonly IDataProtector _protector;
        private readonly IQPMasterService _qPMasterService;
        private readonly IPCPRegistrationService _pCPRegistrationService;

        private readonly IQPTypeService _qPTypeService;
        private readonly ICourseService _courseService;
        private readonly IBranchService _branchService;
        private readonly ICourseDetailsService _courseDetailsService;
        private readonly ICourseBranchMappingService _courseBranchMappingService;
        private readonly IExamMasterService _examMasterService;
        private readonly IExamCourseMappingService _examCourseMappingService;

        public PCPAssignedQPController(ILogger<PCPAssignedQPController> logger, IPCPAssignedQPService pCPAssignedQPService,
            IDataProtectionProvider provider, IQPMasterService qPMasterService, IPCPRegistrationService pCPRegistrationService,
            IQPTypeService qPTypeService, ICourseService courseService, IBranchService branchService,
            ICourseDetailsService courseDetailsService, ICourseBranchMappingService courseBranchMappingService,
            IExamMasterService examMasterService, IExamCourseMappingService examCourseMappingService)
        {
            _logger = logger;
            _pCPAssignedQPService = pCPAssignedQPService;
            _protector = provider.CreateProtector("PCPAssignedQP.PCPAssignedQPController");
            _qPMasterService = qPMasterService;
            _pCPRegistrationService = pCPRegistrationService;

            _qPTypeService = qPTypeService;
            _courseService = courseService;
            _branchService = branchService;
            _courseDetailsService = courseDetailsService;
            _courseBranchMappingService = courseBranchMappingService;
            _examMasterService = examMasterService;
            _examCourseMappingService = examCourseMappingService;

        }
        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                //var data = await _pCPAssignedQPService.GetAllPCPAssignedQP();
                var data = (from assignqp in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                            where assignqp.CreatedBy == CreatedBy
                            select assignqp).ToList();

                //start encrypt id for update, delete & details
                foreach (var _data in data)
                {
                    var id = _data.AssignedQPId.ToString();
                    _data.EncryptedId = _protector.Protect(id);
                }
                //end
                //start generate maxid for create button
                int maxid = 0;
                foreach (var _data in data)
                {
                    maxid = _data.AssignedQPId;
                }
                maxid = maxid + 1;
                ViewBag.MaxPCPAssignedQPId = _protector.Protect(maxid.ToString());
                //return View(data);
                return View("~/Views/PCP/PCPAssignedQP/Index.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                return View("~/Views/PCP/PCPAssignedQP/Index.cshtml");
            }
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPAssignedQPService.GetPCPAssignedQPById(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                //return View(data);
                return View("~/Views/PCP/PCPAssignedQP/Details.cshtml", data);
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }


        //Create Get Action Method
        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<ActionResult> CreateAsync(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                PCPAssignedQPModel pCPAssignedQP = new PCPAssignedQPModel();
                ViewBag.UserList = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                    where reg.IsApproved != null
                                    select reg).Distinct().ToList();
                pCPAssignedQP.ExamList = await _examMasterService.GetAllExamMasterAsync();


                return View("~/Views/PCP/PCPAssignedQP/Create.cshtml", pCPAssignedQP);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                return RedirectToAction(nameof(Index));
            }

        }

        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(PCPAssignedQPModel pCPAssignedQPModel)
        {
            try
            {
                //start check qp already assigned to this user
                int result = 0;
                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                var alreadyexit = (from assignqp in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                   where assignqp.CreatedBy == CreatedBy && assignqp.QPId == pCPAssignedQPModel.QPId
                                   select assignqp).ToList();
                foreach (var useid in alreadyexit)
                {
                    foreach (var userid1 in pCPAssignedQPModel.UserList)
                    {
                        if (useid.UserId == userid1)
                        {
                            result = 1;
                        }
                    }
                }
                //end
                ViewBag.UserList = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                    where reg.IsApproved != null
                                    select reg).Distinct().ToList();
                pCPAssignedQPModel.ExamList = await _examMasterService.GetAllExamMasterAsync();


                pCPAssignedQPModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                pCPAssignedQPModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                if (result == 1)
                {
                    ModelState.AddModelError("", "QP already assigned to this user!");
                    return View("~/Views/PCP/PCPAssignedQP/Create.cshtml", pCPAssignedQPModel);
                }
                else if (ModelState.IsValid)
                {
                    var res = await _pCPAssignedQPService.CreatePCPAssignedQPAsync(pCPAssignedQPModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "QPAssigned has been saved";
                    }
                    else
                    {
                        TempData["error"] = "QPAssigned has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Some thing went wrong");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPAssignedQP/Create.cshtml", pCPAssignedQPModel);
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            var guid_id = _protector.Unprotect(id);
            var data = await _pCPAssignedQPService.GetPCPAssignedQPById(Convert.ToInt32(guid_id));
            try
            {
                data.PCPUserList = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                    where reg.IsApproved != null
                                    select reg).Distinct().ToList();
                data.ExamList = await _examMasterService.GetAllExamMasterAsync();

                data.CourseList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                   where examcoursemapping.ExamId == data.ExamId
                                   select examcoursemapping).ToList();

                data.SemYearList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                    where examcoursemapping.CourseID == data.CourseId
                                    select examcoursemapping).ToList();

                data.BranchList = (from coursbranchemapping in await _courseBranchMappingService.GetAllCourseBranchMapping()
                                   where coursbranchemapping.CourseId == data.CourseId
                                   select coursbranchemapping).ToList();


                data.SessionList = (from examcourseemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                    where examcourseemapping.ExamId == data.ExamId
                                    select examcourseemapping).ToList();


                data.QPList = (from qpmaster in await _qPMasterService.GetAllQPMaster()
                               where qpmaster.CourseId == data.CourseId
                               select qpmaster).ToList();
                if (data == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPAssignedQP/Edit.cshtml", data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int AssignedQPId, PCPAssignedQPModel pCPAssignedQPModel)
        {
            try
            {
                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                var alreadyexit = await _pCPAssignedQPService.alreadyAssignedQP(pCPAssignedQPModel.UserId, pCPAssignedQPModel.QPId);


                pCPAssignedQPModel.PCPUserList = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                    where reg.IsApproved != null
                                    select reg).Distinct().ToList();
                pCPAssignedQPModel.ExamList = await _examMasterService.GetAllExamMasterAsync();

                pCPAssignedQPModel.CourseList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                   where examcoursemapping.ExamId == pCPAssignedQPModel.ExamId
                                   select examcoursemapping).ToList();

                pCPAssignedQPModel.SemYearList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                    where examcoursemapping.CourseID == pCPAssignedQPModel.CourseId
                                    select examcoursemapping).ToList();

                pCPAssignedQPModel.BranchList = (from coursbranchemapping in await _courseBranchMappingService.GetAllCourseBranchMapping()
                                   where coursbranchemapping.CourseId == pCPAssignedQPModel.CourseId
                                   select coursbranchemapping).ToList();


                pCPAssignedQPModel.SessionList = (from examcourseemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                    where examcourseemapping.ExamId == pCPAssignedQPModel.ExamId
                                    select examcourseemapping).ToList();


                pCPAssignedQPModel.QPList = (from qpmaster in await _qPMasterService.GetAllQPMaster()
                               where qpmaster.CourseId == pCPAssignedQPModel.CourseId
                               select qpmaster).ToList();

                pCPAssignedQPModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                pCPAssignedQPModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                if (alreadyexit!=null)
                {
                    ModelState.AddModelError("", "QP already assigned to this user!");
                    return View("~/Views/PCP/PCPAssignedQP/Edit.cshtml", pCPAssignedQPModel);
                }
                else if (ModelState.IsValid)
                {
                    var dbRole = await _pCPAssignedQPService.GetPCPAssignedQPById(AssignedQPId);
                    if (await TryUpdateModelAsync<PCPAssignedQPModel>(dbRole))
                    {
                        var res = await _pCPAssignedQPService.UpdatePCPAssignedQPAsync(pCPAssignedQPModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "QPAssigned has been updated";
                        }
                        else
                        {
                            TempData["error"] = "QPAssigned has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View("~/Views/PCP/PCPAssignedQP/Edit.cshtml", pCPAssignedQPModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var dbRole = await _pCPAssignedQPService.GetPCPAssignedQPById(Convert.ToInt32(guid_id));
                if (dbRole != null)
                {
                    var res = await _pCPAssignedQPService.DeletePCPAssignedQPAsync(dbRole);

                    if (res.Equals(1))
                    {
                        TempData["error"] = "QPAssigned has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "QPAssigned has not been deleted";
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

        public async Task<JsonResult> GetCourse(int ExamId)
        {
            var ExamList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                            where examcoursemapping.ExamId == ExamId
                            select new SelectListItem()
                            {
                                Text = examcoursemapping.CourseName,
                                Value = examcoursemapping.CourseID.ToString(),
                            }).ToList();

            ExamList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(ExamList);
        }

        public async Task<JsonResult> GetSemYear(int CourseId)
        {
            var SemYearList = (from examcoursemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                               where examcoursemapping.CourseID == CourseId
                               select new SelectListItem()
                               {
                                   Text = examcoursemapping.SemYearId.ToString(),
                                   Value = examcoursemapping.SemYearId.ToString(),
                               }).ToList();

            SemYearList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(SemYearList);
        }
        public async Task<JsonResult> GetBranch(int CourseId)
        {
            var BranchList = (from coursbranchemapping in await _courseBranchMappingService.GetAllCourseBranchMapping()
                              where coursbranchemapping.CourseId == CourseId
                              select new SelectListItem()
                              {
                                  Text = coursbranchemapping.BranchName,
                                  Value = coursbranchemapping.BranchId.ToString(),
                              }).ToList();

            BranchList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(BranchList);
        }
        public async Task<JsonResult> GetSyllabus(int ExamId)
        {
            var SyllabusList = (from examcourseemapping in await _examCourseMappingService.GetAllExamCourseMappingAsync()
                                where examcourseemapping.ExamId == ExamId
                                select new SelectListItem()
                                {
                                    Text = examcourseemapping.Session,
                                    Value = examcourseemapping.SessionId.ToString(),
                                }).ToList();

            SyllabusList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(SyllabusList);
        }
        public async Task<JsonResult> GetQP(int CourseId)
        {
            var QPList = (from qpmaster in await _qPMasterService.GetAllQPMaster()
                          where qpmaster.CourseId == CourseId
                          select new SelectListItem()
                          {
                              Text = qpmaster.QPCode,
                              Value = qpmaster.QPId.ToString(),
                          }).ToList();

            QPList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(QPList);
        }
        //public async Task<JsonResult> GetSubjectAsync(int CourseId)
        //{
        //    var CourseList = (from qpmaster in await _qPMasterService.GetAllQPMaster()
        //                      join branch in await _courseBranchMappingService.GetAllCourseBranchMapping() on qpmaster.CourseId equals branch.CourseId
        //                      where qpmaster.CourseId == CourseId
        //                      select new SelectListItem()
        //                      {
        //                          Text = branch.BranchName,
        //                          Value = branch.BranchId.ToString(),
        //                      }).Distinct().ToList();
        //    //select new { branch.BranchID,branch.BranchName }).Distinct().ToList();
        //    //CourseList.Insert(0, new SelectListItem()
        //    //{
        //    //    Text = "----Select----",
        //    //    Value = string.Empty
        //    //});
        //    return Json(CourseList);
        //}
    }
}
