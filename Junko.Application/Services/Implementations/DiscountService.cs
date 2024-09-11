using Junko.Application.Convertor;
using Junko.Application.Services.Interfaces;
using Junko.Domain.Entities.Discount;
using Junko.Domain.InterFaces;
using Junko.Domain.ViewModels.Discount;

namespace Junko.Application.Services.Implementations
{
    public class DiscountService : IDiscountService
    {
        #region consractor

        private readonly IDiscountRepository _discountRepository;

        private readonly IProductRepository _productRepository;

        public DiscountService(IDiscountRepository discountRepository, IProductRepository productRepository)
        {
            _discountRepository = discountRepository;
            _productRepository = productRepository;
        }

        #endregion

        #region Methods

        #region product discount

        public async Task<FilterProductDiscountDTO> FilterProductDiscount(FilterProductDiscountDTO filter)
        {
            var query = await _discountRepository.GetAllProductDiscounts();

            #region Filter

            if (filter.ProductId != null && filter.ProductId != 0)
            {
                query = query.Where(d => d.ProductId == filter.ProductId);
            }

            if (filter.SellerId != null && filter.SellerId != 0)
            {
                query = query.Where(d => d.Product.SellerId == filter.SellerId);
            }

            #endregion

            query = query.OrderByDescending(d => d.CreateDate);

            #region Paging

            await filter.SetPaging(query);  

            #endregion

            return filter;
        }

        public async Task<CreateDiscountResult> CreateProductDiscount(CreateProductDiscountDto discount, long sellerId)
        {
            var product = await _productRepository.GetProductById(discount.ProductId);

            if (product == null)
            {
                return CreateDiscountResult.ProductNotFound;
            }

            if (product.SellerId != sellerId)
            {
                return CreateDiscountResult.ProductIsNotForSeller;
            }

            var newDiscount = new ProductDiscount
            {
                ProductId = product.Id,
                DiscountNumber = discount.DiscountNumber,
                Percentage = discount.Percentage,
                ExpireDate = discount.ExpireDate.ToMiladiDateTime()
            };

            await _discountRepository.AddProductDiscount(newDiscount);
            await _discountRepository.SaveChanges();

            return CreateDiscountResult.Success;
        }

        #endregion

        #endregion
    }
}
