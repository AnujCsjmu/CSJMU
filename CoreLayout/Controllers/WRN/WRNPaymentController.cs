using CoreLayout.Models.WRN;
using CoreLayout.Services.WRN.WRNPayment;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.WRN
{
    public class WRNPaymentController : Controller
    {
        private readonly ILogger<WRNPaymentController> _logger;
        private readonly IDataProtector _protector;
        public readonly IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        public readonly IWRNPaymentService _wRNPaymentService;

        public WRNPaymentController(ILogger<WRNPaymentController> logger,
            IDataProtectionProvider provider, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor, IWRNPaymentService wRNPaymentService)
        {
            _logger = logger;
            _protector = provider.CreateProtector("WRNPayment.WRNPaymentController");
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _wRNPaymentService = wRNPaymentService;
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            WRNPaymentModel wRNPaymentModel = new WRNPaymentModel();
            try
            {
                wRNPaymentModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
                wRNPaymentModel.CreatedBy = HttpContext.Session.GetInt32("SessionId");
                var data = await _wRNPaymentService.GetAllWRNPaymentAsync();
                wRNPaymentModel.DataList = data;
                //encryption
                foreach (var _data in data)
                {
                    var stringId = _data.Id.ToString();
                    _data.EncryptedId = _protector.Protect(stringId);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();
            }
            return View("~/Views/WRN/WRNPayment/Payment.cshtml", wRNPaymentModel);
        }

        [HttpPost]
        public async Task<IActionResult> Payment(WRNPaymentModel wRNPaymentModel)
        {
            try
            {
                string ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                wRNPaymentModel.CreatedBy = HttpContext.Session.GetInt32("SessionCreatedBy");
                wRNPaymentModel.IPAddress = ipAddress;
                wRNPaymentModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();
            }
            //bind data

            var binddata = await _wRNPaymentService.GetAllWRNPaymentAsync();
            wRNPaymentModel.DataList = binddata;
            return View("~/Views/WRN/WRNPayment/Payment.cshtml", wRNPaymentModel);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            WRNPaymentModel wRNPaymentModel = new WRNPaymentModel();
            try
            {
                var guid_id = _protector.Unprotect(id);
                var value = await _wRNPaymentService.GetWRNPaymentByIdAsync(Convert.ToInt32(guid_id));
                if (value != null)
                {
                    var res = await _wRNPaymentService.DeleteWRNPaymentAsync(value);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "Data has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "Data has not been deleted";
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
            return RedirectToAction(nameof(Payment));
        }

        public async Task<ActionResult> paymentList(string id)
        {
            //ViewBag.type = 1;
            var guid_id = _protector.Unprotect(id);
            var data = await _wRNPaymentService.GetWRNPaymentByIdAsync(Convert.ToInt32(guid_id));
            //var data = await _wRNQualificationService.GetAllByIdForDetailsAsync(Convert.ToInt32(guid_id));
            if (data == null)
            {
                return NotFound();
            }
            return PartialView("~/Views/WRN/WRNPayment/_QualificationList.cshtml", data);

        }
    }
}
