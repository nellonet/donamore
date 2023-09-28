using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using Nop.Web.Models.Common;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Web.Models.Customer
{
    public partial record CustomerProductModel : BaseNopEntityModel
    {
        public CustomerProductModel()
        {   
            
        }

        [NopResourceDisplayName("Product.Fields.Name")]
        public string Name { get; set; }

        public bool ProductPublished { get; set; }

        public string FormattedCustomProductAttributes { get; set; }
        public IList<ProductAttributeModel> CustomProductAttributes { get; set; }

    }
}