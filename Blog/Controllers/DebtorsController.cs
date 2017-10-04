using Bankiru.Models.Domain.Debtors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public class DebtorsController : BaseController
    {
        [HttpGet]
        public ActionResult List(VM_DebtorsFilter filter, int page = 1)
        {
            try
            {
                if (_connected)
                {
                    DebtorsManager manager = new DebtorsManager();
                    VM_Debtors model = manager.GetDebtors(filter, page);                    
                    if (model != null)
                        return View(model);
                    else
                    {
                        log.Error(manager.LastError);
                        return View("Error");
                    }
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
        [HttpGet]
        public ActionResult Debtor(int debtor_id)
        {
            try
            {
                if (_connected)
                {
                    DebtorsManager manager = new DebtorsManager();                    
                    VM_Debtor model = manager.GetDebtor(debtor_id);
                    if (model != null)
                    {
                        return View(model);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = manager.LastError;
                        return View("NotFound");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = _errMassage;
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
