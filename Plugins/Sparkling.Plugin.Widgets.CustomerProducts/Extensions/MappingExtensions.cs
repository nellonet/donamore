using Nop.Core.Domain.Catalog;
using Sparkling.Plugin.Widgets.CustomerProducts.Models.CustomerProduct;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Extensions
{
    public static class MappingExtensions
    {
        public static Product ToEntity(this CustomerProductsModel model, bool trimFields = true)
        {
            if (model == null)
                return null;

            var entity = new Product();
            return ToEntity(model, entity, trimFields);
        }

        public static Product ToEntity(this CustomerProductsModel model, Product destination, bool trimFields = true)
        {
            if (model == null)
                return destination;

            if (trimFields)
            {
                if (model.Name != null)
                    model.Name = model.Name.Trim();
            }
            destination.Id = model.Id;
            destination.Name = model.Name;
            destination.ShortDescription = model.ShortDescription;
            destination.FullDescription = model.FullDescription;

            return destination;
        }
    }
}
