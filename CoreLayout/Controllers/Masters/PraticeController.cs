using CoreLayout.Models.Masters;
using CoreLayout.Services.Masters.City;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.Masters.Department;
using CoreLayout.Services.Masters.Pratice;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.Masters.State;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    public class PraticeController : Controller
    {
        private readonly ILogger<PraticeController> _logger;
        private readonly IPraticeService _praticeService;

        private readonly ICityService _cityService;
        private readonly IStateService _stateService;
        private readonly ICountryService _countryService;
        private readonly IDepartmentService _departmentService;
        private readonly IRoleService _roleService;
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;//for file upload

        [Obsolete]
        public PraticeController(ILogger<PraticeController> logger, IPraticeService praticeService, ICityService cityService,
            IStateService stateService, ICountryService countryService, IDepartmentService departmentService, IRoleService roleService, IHostingEnvironment environment)
        {
            _logger = logger;
            _praticeService = praticeService;

            _cityService = cityService;
            _stateService = stateService;
            _countryService = countryService;
            _departmentService = departmentService;
            _roleService = roleService;
            hostingEnvironment = environment;
        }
        public async Task<IActionResult> Index()
        {

                 
            //ViewBag.praticeChildList = v;
            //return ViewResult();
            return View(await _praticeService.GetAllPratice());
        }

        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                return View(await _praticeService.GetPraticeById(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }

        public JsonResult GetState(int CountryId)
        {
            var stateList = (from state in _stateService.GetAllState().Result
                             where state.CountryId == CountryId
                             select new SelectListItem()
                             {
                                 Text = state.StateName,
                                 Value = state.StateId.ToString(),
                             }).ToList();
            stateList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(stateList);
        }

        public JsonResult GetCity(int CountryId, int StateId)
        {
            var cityList = (from city in _cityService.GetAllCity().Result
                            where city.StateId == StateId && city.CountryId == CountryId
                            select new SelectListItem()
                            {
                                Text = city.CityName,
                                Value = city.CityId.ToString(),
                            }).ToList();
            cityList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            return Json(cityList);
        }

        //Create Get Action Method
        public ActionResult Create()
        {
            var CountryList = (from country in _countryService.GetAllCountry().Result
                               select new SelectListItem()
                               {
                                   Text = country.CountryName,
                                   Value = country.CountryId.ToString(),
                               }).ToList();

            CountryList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });

            var DepartmentList = (from department in _departmentService.GetAllDepartment().Result
                                  select new SelectListItem()
                                  {
                                      Text = department.DepartmentName,
                                      Value = department.DepartmentId.ToString(),
                                  }).ToList();

            var RoleList = (from role in _roleService.GetAllRoleAsync().Result
                            select new SelectListItem()
                            {
                                Text = role.RoleName,
                                Value = role.RoleID.ToString(),
                            }).ToList();
            RoleList.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            //List<RoleModel> roleList = _roleService.GetAllRoleAsync().Result.ToList(); 
            //ViewBag.Listofrole = roleList.Select(l => l.Role).ToList();
            ViewBag.RoleList = RoleList;
            ViewBag.CountryList = CountryList;
            ViewBag.DepartmentList = DepartmentList;
            return View();
        }

        [Obsolete]
        private string UploadedFile(PraticeModel model)
        {
            string uniqueFileName = null;

            if (model.ProfileImage != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfileImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create(PraticeModel praticeModel)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {
                if (praticeModel.ProfileImage != null)
                {
                    var supportedTypes = new[] { "jpg", "jpeg", "pdf", "png", "JPG", "JPEG", "PDF", "PNG" };
                    var fileExt = System.IO.Path.GetExtension(praticeModel.ProfileImage.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        ModelState.AddModelError("", "File Extension Is InValid - Only Upload JPG/PDF File");
                    }
                    //else if (file.ContentLength > (filesize * 1024))
                    //{
                    //    red = "File size Should Be UpTo " + filesize + "KB";
                    //    return ErrorMessage;
                    //}
                    else
                    {
                        var uniqueFileName = UploadedFile(praticeModel);
                        praticeModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                        praticeModel.UploadFileName = uniqueFileName;
                        if (ModelState.IsValid && uniqueFileName != null)
                        {
                            var res = await _praticeService.CreatePraticeAsync(praticeModel);
                            if (res > 0)
                            {
                                TempData["success"] = "Pratice has been saved";
                            }
                            else
                            {
                                TempData["error"] = "Pratice has not been saved";
                            }
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                return RedirectToAction("Create");
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("Name") != null)
            {

                var CountryList = (from country in _countryService.GetAllCountry().Result
                                   select new SelectListItem()
                                   {
                                       Text = country.CountryName,
                                       Value = country.CountryId.ToString(),
                                   }).ToList();

                CountryList.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });


                var StateList = (from state in _stateService.GetAllState().Result
                                 select new SelectListItem()
                                 {
                                     Text = state.StateName,
                                     Value = state.StateId.ToString(),
                                 }).ToList();

                StateList.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                var CityList = (from city in _cityService.GetAllCity().Result
                                select new SelectListItem()
                                {
                                    Text = city.CityName,
                                    Value = city.CityId.ToString(),
                                }).ToList();

                CityList.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });

                var DepartmentList = (from department in _departmentService.GetAllDepartment().Result
                                      select new SelectListItem()
                                      {
                                          Text = department.DepartmentName,
                                          Value = department.DepartmentId.ToString(),
                                      }).ToList();

                //DepartmentList.Insert(0, new SelectListItem()
                //{
                //    Text = "----Select----",
                //    Value = string.Empty
                //});

                var RoleList = (from role in _roleService.GetAllRoleAsync().Result
                                select new SelectListItem()
                                {
                                    Text = role.RoleName,
                                    Value = role.RoleID.ToString(),
                                }).ToList();
                RoleList.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                //List<RoleModel> roleList = _roleService.GetAllRoleAsync().Result.ToList(); 
                //ViewBag.Listofrole = roleList.Select(l => l.Role).ToList();
                ViewBag.RoleList = RoleList;
                ViewBag.CountryList = CountryList;
                ViewBag.StateList = StateList;
                ViewBag.CountryList = CityList;
                ViewBag.DepartmentList = DepartmentList;
                return View(await _praticeService.GetPraticeById(id));
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, PraticeModel praticeModel)
        {
            try
            {
                if (HttpContext.Session.GetString("Name") != null)
                {
                    praticeModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                    if (ModelState.IsValid)
                    {
                        var dbCountry = await _praticeService.GetPraticeById(Id);
                        if (await TryUpdateModelAsync<PraticeModel>(dbCountry))
                        {
                            praticeModel.Id = (int)HttpContext.Session.GetInt32("Id");
                            var res = await _praticeService.UpdatePraticeAsync(dbCountry);
                            if (res.ToString().Equals("1"))
                            {
                                TempData["success"] = "Pratice has been updated";
                            }
                            else
                            {
                                TempData["error"] = "Pratice has not been updated";
                            }
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(praticeModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetString("Name") != null)
            {
                try
                {
                    var dbCountry = await _praticeService.GetPraticeById(id);
                    if (dbCountry != null)
                    {
                        var res = await _praticeService.DeletePraticeAsync(dbCountry);

                        if (res.ToString().Equals("1"))
                        {
                            TempData["success"] = "Pratice has been deleted";
                        }
                        else
                        {
                            TempData["error"] = "Pratice has not been deleted";
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
            else
            {
                return RedirectToAction("Login", "Home");

            }
        }
    }
}