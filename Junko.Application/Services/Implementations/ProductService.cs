using Junko.Application.Generators;
using Junko.Application.Services.Interfaces;
using Junko.Application.Statics;
using Junko.Domain.Entities.Account;
using Junko.Domain.Entities.Products;
using Junko.Domain.InterFaces;
using Junko.Domain.ViewModels.Products;
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
                product.AvatarImage.AddImageToServer(imageName, SiteTools.UserAvatarOrigin, 100, 100, SiteTools.ProductImage);

                var newProduct = new Product
                {
                    Title = product.Title,
                    Price = product.Price,
                    ShortDescription = product.ShortDescription,
                    Description = product.Description,
                    IsActive = product.IsActive,
                    SellerId = sellerId,
                    Count = product.Count,
                    ImageName = imageName,
                };

                await _productRepository.AddProduct(newProduct);
                await _productRepository.SaveChanges();

                // create product categories


                // create product colors

                return CreateProductResult.Success;
            }

            return CreateProductResult.Error;
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
