﻿using EduHome.App.Services.Interfaces;
using EduHome.Core.Entities;
using System.Net.Mail;
using System.Net;

namespace EduHome.App.Services.Implementations
{
    
    public class MailService :EduHome.App.Services.Interfaces.IMailService
    {
        private readonly IWebHostEnvironment _env;

        public MailService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task Send(string from, string to, string link, string text, string subject)
        {
            string body = string.Empty;
            string path = Path.Combine(_env.WebRootPath,  "templates", "EmailExample.html");
            using (StreamReader SourceReader = System.IO.File.OpenText(path))
            {
                body = SourceReader.ReadToEnd();
            }
            body = body.Replace("{Text}", text);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential("isans@code.edu.az", "iksmbzmpptwimnim");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mail);
        }
    }
}
