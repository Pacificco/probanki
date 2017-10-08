using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bankiru
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region ПУБЛИКАЦИИ
            routes.MapRoute(
                name: "art_comment",
                url: "publikacii/{cat_id}/{art_id}/comments/{id}",
                defaults: new { controller = "Articles", action = "Comment" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "art_comments",
                url: "publikacii/{cat_id}/{art_id}/comments",
                defaults: new { controller = "Articles", action = "Comments" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "art_item",
                url: "publikacii/{cat_id}/{art_id}",
                defaults: new { controller = "Articles", action = "Art" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "art_comment_send_ajax",
                url: "publikacii/comment-ajax",
                defaults: new { controller = "Articles", action = "CommentAjax" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "art_comment_send",
                url: "publikacii/comment",
                defaults: new { controller = "Articles", action = "Comment" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "art_items",
                url: "publikacii/{cat_id}",
                defaults: new { controller = "Articles", action = "List" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            #endregion
            
            #region ОРГАНИЗАЦИИ
            //Организации (офисы)
            routes.MapRoute(
                name: "org_point",
                url: "orgs/{cat_id}/{org_id}/points/{region_id}/{type}/{point_id}",
                defaults: new { controller = "Orgs", action = "Point", region_id = "voronezhskaya-oblast" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "org_points",
                url: "orgs/{cat_id}/{org_id}/points/{region_id}/{type}",
                defaults: new { controller = "Orgs", action = "Points" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            //Организации (отзывы)
            routes.MapRoute(
                name: "org_leave_review_ajax",
                url: "orgs/new-review-ajax",
                defaults: new { controller = "Orgs", action = "NewReviewAjax" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "org_leave_review",
                url: "orgs/reviews/new-review",
                defaults: new { controller = "Orgs", action = "LeaveReview" },
                namespaces: new[] { "Bankiru.Controllers" }
            );            
            routes.MapRoute(
                name: "org_new_review_org",
                url: "orgs/{org_id}/reviews/new-review",
                defaults: new { controller = "Orgs", action = "NewReview" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "org_review",
                url: "orgs/{cat_id}/{org_id}/reviews/{review_id}",
                defaults: new { controller = "Orgs", action = "Review" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "org_reviews",
                url: "orgs/{cat_id}/{org_id}/reviews",
                defaults: new { controller = "Orgs", action = "Reviews" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            //Организации
            routes.MapRoute(
                name: "org_item",
                url: "orgs/{cat_id}/{org_id}",
                defaults: new { controller = "Orgs", action = "Org" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "org_review_send",
                url: "orgs/new-review",
                defaults: new { controller = "Orgs", action = "NewReview" },
                namespaces: new[] { "Bankiru.Controllers" }
            );            
            routes.MapRoute(
                name: "org_items",
                url: "orgs/{cat_id}",
                defaults: new { controller = "Orgs", action = "List", cat_id = UrlParameter.Optional },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            #endregion

            #region НОВОСТИ
            routes.MapRoute(
                name: "news_item",
                url: "news/{news_id}",
                defaults: new { controller = "News", action = "Show" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "news_items",
                url: "news",
                defaults: new { controller = "News", action = "List" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            #endregion
                        
            #region ПОЛЬЗОВАТЕЛИ
            routes.MapRoute(
                name: "user_profile",
                url: "lk/{user_id}/{action}",
                defaults: new { controller = "Users", action = "Index" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "user_forecasts",
                url: "lk/{user_id}/forecasts/{section_id}/{item_id}",
                defaults: new { controller = "Users", action = "Forecasts", section_id = "dollar" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "user_archive",
                url: "lk/{user_id}/archive/{page}",
                defaults: new { controller = "Users", action = "Archive", page = UrlParameter.Optional },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            #endregion
            
            #region ПРОГНОЗЫ
            routes.MapRoute(
                name: "club_forecast_adduser_ajax",
                url: "club/forecasts/{subject_id}/{id}/adduser_ajax",
                defaults: new { controller = "Forecast", action = "AddUserAjax", subject_id = UrlParameter.Optional, id = UrlParameter.Optional },
                namespaces: new[] { "Bankiru.Controllers" }
            );            
            routes.MapRoute(
                name: "club_forecast_adduser",
                url: "club/forecasts/adduser",
                defaults: new { controller = "Forecast", action = "AddUserToForecast"},
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "club_forecast",
                url: "club/forecasts/{subject_id}/{id}",
                defaults: new { controller = "Forecast", action = "Forecast" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            //routes.MapRoute(
            //    name: "club_forecast_action",
            //    url: "club/forecasts/{forecast_id}/{action}",
            //    defaults: new { controller = "Forecast" },
            //    namespaces: new[] { "Bankiru.Controllers" }
            //);
            routes.MapRoute(
                name: "club_forecasts",
                url: "club/forecasts",
                defaults: new { controller = "Forecast", action = "List" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "club_forecasts_archive",
                url: "club/forecasts/archive/{subject_id}",
                defaults: new { controller = "Forecast", action = "Archive", subject_id = "all" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "club_info",
                url: "club/info",
                defaults: new { controller = "Forecast", action = "About" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "club_rules",
                url: "club/rules",
                defaults: new { controller = "Forecast", action = "Rules" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "club_users",
                url: "club/users",
                defaults: new { controller = "Forecast", action = "Users" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            #endregion            

            #region КУРСЫ ВАЛЮТ И КАТЕРОВКИ
            routes.MapRoute(
                name: "rates",
                url: "currency",
                defaults: new { controller = "Rates", action = "Index" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            #endregion

            #region ДОЛЖНИКИ
            routes.MapRoute(
                name: "debtors",
                url: "debtors",
                defaults: new { controller = "Debtors", action = "Index" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            #endregion

            #region СТАТИЧЕСКИЕ СТРАНИЦЫ
            routes.MapRoute(
                name: "rules",
                url: "rules",
                defaults: new { controller = "Home", action = "Rules" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "feedback",
                url: "obratnaya-svyaz",
                defaults: new { controller = "Home", action = "Feedback" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "feedback_success",
                url: "obratnaya-svyaz/soobsheniye-otpravleno",
                defaults: new { controller = "Home", action = "FeedbackSuccess" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "art_captcha_refresh",
                url: "captcha-ajax",
                defaults: new { controller = "Home", action = "CaptchaAjax" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            routes.MapRoute(
                name: "default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            #endregion                        

            #region ОШИБКИ + 404
            //Ошибки
            routes.MapRoute(
              name: "error",
              url: "Error/{action}",
              defaults: new { controller = "Error", action = "Error" },
              namespaces: new[] { "Bankiru.Controllers" }
            );
            //404
            routes.MapRoute(
                name: "NotFound",
                url: "{*url}",
                defaults: new { controller = "Error", action = "Http404" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            //Остальное
            routes.MapRoute(
                name: "404_catch_all",
                url: "{*catchall}",
                defaults: new { controller = "Error", action = "NotFound" },
                namespaces: new[] { "Bankiru.Controllers" }
            );
            #endregion
            
            /*404
            routes.MapRoute(
                name: "404_admin_catch_all",
                url: "{*catchall}",
                defaults: new { controller = "Error", action = "NotFound" },
                namespaces: new[] { "Bankiru.Areas.Admin.Controllers" }
            );*/
        }
    }
}