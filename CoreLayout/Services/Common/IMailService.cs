using CoreLayout.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLayout.Services.Common
{
    public interface IMailService
    {
        Task<string> SendEmailAsync(MailRequest mailRequest);
    }
}
