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

            if (filter.SellerId != null && filter.SellerId != 0)
            {
                query = query.Where(s => s.SellerId == filter.SellerId.Value);
            }

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
                Price= product.Price,
                ProductColors = product.ProductColors.Select(c => new CreateProductColorDTO()
                {
                    Price = c.Price,
                    ColorName = c.ColorName,
                }).ToList(),
                ProductSizes = product.ProductSizes.Select(s => new CreateProductSizeDTO()
                {
                    Size = s.Size,
                    Count = s.Count
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

            return EditProductResult.Success;
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

        #endregion
    }
}
