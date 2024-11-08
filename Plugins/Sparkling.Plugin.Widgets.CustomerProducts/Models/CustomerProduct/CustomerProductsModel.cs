using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;
using ProductAttributeModel = Nop.Web.Models.Catalog.ProductAttributeModel;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Models.CustomerProduct
{
    public partial record CustomerProductsModel : BaseNopEntityModel, ILocalizedModel<ProductLocalizedModel>
    {
        #region Ctor

        public CustomerProductsModel()
        {
            Locales = new List<ProductLocalizedModel>();
            ProductPictureSearchModel = new CustomerProductsPictureSearchModel();
            ProductVideoSearchModel = new ProductVideoSearchModel();
        }

        #endregion

        #region Properties

        public bool NameRequired { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Fields.Name")]
        public string Name { get; set; }

        public bool ShortDescriptionRequired { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Fields.FullDescription")]
        public string FullDescription { get; set; }

        public bool ProductPublished { get; set; }

        public string FormattedCustomProductAttributes { get; set; }

        public IList<ProductAttributeModel> CustomProductAttributes { get; set; }

        public IList<ProductLocalizedModel> Locales { get; set; }

        public CustomerProductsPictureSearchModel ProductPictureSearchModel { get; set; }

        public ProductVideoSearchModel ProductVideoSearchModel { get; set; }

        public CustomerProductsPictureModel AddPictureModel { get; set; }

        public IList<CustomerProductsPictureModel> ProductPictureModels { get; set; }

        public ProductVideoModel AddVideoModel { get; set; }

        public IList<ProductVideoModel> ProductVideoModels { get; set; }

        #endregion
    }

    #region Nested Classes

    public partial record ProductLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Fields.FullDescription")]
        public string FullDescription { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Fields.SeName")]
        public string SeName { get; set; }
    }

    #endregion
}
