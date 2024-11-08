using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Web.Areas.Admin.Models.Catalog;
using Sparkling.Plugin.Widgets.CustomerProducts.Models.CustomerProduct;
using System.Threading.Tasks;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Factories
{
    public partial interface ICustomerProductsModelFactory
    {
        #region Methods

        Task<CustomerProductsListModel> PrepareCustomerProductsListModelAsync();

        Task PrepareCustomerProductsModelAsync(CustomerProductsModel model, Product product, bool excludeProperties, 
            CustomerProductsSettings customerProductsSettings, bool prePopulateWithCustomerFields = false, Customer customer = null, 
            string overrideAttributesXml = "");

        Task<ProductPictureListModel> PrepareCustomerProductsPictureListModelAsync(ProductPictureSearchModel searchModel, Product product);

        Task<ProductVideoListModel> PrepareCustomerProductsVideoListModelAsync(ProductVideoSearchModel searchModel, Product product);

        #endregion
    }
}
