using System.Linq;
using FluentValidation;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Web.Models.Customer;

namespace Nop.Web.Validators.Customer
{
    public partial class ProductValidator : BaseNopValidator<CustomerProductModel>
    {
        public ProductValidator(ILocalizationService localizationService,
            IStateProvinceService stateProvinceService,            
            ProductSettings productSettings,
            CustomerSettings customerSettings)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessageAwait(localizationService.GetResourceAsync("custom.products.fields.name.required"));                        
            
            if (productSettings.ProductPubblished && productSettings.ProductEnabled)
            {
                //RuleFor(x => x.Company).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Account.Fields.Company.Required"));
            }

            if (productSettings.ShortDescriptionRequired && productSettings.ShortDescriptionEnabled)
            {
                RuleFor(x => x.ShortDescription).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("custom.products.fields.shortdescription.required"));
            }

        }
    }
}