using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Company.WorkflowSystem.Domain.Interfaces
{
    public interface IEmailService
    {
        Task<(bool success, string message)> SendEmail(string from, string to, string subject, string content, bool contentIsHtml = true, string cc = "", string bcc = "");
    }
}
