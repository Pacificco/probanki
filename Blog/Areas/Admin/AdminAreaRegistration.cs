using System.Web.Mvc;

namespace Bankiru.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {                       
            context.MapRoute(
                name: "admin_articles",
                url: "Admin/Articles/{action}/{id}",
                defaults: new { controller = "Articles", action = "List", id = UrlParameter.Optional },
                namespaces: new[] { "Bankiru.Areas.Admin.Controllers" }
            );
            context.MapRoute(
               name: "admin_users",
               url: "Admin/Users/{action}/{id}",
               defaults: new { controller = "Users", action = "List", id = UrlParameter.Optional },
               namespaces: new[] { "Bankiru.Areas.Admin.Controllers" }
           );
            context.MapRoute(
                name: "admin_comments",
                url: "Admin/Comments/{action}/{id}",
                defaults: new { controller = "Comments", action = "List", id = UrlParameter.Optional },
                namespaces: new[] { "Bankiru.Areas.Admin.Controllers" }
            );                   
            context.MapRoute(
                name: "admin_default",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Info", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Bankiru.Areas.Admin.Controllers" }
            );
            context.MapRoute(
              name: "admin_error",
              url: "Admin/Error/{action}",
              defaults: new { controller = "Error", action = "Error" },
              namespaces: new[] { "Bankiru.Areas.Admin.Controllers" }
            );

            //404
            context.MapRoute(
                name: "404_admin_catch_all",
                url: "Admin/{*catchall}",
                defaults: new { controller = "Error", action = "NotFound" },
                namespaces: new[] { "Bankiru.Areas.Admin.Controllers" }
            ); 
        }
    }
}
