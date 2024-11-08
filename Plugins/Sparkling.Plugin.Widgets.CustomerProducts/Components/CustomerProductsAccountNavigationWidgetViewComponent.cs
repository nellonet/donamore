using Microsoft.AspNetCore.Mvc;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Models.Customer;
using Sparkling.Plugin.Widgets.CustomerProducts.Models.AccountNavigation;
using System;
using System.Threading.Tasks;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Components
{
    [ViewComponent(Name = CustomerProductsDefaults.CustomerProductsAccountNavigationWidget)]
    public partial class CustomerProductsAccountNavigationWidgetViewComponent : NopViewComponent
    {
        #region Fields

        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Ctor

        public CustomerProductsAccountNavigationWidgetViewComponent(ISettingService settingService,
            ILocalizationService localizationService)
        {
            _settingService = settingService;
            _localizationService = localizationService;
        }

        #endregion

        #region Methods

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var settings = await _settingService.LoadSettingAsync<CustomerProductsSettings>();

            if (!settings.EnablePlugin)
                return Content(string.Empty);

            if (!widgetZone?.Equals(PublicWidgetZones.AccountNavigationAfter, StringComparison.InvariantCultureIgnoreCase) ?? true)
                return Content(string.Empty);

            if (!(additionalData is CustomerNavigationModel customerNavigationModel))
                return Content(string.Empty);

            var model = new CustomerProductsAccountNavigationModel()
            {
                RouteName = "CustomerProducts",
                Title = await _localizationService.GetResourceAsync("Plugin.Widgets.Account.CustomerProducts"),
                Tab = (int)CustomerProductsNavigationEnum.CustomerProducts,
                ItemClass = "customer-products"
            };

            return View(model);
        }

        #endregion
    }
}
