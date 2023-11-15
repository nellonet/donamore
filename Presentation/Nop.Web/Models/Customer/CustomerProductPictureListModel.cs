using Nop.Web.Framework.Models;
using Nop.Web.Models.Catalog;

namespace Nop.Web.Models.Customer
{
    /// <summary>
    /// Represents a product picture list model
    /// </summary>
    public partial record CustomerProductPictureListModel : BasePagedListModel<CustomerProductPictureModel>
    {
    }
}