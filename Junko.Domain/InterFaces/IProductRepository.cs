using Junko.Domain.Entities.Account;
using Junko.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.InterFaces
{
    public interface IProductRepository
    {
        #region Methods

        #region Product

        Task<IQueryable<Product>> GetProductQuery();

        Task AddProduct(Product product);

        #endregion

        #region Product Category

        Task<List<ProductCategory>> GetAllProductCategories();

        Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long parentId);

        #endregion

        Task SaveChanges();

        #endregion
    }
}
