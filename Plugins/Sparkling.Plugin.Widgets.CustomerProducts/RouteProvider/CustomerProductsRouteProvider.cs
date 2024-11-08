using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Infrastructure;

namespace Sparkling.Plugin.Widgets.CustomerProducts.RouteProvider
{
    public partial class CustomerProductsRouteProvider : BaseRouteProvider, IRouteProvider
    {
        #region Methods

        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            var lang = GetLanguageRoutePattern();

            endpointRouteBuilder.MapControllerRoute(name: "CustomerProducts",
                pattern: $"{lang}/customer/products",
                defaults: new { controller = "CustomerProducts", action = "Products" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerProductEdit",
                pattern: $"{lang}/customer/productedit/{{productId:min(0)}}",
                defaults: new { controller = "CustomerProducts", action = "ProductEdit" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerProductAdd",
               pattern: $"{lang}/customer/productadd",
               defaults: new { controller = "CustomerProducts", action = "ProductAdd" });
        }

        public int Priority => int.MaxValue;

        #endregion
    }
}
