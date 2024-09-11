using Junko.Application.Services.Interfaces;
using Junko.Domain.InterFaces;

namespace Junko.Application.Services.Implementations
{
    public class DiscountService : IDiscountService
    {
        #region consractor

        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        #endregion

        #region Methods



        #endregion
    }
}
