using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Models.CustomerProduct
{
    public partial record CustomerProductsListModel : BaseNopModel
    {
        #region Ctor

        public CustomerProductsListModel()
        {
            Products = new List<CustomerProductsModel>();
        }

        #endregion

        #region Properties

        public IList<CustomerProductsModel> Products { get; set; }

        #endregion
    }
}
