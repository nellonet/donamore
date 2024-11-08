using Nop.Web.Framework.Models;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Models.CustomerProduct
{
    public partial record class CustomerProductsEditModel : BaseNopModel
    {
        #region Ctor

        public CustomerProductsEditModel()
        {
            Product = new CustomerProductsModel();
        }

        #endregion

        #region Properties

        public CustomerProductsModel Product { get; set; }

        #endregion
    }
}
