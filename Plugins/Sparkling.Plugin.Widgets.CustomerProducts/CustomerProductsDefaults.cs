using Nop.Core.Caching;

namespace Sparkling.Plugin.Widgets.CustomerProducts
{
    public static class CustomerProductsDefaults
    {
        #region Static System Names

        public const string SystemName = "Widgets.CustomerProducts";

        public const string CustomerProductsAccountNavigationWidget = "CustomerProductsAccountNavigationWidget";

        public static CacheKey CustomerProductsCacheKey => new("Nop.customer.products.{0}", CustomerProductsPrefix);

        public static CacheKey CustomerProductCacheKey => new("Nop.customer.products.{0}-{1}", CustomerProductsByCustomerPrefix, CustomerProductsPrefix);

        public static string CustomerProductsPrefix => "Nop.customer.products.";

        public static string CustomerProductsByCustomerPrefix => "Nop.customer.products.{0}";

        #endregion
    }
}
