using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using Nop.Web.Models.Common;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Web.Models.Customer
{
    public partial record CustomerProductModel : BaseNopEntityModel, ILocalizedModel<ProductLocalizedModel>
    {
        public CustomerProductModel()
        {
            Locales = new List<ProductLocalizedModel>();
            ProductPictureSearchModel = new ProductPictureSearchModel();
            ProductVideoSearchModel = new ProductVideoSearchModel();
        }

        public bool NameRequired { get; set; }
        [NopResourceDisplayName("Customer.Product.Fields.Name")]
        public string Name { get; set; }

        public bool ShortDescriptionRequired { get; set; }
        [NopResourceDisplayName("Customer.Product.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [NopResourceDisplayName("Customer.Product.Fields.FullDescription")]
        public string FullDescription { get; set; }

        public bool ProductPublished { get; set; }

        public string FormattedCustomProductAttributes { get; set; }
        public IList<ProductAttributeModel> CustomProductAttributes { get; set; }
               
        public IList<ProductLocalizedModel> Locales { get; set; }

        public ProductPictureSearchModel ProductPictureSearchModel { get; set; }

        public ProductVideoSearchModel ProductVideoSearchModel { get; set; }    

        //pictures
        public ProductPictureModel AddPictureModel { get; set; }
        public IList<ProductPictureModel> ProductPictureModels { get; set; }

        //video
        public ProductVideoModel AddVideoModel { get; set; }    
        public IList<ProductVideoModel> ProductVideoModels { get; set; }

    }

    public partial record ProductLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.ShortDescription")]
        public string ShortDescription { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.FullDescription")]
        public string FullDescription { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.Fields.SeName")]
        public string SeName { get; set; }
    }
}