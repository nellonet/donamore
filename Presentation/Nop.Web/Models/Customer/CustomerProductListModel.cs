using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Customer
{
    public partial record CustomerProductListModel : BaseNopModel
    {
        public CustomerProductListModel()
        {
            Products = new List<CustomerProductModel>();
        }

        public IList<CustomerProductModel> Products { get; set; }
    }
}