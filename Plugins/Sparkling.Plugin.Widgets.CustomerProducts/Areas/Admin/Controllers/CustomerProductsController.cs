using Microsoft.AspNetCore.Mvc;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Sparkling.Plugin.Widgets.CustomerProducts.Areas.Admin.Models.Configuration;
using System.Threading.Tasks;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Areas.Admin.Controllers
{
    public partial class CustomerProductsController : BaseAdminController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public CustomerProductsController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
        }

        #endregion

        #region Configuration Methods

        public virtual async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var settings = await _settingService.LoadSettingAsync<CustomerProductsSettings>();

            var model = new ConfigurationModel()
            {
                EnablePlugin = settings.EnablePlugin,
                ShortDescriptionEnabled = settings.ShortDescriptionEnabled,
                ShortDescriptionRequired = settings.ShortDescriptionRequired
            };

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var settings = new CustomerProductsSettings()
            {
                EnablePlugin = model.EnablePlugin,
                ShortDescriptionEnabled = model.ShortDescriptionEnabled,
                ShortDescriptionRequired = model.ShortDescriptionRequired
            };

            await _settingService.SaveSettingAsync(settings);

            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        #endregion
    }
}
