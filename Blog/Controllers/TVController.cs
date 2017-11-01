using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Debtors;
using Bankiru.Models.Domain.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public class TVController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                if (_connected)
                {
                    return View("DevPage");                    
                }
                else
                {
                    log.Error(_errMassage);
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return View("Error");
            }
        }

    }
}
