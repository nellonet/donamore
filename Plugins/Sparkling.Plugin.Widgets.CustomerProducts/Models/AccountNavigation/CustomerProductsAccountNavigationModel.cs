using Nop.Web.Framework.Models;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Models.AccountNavigation
{
    public partial record CustomerProductsAccountNavigationModel : BaseNopEntityModel
    {
        #region Properties

        public string RouteName { get; set; }

        public string Title { get; set; }

        public int Tab { get; set; }

        public string ItemClass { get; set; }

        #endregion
    }
}
