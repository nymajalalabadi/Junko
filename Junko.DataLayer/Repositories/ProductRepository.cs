using Junko.DataLayer.Context;
using Junko.Domain.Entities.Account;
using Junko.Domain.Entities.Products;
using Junko.Domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Junko.DataLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region constractor

        private readonly JunkoDbContext _context;

        public ProductRepository(JunkoDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        #region Product

        public async Task<IQueryable<Product>> GetProductQuery()
        {
            return _context.Products
                .Include(p => p.Seller)
                .Include(c => c.ProductSelectedCategories)
                .ThenInclude(c => c.ProductCategory)
                .Where(p => !p.IsDelete)
                .AsQueryable();
        }

        public async Task<Product?> GetProductById(long id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetProductForOrder(long id)
        {
            return await _context.Products
                .Include(p => p.ProductSizes)
                .Include(p => p.ProductColors)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetProductForShow(long id)
        {
            return await _context.Products
                .Include(p => p.Seller).ThenInclude(s => s.User)
                .Include(p => p.ProductColors)
                .Include(p => p.ProductSizes)
                .Include(p => p.ProductGalleries)
                .Include(p => p.ProductFeatures)
                .Include(p => p.ProductSelectedCategories).ThenInclude(s => s.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product?> GetProductByUserId(long productId, long userId)
        {
            return await _context.Products
                .Include (p => p.Seller)
                .FirstOrDefaultAsync(p => p.Id == productId && p.Seller.UserId == userId);
        }

        public async Task<Product?> GetProductForEdit(long id)
        {
            return await _context.Products
                .Include (p => p.Seller)
                .Include (p => p.ProductColors)
                .Include (p => p.ProductSizes)
                .Include (p => p.ProductFeatures)
                .Include(p => p.ProductSelectedCategories).ThenInclude(p => p.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetRelatedProducts(long productId, List<long> ProductCategoryId)
        {
            return await _context.Products.AsQueryable()
                .Include(p => p.ProductSelectedCategories)
                .Where(p => p.ProductSelectedCategories.Any(s => ProductCategoryId.Contains(s.ProductCategoryId)) 
                && p.Id != productId && p.ProductAcceptanceState == ProductAcceptanceState.Accepted)
                .ToListAsync();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        #endregion

        #region product gallery

        public async Task<ProductGallery?> GetGalleryById(long galleryId, long sellerId)
        {
            return await _context.ProductGalleries
                .Include(g => g.Product)
                .FirstOrDefaultAsync(p => p.Id == galleryId && p.Product.SellerId == sellerId);
        }

        public async Task<List<ProductGallery>> GetAllProductGalleries(long productId)
        {
            return await _context.ProductGalleries.Where(g => g.ProductId.Equals(productId)).ToListAsync();
        }

        public async Task<List<ProductGallery>> GetAllProductGalleriesInSellerPanel(long productId, long sellerId)
        {
            return await _context.ProductGalleries
                .Include(g => g.Product)
                .OrderBy(g => g.DisplayPriority)
                .Where(g => g.ProductId.Equals(productId) && g.Product.SellerId == sellerId)
                .ToListAsync();
        }

        public async Task AddProductGallery(ProductGallery productGallery)
        {
            await _context.ProductGalleries.AddAsync(productGallery);
        }

        public void UpdateProductGallery(ProductGallery productGallery)
        {
            _context.ProductGalleries.Update(productGallery);
        }

        #endregion

        #region Product Category

        public async Task<List<ProductCategory>> GetAllProductCategories()
        {
            return await _context.ProductCategories.AsQueryable()
                .Where(c => !c.IsDelete && c.IsActive)
                .ToListAsync();
        }

        public async Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long parentId)
        {
            return await _context.ProductCategories.AsQueryable()
                .Where(c => !c.IsDelete && c.IsActive && c.ParentId == parentId)
                .ToListAsync();
        }

        public async Task AddRangeProductSelectedCategorys(List<ProductSelectedCategory> productSelectedCategories)
        {
            foreach (var productSelectedCategory in productSelectedCategories)
            {
                await _context.ProductSelectedCategories.AddAsync(productSelectedCategory);
            }

        }

        public async Task RemoveAllProductSelectedCategories(long productId)
        {
            var productSelectedCategories = await _context.ProductSelectedCategories.AsQueryable()
                .Where(s => s.ProductId == productId).ToListAsync();

            foreach (var selected in productSelectedCategories)
            {
                _context.ProductSelectedCategories.Remove(selected);
            }
        }

        #endregion

        #region Product Color

        public async Task AddRangeProductColors(List<ProductColor> productColors)
        {
            foreach (var color in productColors)
            {
                await _context.ProductColors.AddAsync(color);
            }
        }

        public async Task RemoveAllProductSelectedColors(long productId)
        {
            var productSelectedColors = await _context.ProductColors.AsQueryable()
                .Where(s => s.ProductId == productId).ToListAsync();

            foreach (var productColor in productSelectedColors)
            {
                _context.ProductColors.Remove(productColor);
            }
        }

        #endregion

        #region Product Size

        public async Task AddRangeProductSizes(List<ProductSize> productSizes)
        {
            foreach (var size in productSizes)
            {
                await _context.ProductSizes.AddAsync(size);
            }
        }

        public async Task RemoveAllProductSelectedSizes(long productId)
        {
            var productSelectedSizes = await _context.ProductSizes.AsQueryable()
                .Where(s => s.ProductId == productId).ToListAsync();

            foreach (var productSize in productSelectedSizes)
            {
                _context.ProductSizes.Remove(productSize);
            }
        }

        #endregion

        #region Product Feature

        public async Task AddRangeProductFeatures(List<ProductFeature> productFeatures)
        {
            foreach (var feature in productFeatures)
            {
                await _context.ProductFeatures.AddAsync(feature);
            }
        }

        public async Task RemoveAllProductSelectedFeatures(long productId)
        {
            var productSelectedFeatures = await _context.ProductFeatures.AsQueryable()
                .Where(s => s.ProductId == productId).ToListAsync();

            foreach (var productFeature in productSelectedFeatures)
            {
                _context.ProductFeatures.Remove(productFeature);
            }
        }

        public async Task<List<ProductFeature>> GetProductFeaturesByProductId(long productId)
        {
            return await _context.ProductFeatures.Where(f => f.ProductId.Equals(productId)).ToListAsync();
        }

        #endregion

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
