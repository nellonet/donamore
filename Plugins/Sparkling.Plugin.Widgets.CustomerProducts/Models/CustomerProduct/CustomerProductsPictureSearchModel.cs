using Nop.Web.Framework.Models;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Models.CustomerProduct
{
    public partial record CustomerProductsPictureSearchModel : BaseSearchModel
    {
        #region Properties

        public int ProductId { get; set; }
        
        #endregion
    }
}