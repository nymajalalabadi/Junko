using Junko.Application.Extensions;
using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.Products;
using Junko.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.Seller.Controllers
{
    public class ProductController : SellerBaseController
    {
        #region constructor

        private readonly IProductService _productService;

        private readonly ISellerService _sellerService;

        public ProductController(IProductService productService, ISellerService sellerService)
        {
            _productService = productService;
            _sellerService = sellerService;
        }

        #endregion

        #region list

        [HttpGet("products-list")]
        public async Task<IActionResult> Index(FilterProductDTO filter)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());

            filter.SellerId = seller.Id;

            filter.FilterProductState = FilterProductState.All;

            var model = await _productService.FilterProducts(filter);

            return View(model);
        }

        #endregion

        #region create product

        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.Categories = await _productService.GetAllActiveProductCategories();

            return View();
        }

        [HttpPost("create-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductDTO product)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());

                var result = await _productService.CreateProduct(product, seller.Id);

                switch (result)
                {
                    case CreateProductResult.HasNoImage:
                        TempData[WarningMessage] = "لطفا تصویر محصول را وارد نمایید";
                        break;

                    case CreateProductResult.Error:
                        TempData[ErrorMessage] = "عملیات ثبت محصول با خطا مواجه شد";
                        break;

                    case CreateProductResult.Success:
                        TempData[SuccessMessage] = $"محصول مورد نظر با عنوان {product.Title} با موفقیت ثبت شد";
                        return RedirectToAction("Index");
                }
            }

            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }

        #endregion

        #region edit product

        [HttpGet("edit-product/{productId}")]
        public async Task<IActionResult> EditProduct(long productId)
        {
            var product = await _productService.GetProductForEdit(productId);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _productService.GetAllActiveProductCategories();

            return View(product);
        }

        [HttpPost("edit-product/{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductDTO product)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.EditSellerProduct(product, User.GetUserId());

                switch (res)
                {
                    case EditProductResult.NotForUser:
                        TempData[ErrorMessage] = "در ویرایش اطلاعات خطایی رخ داد";
                        break;
                    case EditProductResult.NotFound:
                        TempData[WarningMessage] = "اطلاعات وارد شده یافت نشد";
                        break;
                    case EditProductResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                        return RedirectToAction("Index");
                }
            }

            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }

        #endregion

        #region product galleries

        #region list

        [HttpGet("product-galleries/{id}")]
        public async Task<IActionResult> GetProductGalleries(long id)
        {
            ViewBag.productId = id;

            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());

            var model = await _productService.GetAllProductGalleriesInSellerPanel(id, seller!.Id);

            return View(model);
        }

        #endregion

        #region Create Product Gallery

        [HttpGet("create-product-gallery/{productId}")]
        public async Task<IActionResult> CreateProductGallery(long productId)
        {
            var product = await _productService.GetProductBySellerOwnerId(productId, User.GetUserId());

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.product = product;

            return View();
        }

        [HttpPost("create-product-gallery/{productId}")]
        public async Task<IActionResult> CreateProductGallery(long productId, CreateProductGalleryDTO gallery)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());

                var result = await _productService.CreateProductGallery(gallery, productId, seller!.Id);

                switch (result)
                {
                    case CreateProductGalleryResult.ImageIsNull:
                        TempData[WarningMessage] = "تصویر مربوطه را وارد نمایید";
                        break;
                    case CreateProductGalleryResult.NotForUserProduct:
                        TempData[ErrorMessage] = "محصول مورد نظر در لیست محصولات شما یافت نشد";
                        break;
                    case CreateProductGalleryResult.ProductNotFound:
                        TempData[WarningMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case CreateProductGalleryResult.Success:
                        TempData[SuccessMessage] = "عملیات ثبت گالری محصول با موفقیت انجام شد";
                        return RedirectToAction("GetProductGalleries", "Product", new { id = productId });
                }
            }

            var product = await _productService.GetProductBySellerOwnerId(productId, User.GetUserId());

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.product = product;

            return View(gallery);
        }


        #endregion

        #region Edit Product Gallery

        [HttpGet("product_{productId}/edit-product-gallery/{galleryId}")]
        public async Task<IActionResult> EditProductGallery(long productId, long galleryId)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());

            var mainGallery = await _productService.GetProductGalleryForEdit(galleryId, seller.Id);

            if (mainGallery == null)
            {
                return NotFound();
            }

            return View(mainGallery);
        }

        [HttpPost("product_{productId}/edit-product-gallery/{galleryId}")]
        public async Task<IActionResult> EditProductGallery(long productId, long galleryId, EditProductGalleryDTO gallery)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());

                var result = await _productService.EditProductGallery(galleryId, seller!.Id, gallery);

                switch (result)
                {
                    case EditProductGalleryResult.ProductNotFound:
                        TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد";
                        break;
                    case EditProductGalleryResult.NotForUserProduct:
                        TempData[ErrorMessage] = "این اطلاعات برای شما غیر قابل دسترس می باشد";
                        break;
                    case EditProductGalleryResult.Success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ویرایش شد";
                        return RedirectToAction("GetProductGalleries", "Product", new { id = productId });
                }
            }
            return View(gallery);
        }

        #endregion


        #endregion

    }
}
