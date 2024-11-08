using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Models.CustomerProduct
{
    public partial record CustomerProductsPictureModel : BaseNopEntityModel
    {
        #region Properties

        public int ProductId { get; set; }

        [UIHint("MultiPicture")]
        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Multimedia.Pictures.Fields.Picture")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Multimedia.Pictures.Fields.Picture")]
        public string PictureUrl { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Multimedia.Pictures.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Multimedia.Pictures.Fields.OverrideAltAttribute")]
        public string OverrideAltAttribute { get; set; }

        [NopResourceDisplayName("Plugin.Widgets.CustomerProducts.Catalog.Products.Multimedia.Pictures.Fields.OverrideTitleAttribute")]
        public string OverrideTitleAttribute { get; set; }

        #endregion
    }
}