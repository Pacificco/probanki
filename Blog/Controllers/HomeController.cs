using Bankiru.Models.Domain.Articles;
using Bankiru.Models.Domain.Home;
using Bankiru.Models.Infrastructure;
using Bankiru.Models.OutApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public class HomeController : BaseController
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                if (_connected)
                {
                    ArtsManager _manager = new ArtsManager();
                    VM_ArtItem model = _manager.GetCentralArtItem();                    
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = _manager.LastError;                        
                        return RedirectToAction("Error", "Error", null);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return RedirectToAction("Error", "Error", null);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return RedirectToAction("Error", "Error", null);
            }
        }
        [HttpGet]
        public ActionResult Rules()
        {
            return View();
        }
        [HttpGet]                
        public ActionResult Feedback()
        {
            return View(new VM_Feedback());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feedback(VM_Feedback model, HttpPostedFileBase MessageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Session["captcha_code"] != null)
                    {
                        //string captchaCode = Session["captcha_code"].ToString();
                        if (!model.CaptchaCode.ToLower().Equals(Session["captcha_code"].ToString().ToLower()))
                        {
                            ModelState.AddModelError("", "Код с картинки указан не верно!");
                            ModelState.AddModelError("CaptchaCode", "Код с картинки указан не верно!");                            
                            model.CaptchaCode = "";
                            return View(model);
                        }
                    }

                    EmailModel emailModel = new EmailModel();
                    //emailModel.To = "no-reply@probanki.net";
                    emailModel.From = "no-reply@probanki.net";
                    emailModel.Subject = model.Subject;
                    emailModel.Body = "Имя: " + model.Name + "<br />";
                    emailModel.Body += "E-mail: " + model.Email + "<br />";
                    emailModel.Body += "Сообщение: " + model.Message;

                    switch (model.Subject)
                    {
                        case "question":
                            emailModel.To = "info@probanki.net";
                            break;
                        case "support":
                            emailModel.To = "support@probanki.net";
                            break;
                        case "marketing":
                            emailModel.To = "advert@probanki.net";
                            break;
                        case "publication":
                            emailModel.To = "author@probanki.net";
                            break;
                        default:
                            emailModel.To = "info@probanki.net";
                            break;
                    } 
                                                            
                    var mailController = new EmailController();
                    var email = mailController.SendEmailFromFeedback(emailModel, MessageFile);
                    email.Deliver();
                    
                    ViewData["InfoMessage"] = "Ваше сообщение успешно отправлено";
                    return RedirectToAction("FeedbackSuccess");
                }
                else
                {
                    model.CaptchaCode = "";
                    return View(model);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return RedirectToAction("Error", "Error", null);
            }
        }
        [HttpGet]
        public ActionResult FeedbackSuccess()
        {
            ViewData["InfoMessage"] = "Ваше сообщение успешно отправлено!";
            return View();
        }
    }
}
