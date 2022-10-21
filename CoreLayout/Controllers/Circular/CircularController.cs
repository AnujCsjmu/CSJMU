using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Helper;
using CoreLayout.Models.Circular;
using CoreLayout.Services.Circular;
using CoreLayout.Services.Masters.Course;
using CoreLayout.Services.Masters.District;
using CoreLayout.Services.Masters.Institute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.Circular
{
    //[Authorize(Roles = "Administrator")]
    public class CircularController : Controller
    {
        private readonly ILogger<CircularController> _logger;
        private readonly IDataProtector _protector;
        private readonly ICircularService _circularService;
        private readonly IDistrictService _districtService;
        private readonly ICourseService _courseService;
        private readonly IInstituteService _instituteService;
        public readonly IConfiguration _configuration;
        public CircularController(ILogger<CircularController> logger, IDataProtectionProvider provider, ICircularService circularService, IDistrictService districtService, ICourseService courseService, IInstituteService instituteService, IConfiguration configuration)
        {
            _logger = logger;
            _circularService = circularService;
            _protector = provider.CreateProtector("Circular.CircularController");
            _districtService = districtService;
            _courseService = courseService;
            _instituteService = instituteService;
            _configuration = configuration;
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var data = await _circularService.GetAllCircular();
                foreach (var _data in data)
                {
                    var stringId = _data.CircularId.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int id = 0;
                foreach (var _data in data)
                {
                    id = _data.CircularId;
                }
                id = id + 1;
                ViewBag.MaxCircularId = _protector.Protect(id.ToString());
                //end
                return View(data);

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View();
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Details)]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _circularService.GetCircularById(Convert.ToInt32(guid_id));
                data.EncryptedId = id;
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);

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
                CircularModel circularModel = new CircularModel();
                circularModel.DistrictList = await _districtService.Get7DistrictAsync();
                circularModel.CourseList = await _courseService.GetAllCourse();
                //circularModel.InstituteList = await _instituteService.GetAllInstitute();
                var guid_id = _protector.Unprotect(id);
                return View(circularModel);
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
        public async Task<IActionResult> Create(CircularModel circularModel)
        {
            FileHelper fileHelper = new FileHelper();
            string CircularPath = _configuration.GetSection("FilePaths:PreviousDocuments:Circular").Value.ToString();
            try
            {
                circularModel.DistrictList = await _districtService.Get7DistrictAsync();
                circularModel.CourseList = await _courseService.GetAllCourse();
                //circularModel.InstituteList = await _instituteService.GetAllInstitute();
                circularModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                circularModel.IPAddress = HttpContext.Session.GetString("IPAddress");

                #region upload circular
                if (circularModel.FUCircular != null)
                {
                    var supportedTypes = new[] { "pdf", "PDF" };// "jpg", "JPG", "jpeg", "JPEG", "png", "PNG"
                    var circularExt = Path.GetExtension(circularModel.FUCircular.FileName).Substring(1);
                    if (supportedTypes.Contains(circularExt))
                    {
                        if (circularModel.FUCircular.Length < 2100000)
                        {
                            circularModel.CircularPath = fileHelper.SaveFile(CircularPath, "", circularModel.FUCircular);
                        }
                        else
                        {
                            ModelState.AddModelError("", "previous Paper size must be less than 2 mb");
                            return View(circularModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "previous paper extension is invalid- accept only pdf");//,jpg,jpeg,png
                        return View(circularModel);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please upload previous paper");
                    return View(circularModel);
                }
                #endregion
                if (ModelState.IsValid)
                {
                    var res = await _circularService.CreateCircularAsync(circularModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Circular has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Circular has not been saved";
                        fileHelper.DeleteFileAnyException(CircularPath, circularModel.CircularPath);
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    fileHelper.DeleteFileAnyException(CircularPath, circularModel.CircularPath);
                    ModelState.AddModelError("", "Model state is not valid");
                    return View(circularModel);
                }
            }
            catch (Exception ex)
            {
                fileHelper.DeleteFileAnyException(CircularPath, circularModel.CircularPath);
                ModelState.AddModelError("", ex.ToString());
                return View(circularModel);
            }
        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                
                var guid_id = _protector.Unprotect(id);
                var data = await _circularService.GetCircularById(Convert.ToInt32(guid_id));
                data.DistrictList = await _districtService.Get7DistrictAsync();
                data.CourseList = await _courseService.GetAllCourse();
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
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
        public async Task<IActionResult> Edit(int CircularId, CircularModel circularModel)
        {
            FileHelper fileHelper = new FileHelper();
            string CircularPath = _configuration.GetSection("FilePaths:PreviousDocuments:Circular").Value.ToString();
            try
            {
                circularModel.DistrictList = await _districtService.Get7DistrictAsync();
                circularModel.CourseList = await _courseService.GetAllCourse();
                circularModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                circularModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                #region upload circular
                if (circularModel.FUCircular != null)
                {
                    var supportedTypes = new[] { "pdf", "PDF" };// "jpg", "JPG", "jpeg", "JPEG", "png", "PNG"
                    var circularExt = Path.GetExtension(circularModel.FUCircular.FileName).Substring(1);
                    if (supportedTypes.Contains(circularExt))
                    {
                        if (circularModel.FUCircular.Length < 2100000)
                        {
                            circularModel.CircularPath = fileHelper.SaveFile(CircularPath, "", circularModel.FUCircular);
                        }
                        else
                        {
                            ModelState.AddModelError("", "previous Paper size must be less than 2 mb");
                            return View(circularModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "previous paper extension is invalid- accept only pdf");//,jpg,jpeg,png
                        return View(circularModel);
                    }
                }
               
                #endregion
                if (ModelState.IsValid)
                {
                    var value = await _circularService.GetCircularById(CircularId);
                    if (await TryUpdateModelAsync<CircularModel>(value))
                    {
                        var res = await _circularService.UpdateCircularAsync(circularModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Circular has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Circular has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(circularModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _circularService.GetCircularById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _circularService.DeleteCircularAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Circular has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Circular has not been deleted";
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

  

        public async Task<JsonResult> GetInstitute(string districtId, string courseid)
        {
            var InstituteList = (dynamic)null;

            if (districtId != null && courseid == null)
            {
                List<string> districtlist = districtId.Split(',').ToList();
                InstituteList = (from institute in await _instituteService.GetAllInstitute()
                                 where districtlist.Contains(institute.DistrictID.ToString())
                                 select new SelectListItem()
                                 {
                                     Text = institute.InstituteCode + " - " + institute.InstituteName,
                                     Value = institute.InstituteID.ToString(),
                                 }).ToList();
            }
            else if (districtId != null && courseid == null)
            {
                List<string> courselist = courseid.Split(',').ToList();
                InstituteList = (from institute in await _instituteService.AffiliationInstituteIntakeData()
                                 join b in await _instituteService.GetAllInstitute()
                                 on institute.InstituteID equals b.InstituteID
                                 where courselist.Contains(institute.CourseId.ToString())
                                 select new SelectListItem()
                                 {
                                     Text = b.InstituteCode + " - " + b.InstituteName,
                                     Value = b.InstituteID.ToString(),
                                 }).Distinct().ToList();
            }
            else if (districtId != null && courseid != null)
            {
                List<string> districtlist = districtId.Split(',').ToList();
                List<string> courselist = courseid.Split(',').ToList();
                InstituteList = (from institute in await _instituteService.AffiliationInstituteIntakeData()
                                 join b in await _instituteService.GetAllInstitute()
                                 on institute.InstituteID equals b.InstituteID
                                 where districtlist.Contains(b.DistrictID.ToString()) && courselist.Contains(institute.CourseId.ToString())
                                 select new SelectListItem()
                                 {
                                     Text = b.InstituteCode + " - " + b.InstituteName,
                                     Value = b.InstituteID.ToString(),
                                 }).Distinct().ToList();
            }
            return Json(InstituteList);
        }


        public async Task<IActionResult> ViewFile(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _circularService.GetCircularById(Convert.ToInt32(guid_id));

                if (data == null)
                {
                    return NotFound();
                }
                else
                {

                    #region file download
                    var path = string.Empty;
                    var ext = string.Empty;

                    string CircularDocument = _configuration.GetSection("FilePaths:PreviousDocuments:Circular").Value.ToString();
                    path = Path.Combine(CircularDocument, data.CircularPath);
                    ext = Path.GetExtension(data.CircularPath).Substring(1);

                    string ReportURL = path;
                    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
                    if (ext == "png" || ext == "PNG")
                    {
                        return File(FileBytes, "image/png");
                    }
                    else if (ext == "jpg" || ext == "JPG")
                    {
                        return File(FileBytes, "image/jpg");
                    }
                    else if (ext == "jpeg" || ext == "JPEG")
                    {
                        return File(FileBytes, "image/jpeg");
                    }
                    else
                    {
                        return File(FileBytes, "application/pdf");
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }
    }
}
