using CoreLayout.Enum;
using CoreLayout.Filters;
using CoreLayout.Models.Masters;
using CoreLayout.Models.PCP;
using CoreLayout.Models.QPDetails;
using CoreLayout.Services.Masters.Role;
using CoreLayout.Services.PCP.PCPAssignedQP;
using CoreLayout.Services.PCP.PCPRegistration;
using CoreLayout.Services.QPDetails.QPMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public PCPAssignedQPController(ILogger<PCPAssignedQPController> logger, IPCPAssignedQPService pCPAssignedQPService, IDataProtectionProvider provider, IQPMasterService qPMasterService, IPCPRegistrationService pCPRegistrationService)
        {
            _logger = logger;
            _pCPAssignedQPService = pCPAssignedQPService;
            _protector = provider.CreateProtector("PCPAssignedQP.PCPAssignedQPController");
            _qPMasterService = qPMasterService;
            _pCPRegistrationService = pCPRegistrationService;
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
             
                var data = await _pCPAssignedQPService.GetAllPCPAssignedQP();

                //get how much qp assign to the user
                List<PCPAssignedQPModel> pCPAssignedQPModels = new List<PCPAssignedQPModel>();
                List<List<PCPAssignedQPModel>> qPMasterModels = new List<List<PCPAssignedQPModel>>();
                PCPAssignedQPModel pCPAssignedQPModel = new PCPAssignedQPModel();
                foreach (var _data in data)
                {
                    List<PCPAssignedQPModel> qpmodelt = new List<PCPAssignedQPModel>();
                    string id = _data.PCPRegID.ToString();
                    var data1 = await _pCPAssignedQPService.GetAllQPByPCPRegIdAsync(Convert.ToInt32(id));
                    foreach (var _data1 in data1)
                    {
                        qpmodelt.Add(_data1);
                    }
                    qPMasterModels.Add(qpmodelt);
                }
                ViewBag.QPList = qPMasterModels;
                //end

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
            }
            //return View();
            return View("~/Views/PCP/PCPAssignedQP/Index.cshtml");
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
                ViewBag.QPList = await _qPMasterService.GetAllQPMaster();
                // pCPAssignedQP.QPList = await _qPMasterService.GetAllQPMaster();
                //pCPAssignedQP.UserList = await _pCPRegistrationService.GetAllPCPRegistration();
                pCPAssignedQP.UserList = (from reg in (await _pCPRegistrationService.GetAllPCPRegistration())
                                          where reg.IsApproved != null
                                          select reg).ToList();
                // return View(pCPAssignedQP);
                return View("~/Views/PCP/PCPAssignedQP/Create.cshtml", pCPAssignedQP);
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
        public async Task<IActionResult> Create(PCPAssignedQPModel pCPAssignedQPModel)
        {
            try
            {
                ViewBag.QPList = await _qPMasterService.GetAllQPMaster();
                //pCPAssignedQPModel.QPList = await _qPMasterService.GetAllQPMaster();
                // pCPAssignedQPModel.UserList = await _pCPRegistrationService.GetAllPCPRegistration();
                pCPAssignedQPModel.UserList = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
                                               where reg.IsApproved != null
                                               select reg).ToList();
                pCPAssignedQPModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
                pCPAssignedQPModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                if (ModelState.IsValid)
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
            }
            return View();
        }
        [HttpGet]
        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _pCPAssignedQPService.GetPCPAssignedQPById(Convert.ToInt32(guid_id));
                ViewBag.QPList = await _qPMasterService.GetAllQPMaster();
                //data.QPList = await _qPMasterService.GetAllQPMaster();
                //data.UserList = await _pCPRegistrationService.GetAllPCPRegistration();
                data.UserList = (from reg in await _pCPRegistrationService.GetAllPCPRegistration()
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
                ViewBag.QPList = await _qPMasterService.GetAllQPMaster();
                //pCPAssignedQPModel.QPList = await _qPMasterService.GetAllQPMaster();
                //pCPAssignedQPModel.UserList = await _pCPRegistrationService.GetAllPCPRegistration();
                pCPAssignedQPModel.UserList = (from reg in (await _pCPRegistrationService.GetAllPCPRegistration())
                                               where reg.IsApproved != null
                                               select reg).ToList();
                pCPAssignedQPModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                pCPAssignedQPModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                if (ModelState.IsValid)
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
    }
}
