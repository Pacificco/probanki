using Bankiru.Models.Domain;
using Bankiru.Models.Domain.Account;
using Bankiru.Models.Domain.Articles;
using Bankiru.Models.Domain.Club;
using Bankiru.Models.Domain.Comments;
using Bankiru.Models.Domain.News;
using Bankiru.Models.Domain.Orgs;
using Bankiru.Models.Domain.OrgsCategories;
using Bankiru.Models.Domain.Other;
using Bankiru.Models.Domain.Users;
using Bankiru.Models.Helpers;
using Bankiru.Models.Infrastructure;
using Bankiru.Models.Security;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bankiru.Controllers
{
    public abstract class BaseController : Controller
    {
        #region ПОЛЯ И СВОЙСТВА
        /// <summary>
        /// Подключение
        /// </summary>
        protected SqlConnection _connection;
        /// <summary>
        /// Подключение установлено
        /// </summary>
        protected bool _connected;

        /// <summary>
        /// Сообщение об ошибках
        /// </summary>
        protected string _errMassage;
        /// <summary>
        /// Предупреждение
        /// </summary>
        protected string _warningMassage;
        /// <summary>
        /// Сообщение
        /// </summary>
        protected string _infoMassage;

        /// <summary>
        /// Страница с ошибкой
        /// </summary>
        protected string _errPage;
        /// <summary>
        /// Страница с ошибкой для частичных представлений
        /// </summary>
        protected string _errPartialPage;

        public static readonly ILog log = LogManager.GetLogger(typeof(BaseController));
        #endregion

        public BaseController()
            : base()
        {
            _connection = null;
            _connected = false;

            _errMassage = String.Empty;
            _warningMassage = String.Empty;
            _infoMassage = String.Empty;

            _errPage = "~/Views/Shared/Error.cshtml";
            _errPartialPage = "~/Views/Shared/PartialError.cshtml";

            //AccountManager _manager = new AccountManager();
            //string password = _manager._getMd5Hash("SupperPassword");
            //if (password == "111") return;

            //AccountManager m = new AccountManager();
            //_errMassage = m._getMd5Hash("sE~x6y34");

            Connect();
        }

        /// <summary>
        /// Подключается к базе данных
        /// </summary>
        private void Connect()
        {
            try
            {
                _connection = GlobalParams.GetConnection();
                if (_connection == null)
                {
                    _errMassage = GlobalParams._connectionError;
                    _connected = false;
                }
                else
                {
                    _connected = true;
                }                
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                _errMassage = e.ToString();
                _connected = false;
            }
        }

        #region СПИСКИ
        [ChildActionOnly]
        public ActionResult _getOrgsDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager manager = new OrgsManager();
                    VM_DropDownOrgItems model = new VM_DropDownOrgItems();
                    model.SelectedId = selectedId;
                    model.Items = manager.GetOrgItems();
                    model.FirstItem = firstItem;
                    model.Name = id;
                    return PartialView("_orgsDropDownList", model);
                }
                else
                {
                    log.Error(_errMassage);                    
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getRegionsDropDownList(Guid selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                if (_connected)
                {
                    VM_DropDownRegions model = new VM_DropDownRegions();
                    model.SelectedId = selectedId;
                    model.Items = DbHelper.GetRegions();
                    model.FirstItem = firstItem;
                    model.Name = id;
                    return PartialView("_regionsDropDownList", model);
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());                
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getOrgCategoriesDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager manager = new OrgsManager();
                    VM_DropDownOrgCategories model = new VM_DropDownOrgCategories();
                    model.SelectedId = selectedId;
                    model.Items = manager.GetOrgCategories();
                    model.FirstItem = firstItem;
                    model.Name = id;
                    return PartialView("_orgCategoriesDropDownList", model);
                }
                else
                {
                    log.Error(_errMassage);                    
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());                
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getCategoriesDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                if (_connected)
                {
                    ArtsManager manager = new ArtsManager();
                    VM_DropDownCategories model = new VM_DropDownCategories();
                    model.SelectedId = selectedId;
                    model.Items = manager.GetCategories();
                    model.FirstItem = firstItem;
                    model.Name = id;
                    return PartialView("_categoriesDropDownList", model);
                }
                else
                {
                    log.Error(_errMassage);                    
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());                
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public PartialViewResult _getForecastSubjectDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                if (_connected)
                {
                    ForecastManager manager = new ForecastManager();
                    VM_DropDownForecastSubject model = new VM_DropDownForecastSubject();
                    model.SelectedId = selectedId;
                    model.Items = manager.GetForecastSubjects();
                    model.FirstItem = firstItem;
                    model.Name = id;
                    return PartialView("_forecastSubjectDropDownList", model);
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]        
        public ActionResult _getActiveDropDownList(EnumBoolType selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_DropDownActive model = new VM_DropDownActive();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_activeDropDownList", model);                
            }
            catch (Exception e)
            {
                log.Error(e.ToString());                
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getArchiveDropDownList(EnumBoolType selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_DropDownArchive model = new VM_DropDownArchive();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_archiveDropDownList", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getConfirmDropDownList(EnumBoolType selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                VM_DropDownConfirm model = new VM_DropDownConfirm();
                model.FirstItem = firstItem;
                model.Name = id;
                model.SelectedId = selectedId;
                return PartialView("_confirmDropDownList", model);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getOrgPointTypesDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                if (_connected)
                {
                    VM_DropDownOrgPointTypes model = new VM_DropDownOrgPointTypes();
                    model.SelectedId = selectedId;
                    model.Items = new List<string>();
                    model.Items.Add("Офисы");
                    model.Items.Add("Банкоматы");
                    model.FirstItem = firstItem;
                    model.Name = id;
                    return PartialView("_orgPointTypesDropDownList", model);
                }
                else
                {
                    log.Error(_errMassage);                    
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getTariffDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                if (_connected)
                {
                    VM_DropDownTariff model = new VM_DropDownTariff();
                    model.SelectedId = selectedId;
                    model.FirstItem = firstItem;
                    model.Name = id;
                    return PartialView("_tariffDropDownList", model);
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getTariffPeriodDropDownList(int selectedId, EnumFirstDropDownItem firstItem, string id)
        {
            try
            {
                if (_connected)
                {
                    VM_DropDownTariffPeriod model = new VM_DropDownTariffPeriod();
                    model.SelectedId = selectedId;
                    model.FirstItem = firstItem;
                    model.Name = id;
                    return PartialView("_tariffPeriodDropDownList", model);
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        #endregion

        #region МОДУЛИ
        [ChildActionOnly]
        //[OutputCache(Duration = 86400, VaryByParam = "current_item")]
        public ActionResult _getModuleMainMenu(string current_item = "home")
        {
            return PartialView("_moduleMainMenu", current_item);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "cur_id")]
        public ActionResult _getModuleOrgMenu(int cur_id = -1)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager manager = new OrgsManager();
                    VM_OrgCategories model = new VM_OrgCategories();
                    model.items = manager.GetOrgCategories();
                    model.currentId = cur_id;
                    if (model != null)
                    {
                        return PartialView("_moduleOrgMenu", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }                
            }
            catch (Exception e)
            {
                log.Error(e.ToString());                
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "letter")]
        public ActionResult _getModuleOrgsLetterFilter(string letter)
        {
            try
            {
                return PartialView("_moduleOrgLettersFilter", letter);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "org_id")]
        public ActionResult _getModuleOrgPoints(int org_id)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager manager = new OrgsManager();
                    List<VM_OrgPoint> model = manager.GetPoints(org_id, Guid.Empty);
                    if (model != null)
                    {
                        return PartialView("_moduleOrgPoints", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getModuleSideLeaveOrgReview(string url)
        {
            try
            {
                return PartialView("_moduleSideLeaveOrgReview", url);
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "categoryId;show_article_link")]
        public ActionResult _getModuleLastComments(int categoryId = 0, int row_count = 3, bool show_article_link = true)
        {
            try
            {
                if (_connected)
                {               
                    ArtsManager manager = new ArtsManager();
                    VM_Comments model = manager.GetLastComments(categoryId, row_count, show_article_link);
                    if (model != null)
                    {
                        return PartialView("_moduleLastComments", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "art_id")]
        public ActionResult _getModuleLastArtComments(int art_id)
        {
            try
            {
                if (art_id <= 0)
                {
                    log.Error("Идентификатор публикации не определен");
                    return PartialView(_errPartialPage);
                }
                if (_connected)
                {
                    ArtsManager manager = new ArtsManager();
                    VM_Comments model = manager.GetLastComments(art_id);
                    if (model != null)
                    {
                        return PartialView("_moduleLastArtComments", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        //[OutputCache(Duration = 60, VaryByParam = "exclude_ids;cat_ids")]
        public ActionResult _getModuleLastArticlesLinkList(List<int> exclude_ids, List<int> cat_ids, int row_count = 3)
        {
            try
            {
                if (_connected)
                {
                    ArtsManager manager = new ArtsManager();
                    List<VM_ArtItem> model = manager.GetArtItems(exclude_ids, cat_ids, row_count);
                    if (model != null)
                    {
                        return PartialView("_moduleLastArticlesLinkList", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        //[OutputCache(Duration = 60, VaryByParam = "exclude_ids;cat_ids")]
        public PartialViewResult _getModuleLastArticlesImageList(List<int> exclude_ids, List<int> cat_ids, int row_count = 3)
        {
            try
            {
                if (_connected)
                {
                    ArtsManager manager = new ArtsManager();
                    List<VM_ArtItem> model = manager.GetArtItems(exclude_ids, cat_ids, row_count);
                    if (model != null)
                    {
                        return PartialView("_moduleLastArticlesImageList", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60)]
        public ActionResult _getModuleLastNews(int row_count = 10)
        {
            try
            {
                if (_connected)
                {
                    NewsManager manager = new NewsManager();
                    List<VM_NewsItem> model = manager.GetLastNews(row_count);
                    if (model != null)
                    {
                        return PartialView("_moduleLastNews", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);                        
                        return PartialView(_errPartialPage);
                    }                    
                }
                else
                {
                    log.Error(_errMassage);                    
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());                
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60)]
        public ActionResult _getModuleLastPopularOrgs(int row_count)
        {
            try
            {
                if (_connected)
                {
                    return PartialView("_modulePopularOrgs", null);
                }
                else
                {
                    log.Error(_errMassage);
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());                
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60, VaryByParam = "org_id;categoryId")]
        public ActionResult _getModuleLastReviews(int? org_id = null,  int? categoryId = null, int row_count = 10)
        {
            try
            {
                if (_connected)
                {
                    OrgsManager manager = new OrgsManager();
                    List<VM_Review> model = manager.GetLastReview(org_id, categoryId, row_count);
                    if (model != null)
                    {
                        return PartialView("_moduleLastReviews", model);
                    }
                    else
                    {
                        log.Error(manager.LastError);                        
                        return PartialView(_errPartialPage);
                    }
                }
                else
                {
                    log.Error(_errMassage);                    
                    return PartialView(_errPartialPage);
                }
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getModuleCaptchaBlock()
        {
            try
            {
                return PartialView("_moduleCaptchaBlock");
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 120)]
        public ActionResult _getModuleCounters()
        {
            return PartialView("_moduleCounters");
        }
        [ChildActionOnly]
        public ActionResult _getModuleLogin()
        {
            try
            {
                return PartialView("_moduleFormLogin", new VM_UserLogin());
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        [OutputCache(Duration = 60)]
        public ActionResult _getModuleShareInSocial()
        {
            try
            {
                return PartialView("_moduleShareInSocial");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public ActionResult _getModuleWellcomeBlock(string user_name)
        {
            try
            {
                AccountManager manager = new AccountManager();
                VM_User user = manager.FindUser(user_name);
                if (user == null)
                    return PartialView(_errPartialPage);
                else
                    return PartialView("_moduleWellcomeBlock", user);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        #endregion

        #region СОЦ СЕТИ
        [ChildActionOnly]
        public PartialViewResult _getWidget_VK()
        {
            try
            {
                return PartialView("_widget_vk");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public PartialViewResult _getWidget_OK()
        {
            try
            {
                return PartialView("_widget_ok");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        [ChildActionOnly]
        public PartialViewResult _getWidget_FB()
        {
            try
            {
                return PartialView("_widget_fb");
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return PartialView(_errPartialPage);
            }
        }
        #endregion

        #region CAPTCHA
        [HttpPost]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public ActionResult CaptchaAjax()
        {
            try
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_moduleCaptchaBlock");
                }
                else
                {
                    return PartialView("_partialView", "<p class=\"text-red\">Не удалось загрузить Контрольную строку!</p>");
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
                return PartialView("_partialView", "<p class=\"text-red\">Не удалось загрузить Контрольную строку!</p>");
            }
        }
        [HttpGet]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public CaptchaImageResult ShowCaptchaImage()
        {
            return new CaptchaImageResult(140, 50, 5);
        }
        #endregion
    }
}