using Bankiru.Controllers;
using Bankiru.Models.Domain.Images;
using Bankiru.Models.Infrastructure;
using Bankiru.Models.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class ImagesController : BaseController
    {
        [HttpGet]
        public ActionResult Index(string path = null)
        {            
            try
            {
                if (_connected)
                {
                    ImageManager _manager = new ImageManager();
                    string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Images\\";                    
                    VM_ImageFolder model = _manager.GetFolder(folderPath, path);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(List<HttpPostedFileBase> uploads, string path = null)
        {
            try
            {
                if (_connected)
                {
                    ImageManager _manager = new ImageManager();
                    string folderPath = AppDomain.CurrentDomain.BaseDirectory + "Images\\";
                    if (uploads != null && uploads.Count > 0)
                    {
                        string err = "";
                        foreach (HttpPostedFileBase file in uploads)
                        {
                            if (!_manager.CreateFile(file, folderPath + (String.IsNullOrEmpty(path) ? "" : path)))
                            {
                                err += _manager.LastError + "<br />";
                            }
                        }
                        ViewBag.ErrorMessage = err;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "";
                    }
                    return RedirectToAction("Index", new { path = path });
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
        public ActionResult Show(string fileName)
        {
            try
            {
                if (_connected)
                {
                    VM_ImageFile model = new VM_ImageFile();                    
                    if (fileName.IndexOf("\\") == -1)
                    {
                        model.FileName = fileName;
                        model.FilePath = "";
                    }
                    else
                    {
                        model.FileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                        model.FilePath = fileName.Substring(0, fileName.LastIndexOf("\\") + 1);
                    }
                    if(System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Images\\" + model.FilePath + model.FileName))
                    {
                        return View(model);
                    }
                    else
                    {
                        return View();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewFolder(VM_Folder folder)
        {
            string folderPath = String.Empty;
            try
            {
                if (String.IsNullOrEmpty(folder.Name))
                {
                    ViewBag.WarningMessage = "Вы не указали название новой категории!";
                }
                else
                {
                    folderPath = Server.MapPath("\\") +  "Images\\";
                    //folderPath = Dir.FullName + "Images\\";
                    if (!String.IsNullOrEmpty(folder.Path))
                        folderPath += folder.Path.Trim('\\') + "\\";
                    folderPath += folder.Name;
                    //DirectoryInfo Dir = new DirectoryInfo(Request.MapPath(".") + folderPath);
                    //Dir.CreateSubdirectory(folder.Name);
                    //Directory.CreateDirectory(folderPath);

                    //Directory.CreateDirectory(Server.MapPath("\\") +  "Images\\test");
                    Directory.CreateDirectory(folderPath);
                }
                //return RedirectToAction("Index", null, new { path = String.IsNullOrEmpty(folder.Path) ? null : folder.Path.Trim('\\') });
                return RedirectToAction("Index", null, null );
            }
            catch (Exception e)
            {
                //ViewBag.ErrorMessage = e.ToString();
                //ViewData["ErrorMessage"] = folderPath + "<br />" + e.ToString();
                return RedirectToAction("Error", "Error", new { err_message = e.ToString() });
            }
        }

        #region ДОЧЕРНИЕ МЕТОДЫ
        [ChildActionOnly]
        [OutputCache(Duration=60)]
        public ActionResult _getImagesLoader(string path = null)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_getImagesLoader", path);
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView(_errPartialPage);
            }
        }        
        [OutputCache(Duration = 60)]
        public ActionResult _getFolderCreator(string path = null)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_getFolderCreator", new VM_Folder() { Name = "", Path = path });
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView(_errPartialPage);
            }
        }
        #endregion
    }
}
