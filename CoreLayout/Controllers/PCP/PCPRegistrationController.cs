using CoreLayout.Helper;
using CoreLayout.Models.PCP;
using CoreLayout.Services.Masters.Branch;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.CourseBranchMapping;
using CoreLayout.Services.Masters.Institute;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.PCP.PCPRegistration;
using CoreLayout.Services.QPDetails.QPMaster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.PSP
{
    public class PCPRegistrationController : Controller
    {
        private readonly ILogger<PCPRegistrationController> _logger;
        private readonly IPCPRegistrationService _pCPRegistrationService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;
        private readonly CommonController _commonController;
        private static string errormsg = "";
        private readonly IQPMasterService _qPMasterService;
        private readonly ICourseService _courseService;
        private readonly IBranchService _branchService;
        private readonly IInstituteService _instituteService;
        private readonly ICourseBranchMappingService _courseBranchMappingService;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;//for file upload
        public IConfiguration _configuration;

        [Obsolete]
        public PCPRegistrationController(ILogger<PCPRegistrationController> logger, IPCPRegistrationService pCPRegistrationService, IHttpContextAccessor httpContextAccessor, IRoleService roleService, CommonController commonController, IQPMasterService qPMasterService, ICourseService courseService, IBranchService branchService, IInstituteService instituteService, IHostingEnvironment environment, ICourseBranchMappingService courseBranchMappingService, IConfiguration configuration)
        {
            _logger = logger;
            _pCPRegistrationService = pCPRegistrationService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
            _commonController = commonController;
            _qPMasterService = qPMasterService;
            _courseService = courseService;
            _branchService = branchService;
            _instituteService = instituteService;
            hostingEnvironment = environment;
            _courseBranchMappingService = courseBranchMappingService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            PCPRegistrationModel pCPRegistrationModel = new PCPRegistrationModel();
            pCPRegistrationModel.QPCodeList =await _qPMasterService.GetAllQPMaster();
            pCPRegistrationModel.CourseList = await _courseService.GetAllCourse();

            pCPRegistrationModel.BranchList = await _courseBranchMappingService.GetAllCourseBranchMapping();
            return View("~/Views/PCP/PCPRegistration/Registration.cshtml", pCPRegistrationModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Registration(PCPRegistrationModel pCPRegistrationModel)
        {
            try
            {
                string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                //string salt = CreateSalt();
                //string saltedHash = ComputeSaltedHash(pCPRegistrationModel.Password, salt);
                //pCPRegistrationModel.Salt = salt;
                //pCPRegistrationModel.SaltedHash = saltedHash;
                pCPRegistrationModel.IPAddress = ipAddress;
                //registrationModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                pCPRegistrationModel.QPCodeList = await _qPMasterService.GetAllQPMaster();
                pCPRegistrationModel.CourseList = await _courseService.GetAllCourse();
                pCPRegistrationModel.BranchList = await _courseBranchMappingService.GetAllCourseBranchMapping();
                //pCPRegistrationModel.InstituteList = await _instituteService.GetAllInstitute();
                int emailAlreadyExit = _commonController.emailAlreadyExitsPSP(pCPRegistrationModel.EmailID);
                int mobileAlreadyExit = _commonController.mobileAlreadyExitsPSP(pCPRegistrationModel.MobileNo);
                if (emailAlreadyExit == 0 && mobileAlreadyExit == 0)
                {
                    if (pCPRegistrationModel.ProfileImage != null)
                    {
                        var supportedTypes = new[] { "jpg", "JPG", "jpeg", "JPEG", "png", "PNG" };
                        var fileExt = System.IO.Path.GetExtension(pCPRegistrationModel.ProfileImage.FileName).Substring(1);
                        if (!supportedTypes.Contains(fileExt))
                        {
                            ModelState.AddModelError("", "File Extension Is InValid - Only Upload jpg/jpeg/png File");
                            return View("~/Views/PCP/PCPRegistration/Registration.cshtml", pCPRegistrationModel);
                        }
                        else if (pCPRegistrationModel.ProfileImage.Length> 102400)
                        {
                            ModelState.AddModelError("", "Image size must be less than 100kb");
                            return View("~/Views/PCP/PCPRegistration/Registration.cshtml", pCPRegistrationModel);
                        }
                        else
                        {
                            FileHelper fileHelper = new FileHelper();
                            string uploadsFolder = _configuration.GetSection("FilePaths:PreviousDocuments:PCPPhoto").Value.ToString();
                            pCPRegistrationModel.UploadFileName = fileHelper.SaveFile(uploadsFolder, "", pCPRegistrationModel.ProfileImage);
                            //var uniqueFileName = UploadedFile(pCPRegistrationModel);
                            //pCPRegistrationModel.UploadFileName = uniqueFileName;
                            if (pCPRegistrationModel.UploadFileName != null)
                            {
                                if (ModelState.IsValid)
                                {
                                    var res = await _pCPRegistrationService.CreatePCPRegistrationAsync(pCPRegistrationModel);
                                    if (res.Equals(1))
                                    {
                                        ModelState.Clear();
                                        //success
                                        ModelState.AddModelError("", "You are successfully registered for paper setter. After COE approval, You will get userid and password in registered emailid and mobileno");
                                        //errormsg = String.Format("Hello {0}, \n You are successfully registered for paper setter. \n After COE approval, You will get userid and password \n in registered emailid and mobileno", pCPRegistrationModel.UserName);
                                        return await Registration();
                                    }
                                    else
                                    {
                                        //falure
                                        ModelState.AddModelError("", "Data Not Saved!!");
                                    }

                                }
                                else
                                {
                                    ModelState.AddModelError("", "Model state is not valid!");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Issue in file upload");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please select photo!");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Mobile or Email already exits!");
                }
            }
            catch (Exception ex)
            {
                //errormsg = ex.StackTrace.ToString();
                ModelState.AddModelError("", ex.ToString());
                //return RedirectToAction("Registration", "PSPRegistration");
            }
            //return RedirectToAction(nameof(Registration));
            return View("~/Views/PCP/PCPRegistration/Registration.cshtml","PCPRegistration");
        }

        //[Obsolete]
        //private string UploadedFile(PCPRegistrationModel model)
        //{
        //    try
        //    {
        //        string uniqueFileName = null;

        //        if (model.ProfileImage != null)
        //        {
        //            string uploadsFolder = _configuration.GetSection("FilePaths:PreviousDocuments:PCPPhoto").Value.ToString();
        //            //string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "PCPPhoto");
        //            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
        //            //int mobile = (int)HttpContext.Session.GetInt32("UserId");
        //            uniqueFileName = model.MobileNo + "_" + datetime + "_" + model.ProfileImage.FileName;
        //            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //            //if folder not exit
        //            if (!Directory.Exists(System.IO.Path.Combine(hostingEnvironment.WebRootPath, "PCPPhoto")))
        //            {
        //                Directory.CreateDirectory(System.IO.Path.Combine(hostingEnvironment.WebRootPath, "PCPPhoto"));
        //            }
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                model.ProfileImage.CopyTo(fileStream);
        //            }
        //        }
        //        return uniqueFileName;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyMobile(string mobileNo)
        {
            var already = (from data in _pCPRegistrationService.GetAllPCPRegistration().Result
                           where data.MobileNo == mobileNo.Trim()
                           select data).ToList();
            if (already.Count > 0)
            {
                return Json($"{mobileNo} is already in use.");
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmail(string emailID)
        {
            var already = (from data in _pCPRegistrationService.GetAllPCPRegistration().Result
                           where data.EmailID == emailID.Trim()
                           select data).ToList();
            if (already.Count > 0)
            {
                return Json($"{emailID} is already in use.");
            }
            return Json(true);
        }


        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyAadhar(string aadhar)
        {
            var already = (from data in _pCPRegistrationService.GetAllPCPRegistration().Result
                           where data.Aadhar == aadhar.Trim()
                           select data).ToList();
            if (already.Count > 0)
            {
                return Json($"{aadhar} is already in use.");
            }
            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPan(string pan)
        {
            var already = (from data in _pCPRegistrationService.GetAllPCPRegistration().Result
                           where data.PAN == pan.Trim()
                           select data).ToList();
            if (already.Count > 0)
            {
                return Json($"{pan} is already in use.");
            }
            return Json(true);
        }

        public JsonResult GetBranch(int CourseId)
        {
            //var branch = _branchService.GetCourseBranchMappingByIdAsync(CourseId);
            var BranchList = (from brn in _courseBranchMappingService.GetAllCourseBranchMapping().Result
                          where brn.CourseId == CourseId
                          select new SelectListItem()
                               {
                                   Text = brn.BranchName,
                                   Value = brn.BranchId.ToString(),
                               }).ToList();
            return Json(BranchList);
        }
    }
}
