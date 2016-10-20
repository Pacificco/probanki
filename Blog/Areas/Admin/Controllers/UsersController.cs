using Bankiru.Controllers;
using Bankiru.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        [HttpGet]
        public ActionResult List(VM_UsersFilters filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    //HttpCookie cookie = _setUsersFiltersCookie(filter, page);
                    UserManager _manager = new UserManager();
                    VM_Users model = _manager.GetUsers(filter, page);
                    if (model != null)
                    {
                        //if (cookie != null)
                        //    HttpContext.Response.Cookies.Add(cookie);
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

    }
}
