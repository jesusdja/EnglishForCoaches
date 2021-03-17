using System.Web;
using System.Web.Optimization;

namespace EnglishForCoachesWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Parte privada
            bundles.Add(new StyleBundle("~/Content/css").Include(
                   "~/Content/Intranet/assets/js/plugins/slick/slick.min.css",
                   "~/Content/Intranet/assets/js/plugins/slick/slick-theme.min.css",
                   "~/Content/Intranet/assets/js/plugins/magnific-popup/magnific-popup.min.css",
                   "~/Content/Intranet/assets/css/oneui.css",
                   "~/Content/Intranet/assets/css/themes/flat.min.css",
                   "~/Content/Intranet/assets/css/styles.css",
                   "~/Content/Intranet/assets/js/plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css",
                   "~/Content/Intranet/custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/oneui").Include(
                      "~/Content/Intranet/assets/js/oneui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                         "~/Scripts/Shared/FileSizeValidator.js"));

            bundles.Add(new ScriptBundle("~/bundles/globalization").Include(
                        "~/Scripts/cldr.js",
                        "~/Scripts/cldr/event.js",
                        "~/Scripts/cldr/supplemental.js",
                        "~/Scripts/globalize.js",
                        "~/Scripts/globalize/message.js",
                        "~/Scripts/globalize/number.js",
                        "~/Scripts/globalize/plural.js",
                        "~/Scripts/globalize/date.js",
                        "~/Scripts/globalize/date.js",
                        "~/Scripts/globalize/relative-time.js",
                        "~/Scripts/localization/messages_es.js",
                        "~/Content/Intranet/assets/js/jQueryFixes.js"));

    
            bundles.Add(new ScriptBundle("~/bundles/appjs").Include(
                        "~/Content/Intranet/assets/js/core/jquery.min.js",

                        "~/Scripts/moment/moment-with-locales.js",
                        "~/Scripts/bootstrap/js/transition.js",
                        "~/Scripts/abootstrap/js/collapse.js",

                        "~/Content/Intranet/assets/js/core/bootstrap.min.js",

                        "~/Scripts/bootstrap-datetimepicker.min.js",

                        "~/Content/Intranet/assets/js/core/jquery.slimscroll.min.js",
                        "~/Content/Intranet/assets/js/core/jquery.scrollLock.min.js",
                        "~/Content/Intranet/assets/js/core/jquery.appear.min.js",
                        "~/Content/Intranet/assets/js/core/jquery.countTo.min.js",
                        "~/Content/Intranet/assets/js/core/jquery.placeholder.min.js",
                        "~/Content/Intranet/assets/js/core/js.cookie.min.js",
                        "~/Content/Intranet/assets/js/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/maskedinput").Include(
                        "~/Content/Intranet/assets/js/plugins/masked-inputs/jquery.maskedinput.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/slick").Include(
                        "~/Content/Intranet/assets/js/plugins/slick/slick.min.js"));


            // Alumno

            bundles.Add(new StyleBundle("~/Content/AlumnoCss").Include(
                        "~/Content/Alumno/css/style.min.css",
                        "~/Content/Alumno/css/summernote.min.css",
                        "~/Content/Alumno/vendor/sweetalert.min.css",
                        "~/Content/Alumno/css/custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/AlumnoJs").Include(
                        "~/Content/Alumno/vendor/jquery.min.js",
                        "~/Content/Alumno/vendor/tether.min.js",
                        "~/Content/Alumno/vendor/bootstrap.min.js",
                        "~/Content/Alumno/vendor/adminplus.js",
                        "~/Content/Alumno/js/main.min.js",
                        "~/Content/Alumno/js/colors.js"));

            bundles.Add(new ScriptBundle("~/bundles/AlumnoJQueryJs").Include(
                        "~/Content/Alumno/vendor/jquery-ui.js",
                        "~/Content/Alumno/vendor/jquery.ui.touch-punch.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/AlumnoChartsJs").Include(
                        "~/Content/Alumno/vendor/raphael-min.js",
                        "~/Content/Alumno/vendor/morris.min.js",
                        "~/Content/Alumno/js/chart.js"));
            // Usuario

            bundles.Add(new StyleBundle("~/Content/UsuarioCss").Include(
                        "~/Content/Usuario/css/style.min.css",
                        "~/Content/Usuario/css/custom.css"));

            bundles.Add(new ScriptBundle("~/bundles/UsuarioJs").Include(
                        "~/Content/Usuario/vendor/jquery.min.js",
                        "~/Content/Usuario/vendor/tether.min.js",
                        "~/Content/Usuario/vendor/bootstrap.min.js",
                        "~/Content/Usuario/vendor/adminplus.js",
                        "~/Content/Usuario/js/main.min.js",
                        "~/Content/Usuario/js/colors.js"));

            bundles.Add(new ScriptBundle("~/bundles/UsuarioChartsJs").Include(
                        "~/Content/Usuario/vendor/raphael-min.js",
                        "~/Content/Usuario/vendor/morris.min.js",
                        "~/Content/Usuario/js/chart.js"));

            //Parte publica

        }
    }
}
