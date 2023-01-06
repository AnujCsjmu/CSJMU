using CoreLayout.Models.Masters;
using CoreLayout.Services.Common;
using CoreLayout.Services.Masters.Country;
using CoreLayout.Services.Masters.State;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using CoreLayout.Models.Common;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using CoreLayout.Filters;
using CoreLayout.Enum;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CoreLayout.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class StateController : Controller
    {
        private readonly ILogger<StateController> _logger;
        private readonly IStateService _stateService;
        private readonly ICountryService _countryService;
        private readonly IDataProtector _protector;
        private readonly ICommonService _commonService;
        private readonly CommonController _commonController;
        [Obsolete]
        private readonly IHostingEnvironment _env;

        //private readonly BaseEntity _baseEntity;
        [Obsolete]
        public StateController(ILogger<StateController> logger, IStateService stateService, ICountryService countryService, IDataProtectionProvider provider, ICommonService commonService, IHostingEnvironment env, CommonController commonController)
        {
            _logger = logger;
            _stateService = stateService;
            _countryService = countryService;
            _protector = provider.CreateProtector("State.StateController");
            _commonService = commonService;
            _env = env;
            _commonController = commonController;
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                //start encrypt id for update,delete & details
                var state = await _stateService.GetAllState();
                foreach (var _state in state)
                {
                    var stringId = _state.StateId.ToString();
                    _state.EncryptedId = _protector.Protect(stringId);
                }
                //end

                //start generate maxid for create button
                int maxstateid = 0;
                foreach (var _states in state)
                {
                    maxstateid = _states.StateId;
                }
                maxstateid = maxstateid + 1;
                ViewBag.MaxStateId = _protector.Protect(maxstateid.ToString());
                //end
                return View(state);
                //return RedirectToAction("Index", "State", new { area = "Admin" });
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
                var data = await _stateService.GetStateById(Convert.ToInt32(guid_id));
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

        //public void BindCountry()
        //{
        //    var countryList = (from country in _countryService.GetAllCountry().Result
        //                       select new SelectListItem()
        //                       {
        //                           Text = country.CountryName,
        //                           Value = country.CountryId.ToString(),
        //                       }).ToList();

        //    countryList.Insert(0, new SelectListItem()
        //    {
        //        Text = "----Select----",
        //        Value = string.Empty
        //    });
        //    ViewBag.CountryList = countryList;//roleList.Select(l => l.CountryId).ToList();
        //}

        //Create Get Action Method
        [HttpGet]
        [AuthorizeContext(ViewAction.Add)]
        public async Task<IActionResult> Create(string id)
        {
            try
            {
                StateModel stateModel = new StateModel();
                stateModel.CountryList = await _countryService.GetAllCountry();
                var guid_id = _protector.Unprotect(id);
                return View(stateModel);
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
        public async Task<IActionResult> Create(StateModel stateModel)
        {
            stateModel.CountryList = await _countryService.GetAllCountry();
            stateModel.CreatedBy = HttpContext.Session.GetInt32("UserId");
            stateModel.IPAddress = HttpContext.Session.GetString("IPAddress");
            if (ModelState.IsValid)
            {
                int alradyExits = AlreadyExits(stateModel.StateName.Trim());
                if (alradyExits == 0)
                {
                    var res = await _stateService.CreateStateAsync(stateModel);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "State has been saved";
                    }
                    else
                    {
                        TempData["error"] = "State has not been saved";
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "State Name already exits");
                }
            }
            return View(stateModel);

        }

        [AuthorizeContext(ViewAction.Edit)]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var data = await _stateService.GetStateById(Convert.ToInt32(guid_id));
                data.CountryList = await _countryService.GetAllCountry();
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
        public async Task<IActionResult> Edit(int StateId, StateModel stateModel)
        {
            try
            {
                stateModel.IPAddress = HttpContext.Session.GetString("IPAddress");
                stateModel.ModifiedBy = HttpContext.Session.GetInt32("UserId");
                if (ModelState.IsValid)
                {
                    var dbState = await _stateService.GetStateById(StateId);
                    if (await TryUpdateModelAsync<StateModel>(dbState))
                    {
                        //stateModel.StateId = (int)HttpContext.Session.GetInt32("Id");
                        //stateModel.CountryId = Convert.ToInt32(Request.Form["CountryId"]);//get country id from form collection
                        var res = await _stateService.UpdateStateAsync(stateModel);
                        if (res.Equals(1))
                        {
                            TempData["success"] = "State has been updated";
                        }
                        else
                        {
                            TempData["error"] = "State has not been updated";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.ToString());
            }
            return View(stateModel);
        }

        [HttpGet]
        [AuthorizeContext(ViewAction.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var guid_id = _protector.Unprotect(id);
                var dbState = await _stateService.GetStateById(Convert.ToInt32(guid_id));
                if (dbState != null)
                {
                    var res = await _stateService.DeleteStateAsync(dbState);
                    if (res.Equals(1))
                    {
                        TempData["success"] = "State has been deleted";
                    }
                    else
                    {
                        TempData["error"] = "State has not been deleted";
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
        public int AlreadyExits(string name)
        {
            int result = 0;
            try
            {
                var already = (from state in _stateService.GetAllState().Result
                               where state.StateName == name
                               select new SelectListItem()
                               {
                                   Text = state.StateName,
                                   Value = state.StateId.ToString(),
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


        public FileResult CreatePdf()
        {
            //var stateData = _stateService.GetAllState().Result;
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            string strPDFFileName = string.Format("CustomerDetailPdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0, 0, 0, 0);
            PdfPTable pdfPTable = new PdfPTable(4);
            doc.SetMargins(10, 10, 10, 0);
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fontInvoice = new iTextSharp.text.Font(bf, 20, iTextSharp.text.Font.UNDERLINE);
            iTextSharp.text.Font fontInvoice1 = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.ITALIC);
            Paragraph paragraph = new Paragraph("State Detail", fontInvoice);
            Paragraph paragraph1 = new Paragraph("Basic Detail", fontInvoice1);
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph1.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);
            doc.Add(paragraph1);
            Paragraph p3 = new Paragraph();
            p3.SpacingAfter = 6;
            doc.Add(p3);
            doc.Add(Add_Content_To_PDF(pdfPTable));
            doc.Close();
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            return File(workStream, "application/pdf", strPDFFileName);
        }
        protected PdfPTable Add_Content_To_PDF(PdfPTable pdfPTable)
        {
            float[] headers = { 24, 45, 20, 35 }; //Header Widths  
            pdfPTable.SetWidths(headers); //Set the pdf headers  
            pdfPTable.WidthPercentage = 100; //Set the PDF File witdh percentage  
            pdfPTable.HeaderRows = 1;
            var count = 1;
            //Add header  
            AddCellToHeader(pdfPTable, "CountryName");
            AddCellToHeader(pdfPTable, "StateName");
            AddCellToHeader(pdfPTable, "CreatedBy");
            AddCellToHeader(pdfPTable, "CreatedDate");
            var customerData = _stateService.GetAllState().Result;
            foreach (var state in customerData)
            {
                if (count >= 1)
                {
                    //Add body  
                    AddCellToBody(pdfPTable, state.CountryName.ToString(), count);
                    AddCellToBody(pdfPTable, state.StateName.ToString(), count);
                    AddCellToBody(pdfPTable, state.CreatedBy.ToString(), count);
                    AddCellToBody(pdfPTable, state.CreatedDate.ToString(), count);
                    count++;
                }
            }
            return pdfPTable;
        }
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.HELVETICA, 8, 1, BaseColor.Black)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 8,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }
        private static void AddCellToBody(PdfPTable tableLayout, string cellText, int count)
        {
            if (count % 2 == 0)
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.HELVETICA, 8, 1, iTextSharp.text.BaseColor.Black)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
                });
            }
            else
            {
                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.HELVETICA, 8, 1, iTextSharp.text.BaseColor.Black)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 8,
                    BackgroundColor = new iTextSharp.text.BaseColor(211, 211, 211)
                });
            }
        }
    }
}