using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Sparkling.Plugin.Widgets.CustomerProducts.Factories;
using Sparkling.Plugin.Widgets.CustomerProducts.Models.CustomerProduct;
using Sparkling.Plugin.Widgets.CustomerProducts.Validators;
using Sparkling.Plugin.Widgets.CustomerProducts.ViewEngine;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Infrastructure
{
    public partial class NopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //factories
            services.AddScoped<ICustomerProductsModelFactory, CustomerProductsModelFactory>();

            //validators
            services.AddTransient<IValidator<CustomerProductsModel>, CustomerProductsValidator>();
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new CustomerProductsViewEngine());
            });
        }

        public void Configure(IApplicationBuilder application)
        {
        }

        public int Order => int.MaxValue;
    }
}
