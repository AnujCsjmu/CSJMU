using CoreLayout.Models.WRN;
using CoreLayout.Services.WRN.WRNCourseDetails;
using CoreLayout.Services.WRN.WRNPayment;
using CoreLayout.Services.WRN.WRNQualification;
using CoreLayout.Services.WRN.WRNRegistration;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers
{
    //[Authorize(Roles = "Administrator,Institute,Sub-Institute,Controller Of Examination,Paper Setter,Assistant Registrar,Examination AO,Paper Printing")]
    public class WRNDashBoardController : Controller
    {
        private readonly IDataProtector _protector;
        private readonly ILogger<DashBoardController> _logger;
        public IConfiguration _configuration;
        public readonly IWRNRegistrationService _wRNRegistrationService;
        public readonly IWRNQualificationService _wRNQualificationService;
        public readonly IWRNCourseDetailsService _wRNCourseDetailsService;
        public readonly IWRNPaymentService _wRNPaymentService;
        public WRNDashBoardController(ILogger<DashBoardController> logger, IDataProtectionProvider provider, 
            IConfiguration configuration, IWRNRegistrationService wRNRegistrationService, 
            IWRNQualificationService wRNQualificationService, IWRNCourseDetailsService wRNCourseDetailsService,
            IWRNPaymentService wRNPaymentService)
        {
            _logger = logger;
            _configuration = configuration;
            _protector = provider.CreateProtector("WRNDashBoard.WRNDashBoardController");
            _wRNRegistrationService = wRNRegistrationService;
            _wRNQualificationService = wRNQualificationService;
            _wRNCourseDetailsService = wRNCourseDetailsService;
            _wRNPaymentService = wRNPaymentService;
        }
        [HttpGet]
        public async Task<IActionResult> DashBoardAsync()
        {
            WRNRegistrationModel wRNRegistrationModel = new WRNRegistrationModel();
            string regno = HttpContext.Session.GetString("SessionRegistrationNo");
            string dob = HttpContext.Session.GetString("SessionDOB");
            string mobile = HttpContext.Session.GetString("SessionMobileNo");
            if (regno != null && dob != null && mobile != null)
            {
                var registrationdata = await _wRNRegistrationService.GetWRNRegistrationByLoginAsync(regno, mobile, dob);
                var qualificationdata = (from s in await _wRNQualificationService.GetAllWRNQualificationAsync()
                                         where s.RegistrationNo == regno
                                         select s).Distinct().ToList();
                var coursedata = (from s in await _wRNCourseDetailsService.GetAllWRNCourseDetailsAsync()
                                         where s.RegistrationNo == regno
                                         select s).Distinct().ToList();
                var photosignaturedata = (from s in await _wRNRegistrationService.GetAllWRNRegistrationAsync()
                                  where s.RegistrationNo == regno && s.DOB == dob && s.MobileNo == mobile
                                  && s.PhotoPath !=null && s.SignaturePath !=null
                                  select s).Distinct().ToList();
                var paymentdata = (from s in await _wRNPaymentService.GetAllWRNPaymentAsync()
                                          where s.RegistrationNo == regno
                                          select s).Distinct().ToList();
                var printregistration = (from s in await _wRNRegistrationService.GetAllWRNRegistrationAsync()
                                         where s.RegistrationNo == regno && s.DOB == dob && s.MobileNo == mobile
                                         && s.PrintStatus != null
                                         select s).Distinct().ToList();

                wRNRegistrationModel.RegistrationNo = regno;
                wRNRegistrationModel.DOB = dob;
                wRNRegistrationModel.MobileNo = mobile;

                ViewBag.registrationdata = registrationdata;
                ViewBag.qualificationdata = qualificationdata;
                ViewBag.coursedata = coursedata;
                ViewBag.photosignaturedata = photosignaturedata;
                ViewBag.paymentdata = paymentdata;
                ViewBag.printregistration = printregistration;
            }
            else
            {
                return RedirectToAction("Logout", "WRNRegistration");
            }
            return View("~/Views/WRN/WRNDashboard/Dashboard.cshtml", wRNRegistrationModel);
        }

        [HttpGet]
        public async Task<IActionResult> CompleteRegistrationAsync()
        {
            WRNRegistrationModel wRNRegistrationModel = new WRNRegistrationModel();
            string regno = HttpContext.Session.GetString("SessionRegistrationNo");
            string dob = HttpContext.Session.GetString("SessionDOB");
            string mobile = HttpContext.Session.GetString("SessionMobileNo");
            if (regno != null && dob != null && mobile != null)
            {
                var data = await _wRNRegistrationService.GetWRNRegistrationByLoginAsync(regno, mobile, dob);
                if (data == null)
                {
                    return RedirectToAction("Logout", "WRNRegistration");
                }
                wRNRegistrationModel.RegistrationNo = regno;
                wRNRegistrationModel.DOB = dob;
                wRNRegistrationModel.MobileNo = mobile;
                wRNRegistrationModel.FinalSubmit = data.FinalSubmit;
                wRNRegistrationModel.PhotoPath = data.PhotoPath;
                wRNRegistrationModel.SignaturePath = data.SignaturePath;
            }
            else
            {
                return RedirectToAction("Logout", "WRNRegistration");
            }
            return View("~/Views/WRN/WRNRegistration/CompleteRegistration.cshtml", wRNRegistrationModel);
        }

        [HttpGet]
        public IActionResult Qualification()
        {
            WRNQualificationModel wRNQualificationModel = new WRNQualificationModel();
            wRNQualificationModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
            //var data = await _wRNQualificationService.GetWRNQualificationByIdAsync(regno, mobile, dob);
            //if (data == null)
            //{
            //    return RedirectToAction("Logout", "WRNRegistration");
            //}
            return View("~/Views/WRN/WRNQualification/Qualification.cshtml", wRNQualificationModel);
        }

        public IActionResult UploadPhotoSignature()
        {
            WRNRegistrationModel wRNRegistrationModel = new WRNRegistrationModel();
            wRNRegistrationModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
            return View("~/Views/WRN/WRNRegistration/UploadPhotoSignature.cshtml", wRNRegistrationModel);
        }
        public IActionResult WRNCourse()
        {
            WRNCourseDetailsModel wRNCourseDetailsModel = new WRNCourseDetailsModel();
            wRNCourseDetailsModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
            return View("~/Views/WRN/WRNCourseDetails/WRNCourse.cshtml", wRNCourseDetailsModel);
        }
        public IActionResult Payment()
        {
            WRNPaymentModel wRNPaymentModel = new WRNPaymentModel();
            wRNPaymentModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
            return View("~/Views/WRN/WRNPayment/Payment.cshtml", wRNPaymentModel);
        }
        public IActionResult PrintRegistration()
        {
            WRNRegistrationModel wRNRegistrationModel = new WRNRegistrationModel();
            wRNRegistrationModel.RegistrationNo = HttpContext.Session.GetString("SessionRegistrationNo");
            return View("~/Views/WRN/WRNRegistration/Print.cshtml", wRNRegistrationModel);
        }
    }

}
