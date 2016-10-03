using Bankiru.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "admin")]
    public class InfoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
