using Microsoft.AspNetCore.Mvc.Razor;
using Nop.Core.Infrastructure;
using Nop.Web.Framework;
using Nop.Web.Framework.Themes;
using System.Collections.Generic;
using System.Linq;

namespace Sparkling.Plugin.Widgets.CustomerProducts.ViewEngine
{
    public partial class CustomerProductsViewEngine : IViewLocationExpander
    {
        #region View Location Expanders

        private const string THEME_KEY = "nop.themename";

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            if (context.AreaName?.Equals(AreaNames.Admin) ?? false)
                return;

            context.Values[THEME_KEY] = EngineContext.Current.Resolve<IThemeContext>().GetWorkingThemeNameAsync().Result;
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            context.Values.TryGetValue(THEME_KEY, out string theme);

            if (context.AreaName == "Admin")
            {
                viewLocations = new[] {
                        $"~/Plugins/Widgets.CustomerProducts/Areas/Admin/Views/{{1}}/{{0}}.cshtml",
                        $"~/Plugins/Widgets.CustomerProducts/Areas/Admin/Views/Shared/{{0}}.cshtml"
                    }.Concat(viewLocations);
            }
            else
            {
                viewLocations = new[] {
                        $"~/Plugins/Widgets.CustomerProducts/Themes/{theme}/Views/{{1}}/{{0}}.cshtml",
                        $"~/Plugins/Widgets.CustomerProducts/Themes/{theme}/Views/Shared/{{0}}.cshtml",
                        $"~/Plugins/Widgets.CustomerProducts/Views/{{1}}/{{0}}.cshtml",
                        $"~/Plugins/Widgets.CustomerProducts/Views/Shared/{{0}}.cshtml"
                    }.Concat(viewLocations);
            }

            return viewLocations;
        }

        #endregion
    }
}
