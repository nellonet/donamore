using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Areas.Admin.Models.Configuration
{
    public partial record ConfigurationModel : BaseNopEntityModel
    {
        #region Properties

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Admin.Configuration.Fields.EnablePlugin")]
        public bool EnablePlugin { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Admin.Configuration.Fields.ShortDescriptionEnabled")]
        public bool ShortDescriptionEnabled { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Admin.Configuration.Fields.ShortDescriptionRequired")]
        public bool ShortDescriptionRequired { get; set; }

        #endregion
    }
}
