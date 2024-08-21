using Junko.Domain.Entities.Products;

namespace Junko.Domain.InterFaces
{
    public interface IProductRepository
    {
        #region Methods

        #region Product

        Task<IQueryable<Product>> GetProductQuery();

        Task<Product?> GetProductById(long id);

        Task<Product?> GetProductForEdit(long id);

        void UpdateProduct(Product product);    

        Task AddProduct(Product product);

        #endregion

        #region Product Category

        Task<List<ProductCategory>> GetAllProductCategories();

        Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long parentId);

        Task AddRangeProductSelectedCategorys(List<ProductSelectedCategory> productSelectedCategories);

        #endregion

        #region Product Color

        Task AddRangeProductColors(List<ProductColor> productColors);

        #endregion

        #region Product Size

        Task AddRangeProductSizes(List<ProductSize> productSizes);

        #endregion


        Task SaveChanges();

        #endregion
    }
}
