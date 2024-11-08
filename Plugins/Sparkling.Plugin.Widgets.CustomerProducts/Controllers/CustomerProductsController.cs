using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Media;
using Nop.Core.Http;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Controllers;
using Nop.Web.Extensions;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Validators;
using Sparkling.Plugin.Widgets.CustomerProducts.Extensions;
using Sparkling.Plugin.Widgets.CustomerProducts.Factories;
using Sparkling.Plugin.Widgets.CustomerProducts.Models.CustomerProduct;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sparkling.Plugin.Widgets.CustomerProducts.Controllers
{
    public partial class CustomerProductsController : BasePublicController
    {
        #region Fields

        private readonly ICustomerProductsModelFactory _customerProductsModelFactory;
        private readonly ICustomerService _customerService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPictureService _pictureService;
        private readonly IProductService _productService;
        private readonly IVideoService _videoService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly CustomerProductsSettings _customerProductsSettings;

        #endregion

        #region Ctor

        public CustomerProductsController(ICustomerProductsModelFactory customerProductsModelFactory,
            ICustomerService customerService,
            IHttpClientFactory httpClientFactory,
            ILocalizationService localizationService,
            INotificationService notificationService,
            IPictureService pictureService,
            IProductService productService,
            IVideoService videoService,
            IWebHelper webHelper,
            IWorkContext workContext,
            CustomerProductsSettings customerProductsSettings)
        {
            _customerProductsModelFactory = customerProductsModelFactory;
            _customerService = customerService;
            _httpClientFactory = httpClientFactory;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _pictureService = pictureService;
            _productService = productService;
            _videoService = videoService;
            _webHelper = webHelper;
            _workContext = workContext;
            _customerProductsSettings = customerProductsSettings;
        }

        #endregion

        #region Utilities

        protected async Task SaveVideoUrl(string videoUrl, int productId, int displayOrder)
        {
            var video = new Video
            {
                VideoUrl = videoUrl
            };

            await _videoService.InsertVideoAsync(video);

            await _productService.InsertProductVideoAsync(new ProductVideo
            {
                VideoId = video.Id,
                ProductId = productId,
                DisplayOrder = displayOrder
            });
        }

        protected virtual async Task PingVideoUrlAsync(string videoUrl)
        {
            var path = videoUrl.StartsWith("/") ? $"{_webHelper.GetStoreLocation()}{videoUrl.TrimStart('/')}" : videoUrl;

            var client = _httpClientFactory.CreateClient(NopHttpDefaults.DefaultHttpClient);
            await client.GetStringAsync(path);
        }

        #endregion

        #region Methods

        public virtual async Task<IActionResult> Products()
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Challenge();

            var model = await _customerProductsModelFactory.PrepareCustomerProductsListModelAsync();

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProductDelete(int productId)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            var product = await _customerService.GetCustomerProductAsync(customer.Id, productId);
            if (product != null)
                await _productService.DeleteProductAsync(product);

            return Json(new
            {
                redirect = Url.RouteUrl("CustomerProducts"),
            });
        }

        public virtual async Task<IActionResult> ProductAdd()
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Challenge();

            var model = new CustomerProductsEditModel();
            await _customerProductsModelFactory.PrepareCustomerProductsModelAsync(model.Product, product: null,
                excludeProperties: false, customerProductsSettings: _customerProductsSettings);

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProductAdd(CustomerProductsEditModel model, IFormCollection form)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            if (ModelState.IsValid)
            {
                var product = model.Product.ToEntity();
                product.VendorId = customer.Id;
                product.ProductTypeId = 1;
                product.CreatedOnUtc = DateTime.UtcNow;

                await _productService.InsertProductAsync(product);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugin.Widgets.CustomerProducts.Catalog.Products.Added"));

                return RedirectToAction("ProductEdit", new { productId = product.Id });
            }

            return View(model);
        }

        public virtual async Task<IActionResult> ProductEdit(int productId)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            var product = await _customerService.GetCustomerProductAsync(customer.Id, productId);
            if (product == null)
                return RedirectToRoute("CustomerProducts");

            var model = new CustomerProductsEditModel();
            await _customerProductsModelFactory.PrepareCustomerProductsModelAsync(model.Product, product: product,
                excludeProperties: false, customerProductsSettings: _customerProductsSettings);

            return View(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProductEdit(CustomerProductsEditModel model, IFormCollection form)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            var product = await _customerService.GetCustomerProductAsync(customer.Id, model.Product.Id);
            if (product == null)
                return RedirectToRoute("CustomerProducts");

            if (ModelState.IsValid)
            {
                product = model.Product.ToEntity(product);
                await _productService.UpdateProductAsync(product);

                if (model.Product.AddVideoModel != null && string.IsNullOrEmpty(model.Product.AddVideoModel.VideoUrl) == false)
                {
                    await PingVideoUrlAsync(model.Product.AddVideoModel.VideoUrl);
                    await SaveVideoUrl(model.Product.AddVideoModel.VideoUrl, product.Id, model.Product.AddVideoModel.DisplayOrder);
                }

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Plugin.Widgets.CustomerProducts.Catalog.Products.Updated"));

                return RedirectToRoute("CustomerProducts");
            }

            await _customerProductsModelFactory.PrepareCustomerProductsModelAsync(model.Product, product: null,
                excludeProperties: false, customerProductsSettings: _customerProductsSettings);

            return View(model);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public virtual async Task<IActionResult> ProductPictureAdd(int productId, IFormCollection form)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            if (productId == 0)
                throw new ArgumentException();

            var product = await _productService.GetProductByIdAsync(productId)
                ?? throw new ArgumentException("No product found with the specified id");

            var files = form.Files.ToList();
            if (!files.Any())
                return Json(new { success = false });

            var currentVendor = await _workContext.GetCurrentVendorAsync();
            if (currentVendor != null && product.VendorId != currentVendor.Id)
                return RedirectToAction("List");
            try
            {
                foreach (var file in files)
                {
                    //insert picture
                    var picture = await _pictureService.InsertPictureAsync(file);

                    await _pictureService.SetSeoFilenameAsync(picture.Id, await _pictureService.GetPictureSeNameAsync(product.Name));

                    await _productService.InsertProductPictureAsync(new ProductPicture
                    {
                        PictureId = picture.Id,
                        ProductId = product.Id,
                        DisplayOrder = 0
                    });
                }
            }
            catch (Exception exc)
            {
                return Json(new
                {
                    success = false,
                    message = $"{await _localizationService.GetResourceAsync("Plugin.Widgets.CustomerProducts.Catalog.Products.Multimedia.Pictures.Alert.PictureAdd")} {exc.Message}",
                });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProductPictureUpdate(ProductPictureModel model)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            var productPicture = await _productService.GetProductPictureByIdAsync(model.Id)
                ?? throw new ArgumentException("No product picture found with the specified id");

            var currentVendor = await _workContext.GetCurrentVendorAsync();
            if (currentVendor != null)
            {
                var product = await _productService.GetProductByIdAsync(productPicture.ProductId);
                if (product != null && product.VendorId != currentVendor.Id)
                    return Content("This is not your product");
            }

            var picture = await _pictureService.GetPictureByIdAsync(productPicture.PictureId)
                ?? throw new ArgumentException("No picture found with the specified id");

            await _pictureService.UpdatePictureAsync(picture.Id,
                await _pictureService.LoadPictureBinaryAsync(picture),
                picture.MimeType,
                picture.SeoFilename,
                model.OverrideAltAttribute,
                model.OverrideTitleAttribute);

            productPicture.DisplayOrder = model.DisplayOrder;
            await _productService.UpdateProductPictureAsync(productPicture);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProductPictureDelete(int id)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            var productPicture = await _productService.GetProductPictureByIdAsync(id)
                ?? throw new ArgumentException("No product picture found with the specified id");

            var currentVendor = await _workContext.GetCurrentVendorAsync();
            if (currentVendor != null)
            {
                var product = await _productService.GetProductByIdAsync(productPicture.ProductId);
                if (product != null && product.VendorId != currentVendor.Id)
                    return Content("This is not your product");
            }

            var pictureId = productPicture.PictureId;
            await _productService.DeleteProductPictureAsync(productPicture);

            var picture = await _pictureService.GetPictureByIdAsync(pictureId)
                ?? throw new ArgumentException("No picture found with the specified id");

            await _pictureService.DeletePictureAsync(picture);

            return new NullJsonResult();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public virtual async Task<IActionResult> PictureAsyncUpload()
        {
            var httpPostedFile = Request.Form.Files.FirstOrDefault();
            if (httpPostedFile == null)
                return Json(new { success = false, message = "No file uploaded" });

            const string qqFileNameParameter = "qqfilename";

            var qqFileName = Request.Form.ContainsKey(qqFileNameParameter)
                ? Request.Form[qqFileNameParameter].ToString()
                : string.Empty;

            var picture = await _pictureService.InsertPictureAsync(httpPostedFile, qqFileName);

            if (picture == null)
                return Json(new { success = false, message = "Wrong file format" });

            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = (await _pictureService.GetPictureUrlAsync(picture, 100)).Url
            });
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProductPictureList(ProductPictureSearchModel searchModel)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            var product = await _productService.GetProductByIdAsync(searchModel.ProductId)
                ?? throw new ArgumentException("No product found with the specified id");

            var currentVendor = await _workContext.GetCurrentVendorAsync();
            if (currentVendor != null && product.VendorId != currentVendor.Id)
                return Content("This is not your product");

            var model = await _customerProductsModelFactory.PrepareCustomerProductsPictureListModelAsync(searchModel, product);

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProductVideoList(ProductVideoSearchModel searchModel)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            var product = await _productService.GetProductByIdAsync(searchModel.ProductId)
                ?? throw new ArgumentException("No product found with the specified id");

            var currentVendor = await _workContext.GetCurrentVendorAsync();
            if (currentVendor != null && product.VendorId != currentVendor.Id)
                return Content("This is not your product");

            var model = await _customerProductsModelFactory.PrepareCustomerProductsVideoListModelAsync(searchModel, product);

            return Json(model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProductVideoAdd(int productId, [Validate] ProductVideoModel model)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            if (productId == 0 || string.IsNullOrEmpty(model.VideoUrl))
                throw new ArgumentException();

            var product = await _productService.GetProductByIdAsync(productId)
                ?? throw new ArgumentException("No product found with the specified id");

            var videoUrl = model.VideoUrl.TrimStart('~');

            try
            {
                await PingVideoUrlAsync(videoUrl);
            }
            catch (Exception exc)
            {
                return Json(new
                {
                    success = false,
                    error = $"{await _localizationService.GetResourceAsync("Plugin.Widgets.CustomerProducts.Catalog.Products.Multimedia.Videos.Alert.VideoAdd")} {exc.Message}",
                });
            }

            if (!ModelState.IsValid)
                return ErrorJson(ModelState.SerializeErrors());

            var currentVendor = await _workContext.GetCurrentVendorAsync();
            if (currentVendor != null && product.VendorId != currentVendor.Id)
                return RedirectToAction("List");
            try
            {
                await SaveVideoUrl(videoUrl, product.Id, model.DisplayOrder);
            }
            catch (Exception exc)
            {
                return Json(new
                {
                    success = false,
                    error = $"{await _localizationService.GetResourceAsync("Plugin.Widgets.CustomerProducts.Catalog.Products.Multimedia.Videos.Alert.VideoAdd")} {exc.Message}",
                });
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProductVideoUpdate([Validate] ProductVideoModel model)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            var productVideo = await _productService.GetProductVideoByIdAsync(model.Id)
                ?? throw new ArgumentException("No product video found with the specified id");

            var currentVendor = await _workContext.GetCurrentVendorAsync();
            if (currentVendor != null)
            {
                var product = await _productService.GetProductByIdAsync(productVideo.ProductId);
                if (product != null && product.VendorId != currentVendor.Id)
                    return Content("This is not your product");
            }

            var video = await _videoService.GetVideoByIdAsync(productVideo.VideoId)
                ?? throw new ArgumentException("No video found with the specified id");

            var videoUrl = model.VideoUrl.TrimStart('~');

            try
            {
                await PingVideoUrlAsync(videoUrl);
            }
            catch (Exception exc)
            {
                return Json(new
                {
                    success = false,
                    error = $"{await _localizationService.GetResourceAsync("Plugin.Widgets.CustomerProducts.Catalog.Products.Multimedia.Videos.Alert.VideoUpdate")} {exc.Message}",
                });
            }

            video.VideoUrl = videoUrl;

            await _videoService.UpdateVideoAsync(video);

            productVideo.DisplayOrder = model.DisplayOrder;
            await _productService.UpdateProductVideoAsync(productVideo);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProductVideoDelete(int id)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            var productVideo = await _productService.GetProductVideoByIdAsync(id)
                ?? throw new ArgumentException("No product video found with the specified id");

            var currentVendor = await _workContext.GetCurrentVendorAsync();
            if (currentVendor != null)
            {
                var product = await _productService.GetProductByIdAsync(productVideo.ProductId);
                if (product != null && product.VendorId != currentVendor.Id)
                    return Content("This is not your product");
            }

            var videoId = productVideo.VideoId;
            await _productService.DeleteProductVideoAsync(productVideo);

            var video = await _videoService.GetVideoByIdAsync(videoId)
                ?? throw new ArgumentException("No video found with the specified id");

            await _videoService.DeleteVideoAsync(video);

            return new NullJsonResult();
        }

        #endregion
    }
}
