using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public class LocalMailService : IMailService
    {
        private string _from=Startup.Configuration["mailSettings:mailFrom"];
        private string _to= Startup.Configuration["mailSettings:mailTo"];
        public void Send(string subject, string message)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Mail sent from {_from} to {_to} with LocalMailService");
            sb.AppendLine($"Subject: {subject}");
            sb.AppendLine($"Message: {message}");
            Debug.WriteLine(sb.ToString());
        }
    }
}
