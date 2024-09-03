using Junko.Application.Convertor;
using Junko.Application.Generators;
using Junko.Application.Services.Interfaces;
using Junko.Application.Statics;
using Junko.DataLayer.Repositories;
using Junko.Domain.Entities.Account;
using Junko.Domain.Entities.Products;
using Junko.Domain.Entities.Store;
using Junko.Domain.InterFaces;
using Junko.Domain.ViewModels.Products;
using Junko.Domain.ViewModels.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region consractor

        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        #endregion

        #region Methods

        #region product

        public async Task<FilterProductDTO> FilterProducts(FilterProductDTO filter)
        {
            var query = await _productRepository.GetProductQuery();

            #region filter

            if (!string.IsNullOrEmpty(filter.ProductTitle))
            {
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filter.ProductTitle}%"));
            }

            if (!string.IsNullOrEmpty(filter.FilterByCategory))
            {
                query = query.Where(p => p.ProductSelectedCategories.Any(s => s.ProductCategory.UrlName == filter.FilterByCategory));
            }

            if (filter.SellerId != null && filter.SellerId != 0)
            {
                query = query.Where(s => s.SellerId == filter.SellerId.Value);
            }

            #region price

            if (!query.IsNullOrEmpty())
            {
                var expensiveProduct = await query.OrderByDescending(s => s.Price).FirstOrDefaultAsync();
                filter.FilterMaxPrice = expensiveProduct.Price;

                if (filter.SelectedMaxPrice == 0)
                {
                    filter.SelectedMaxPrice = expensiveProduct.Price;
                }

                query = query.Where(s => s.Price >= filter.SelectedMinPrice);
                query = query.Where(s => s.Price <= filter.SelectedMaxPrice);
            }

            #endregion

            #endregion

            #region state

            switch (filter.FilterProductState)
            {
                case FilterProductState.All:
                    break;

                case FilterProductState.Active:
                    query = query.Where(s => s.IsActive && s.ProductAcceptanceState == ProductAcceptanceState.Accepted);
                    break;

                case FilterProductState.NotActive:
                    query = query.Where(s => !s.IsActive && s.ProductAcceptanceState == ProductAcceptanceState.Accepted);
                    break;

                case FilterProductState.Accepted:
                    query = query.Where(s => s.ProductAcceptanceState == ProductAcceptanceState.Accepted);
                    break;

                case FilterProductState.Rejected:
                    query = query.Where(s => s.ProductAcceptanceState == ProductAcceptanceState.Rejected);
                    break;

                case FilterProductState.UnderProgress:
                    query = query.Where(s => s.ProductAcceptanceState == ProductAcceptanceState.UnderProgress);
                    break;
            }

            switch (filter.FilterProductOrderBy)
            {
                case FilterProductOrderBy.CreateData_Des:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
                case FilterProductOrderBy.CreateDate_Asc:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterProductOrderBy.Price_Des:
                    query = query.OrderByDescending(s => s.Price);
                    break;
                case FilterProductOrderBy.Price_Asc:
                    query = query.OrderBy(s => s.Price);
                    break;
            }

            #endregion

            #region paging

            await filter.SetPaging(query);

            #endregion

            return filter;
        }

        public async Task<CreateProductResult> CreateProduct(CreateProductDTO product, long sellerId)
        {
            if (product.AvatarImage != null && product.AvatarImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(product.AvatarImage.FileName);
                product.AvatarImage.AddImageToServer(imageName, SiteTools.ProductImage, 100, 100, SiteTools.ProductThumbImage);

                var newProduct = new Product
                {
                    Title = product.Title,
                    Price = product.Price,
                    ShortDescription = product.ShortDescription,
                    Description = product.Description,
                    IsActive = product.IsActive,
                    SellerId = sellerId,
                    ImageName = imageName,
                    ProductAcceptanceState = ProductAcceptanceState.UnderProgress
                };

                await _productRepository.AddProduct(newProduct);
                await _productRepository.SaveChanges();

                #region create product categories

                var productSelectedCategories = new List<ProductSelectedCategory>();

                foreach (var categoryId in product.SelectedCategories)
                {
                    productSelectedCategories.Add(new ProductSelectedCategory()
                    {
                        ProductCategoryId = categoryId,
                        ProductId = newProduct.Id
                    });
                }

                await _productRepository.AddRangeProductSelectedCategorys(productSelectedCategories);
                await _productRepository.SaveChanges();

                #endregion


                #region create product colors

                var productSelectedColors = new List<ProductColor>();

                foreach (var color in product.ProductColors)
                {
                    productSelectedColors.Add(new ProductColor()
                    {
                        ColorName = color.ColorName,
                        ColorCode = color.ColorCode,
                        Price = color.Price,
                        ProductId = newProduct.Id
                    });
                };

                await _productRepository.AddRangeProductColors(productSelectedColors);
                await _productRepository.SaveChanges();

                #endregion

                #region create product sizes

                var productSelectedSizes = new List<ProductSize>();

                foreach (var size in product.ProductSizes)
                {
                    productSelectedSizes.Add(new ProductSize()
                    {
                        Size = size.Size,
                        Count = size.Count,
                        ProductId = newProduct.Id
                    });
                }

                await _productRepository.AddRangeProductSizes(productSelectedSizes);
                await _productRepository.SaveChanges();

                #endregion

                #region create product feature

                var productFeatures = new List<ProductFeature>();

                foreach (var feature in product.ProductFeatures)
                {
                    productFeatures.Add(new ProductFeature()
                    {
                        FeatureTitle = feature.FeatureTitle,
                        FeatureValue = feature.FeatureValue,
                        ProductId = newProduct.Id
                    });
                }

                await _productRepository.AddRangeProductFeatures(productFeatures);
                await _productRepository.SaveChanges();

                #endregion

                return CreateProductResult.Success;
            }

            return CreateProductResult.HasNoImage;
        }

        public async Task<bool> AcceptSellerProduct(long productId)
        {
            var product = await _productRepository.GetProductById(productId);

            if (product == null)
            {
                return false;
            }

            product.ProductAcceptanceState = ProductAcceptanceState.Accepted;
            product.ProductAcceptOrRejectDescription = $"محصول مورد نظر در تاریخ {DateTime.Now.ToShamsi()} مورد تایید سایت قرار گرفت";

            _productRepository.UpdateProduct(product);
            await _productRepository.SaveChanges();

            return true;
        }

        public async Task<bool> RejectProductRequest(RejectItemDTO reject)
        {
            var productRequest = await _productRepository.GetProductById(reject.Id);

            if (productRequest == null)
            {
                return false;
            }

            productRequest.ProductAcceptanceState = ProductAcceptanceState.Rejected;
            productRequest.ProductAcceptOrRejectDescription = reject.RejectMessage;

            _productRepository.UpdateProduct(productRequest);
            await _productRepository.SaveChanges();

            return true;
        }

        public async Task<EditProductDTO> GetProductForEdit(long productId)
        {
            var product = await _productRepository.GetProductForEdit(productId);

            if (product == null)
            {
                return null;
            }

            return new EditProductDTO()
            {
                Id = product.Id,
                Description = product.Description,
                Title = product.Title,
                ShortDescription = product.ShortDescription,
                ImageName = product.ImageName,
                IsActive = product.IsActive,
                Price = product.Price,
                ProductColors = product.ProductColors.Select(c => new CreateProductColorDTO()
                {
                    Price = c.Price,
                    ColorName = c.ColorName,
                    ColorCode = c.ColorCode,
                }).ToList(),
                ProductSizes = product.ProductSizes.Select(s => new CreateProductSizeDTO()
                {
                    Size = s.Size,
                    Count = s.Count
                }).ToList(),
                ProductFeatures = product.ProductFeatures.Select(s => new CreateProductFeatureDTO()
                {
                    FeatureTitle = s.FeatureTitle,
                    FeatureValue = s.FeatureValue
                }).ToList(),
                SelectedCategories = product.ProductSelectedCategories.Where(s => s.ProductId == product.Id)
                .Select(c => c.ProductCategoryId).ToList(),
            };
        }

        public async Task<EditProductResult> EditSellerProduct(EditProductDTO product, long userId)
        {
            var currentProduct = await _productRepository.GetProductForEdit(product.Id);

            if (currentProduct == null)
            {
                return EditProductResult.NotFound;
            }

            if (currentProduct.Seller.UserId != userId)
            {
                return EditProductResult.NotForUser;
            }

            currentProduct.Title = product.Title;
            currentProduct.ShortDescription = product.ShortDescription;
            currentProduct.Description = product.Description;
            currentProduct.IsActive = product.IsActive;
            currentProduct.Price = product.Price;
            currentProduct.ProductAcceptanceState = ProductAcceptanceState.UnderProgress;

            if (product.AvatarImage != null)
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(product.AvatarImage.FileName);
                product.AvatarImage.AddImageToServer(imageName, SiteTools.ProductImage, 100, 100, SiteTools.ProductThumbImage, currentProduct.ImageName);

                currentProduct.ImageName = imageName;
            }

            _productRepository.UpdateProduct(currentProduct);

            #region selected product categories

            await _productRepository.RemoveAllProductSelectedCategories(currentProduct.Id);

            var productSelectedCategories = new List<ProductSelectedCategory>();

            foreach (var categoryId in product.SelectedCategories)
            {
                productSelectedCategories.Add(new ProductSelectedCategory()
                {
                    ProductCategoryId = categoryId,
                    ProductId = currentProduct.Id
                });
            }

            await _productRepository.AddRangeProductSelectedCategorys(productSelectedCategories);
            await _productRepository.SaveChanges();

            #endregion

            #region color product

            await _productRepository.RemoveAllProductSelectedColors(currentProduct.Id);

            var productSelectedColors = new List<ProductColor>();

            foreach (var color in product.ProductColors)
            {
                productSelectedColors.Add(new ProductColor()
                {
                    ColorName = color.ColorName,
                    ColorCode = color.ColorCode,
                    Price = color.Price,
                    ProductId = currentProduct.Id
                });
            }

            await _productRepository.AddRangeProductColors(productSelectedColors);
            await _productRepository.SaveChanges();

            #endregion

            #region size product

            await _productRepository.RemoveAllProductSelectedSizes(currentProduct.Id);

            var productSelectedSizes = new List<ProductSize>();

            foreach (var size in product.ProductSizes)
            {
                productSelectedSizes.Add(new ProductSize()
                {
                    Size = size.Size,
                    Count = size.Count,
                    ProductId = currentProduct.Id
                });
            }

            await _productRepository.AddRangeProductSizes(productSelectedSizes);
            await _productRepository.SaveChanges();

            #endregion

            #region feature product

            await _productRepository.RemoveAllProductSelectedFeatures(currentProduct.Id);

            var productSelectedFeatures = new List<ProductFeature>();

            foreach (var feature in product.ProductFeatures)
            {
                productSelectedFeatures.Add(new ProductFeature()
                {
                    FeatureTitle = feature.FeatureTitle,
                    FeatureValue = feature.FeatureValue,
                    ProductId = currentProduct.Id
                });
            }

            await _productRepository.AddRangeProductFeatures(productSelectedFeatures);
            await _productRepository.SaveChanges();

            #endregion

            return EditProductResult.Success;
        }

        public async Task<Product?> GetProductBySellerOwnerId(long productId, long userId)
        {
            return await _productRepository.GetProductByUserId(productId, userId);
        }

        public async Task<ProductDetailDTO?> GetProductDetailById(long productId)
        {
            var product = await _productRepository.GetProductForShow(productId);

            if (product == null)
            {
                return null;
            }

            return new ProductDetailDTO()
            {
                Title = product.Title,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                ImageName = product.ImageName,
                Price = product.Price,
                SellerId = product.SellerId,
                Seller = product.Seller,
                ProductColors = product.ProductColors.ToList(),
                ProductSizes = product.ProductSizes.ToList(),
                ProductGalleries = product.ProductGalleries.ToList(),
                ProductFeatures = product.ProductFeatures.ToList(),
                ProductCategories = product.ProductSelectedCategories.Select(s => s.ProductCategory).ToList()
            };
        }

        #endregion

        #region product gallery

        public async Task<List<ProductGallery>> GetAllProductGalleries(long productId)
        {
            return await _productRepository.GetAllProductGalleries(productId);
        }

        public async Task<List<ProductGallery>> GetAllProductGalleriesInSellerPanel(long productId, long sellerId)
        {
            return await _productRepository.GetAllProductGalleriesInSellerPanel(productId, sellerId);
        }

        public async Task<CreateProductGalleryResult> CreateProductGallery(CreateProductGalleryDTO gallery, long productId, long sellerId)
        {
            var product = await _productRepository.GetProductById(productId);

            if (product == null)
            {
                return CreateProductGalleryResult.ProductNotFound;
            }

            if (product.SellerId != sellerId)
            {
                return CreateProductGalleryResult.NotForUserProduct;
            }

            if (gallery.AvatarImage == null || !gallery.AvatarImage.IsImage())
            {
                return CreateProductGalleryResult.ImageIsNull;
            }

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(gallery.AvatarImage.FileName);

            gallery.AvatarImage.AddImageToServer(imageName, SiteTools.ProductGalleryImage, 100, 100,
                SiteTools.ProductGalleryThumbImage);

            var productGallery = new ProductGallery()
            {
                ProductId = productId,
                ImageName = imageName,
                DisplayPriority = gallery.DisplayPriority
            };

            await _productRepository.AddProductGallery(productGallery);
            await _productRepository.SaveChanges();

            return CreateProductGalleryResult.Success;
        }

        public async Task<EditProductGalleryDTO> GetProductGalleryForEdit(long galleryId, long sellerId)
        {
            var currentGallery = await _productRepository.GetGalleryById(galleryId, sellerId);

            if (currentGallery == null)
            {
                return null;
            }

            return new EditProductGalleryDTO
            {
                ImageName = currentGallery.ImageName,
                DisplayPriority = currentGallery.DisplayPriority
            };
        }

        public async Task<EditProductGalleryResult> EditProductGallery(long galleryId, long sellerId, EditProductGalleryDTO gallery)
        {
            var currentGallery = await _productRepository.GetGalleryById(galleryId, sellerId);

            if (currentGallery == null)
            {
                return EditProductGalleryResult.ProductNotFound;
            }

            if (currentGallery.Product.SellerId != sellerId)
            {
                return EditProductGalleryResult.NotForUserProduct;
            }

            if (gallery.Image != null && gallery.Image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(gallery.Image.FileName);

                gallery.Image.AddImageToServer(imageName, SiteTools.ProductGalleryImage, 100, 100,
                     SiteTools.ProductGalleryThumbImage, currentGallery.ImageName);

                currentGallery.ImageName = imageName;
            }

            currentGallery.DisplayPriority = gallery.DisplayPriority;

            _productRepository.UpdateProductGallery(currentGallery);
            await _productRepository.SaveChanges();

            return EditProductGalleryResult.Success;
        }

        #endregion

        #region product categories

        public async Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long parentId)
        {
            return await _productRepository.GetAllProductCategoriesByParentId(parentId);
        }

        public async Task<List<ProductCategory>> GetAllActiveProductCategories()
        {
            return await _productRepository.GetAllProductCategories();
        }

        #endregion

        #region product feature

        public async Task CreateProductFeatures(List<CreateProductFeatureDTO> features)
        {
            var newFeatures = new List<ProductFeature>();

            if (features.Any() && features != null)
            {
                foreach (var feature in features)
                {
                    newFeatures.Add(new ProductFeature()
                    {
                        FeatureValue = feature.FeatureValue,
                        FeatureTitle = feature.FeatureTitle,
                    });
                }

                await _productRepository.AddRangeProductFeatures(newFeatures);
                await _productRepository.SaveChanges();
            }

        }

        public async Task RemoveAllProductFeatures(long productId)
        {
            var productFeatures = await _productRepository.GetProductFeaturesByProductId(productId);

            _productRepository.RemoveRangeProductFeatures(productFeatures);
        }

        #endregion

        #endregion
    }
}
