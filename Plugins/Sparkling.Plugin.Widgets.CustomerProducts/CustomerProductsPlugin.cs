using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Core.Domain.Localization;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using Sparkling.Plugin.Widgets.CustomerProducts.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sparkling.Plugin.Widgets.CustomerProducts
{
    public partial class CustomerProductsPlugin : BasePlugin, IAdminMenuPlugin, IWidgetPlugin
    {
        #region Fields

        private readonly IRepository<Language> _languageRepository;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly INopFileProvider _nopFileProvider;
        private readonly IPermissionService _permissionService;
        private readonly WidgetSettings _widgetSettings;

        #endregion

        #region Ctor

        public CustomerProductsPlugin(IRepository<Language> languageRepository,
            ILocalizationService localizationService,
            ISettingService settingService,
            IWebHelper webHelper,
            INopFileProvider nopFileProvider,
            IPermissionService permissionService,
            WidgetSettings widgetSettings)
        {
            _languageRepository = languageRepository;
            _localizationService = localizationService;
            _settingService = settingService;
            _webHelper = webHelper;
            _nopFileProvider = nopFileProvider;
            _permissionService = permissionService;
            _widgetSettings = widgetSettings;
        }

        #endregion

        #region Plugin Configuration Methods

        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/CustomerProducts/Configure";
        }

        #endregion

        #region Plugin Install/Uninstall Methods

        public override async Task InstallAsync()
        {
            await InstallLocaleResourcesAsync();

            var settings = new CustomerProductsSettings
            {
                EnablePlugin = true,
                ShortDescriptionEnabled = true,
                ShortDescriptionRequired = true
            };

            if (!_widgetSettings.ActiveWidgetSystemNames.Contains(CustomerProductsDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Add(CustomerProductsDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            await _settingService.SaveSettingAsync(settings);

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await DeleteLocalResourcesAsync();

            if (_widgetSettings.ActiveWidgetSystemNames.Contains(CustomerProductsDefaults.SystemName))
            {
                _widgetSettings.ActiveWidgetSystemNames.Remove(CustomerProductsDefaults.SystemName);
                await _settingService.SaveSettingAsync(_widgetSettings);
            }

            await _settingService.DeleteSettingAsync<CustomerProductsSettings>();

            await base.UninstallAsync();
        }

        #endregion

        #region Install / Uninstall Local Resources

        protected virtual async Task InstallLocaleResourcesAsync()
        {
            //'English' language
            var languages = _languageRepository.Table.Where(l => l.Published).ToList();
            foreach (var language in languages)
            {
                //save resources
                foreach (var filePath in Directory.EnumerateFiles(_nopFileProvider.MapPath("~/Plugins/Widgets.CustomerProducts/Localization"), "ResourceStrings.xml", SearchOption.TopDirectoryOnly))
                {
                    using (var streamReader = new StreamReader(filePath))
                    {
                        await _localizationService.ImportResourcesFromXmlAsync(language, streamReader);
                    }
                }
            }
        }

        protected virtual async Task DeleteLocalResourcesAsync()
        {
            var file = Path.Combine(_nopFileProvider.MapPath("~/Plugins/Widgets.CustomerProducts/Localization"), "ResourceStrings.xml");
            var languageResourceNames = from name in XDocument.Load(file).Document.Descendants("LocaleResource")
                                        select name.Attribute("Name").Value;

            foreach (var item in languageResourceNames)
            {
                await _localizationService.DeleteLocaleResourcesAsync(item);
            }
        }

        #endregion

        #region Manage Sitemap Methods

        [Area(AreaNames.Admin)]
        public async Task ManageSiteMapAsync(SiteMapNode siteMapNode)
        {
            var mainMenuItem = new SiteMapNode()
            {
                Title = await _localizationService.GetResourceAsync("Plugin.Widgets.CustomerProducts.MainMenu.Title"),
                Visible = await Authenticate(),
                IconClass = "fab fa-buysellads"
            };

            var pluginMenuItem = new SiteMapNode()
            {
                Title = await _localizationService.GetResourceAsync("Plugin.Widgets.CustomerProducts.ChildMenu.CustomerProducts"),
                Visible = await Authenticate(),
                IconClass = "far fa-dot-circle"
            };
            mainMenuItem.ChildNodes.Add(pluginMenuItem);

            var configure = new SiteMapNode()
            {
                SystemName = "Sparkling.Plugin.Widgets.CustomerProducts.Configure",
                Title = await _localizationService.GetResourceAsync("Sparkling.Plugin.Widgets.CustomerProducts.Configuration"),
                ControllerName = "CustomerProducts",
                ActionName = "Configure",
                Visible = await Authenticate(),
                IconClass = "far fa-circle",
                RouteValues = new RouteValueDictionary() { { "Sparkling.Plugin.Widgets.CustomerProducts", null } },
            };

            pluginMenuItem.ChildNodes.Add(configure);

            var title = await _localizationService.GetResourceAsync("Plugin.Widgets.CustomerProducts.MainMenu.Title");
            var targetMenu = siteMapNode.ChildNodes.FirstOrDefault(x => x.Title == title);

            if (targetMenu != null)
                targetMenu.ChildNodes.Add(pluginMenuItem);
            else
                siteMapNode.ChildNodes.Add(mainMenuItem);
        }

        public async Task<bool> Authenticate()
        {
            bool flag = false;
            if (await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        #endregion

        #region List of Widget Methods

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string>
            {
                PublicWidgetZones.AccountNavigationAfter
            });
        }

        public Type GetWidgetViewComponent(string widgetZone)
        {
            if (widgetZone == PublicWidgetZones.AccountNavigationAfter)
                return typeof(CustomerProductsAccountNavigationWidgetViewComponent);

            return null;
        }

        public bool HideInWidgetList => false;

        #endregion
    }
}
