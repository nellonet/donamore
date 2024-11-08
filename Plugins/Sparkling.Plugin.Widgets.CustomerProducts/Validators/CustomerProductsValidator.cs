using FluentValidation;
using Nop.Core.Domain.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Sparkling.Plugin.Widgets.CustomerProducts.Models.CustomerProduct;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Validators
{
    internal class CustomerProductsValidator : BaseNopValidator<CustomerProductsModel>
    {
        public CustomerProductsValidator(ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,
            CustomerProductsSettings customerProductsSettings,
            CustomerSettings customerSettings)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessageAwait(localizationService.GetResourceAsync("Plugin.Widgets.CustomerProducts.Catalog.Products.Fields.Name.Required"));

            if (customerProductsSettings.ShortDescriptionRequired && customerProductsSettings.ShortDescriptionEnabled)
            {
                RuleFor(x => x.ShortDescription).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Plugin.Widgets.CustomerProducts.Catalog.Products.Fields.ShortDescription.Required"));
            }

        }
    }
}
