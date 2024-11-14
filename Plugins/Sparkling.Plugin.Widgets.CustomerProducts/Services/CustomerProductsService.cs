using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Services
{
    public partial class CustomerProductsService : ICustomerProductsService
    {
        #region Fields

        private readonly IRepository<Product> _productRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public CustomerProductsService(IRepository<Product> productRepository,
            IStaticCacheManager staticCacheManager)
        {
            _productRepository = productRepository;
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Methods

        public virtual async Task<IList<Product>> GetProductsByCustomerIdAsync(int customerId)
        {
            var query = from product in _productRepository.Table
                        where (product.VendorId == customerId && product.Deleted == false)
                        select product;

            var key = _staticCacheManager.PrepareKeyForShortTermCache(CustomerProductsDefaults.CustomerProductsCacheKey, customerId);
            key.CacheTime = 0;

            return await _staticCacheManager.GetAsync(key, async () => await query.ToListAsync());
        }

        public virtual async Task<Product> GetCustomerProductAsync(int customerId, int productId)
        {
            if (customerId == 0 || productId == 0)
                return null;

            var query = from product in _productRepository.Table
                        where product.Id == productId
            select product;

            var key = _staticCacheManager.PrepareKeyForShortTermCache(CustomerProductsDefaults.CustomerProductCacheKey, customerId, productId);

            return await _staticCacheManager.GetAsync(key, async () => await query.FirstOrDefaultAsync());
        }

        #endregion
    }
}
