﻿using Junko.Domain.Entities.Products;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Junko.Domain.InterFaces
{
    public interface IProductRepository
    {
        #region Methods

        #region Product

        Task<IQueryable<Product>> GetProductQuery();

        Task<Product?> GetProductById(long id);

        Task<Product?> GetProductForOrder(long id);

        Task<Product?> GetProductForShow(long id);

        Task<Product?> GetProductByUserId(long productId, long userId);

        Task<Product?> GetProductForEdit(long id);

        Task<List<Product>> GetRelatedProducts(long productId , List<long> ProductCategoryId);

        Task<List<SelectListItem>> FilterProductsForSellerByProductName(long sellerId);

        void UpdateProduct(Product product);    

        Task AddProduct(Product product);

        #endregion

        #region product gallery

        Task<ProductGallery?> GetGalleryById(long galleryId, long sellerId);

        Task<List<ProductGallery>> GetAllProductGalleries(long productId);

        Task<List<ProductGallery>> GetAllProductGalleriesInSellerPanel(long productId, long sellerId);

        Task AddProductGallery(ProductGallery productGallery);

        void UpdateProductGallery(ProductGallery productGallery);

        #endregion

        #region Product Category

        Task<List<ProductCategory>> GetAllProductCategories();

        Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long parentId);

        Task AddRangeProductSelectedCategorys(List<ProductSelectedCategory> productSelectedCategories);

        Task RemoveAllProductSelectedCategories(long productId);

        #endregion

        #region Product Color

        Task AddRangeProductColors(List<ProductColor> productColors);

        Task RemoveAllProductSelectedColors(long productId);

        #endregion

        #region Product Size

        Task AddRangeProductSizes(List<ProductSize> productSizes);

        Task RemoveAllProductSelectedSizes(long productId);

        #endregion

        #region Product Feature

        Task AddRangeProductFeatures(List<ProductFeature> productFeatures);

        Task RemoveAllProductSelectedFeatures(long productId);

        Task<List<ProductFeature>> GetProductFeaturesByProductId(long productId);

        #endregion

        Task SaveChanges();

        #endregion
    }
}
