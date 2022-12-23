using CoreLayout.Models.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Controllers.Mail
{
    public class MailController : Controller
    {
        [Obsolete]
        private IHostingEnvironment Environment;
        private IConfiguration Configuration;
        private CommonController commonController;
        [Obsolete]
        public MailController(IHostingEnvironment _environment, IConfiguration _configuration, CommonController _commonController)
        {
            Environment = _environment;
            Configuration = _configuration;
            commonController = _commonController;
        }

        public IActionResult Index()
        {
            //return View(GetCustomerList());
            MailRequest mailRequest = new MailRequest();
            mailRequest.SendMailList = GetCustomerList();
            return View("~/Views/Mail/Index.cshtml", mailRequest);
        }

        [HttpPost]
        [Obsolete]
        public IActionResult Index(MailRequest mailRequest)
        {
            if (mailRequest.Attachments != null)
            {
                string path = Path.Combine(this.Environment.WebRootPath, "MailUploads");
                string fileName = Path.GetFileName(mailRequest.Attachments.FileName);
                string filePath = Path.Combine(path, fileName);
                //file upload
                filesize = 50;
                string us = UploadUserFile(mailRequest.Attachments, fileName, filePath);
                if (us != null)
                {
                    if (us == "File Is Successfully Uploaded")
                    {
                        DataTable excelRead = ReadAndWriteExcel(filePath);
                        if (excelRead.Rows.Count > 0)
                        {
                            InsertDataFromExcel(excelRead);
                        }
                        else
                        {
                            TempData["error"] = "Some thing went wrong in excel read/write";
                            ModelState.AddModelError("", "Some thing went wrong in file upload");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", us);
                    }
                }
                else
                {
                    TempData["error"] = "Some thing went wrong in file upload";
                    ModelState.AddModelError("", "Some thing went wrong in file upload");
                }
            }
            else
            {
                TempData["error"] = "File upload is null/empty";
                ModelState.AddModelError("", "File upload is null/empty");
            }

            mailRequest.SendMailList = GetCustomerList();
            //return View("~/Views/Mail/Index.cshtml", mailRequest);
            return View(mailRequest);
        }

        public string ErrorMessage { get; set; }
        public decimal filesize { get; set; }

        public void InsertDataFromExcel(DataTable dt)
        {
            //Insert the Data read from the Excel file to Database Table.
            string conString = this.Configuration.GetConnectionString("DefaultConnection");
            int CreatedBy = (int)HttpContext.Session.GetInt32("UserId");
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                var transaction = con.BeginTransaction();
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, transaction))
                {
                    //Set the database table name.
                    sqlBulkCopy.BulkCopyTimeout = 0;
                    sqlBulkCopy.BatchSize = 10000;
                    sqlBulkCopy.DestinationTableName = "dbo.BulkMail";

                    //[OPTIONAL]: Map the Excel columns with that of the database table.
                    sqlBulkCopy.ColumnMappings.Add("EmailId", "EmailId");
                    try
                    {
                        sqlBulkCopy.WriteToServer(dt);
                        ModelState.AddModelError("", "File upload successfully");
                        TempData["success"] = "File upload successfully";
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "File not upload");
                        TempData["error"] = "File not upload";
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                    }
                    finally
                    {
                        transaction.Dispose();
                        sqlBulkCopy.Close();
                    }
                }
            }
        }
        public DataTable ReadAndWriteExcel(string filepath)
        {
            DataTable dt = new DataTable();
            try
            {
                //Read the connection string for the Excel file.
                string conString = this.Configuration.GetConnectionString("ExcelConString");

                conString = string.Format(conString, filepath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();

                            TempData["success"] = "Data read successfully";
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Upload Container Should Not Be Empty";
                return dt;
            }
        }

        [Obsolete]
        public string UploadUserFile(IFormFile file, string filename, string filepath)
        {
            try
            {
                var supportedTypes = new[] { "xls", "xlsx" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                {
                    ErrorMessage = "File Extension Is InValid - Only Upload EXCEL File";
                    TempData["error"] = "File Extension Is InValid - Only Upload EXCEL File";
                    return ErrorMessage;
                }
                else if (file.Length > (filesize * 1024))
                {
                    ErrorMessage = "File size Should Be UpTo " + filesize + "KB";
                    TempData["error"] = "File size Should Be UpTo " + filesize + "KB";
                    return ErrorMessage;
                }
                else
                {
                    //Create a Folder.
                    string path = Path.Combine(this.Environment.WebRootPath, "MailUploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //Save the uploaded Excel file.
                    //string fileName = Path.GetFileName(file.FileName);
                    //string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filepath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    ErrorMessage = "File Is Successfully Uploaded";
                    TempData["success"] = "File Is Successfully Uploaded";
                    return ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Upload Container Should Not Be Empty";
                TempData["error"] = "Upload Container Should Not Be Empty";
                return ErrorMessage;
            }
        }
        public List<MailRequest> GetCustomerList()
        {
            List<MailRequest> customers = new List<MailRequest>();
            try
            {
                string connection = this.Configuration.GetConnectionString("DefaultConnection");
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("Select * From BulkMail where MailStatus is null", con);
                con.Open();
                SqlDataReader idr = cmd.ExecuteReader();

                if (idr.HasRows)
                {
                    while (idr.Read())
                    {
                        customers.Add(new MailRequest
                        {
                            //CustId = Convert.ToInt32(idr["customerId"]),
                            ToEmail = Convert.ToString(idr["EmailId"]),
                            Id = Convert.ToInt32(idr["Id"]),
                            CreatedDate = Convert.ToDateTime(idr["CreatedDate"])
                        });
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return customers;
        }

        public List<MailRequest> GetCustomerById(string ids)
        {
            List<MailRequest> customers = new List<MailRequest>();
            try
            {
                string connection = this.Configuration.GetConnectionString("DefaultConnection");
                SqlConnection con = new SqlConnection(connection);
                SqlCommand cmd = new SqlCommand("Select * From BulkMail where MailStatus is null and id in (" + ids + ")", con);
                con.Open();
                SqlDataReader idr = cmd.ExecuteReader();

                if (idr.HasRows)
                {
                    while (idr.Read())
                    {
                        customers.Add(new MailRequest
                        {
                            //CustId = Convert.ToInt32(idr["customerId"]),
                            ToEmail = Convert.ToString(idr["EmailId"]),
                            Id = Convert.ToInt32(idr["Id"]),
                            CreatedDate = Convert.ToDateTime(idr["CreatedDate"])
                        });
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return customers;
        }

        [HttpPost]
        [Obsolete]
        public IActionResult Send(MailRequest mailRequest)
        {
            try
            {
                mailRequest.CreatedBy = HttpContext.Session.GetInt32("UserId");
                int successCounter = 0;
                int failCounter = 0;
                string ids = Request.Form["ids"];
                if (ids != "")
                {
                    var data = GetCustomerById(ids);

                    string connection = this.Configuration.GetConnectionString("DefaultConnection");
                    SqlConnection con = new SqlConnection(connection);
                    if (data.Count > 0)
                    {
                        foreach (var _data in data)
                        {
                            //add mail template
                            string path = Path.Combine(this.Environment.WebRootPath, "MailTemplate");
                            string fileName = "MailTemp.html";
                            string filePath = Path.Combine(path, fileName);
                            StreamReader str = new StreamReader(filePath);
                            string MailText = str.ReadToEnd();
                            str.Close();
                            //Repalce [newusername] = signup user name   
                            MailText = MailText.Replace("[newusername]", "Anuj Nigam");
                            mailRequest.Body = MailText;
                            bool result = commonController.SendMail_Fromcsjmusms(_data.ToEmail, mailRequest.Subject, mailRequest.Body);
                            if (result == true)
                            {
                                //update success mail record
                                SqlCommand cmd = new SqlCommand("update BulkMail set MailSubject='" + mailRequest.Subject + "',MailBody='" + mailRequest.Body + "',MailStatus='Sent',MailSentDate=getdate(),MailSentBy='" + mailRequest.CreatedBy + "' where Id='" + _data.Id + "'", con);
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                                successCounter = successCounter + 1;
                            }
                            else
                            {
                                //update fail mail record
                                SqlCommand cmd = new SqlCommand("update BulkMail set MailSubject='',MailBody='',MailStatus='Not Sent',MailSentBy='" + mailRequest.CreatedBy + "' where Id='" + _data.Id + "'", con);
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                                failCounter = failCounter + 1;
                            }

                        }
                        TempData["success"] = successCounter + " mail send successfully and " + failCounter + " mail failed";
                        ModelState.AddModelError("", successCounter + " mail send successfully and " + failCounter + " mail failed");
                    }
                    else
                    {
                        TempData["error"] = "No record found!";
                        ModelState.AddModelError("", "No record found!");
                    }
                }
                else
                {
                    TempData["error"] = "Select atleast one emailid";
                    ModelState.AddModelError("", "Select atleast one emailid");
                }
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.ToString();
                ModelState.AddModelError("", ex.ToString());
            }
            mailRequest.SendMailList = GetCustomerList();
            return View("~/Views/Mail/Index.cshtml", mailRequest);
            //return View(mailRequest);
        }
    }
}
