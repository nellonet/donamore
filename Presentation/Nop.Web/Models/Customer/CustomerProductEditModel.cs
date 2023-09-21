using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Customer
{
    public partial record CustomerProductEditModel : BaseNopModel
    {
        public CustomerProductEditModel()
        {
            Product = new CustomerProductModel();
        }
        
        public CustomerProductModel Product { get; set; }
    }
}