using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
