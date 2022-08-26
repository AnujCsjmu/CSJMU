using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.Masters.District;
using CoreLayout.Services.Masters.Institute;
using CoreLayout.Services.Masters.InstituteCategory;
using CoreLayout.Services.Masters.InstituteType;
using CoreLayout.Services.Masters.State;
using CoreLayout.Services.Masters.Tehsil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class InstituteController : Controller
    {
        private readonly ILogger<InstituteController> _logger;
        private readonly IStateService _stateService;
        private readonly ICountryService _countryService;
        private readonly IDataProtector _protector;
        private readonly ICommonService _commonService;
        private readonly CommonController _commonController;
        private readonly IDistrictService _districtService;
        private readonly ITehsilService _tehsilService;
        private readonly IInstituteService _instituteService;
        private readonly IInstituteTypeService _instituteTypeService;
        private readonly IInstituteCategoryService _instituteCategoryService;
        [Obsolete]
        private readonly IHostingEnvironment _env;

        //private readonly BaseEntity _baseEntity;
        [Obsolete]
        public InstituteController(ILogger<InstituteController> logger, IStateService stateService, ICountryService countryService, IDataProtectionProvider provider, ICommonService commonService, IHostingEnvironment env, CommonController commonController, IDistrictService districtService, ITehsilService tehsilService, IInstituteService instituteService, IInstituteTypeService instituteTypeService, IInstituteCategoryService instituteCategoryService)
        {
            _logger = logger;
            _stateService = stateService;
            _countryService = countryService;
            _protector = provider.CreateProtector("Institute.InstituteController");
            _commonService = commonService;
            _env = env;
            _commonController = commonController;
            _districtService = districtService;
            _tehsilService = tehsilService;
            _instituteService = instituteService;
            _instituteTypeService = instituteTypeService;
            _instituteCategoryService = instituteCategoryService;
        }

        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update, delete & details
                var institute = await _instituteService.GetAllInstitute();
                foreach (var _institute in institute)
                {
                    var stringId = _institute.InstituteID.ToString();
                    _institute.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int maxinstituteid = 0;
                foreach (var _institute in institute)
                {
                    maxinstituteid = _institute.InstituteID;
                }
                maxinstituteid = maxinstituteid + 1;
                ViewBag.MaxInstituteId = _protector.Protect(maxinstituteid.ToString());
                //end
                return View(institute);

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
                var data = await _instituteService.GetInstituteById(Convert.ToInt32(guid_id));
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

     

        //Create Get Action Method
        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(string id)
        {
            try
            {
                InstituteModel instituteModel = new InstituteModel();
                instituteModel.InstituteTypeList = await _instituteTypeService.GetAllInstituteType();
                instituteModel.InstituteCategoryList = await _instituteCategoryService.GetAllInstituteCategory();
                instituteModel.StateList = await _stateService.GetAllState();
                var guid_id = _protector.Unprotect(id);
                return View(instituteModel);
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
        public async Task<IActionResult> Create(InstituteModel instituteModel)
        {
            instituteModel.InstituteTypeList = await _instituteTypeService.GetAllInstituteType();
            instituteModel.InstituteCategoryList = await _instituteCategoryService.GetAllInstituteCategory();
            instituteModel.StateList = await _stateService.GetAllState();
            instituteModel.DistrictList = await _districtService.GetAllDistrict();
            instituteModel.TehsilList = await _tehsilService.GetAllTehsil();
            instituteModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            instituteModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            if (ModelState.IsValid)
            {
                int alradyExits = AlreadyExits(instituteModel.InstituteCode.Trim());
                if (alradyExits == 0)
                {
                    var res = await _instituteService.CreateInstituteAsync(instituteModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Institute has been saved";
                    }
                    else
                    {
                        TempData["error"] = "Institute has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Institute Code already exits");
                }
            }
            return View(instituteModel);
        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _instituteService.GetInstituteById(Convert.ToInt32(guid_id));
                data.InstituteTypeList = await _instituteTypeService.GetAllInstituteType();
                data.InstituteCategoryList = await _instituteCategoryService.GetAllInstituteCategory();
                data.StateList = await _stateService.GetAllState();
                //data.DistrictList = await _districtService.GetAllDistrict();
                //data.TehsilList = await _tehsilService.GetAllTehsil();

                JsonResult obj = GetDistrict(data.StateId);
               
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
        public async Task<IActionResult> Edit(int InstituteId, InstituteModel instituteModel)
        {
            try
            {
                instituteModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                instituteModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var dbState = await _instituteService.GetInstituteById(InstituteId);
                    if (await TryUpdateModelAsync<InstituteModel>(dbState))
                    {
                        var res = await _instituteService.UpdateInstituteAsync(instituteModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "Institute has been updated";
                        }
                        else
                        {
                            TempData["error"] = "Institute has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(instituteModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _instituteService.GetInstituteById(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _instituteService.DeleteInstituteAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Intitute has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Intitute has not been deleted";
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
        public int AlreadyExits(string institutecode)
        {
            int result = 0;
            try
            {
                var already = (from institute in _instituteService.GetAllInstitute().Result
                               where institute.InstituteCode == institutecode
                               select new SelectListItem()
                               {
                                   Text = institute.InstituteCode,
                                   Value = institute.InstituteID.ToString(),
                               }).ToList();
                if (already.Count > 0)
                {
                    result = 1;
                }
            }
            catch (Exception)
            {
                result = 2;
            }
            return result;
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
    }
}
