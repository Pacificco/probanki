using System.Web;
using System.Web.Optimization;

namespace Bankiru
{
    public class BundleConfig
    {
        // Дополнительные сведения о Bundling см. по адресу http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-migrate-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/forms").Include(
                        "~/Scripts/jquery.form.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство построения на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/themes/site.css"));
            
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            //Админка
            bundles.Add(new StyleBundle("~/Content/admin/css").Include("~/Content/admin_themes/admin.css"));

            //Организации
            bundles.Add(new StyleBundle("~/Content/admin/orgs/css").Include(
                "~/Content/admin_themes/orgs/org.css",
                "~/Content/admin_themes/orgs/table.css"
                ));
            //Офисы и банкоматы организаций
            bundles.Add(new StyleBundle("~/Content/admin/org_points/css").Include(
                "~/Content/admin_themes/orgs/point.css",
                "~/Content/admin_themes/orgs/points_table.css"
                ));
            //Отзывы организаций
            bundles.Add(new StyleBundle("~/Content/admin/org_reviews/css").Include(
                "~/Content/admin_themes/orgs/review.css",
                "~/Content/admin_themes/orgs/reviews_table.css"
                ));
            //Статьи
            bundles.Add(new StyleBundle("~/Content/admin/arts/css").Include(
                "~/Content/admin_themes/arts/art.css",
                "~/Content/admin_themes/arts/table.css"
                ));
            //Пользователи
            bundles.Add(new StyleBundle("~/Content/admin/users/css").Include(
                "~/Content/admin_themes/users/users.css",
                "~/Content/admin_themes/users/table.css"
                ));
            //Комментарии
            bundles.Add(new StyleBundle("~/Content/admin/comments/css").Include(
                "~/Content/admin_themes/comments/comments.css",
                "~/Content/admin_themes/comments/table.css"
                ));
            //Новости
            bundles.Add(new StyleBundle("~/Content/admin/news/css").Include(
                "~/Content/admin_themes/news/news.css",
                "~/Content/admin_themes/news/table.css"
                ));
            //Галерея изображений
            bundles.Add(new StyleBundle("~/Content/admin/images/css").Include(
                "~/Content/admin_themes/images/images.css"                
                ));



            //Статьи
            bundles.Add(new StyleBundle("~/Content/arts/css").Include(
                "~/Content/themes/arts/articles.css",
                "~/Content/themes/arts/comments.css"
                ));

            //Ошибка
            bundles.Add(new StyleBundle("~/Content/errors/css").Include(
                "~/Content/themes/error.css"
                ));
        }
    }
}