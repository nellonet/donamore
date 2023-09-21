using Nop.Core.Configuration;

namespace Nop.Core.Domain.Common
{
    /// <summary>
    /// Address settings
    /// </summary>
    public partial class ProductSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether 'Product' is enabled
        /// </summary>
        public bool ProductEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 'Product' is published
        /// </summary>
        public bool ProductPubblished { get; set; }        
    }
}