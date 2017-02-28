using Bankiru.Models.Domain.Users;
using Bankiru.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public class UsersController : BaseController
    {
                
        [ChildActionOnly]
        //[OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public PartialViewResult _getModuleSideUserProfile()
        {
            try
            {
                if (_connected)
                {
                    UserManager manager = new UserManager();
                    VM_UserProfileInfo profile = null;
                    if(String.IsNullOrEmpty(SessionPersister.Username))
                    {
                        profile = manager.GetUserProfiletInfo(-1);
                    }
                    else
                    {
                        profile = manager.GetUserProfiletInfo(SessionPersister.CurrentUser.Id);
                    }                    
                    return PartialView("_moduleSideUserProfile", profile);
                }
                else
                {
                    log.Error("Ошибка во время отображения блока с профилем пользователя!\r\n" + _errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception ex)
            {
                log.Error("Ошибка во время отображения блока с профилем пользователя!\r\n" + ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
    }
}
