using System.Linq;
using FluentValidation;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Web.Models.Customer;

namespace Nop.Web.Validators.Cuatomer
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
                .WithMessageAwait(localizationService.GetResourceAsync("Product.Fields.Name.Required"));                        
            
            if (productSettings.ProductPubblished && productSettings.ProductEnabled)
            {
                //RuleFor(x => x.Company).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Account.Fields.Company.Required"));
            }
            
        }
    }
}