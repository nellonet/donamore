using Nop.Core.Configuration;

namespace Sparkling.Plugin.Widgets.CustomerProducts
{
    public partial class CustomerProductsSettings : ISettings
    {
        #region Properties

        public bool EnablePlugin { get; set; }

        public bool ShortDescriptionEnabled { get; set; }

        public bool ShortDescriptionRequired { get; set; }

        #endregion
    }
}
