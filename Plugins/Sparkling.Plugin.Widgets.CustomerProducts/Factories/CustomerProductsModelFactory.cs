using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models.Extensions;
using Sparkling.Plugin.Widgets.CustomerProducts.Models.CustomerProduct;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Factories
{
    public partial class CustomerProductsModelFactory : ICustomerProductsModelFactory
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IPictureService _pictureService;
        private readonly IProductService _productService;
        private readonly IVideoService _videoService;
        private readonly IWorkContext _workContext;
        private readonly CustomerProductsSettings _customerProductsSettings;

        #endregion

        #region Ctor

        public CustomerProductsModelFactory(ICustomerService customerService,
            IPictureService pictureService,
            IProductService productService,
            IVideoService videoService,
            IWorkContext workContext,
            CustomerProductsSettings customerProductsSettings)
        {
            _customerService = customerService;
            _pictureService = pictureService;
            _productService = productService;
            _videoService = videoService;
            _workContext = workContext;
            _customerProductsSettings = customerProductsSettings;
        }

        #endregion

        #region Methods

        public virtual async Task<CustomerProductsListModel> PrepareCustomerProductsListModelAsync()
        {
            var customer = await _workContext.GetCurrentCustomerAsync();

            var products = await (await _customerService.GetProductsByCustomerIdAsync(customer.Id)).ToListAsync();

            var model = new CustomerProductsListModel();

            foreach (var product in products)
            {
                var customerProductsModel = new CustomerProductsModel();

                await PrepareCustomerProductsModelAsync(customerProductsModel,
                    product: product,
                    excludeProperties: false,
                    customerProductsSettings: _customerProductsSettings);

                model.Products.Add(customerProductsModel);
            }
            return model;
        }

        public virtual async Task PrepareCustomerProductsModelAsync(CustomerProductsModel model, Product product, bool excludeProperties, 
            CustomerProductsSettings customerProductsSettings, bool prePopulateWithCustomerFields = false, Customer customer = null, string overrideAttributesXml = "")
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!excludeProperties && product != null)
            {
                model.Id = product.Id;
                model.Name = product.Name;
                model.ShortDescription = product.ShortDescription;
                model.FullDescription = product.FullDescription;
            }

            if (product == null && prePopulateWithCustomerFields)
            {
                if (customer == null)
                    throw new Exception("Customer cannot be null when prepopulating a Campaign");
            }

            model.ProductPublished = false;
            model.ShortDescriptionRequired = true;

            if (product != null)
            {
                var prova = await _workContext.GetCurrentVendorAsync();
            }
        }

        public virtual async Task<ProductPictureListModel> PrepareCustomerProductsPictureListModelAsync(ProductPictureSearchModel searchModel, 
            Product product)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var productPictures = (await _productService.GetProductPicturesByProductIdAsync(product.Id)).ToPagedList(searchModel);

            var model = await new ProductPictureListModel().PrepareToGridAsync(searchModel, productPictures, () =>
            {
                return productPictures.SelectAwait(async productPicture =>
                {
                    var productPictureModel = productPicture.ToModel<ProductPictureModel>();

                    var picture = (await _pictureService.GetPictureByIdAsync(productPicture.PictureId))
                        ?? throw new Exception("Picture cannot be loaded");

                    productPictureModel.PictureUrl = (await _pictureService.GetPictureUrlAsync(picture)).Url;

                    productPictureModel.OverrideAltAttribute = picture.AltAttribute;
                    productPictureModel.OverrideTitleAttribute = picture.TitleAttribute;

                    return productPictureModel;
                });
            });

            return model;
        }

        public virtual async Task<ProductVideoListModel> PrepareCustomerProductsVideoListModelAsync(ProductVideoSearchModel searchModel, 
            Product product)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            //get product videos
            var productVideos = (await _productService.GetProductVideosByProductIdAsync(product.Id)).ToPagedList(searchModel);

            //prepare grid model
            var model = await new ProductVideoListModel().PrepareToGridAsync(searchModel, productVideos, () =>
            {
                return productVideos.SelectAwait(async productVideo =>
                {
                    var productVideoModel = productVideo.ToModel<ProductVideoModel>();

                    var video = (await _videoService.GetVideoByIdAsync(productVideo.VideoId))
                        ?? throw new Exception("Video cannot be loaded");

                    productVideoModel.VideoUrl = video.VideoUrl;

                    return productVideoModel;
                });
            });

            return model;
        }

        #endregion
    }
}
