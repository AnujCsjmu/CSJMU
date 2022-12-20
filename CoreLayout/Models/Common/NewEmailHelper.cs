using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayout.Models.Common
{
    public class NewEmailHelper
    {
        public static IConfiguration _configuration;

        public NewEmailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #region Private Data
        static string _emailPostURL;
        static string _emailUserId;
        static string _emailPassword;
        static string _emailDomainName;
        static string _emailFrom;
        #endregion

        #region Public Data
        public static string EmailPostURL
        {
            get
            {
                if (string.IsNullOrEmpty(_emailPostURL))
                    _emailPostURL = _configuration.GetSection("NewEmail:PreviousDocuments:BulkEmailPostURL").Value.ToString(); //ConfigurationManager.AppSettings["BulkEmailPostURL"].ToString();
                return _emailPostURL;
            }
            set
            {
                _emailPostURL = value;
            }
        }

        public static string EmailUserId
        {
            get
            {
                if (string.IsNullOrEmpty(_emailUserId))
                    _emailUserId = _configuration.GetSection("NewEmail:PreviousDocuments:BulkEmailUserId").Value.ToString(); //ConfigurationManager.AppSettings["BulkEmailUserId"].ToString();
                return _emailUserId;
            }
            set
            {
                _emailUserId = value;
            }
        }
        public static string EmailPassword
        {
            get
            {
                if (string.IsNullOrEmpty(_emailPassword))
                    _emailPassword = _configuration.GetSection("NewEmail:PreviousDocuments:BulkEmailPassword").Value.ToString();//ConfigurationManager.AppSettings["BulkEmailPassword"].ToString();
                return _emailPassword;
            }
            set
            {
                _emailPassword = value;
            }
        }
        public static string EmailDomainName
        {
            get
            {
                if (string.IsNullOrEmpty(_emailDomainName))
                    _emailDomainName = _configuration.GetSection("NewEmail:PreviousDocuments:BulkEmailDomain").Value.ToString(); //ConfigurationManager.AppSettings["BulkEmailDomain"].ToString();
                return _emailDomainName;
            }
            set
            {
                _emailDomainName = value;
            }
        }

        public static string EmailFrom
        {
            get
            {
                if (string.IsNullOrEmpty(_emailFrom))
                    _emailFrom = _configuration.GetSection("NewEmail:PreviousDocuments:BulkEmailFrom").Value.ToString();//ConfigurationManager.AppSettings["BulkEmailFrom"].ToString();
                return _emailFrom;
            }
            set
            {
                _emailFrom = value;
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiver"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="sendAsync"></param>
        /// 

        #region PostData

        #endregion

        public static bool SendMsg(string receiver, string subject, string body, bool sendAsync)
        {
            bool response = false;
            if (!string.IsNullOrEmpty(receiver))
            {
                StringBuilder APIURL = new StringBuilder();

                //APIURL.Append("https://");
                //APIURL.Append("dlr.tbms.in/EmailAPI.jsp?");

                APIURL.Append(EmailPostURL);

                APIURL.Append("User_ID=");
                APIURL.Append(EmailUserId);
                APIURL.Append("&Password=");
                APIURL.Append(EmailPassword);
                APIURL.Append("&Domain_Name=");
                APIURL.Append(EmailDomainName);

                APIURL.Append("&Email_Subject=");
                APIURL.Append(subject);

                APIURL.Append("&Email_From=");
                APIURL.Append(EmailFrom);

                APIURL.Append("&Display_Name=");
                APIURL.Append("CSJM University");


                APIURL.Append("&Email_Body=");
                APIURL.Append(body);

                APIURL.Append("&Email_To=");
                APIURL.Append(receiver);


                string result = EmailAPICall(APIURL.ToString());

                if (result.Trim() == "200, Request accepted successfully !")
                {
                    response = true;
                }

                //DataRepository.Provider.ExecuteDataSet("cusp_BulkEmailLogs_LogActivity", "TBMS", receiver, subject, body, result);
            }
            return response;
        }

        private static string EmailAPICall(string APIPostContent)
        {
            HttpWebRequest httpreq = (HttpWebRequest)WebRequest.Create(APIPostContent);
            try
            {
                HttpWebResponse httpres = (HttpWebResponse)httpreq.GetResponse();

                StreamReader sr = new StreamReader(httpres.GetResponseStream());

                string results = sr.ReadToEnd();

                sr.Close();
                return results;
            }
            catch
            {
                return "0";
            }
        }
    }
}
