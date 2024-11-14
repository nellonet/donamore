using Nop.Core.Domain.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Services
{
    public partial interface ICustomerProductsService
    {
        #region Methods

        Task<IList<Product>> GetProductsByCustomerIdAsync(int customerId);

        Task<Product> GetCustomerProductAsync(int customerId, int productId);

        #endregion
    }
}
