using ActionMailer.Net.Mvc;
using Bankiru.Models.Domain.Account;
using Bankiru.Models.Domain.Other;
using Bankiru.Models.OutApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public class EmailController : MailerBase
    {
        [ChildActionOnly]
        public EmailResult SendEmailFromFeedback(EmailModel model, HttpPostedFileBase file)
        {
            To.Add(model.To);
            From = model.From;
            Subject = model.Subject;
            MessageEncoding = Encoding.UTF8;

            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);
                byte[] fileData;
                using (var reader = new BinaryReader(file.InputStream, Encoding.UTF8))
                {
                    fileData = reader.ReadBytes(file.ContentLength);
                }
                int i = fileName.LastIndexOf(".");
                string attachName = fileName;
                if (i != -1)
                {
                    attachName = TransliterationManager.Front(fileName.Substring(0, i));
                    attachName += fileName.Substring(i);
                }
                Attachments.Add(attachName, fileData);
            }
            switch (model.Subject)
            {
                case "question":
                    Subject = "Общие вопросы";
                    return Email("_emailInfo", model);
                case "support":
                    Subject = "Техническая поддержка";
                    return Email("_emailSupport", model, null, true);
                case "marketing":
                    Subject = "По вопросам размещения рекламы";
                    return Email("_emailMarketing", model);
                case "publication":
                    Subject = "От авторов";
                    return Email("_emailPublication", model);
                default:
                    return null;
            }
        }
        [ChildActionOnly]
        public EmailResult SendEmailRegister(EmailModel model, VM_UserEmailConfirmed user)
        {
            To.Add(model.To);
            From = model.From;
            Subject = model.Subject;
            MessageEncoding = Encoding.UTF8;
            Subject = "Регистрация на ProBanki.net";
            return Email("_emailRegister", user);
        }
        [ChildActionOnly]
        public EmailResult SendEmailPasswordRecover(EmailModel model, VM_UserEmailConfirmed user)
        {
            To.Add(model.To);
            From = model.From;
            Subject = model.Subject;
            MessageEncoding = Encoding.UTF8;
            Subject = "Восстановление пароля";
            return Email("_emailPasswordRecover", user);
        }
    }
}
