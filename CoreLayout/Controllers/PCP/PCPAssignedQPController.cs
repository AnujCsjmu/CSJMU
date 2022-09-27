using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Models.PCP;
using CoreLayout.Models.QPDetails;
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

        public PCPAssignedQPController(ILogger<PCPAssignedQPController> logger, IPCPAssignedQPService pCPAssignedQPService,
            IDataProtectionProvider provider, IQPMasterService qPMasterService, IPCPRegistrationService pCPRegistrationService,
            IQPTypeService qPTypeService, ICourseService courseService, IBranchService branchService, ICourseDetailsService courseDetailsService, ICourseBranchMappingService courseBranchMappingService)
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
                pCPAssignedQP.QPList = await _qPMasterService.GetAllQPMaster();
                ViewBag.UserList = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                    where reg.IsApproved != null
                                    select reg).ToList();
                //new
                // pCPAssignedQP.QPTypeList = await _qPTypeService.GetAllQPType();//


                //pCPAssignedQP.CourseList = await _courseService.GetAllCourse();
                //pCPAssignedQP.CourseList = (from qpmaster in await _qPMasterService.GetAllQPMaster()
                //                            join course in await _courseService.GetAllCourse() on qpmaster.CourseId equals course.CourseID
                //                            //where qpmaster.CourseId==course.CourseID
                //                            select course).Distinct().ToList();

                ////pCPAssignedQP.BranchList = await _branchService.GetAllBranch();//subject
                //pCPAssignedQP.BranchList = (from qpmaster in await _qPMasterService.GetAllQPMaster()
                //                            join branch in await _branchService.GetAllBranch() on qpmaster.BranchId equals branch.BranchID
                //                            //where qpmaster.CourseId==course.CourseID
                //                            select branch).Distinct().ToList();

                ////pCPAssignedQP.SessionList = await _courseDetailsService.GetAllSession();//sllabus
                //pCPAssignedQP.SessionList = (from qpmaster in await _qPMasterService.GetAllQPMaster()
                //                             join session in await _courseDetailsService.GetAllSession() on qpmaster.SyllabusId equals session.SessionId
                //                             //where qpmaster.CourseId==course.CourseID
                //                             select session).Distinct().ToList();


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

                pCPAssignedQPModel.QPList = await _qPMasterService.GetAllQPMaster();
                ViewBag.UserList = (from reg in (await _pCPRegistrationService.GetAllPCPRegistration())
                                    where reg.IsApproved != null
                                    select reg).ToList();

                //new 
                //pCPAssignedQPModel.QPTypeList = await _qPTypeService.GetAllQPType();//
                //pCPAssignedQPModel.CourseList = await _courseService.GetAllCourse();
                //pCPAssignedQPModel.BranchList = await _branchService.GetAllBranch();//subject
                //pCPAssignedQPModel.SessionList = await _courseDetailsService.GetAllSession();//sllabus

                pCPAssignedQPModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                //pCPAssignedQPModel.UserId = HttpContext.Session.GetInt32("UserId");
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
                return View(pCPAssignedQPModel);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
                return View();
            }

        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPAssignedQPService.GetPCPAssignedQPById(Convert.ToInt32(guid_id));
                data.QPList = await _qPMasterService.GetAllQPMaster();
                ViewBag.UserList = (from reg in (await _pCPRegistrationService.GetAllPCPRegistration())
                                    where reg.IsApproved != null
                                    select reg).ToList();
                if (data == null)
                {
                    return NotFound();
                }
                //return View(data);
                return View("~/Views/PCP/PCPAssignedQP/Edit.cshtml", data);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(int PCPAssignedQPId, PCPAssignedQPModel pCPAssignedQPModel)
        {
            try
            {
                int result = 0;
                int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
                var alreadyexit = (from assignqp in await _pCPAssignedQPService.GetAllPCPAssignedQP()
                                   where assignqp.CreatedBy == CreatedBy
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
                pCPAssignedQPModel.QPList = await _qPMasterService.GetAllQPMaster();
                ViewBag.UserList = (from reg in (await _pCPRegistrationService.GetAllPCPRegistration())
                                    where reg.IsApproved != null
                                    select reg).ToList();
                pCPAssignedQPModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                pCPAssignedQPModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                //pCPAssignedQPModel.UserId = HttpContext.Session.GetInt32("UserId");
                if (result == 1)
                {
                    ModelState.AddModelError("", "QP already assigned to this user!");
                    return View("~/Views/PCP/PCPAssignedQP/Create.cshtml", pCPAssignedQPModel);
                }
                else if (ModelState.IsValid)
                {
                    var dbRole = await _pCPAssignedQPService.GetPCPAssignedQPById(PCPAssignedQPId);
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
            //return View(pCPAssignedQPModel);
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
