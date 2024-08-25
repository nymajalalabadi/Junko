using Junko.Domain.Entities.Products;
using Junko.Domain.ViewModels.Products;
using Junko.Domain.ViewModels.Store;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Interfaces
{
    public interface IProductService
    {
        #region Methods

        #region Products

        Task<FilterProductDTO> FilterProducts(FilterProductDTO filter);

        Task<CreateProductResult> CreateProduct(CreateProductDTO product, long sellerId);

        Task<bool> AcceptSellerProduct(long productId);

        Task<bool> RejectProductRequest(RejectItemDTO reject);

        Task<EditProductDTO> GetProductForEdit(long productId);

        Task<EditProductResult> EditSellerProduct(EditProductDTO product, long userId);

        #endregion

        #region product gallery

        Task<List<ProductGallery>> GetAllProductGalleries(long productId);

        Task<List<ProductGallery>> GetAllProductGalleriesInSellerPanel(long productId, long userId);

        #endregion

        #region product categories

        Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long parentId);

        Task<List<ProductCategory>> GetAllActiveProductCategories();

        #endregion

        #endregion
    }
}
